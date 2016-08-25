﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using ServerData;


namespace Client_Forms {


    public partial class ChatForm : Form {

        Socket clientScoket;
        string clientID;
        IPEndPoint hostAddress;
        IPAddress hostIPAddress;
        bool connected = false;
        Thread receiveThread;


        public ChatForm() {
            InitializeComponent();
        }

        private void btnSend_Click( object sender, EventArgs e ) {

            Packet p = new Packet( PacketType.Chat, clientID );
            p.data.Add( txtName.Text );
            p.data.Add( txtOut.Text );

            //Send message to server

            WriteLine( txtIn, txtName.Text + ": " + txtOut.Text );

            clientScoket.Send(p.ToBytes());

            txtOut.Text = "";
            txtOut.Focus();
        }


        private void chatKeyDown( object sender, KeyEventArgs e ) {

            if (e.KeyCode == Keys.Enter && e.Modifiers != Keys.Control) {
                btnSend_Click( sender, (EventArgs)e );
                e.Handled = true;
            }
        }

        private void btnConnect_Click( object sender, EventArgs e ) {
            AttemptConnection connect = new AttemptConnection( ConnectToServer );
            connect.BeginInvoke(null, null );
        }

        delegate void WriteDelegate( RichTextBox obj, string text );
        void Write( RichTextBox obj, string text ) {
            Debug.Assert( txtIn.InvokeRequired == false );
            obj.Text += text;
        }

        delegate void WriteLineDelegate( RichTextBox obj, string text );
        void WriteLine( RichTextBox obj, string text ) {
            Debug.Assert( txtIn.InvokeRequired == false );
            obj.Text += text + '\n';
        }

        delegate void AttemptConnection();
        void ConnectToServer() {

            if (!connected) {

                WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );

                //TODO: quitar la ip harcodeada
                hostIPAddress = NetData.localhost;

                clientScoket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

                hostAddress = new IPEndPoint( hostIPAddress, NetData.PORT );

                // Try to connect to host
                int connAttempts = 0;

                while (connAttempts < 10) {

                    try {

                        connAttempts++;
                        clientScoket.Connect( hostAddress );
                        connected = true;
                        break;
                    }
                    catch (SocketException ex) {

                        this.Invoke( writeLine, new object[] { txtOut, "Could not connect to host... Attempt " + connAttempts.ToString() } );

                        if (connAttempts == 10)
                            this.Invoke( writeLine, new object[] { txtIn, ex.Message } );
                    }
                }

                if (connected) {
                    receiveThread = new Thread( GetPacket );
                    receiveThread.Start();
                }
            }
            else {
                MessageBox.Show( "Already connected." );
            }
        }

        void GetPacket() {

            byte[] buffer = new byte[clientScoket.ReceiveBufferSize];
            int recivedData;

            try {
                while (true) {

                    //TODO: User disconnections;

                    recivedData = clientScoket.Receive( buffer );

                    if (recivedData > 0) {

                        Packet packet = new Packet( buffer );
                        DispatchPacket( packet );

                    }

                }
            }
            catch( SocketException ex) {

            }

        }

        void DispatchPacket( Packet p ) {

            WriteDelegate write = new WriteDelegate( Write );
            WriteLineDelegate writeLine = new WriteLineDelegate( WriteLine );

            switch(p.type) {
                case PacketType.Registration: {

                        clientID = p.senderID;
                        this.Invoke( writeLine, new object[] { txtIn, "Connected to server." } );
                        this.Invoke( writeLine, new object[] { txtIn, "Client id recieved: " + clientID } );
                        break;
                    }
                case PacketType.Chat: {

                        this.Invoke( writeLine, new object[] { txtIn, p.data[0] + ": " + p.data[1] } );

                        break;
                    }
            }

        }

        private void ChatForm_FormClosing( object sender, FormClosingEventArgs e ) {
            if(receiveThread != null)
                receiveThread.Abort();

            //TODO: Enviarlo a una funcion con validaciones
            clientScoket.Shutdown( SocketShutdown.Both );
            clientScoket.Close();

        }
    }
}

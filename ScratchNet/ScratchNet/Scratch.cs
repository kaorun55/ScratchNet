using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class Scratch
    {
        TcpClient client;
        IPEndPoint destination;

        SensorUpdateBuilder sensorUpdateMessage = new SensorUpdateBuilder();
        BroadcastBuilder broadcast = new BroadcastBuilder();

        static string localhost = "127.0.0.1";

        Thread thread;
        bool isRunning = true;

        public delegate void BroadcastEvent( object sender, string value );
        public event BroadcastEvent OnBroadcast;

        public delegate void SensorUpdateEvent( object sender, Dictionary<string, string> value );
        public event SensorUpdateEvent OnSensorUpdate;

        SynchronizationContext context = SynchronizationContext.Current;

        public Scratch()
            : this( localhost, 42001 )
        {
        }

        public Scratch( string address, int port )
        {
            client = new TcpClient();
            destination = new IPEndPoint( IPAddress.Parse( address ), port );
        }

        public async void Connect()
        {
            if ( client.Connected ) {
                return;
            }

            await client.ConnectAsync( destination.Address, destination.Port );
            Receive();
        }

        public void Close()
        {
            if ( !client.Connected ) {
                return;
            }

            isRunning = false;
            client.Close();

            if ( thread != null ) {
                thread.Join();
                thread = null;
            }
        }

        void Receive()
        {
            thread = new Thread( () =>
            {
                try {
                    isRunning = true;
                    while ( isRunning ) {
                        var stream = client.GetStream();

                        byte[] size = new byte[4];
                        stream.Read( size, 0, size.Count() );

                        int count = ((int)size[3]) + ((int)size[2] << 8) + ((int)size[1] << 16) + ((int)size[0] << 24);
                        if ( count == 0 ) {
                            continue;
                        }

                        byte[] buffer  = new byte[count];
                        stream.Read( buffer, 0, buffer.Count() );

                        var message = Encoding.UTF8.GetString( buffer );
                        if ( BroadcastParser.IsValid( message ) ) {
                            context.Post( _ =>
                            {
                                var value = BroadcastParser.Parse( message );
                                if ( OnBroadcast != null ) {
                                    OnBroadcast( this, value );
                                }
                            }, null );
                        }
                        else if ( SensorUpdateParser.IsValid( message ) ) {
                            context.Post( _ =>
                            {
                                var value = SensorUpdateParser.Parse( message );
                                if ( OnSensorUpdate != null ) {
                                    OnSensorUpdate( this, value );
                                }
                            }, null );
                        }
                    }
                }
                catch ( Exception ) {
                }
            } );

            thread.Start();
        }

        public void AddSensorValue( string sensor, string val )
        {
            sensorUpdateMessage.AddMessage( sensor, val );
        }

        public void SensorUpdate()
        {
            if ( sensorUpdateMessage.Message != "sensor-update" ) {
                SendMessage( sensorUpdateMessage.Message );
            }

            sensorUpdateMessage.Clear();
        }

        public void Broadcast( string value )
        {
            broadcast.AddValue( value );
            SendMessage( broadcast.Message );
            broadcast.Clear();
        }

        async void SendMessage( string message ){
            if ( !client.Connected ) {
                throw new Exception( "Not connected to Scratch." );
            }

            var sizeBytes = new byte[4];
            int len = message.Length;

            sizeBytes[0] =(byte)(len >> 24);
            sizeBytes[1] =(byte)((len << 8) >> 24);
            sizeBytes[2] =(byte)((len << 16) >> 24);
            sizeBytes[3] =(byte)((len << 24) >> 24);

            var stream = client.GetStream();
            await stream.WriteAsync( sizeBytes, 0, 4 );

            var m = Encoding.UTF8.GetBytes( message );
            await stream.WriteAsync( m, 0, m.Length );
        }
    }
}

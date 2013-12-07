using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

        public Scratch()
            : this( localhost, 42001 )
        {
        }

        public Scratch( string address, int port )
        {
            client = new TcpClient();
            destination = new IPEndPoint( IPAddress.Parse( address ), port );

            //Receive();
        }

        public void Connect()
        {
            client.Connect( destination );
        }

        //async void Receive()
        //{
        //    udpReceiver = new UdpClient( 42001 );

        //    while ( true ) {

        //        var recv = await udp.ReceiveAsync();
        //        var b = recv.Buffer;
        //    }
        //}

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

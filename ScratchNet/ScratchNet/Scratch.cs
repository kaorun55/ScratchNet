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
        UdpClient udp;
        //UdpClient udpReceiver;
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
            udp = new UdpClient();
            destination = new IPEndPoint( IPAddress.Parse( address ), port );

            //Receive();
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

        void SendMessage( string message ){
            var sizeBytes = new byte[4];
            int len = message.Length;

            sizeBytes[0] =(byte)(len >> 24);
            sizeBytes[1] =(byte)((len << 8) >> 24);
            sizeBytes[2] =(byte)((len << 16) >> 24);
            sizeBytes[3] =(byte)((len << 24) >> 24);

            udp.Send( sizeBytes, 4, destination );

            var m = Encoding.UTF8.GetBytes( message );
            udp.Send( m, m.Length, destination );
        }
    }
}

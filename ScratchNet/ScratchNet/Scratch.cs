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
        IPEndPoint destination;

        SensorUpdateBuilder sensorUpdateMessage = new SensorUpdateBuilder();

        public Scratch()
            : this( "127.0.0.1", 42001 )
        {
        }

        public Scratch( string address, int port )
        {
            udp = new UdpClient();
            destination = new IPEndPoint( IPAddress.Parse( address ), port );

            InitializeMessage();
        }

        private void InitializeMessage()
        {
            sensorUpdateMessage.Clear();
        }

        public void AddSensorValue( string sensor, string val )
        {
            sensorUpdateMessage.AddMessage( sensor, val );
        }

        public void UpdateSensor()
        {
            if ( sensorUpdateMessage.Message != "sensor-update" ) {
                SendMessage( sensorUpdateMessage.Message );
            }

            InitializeMessage();
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

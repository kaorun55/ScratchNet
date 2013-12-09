using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class SensorUpdateParser
    {
        private static string[] InnerParse( string message )
        {
            var content = message.Split( " ".ToCharArray() );
            if ( content.Length <= 3 ) {
                return null;
            }

            if ( content[0] != "sensor-update" ) {
                return null;
            }

            return content;
        }

        public static Dictionary<string, string> Parse( string message )
        {
            var content = InnerParse( message );
            if ( content == null ) {
                return new Dictionary<string, string>();
            }

            var value = new Dictionary<string, string>();
            for ( int i = 1; i < content.Length; i += 2 ) {
                if ( string.IsNullOrEmpty( content[i] ) ) {
                    break;
                }

                value.Add( content[i].Replace( "\"", "" ), content[i + 1] );
            }

            return value;
        }

        public static bool IsValid( string message )
        {
            return InnerParse( message ) != null;
        }
    }
}

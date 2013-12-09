using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchNet
{
    public class BroadcastParser
    {
        private static string[] InnerParse( string message )
        {
            var content = message.Split( " ".ToCharArray() );
            if ( content.Length != 2 ) {
                return null;
            }

            if ( content[0] != "broadcast" ) {
                return null;
            }

            return content;
        }
        
        public static string Parse( string message )
        {
            var content = InnerParse( message );
            if ( content == null ) {
                return string.Empty;
            }

            return content[1].Replace( "\"", "" );
        }

        public static bool IsValid( string message )
        {
            return InnerParse( message ) != null;
        }
    }
}

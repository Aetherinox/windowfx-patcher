using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFXPatch
{
    public sealed class Log
    {

        #region "Fileinfo"

            /*
                Define > File Name
            */

            readonly static string log_file = "Log.cs";

		#endregion

		public static StringBuilder LogString = new StringBuilder( ); 

        public static void Out( string str )
        {
            Console.WriteLine( str );
            LogString.Append( str ).Append( Environment.NewLine );
        }

		/*
			Log > Get Storage File
				specifies where logs will be stored.
		*/

		public static string GetStorageFile( )
		{
            DateTime dt             = DateTime.Now;
            string now              = dt.ToString( "MM_dd_yy" );
            
			return String.Format( "{0}_devlog.log", now );
		}

		/*
			convert list of string arrays to a string with proper column formatting / padding.
			each array must contain the same number of arguments

			string a	= "Some Data";
			string b	= "More Data"
			var lines	= new List<string[]>();

			lines.Add( new[] { "Column Name", a, b } );

			var output	= Log.PrintLines( lines, 3 );
		*/

		public static void PrintColumn( List<string[]> lines, int padding = 1 )
		{
			var numElements		= lines[ 0 ].Length;
			var maxValues		= new int[numElements ];

			for ( int i = 0; i < numElements; i++ )
			{
				maxValues[ i ] = lines.Max( x => x[ i ].Length ) + padding;
			}

			var sb			= new StringBuilder( );
			bool bFirst		= true;

			foreach (var line in lines)
			{
				if ( !bFirst )
				{
					sb.AppendLine( );
				}

				bFirst = false;

				for ( int i = 0; i < line.Length; i++ )
				{
					var value = line[i];
					sb.Append( value.PadRight( maxValues[ i ] ) );
				}
			}

			Console.WriteLine( sb.ToString( ) );

			//return sb.ToString();
		}

		public static void Send( string cat = "", int line = 0, string msg = "", string val = "" )
		{
			DateTime dt			= DateTime.Now;
			string now			= dt.ToString( "MM.dd.yy HH:mm" );
			string line_file	= String.Format( "{0}[{1}]", cat, line );

			Console.WriteLine( "{0,-18}{1,-24}{2,-30}{3,-15}", now, line_file, msg, val );
		}

	}

}

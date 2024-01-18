/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using WFXPatch;
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Resources;
using Res = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;

namespace WFXPatch
{

    public sealed class Program
    {

        #region "Fileinfo"

            /*
                Define > File Name
            */

            readonly static string log_file = "Program.cs";

        #endregion

        /*
            Define > Classes
        */

        static AppInfo AppInfo              = new AppInfo( );

        /*
            Define > Misc
        */

        readonly static Action<string> wl   = Console.WriteLine;

        /*
            Program > Main
        */

        [STAThread]
        static void Main( string[] args )
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );

            string log_filename     = Log.GetStorageFile( );
            ConsoleFileOutput cf    = new ConsoleFileOutput( log_filename, Console.Out);
            Console.SetOut( cf );

            /*
                 developer mode
            */

            if ( args.Length > 0 && args[ 0 ] == "--debug" )
            {
                Cfg.Default.app_bDevmode = true;
            }

            /*
                 Elevate to admin so we can modify the windows host file.
            */

            if ( !IsAdmin( ) )
            {
                ProcessStartInfo procStart  = new ProcessStartInfo( );
                procStart.UseShellExecute   = true;
                procStart.Verb              = "runas";
                procStart.FileName          = Application.ExecutablePath;
                try
                {
                    Process.Start( procStart );
                }
                catch
                {
                    MessageBox.Show
                    (
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        Res.msgbox_core_runas_msg,
                        Res.msgbox_core_runas_title,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );

                    return;
                }
                return;
            }


            //Log.Send( log_file, "new FormParent()" );

            Application.Run( new FormParent( ) );
        }

        /*
            Console Override
        */

        public class ConsoleFileOutput : TextWriter
        {
            private Encoding encoding = Encoding.UTF8;
            private StreamWriter writer;
            private TextWriter console;

            public override Encoding Encoding
            {
                get { return encoding; }
            }

            public ConsoleFileOutput( string file, TextWriter console, Encoding encoding = null)
            {
                if ( encoding != null )
                    this.encoding = encoding;

                this.console    = console;
                this.writer     = new StreamWriter( file, true, this.encoding );

                this.writer.AutoFlush = true;
            }

            public override void Write(string value)
            {
                Console.SetOut  ( console );
                Console.Write   ( value );
                Console.SetOut  ( this );

                this.writer.Write( value );
            }

            public override void WriteLine( string msg )
            {
                Console.SetOut( console );
                Console.WriteLine( msg );
                this.writer.WriteLine( msg );

                Console.SetOut( this );
            }

            public override void Flush( )
            {
                this.writer.Flush( );
            }

            public override void Close( )
            {
                this.writer.Close( );
            }

            new public void Dispose( )
            {
                this.writer.Flush( );
                this.writer.Close( );
                this.writer.Dispose( );
                base.Dispose( );
            }

        }
   
        /*
            Check if running as admin
        */

        private static bool IsAdmin( )
        {
            WindowsIdentity id          = WindowsIdentity.GetCurrent( );
            WindowsPrincipal principal  = new WindowsPrincipal( id );

            return principal.IsInRole( WindowsBuiltInRole.Administrator );
        }

    }
}

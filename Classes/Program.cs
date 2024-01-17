/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;
using Res = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;

namespace WFXPatch
{

    static class Program
    {
        [STAThread]
        static void Main( )
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );

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

            Application.Run( new FormParent( ) );
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

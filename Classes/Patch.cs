/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using Lng = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;
using System.Security.Policy;
using System.Management.Automation.Language;

namespace WFXPatch
{

    /*
        Class > Patch
    */

    class Patch
    {

        /*
            Define > Dependency Classes
        */

        private Helpers Helpers         = new Helpers( );
        private Perms Perms             = new Perms( );
        private AppInfo AppInfo         = new AppInfo( );

        /*
            Define > Paths
        */

        private static string patch_launch_dir  = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetEntryAssembly( ).Location );
        private static string app_target_exe    = Cfg.Default.app_target_exe;

        /*
             Define > Target Program Search Locations
        */

        private static string find_InAppData    = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                                                    "Stardock",
                                                    "WindowFX",
                                                    app_target_exe
                                                );

        private static string find_InProg64     = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ),
                                                    "Stardock",
                                                    "WindowFX",
                                                    app_target_exe
                                                );

        private static string find_InProg86     = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86 ),
                                                    "Stardock",
                                                    "WindowFX",
                                                    app_target_exe
                                                );

        private static string find_InAppHome    = Path.Combine(
                                                    patch_launch_dir,
                                                    "Stardock",
                                                    "WindowFX",
                                                    app_target_exe
                                                );

        /*
             Start Patch

             arg        : str path_selected
        */

        public void Start( string path_selected = "auto" )
        {

            /*
                define arrays
            */

            string[] paths_arr      = new string[] { };
            string[] paths_lst      = new string[] { };

            /*
                Status > start locate
            */

            StatusBar.Update( string.Format( Lng.status_patch_locating, Cfg.Default.app_name ) );

            /*
                populate path list array
            */

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = find_InAppData;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = find_InProg64;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = find_InProg86;

            Array.Resize( ref paths_lst, paths_lst.Length + 1 );
            paths_lst [ paths_lst.Length - 1 ] = find_InAppHome;

            /*
                define
            */

            if ( File.Exists( find_InAppData ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = find_InAppData;
            }

            if ( File.Exists( find_InProg64 ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = find_InProg64;
            }

            if ( File.Exists( find_InProg86 ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = find_InProg86;
            }

            if ( File.Exists( find_InAppHome ) )
            {
                Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                paths_arr [ paths_arr.Length - 1 ] = find_InAppHome;
            }

            /*
                custom file loaded
            */

            if ( path_selected != "auto" )
            {
                if ( File.Exists( path_selected ) )
                {
                    paths_arr = new string [ ] { };
                    paths_lst = new string [ ] { };

                    Array.Resize( ref paths_arr, paths_arr.Length + 1 );
                    paths_arr [ paths_arr.Length - 1 ] = path_selected;

                    Array.Resize( ref paths_lst, paths_lst.Length + 1 );
                    paths_lst [ paths_lst.Length - 1 ] = path_selected;
                }
            }

            /*
                count results
            */

            int i_arr = paths_arr.Length;

            /*
                if list of paths empty, concat array into string and return error message listing what
                paths were checked.
            */

            if ( i_arr == 0 )
            {

                string path_compiled    = "";
                StringBuilder sb        = new StringBuilder( );

                sb.Append( Environment.NewLine );

                foreach ( string file in paths_lst )
                {
                    sb.Append( Environment.NewLine );
                    sb.Append( file );

                    path_compiled = sb.ToString( );
                }

                StatusBar.Update( Lng.status_manual_locate );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    string.Format( Lng.msgbox_nolocpath_msg, path_compiled ),
                    Lng.msgbox_nolocpath_title,
                    MessageBoxButtons.OK, MessageBoxIcon.Error
                );

                /*
                    Attempt to allow user to manually select file to be patched.
                */

                string src_file_path    = Helpers.FindApp( );
                string ext_default      = @"c:\";

                if ( !String.IsNullOrEmpty( src_file_path ) )
                {
                    ext_default         = System.IO.Path.GetDirectoryName( src_file_path );
                }

                OpenFileDialog dlg      = new OpenFileDialog( );
                dlg.Title               = Lng.dlg_title;
                dlg.InitialDirectory    = ext_default;
                dlg.Filter              = "WindowFX EXE|WindowFXConfig.exe|All files (*.*)|*.*";
                DialogResult result     = dlg.ShowDialog( );

                /*
                    Dialog > User Input > Cancel
                */

                if ( result == DialogResult.Cancel )
                {
                    StatusBar.Update( Lng.dlg_cancelled );

                    return;
                }

                /*
                    Dialog > User Input > OK
                */

                if ( result == DialogResult.OK )
                {
                    StreamReader sr     = File.OpenText( dlg.FileName );

                    string s            = sr.ReadLine( );
                    StringBuilder sbr   = new StringBuilder( );

                    while ( s != null )
                    {
                        sbr.Append( s );
                        s = sr.ReadLine( );
                    }
                    sr.Close( );

                    StatusBar.Update( string.Format( Lng.status_dlg_loaded , dlg.FileName ) );
                }

                StatusBar.Update( Lng.status_wfx_not_found );
            }

            /*
                Temp kill automatic shell restart when process is killed
                AutoRestartShell = 0
            */

            /*
             * 
            RegistryKey ourKey  = Registry.LocalMachine;
            ourKey              = ourKey.OpenSubKey(
                                    @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon",
                                    true
                                );
            ourKey.SetValue     ( "AutoRestartShell", 0 );

            */

            /*
                 kill task explorer.exe
            */

          
            try
            {
                Process[] processes = Process.GetProcessesByName( "WindowFXConfig" );
                foreach ( Process proc in processes )
                {
                    proc.Kill( );
                }
            }
            catch ( Exception )
            {
                StatusBar.Update( String.Format( Lng.status_taskkill_fail, "WindowFXConfig" ) );
                return;
            }
            finally
            {
                StatusBar.Update( string.Format( Lng.status_taskkill_succ, "WindowFXConfig.exe" ) );
                Console.WriteLine( String.Format( "Service kill [Complete]: {0}", "WindowFXConfig.exe" ) );
            }

            /*
                 kill / restart task explorer.exe
            */

            try
            {
                Process.Start( "cmd", "/c taskkill /f /im WindowFXConfig.exe" ).WaitForExit( );
            }
            catch ( Exception )
            {
                StatusBar.Update( String.Format( Lng.status_taskkill_fail, "WindowFXConfig.exe" ) );
                return;
            }
            finally
            {
                StatusBar.Update( string.Format( Lng.status_taskkill_succ, "WindowFXConfig.exe" ) );
                Console.WriteLine( String.Format( "Service kill [Complete]: {0}", "WindowFXConfig.exe" ) );
            }


            /*
                 Kill WindowFX services

                    Stardock WindowFX 6 Helper Process              =>  wfx32.exe
                    WindowFX Service.  Part of Stardock WindowFX 6  =>  WindowFXSRV.exe
             */

            StatusBar.Update( string.Format( Lng.status_services_stopping, Cfg.Default.app_name ) );

            try
            {
                Process[] processes = Process.GetProcessesByName( "wfx32" );
                foreach ( Process proc in processes )
                {
                    proc.Kill( );
                }
            }
            catch ( Exception )
            {
                StatusBar.Update( String.Format( Lng.status_taskkill_fail, "wfx32" ) );
                return;
            }
            finally
            {
                StatusBar.Update( string.Format( Lng.status_taskkill_succ, "wfx32" ) );
                Console.WriteLine( String.Format( "Service kill [Complete]: {0}", "wfx32" ) );
            }

            try
            {
                Process[] processes = Process.GetProcessesByName( "WindowFXSRV" );
                foreach ( Process proc in processes )
                {
                    proc.Kill( );
                }
            }
            catch ( Exception )
            {
                StatusBar.Update( Lng.status_taskkill_fail );
                return;
            }
            finally
            {
                StatusBar.Update( string.Format( Lng.status_taskkill_succ, "WindowFXSRV" ) );
                Console.WriteLine( String.Format( "Service kill [Complete]: {0}", "WindowFXSRV" ) );
            }

            /*
                loop each dll path

                    path_exe returns full path to program exe to back up
                    ->  Stardock\WindowFX\app_target_exe.exe
            */

            foreach ( string WFX_path_exe in paths_arr )
            {

                string WFX_path_fol     = Path.GetDirectoryName( WFX_path_exe );
                string WFX_path_bak     = WFX_path_exe + ".bak";
                string psq_var          = "$user_current = $env:username";
                string psq_takeown      = "takeown /f \"" + WFX_path_bak + "\" y";
                string psq_icalcs       = "icacls \"" + WFX_path_bak + "\" /grant \"${user_current}:F\" /C /L";

                /*
                    if full backup path exists
                        x:\path\to\WindowFXConfig.exe.bak
                */

                if ( File.Exists( WFX_path_bak ) )
                {

                    /*
                        run powershell commands to adjust permissions
                    */

                    using ( PowerShell ps = PowerShell.Create( ) )
                    {

                        ps.AddScript( psq_var );
                        ps.AddScript( psq_takeown );
                        ps.AddScript( psq_icalcs );

                        Collection<PSObject> PSOutput   = ps.Invoke( );
                        StringBuilder sb                = new StringBuilder( );

                        foreach ( PSObject PSItem in PSOutput )
                        {
                            if ( PSItem != null )
                            {
                                // Console.WriteLine( $"Output line: [{PSItem}]" );
                                sb.AppendLine( PSItem.ToString( ) );
                            }
                        }

                        if ( ps.Streams.Error.Count > 0 )
                        {
                            // Error collection
                        }
                    }

                    if ( AppInfo.bIsDebug( ) )
                    {
                        MessageBox.Show( new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen }, string.Format( ".bak backup file already exists, deleting it and creating new\n\n{0}", WFX_path_bak ),
                            "Debug: Found existing",
                            MessageBoxButtons.OK, MessageBoxIcon.None
                        );
                    }
                    
                    /*
                        DELETE existing x:\path\to\WindowFXConfig.exe.bak
                    */

                    File.Delete         ( WFX_path_exe );
                    File.Move           ( WFX_path_bak, WFX_path_exe );
                }

                /*
                    SET     attributes on WindowFXConfig.exe
                    COPY    WindowFXConfig.exe -> WindowFXConfig.exe.bak
                    SET     attributes on WindowFXConfig.exe.bak
                */

                StatusBar.Update( string.Format( Lng.status_bak_create, WFX_path_exe + ".bak" ) );

                File.SetAttributes      ( WFX_path_exe,     FileAttributes.Normal );
                File.Copy               ( WFX_path_exe,     WFX_path_bak );
                File.SetAttributes      ( WFX_path_bak,     FileAttributes.Normal );

                /*
                    modify bytes for exe
                */

                double i_progress = 0;

                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "01 00 00 00 00 00 00 C0 4D 00 00 04 00 00 4E BB 4E 00 02 00 40 81 00 00 10 00 00 10 00 00 00 00 10 00 00 10 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 E4 6A 3C 00 F4 01 00 00 00 30 3F 00 F4 84 0A 00 00 00 00 00 00 00 00 00 00 86 4C 00 88 61 01 00 00 C0 49 00 18 F4 03 00 B0 33 31 00 1C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 20 31 00 B0 0E 00 00",
                    "01 00 00 00 00 00 00 C0 4D 00 00 04 00 00 00 00 00 00 02 00 40 81 00 00 10 00 00 10 00 00 00 00 10 00 00 10 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 E4 6A 3C 00 F4 01 00 00 00 30 3F 00 F4 84 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 C0 49 00 18 F4 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "FF FF 6A 01 50 E8 6D 60 02 00 83 C4 14 85 C0 78 17 83 BD 58 FF FF FF 0A 7E 0E 33 C0 83 BD",
                    "FF FF 6A 01 50 E8 6D 60 02 00 83 C4 14 85 C0 78 17 C7 85 58 FF FF FF 0B 00 00 00 90 83 BD"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "45 FC C9 C3 55 8B EC 83 EC 7C A1 D8 FA",
                    "45 FC C9 C3 B8 01 00 00 00 C3 A1 D8 FA"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "85 C9 74 06 C7 01 01 00 00 00 83 C0 10",
                    "85 C9 74 06 C7 01 0E 00 00 00 83 C0 10"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "6A 6C 66 89 45 D4 58 6A 69 66 89 45 D8",
                    "6A 6C 66 89 45 D4 58 6A 6F 66 89 45 D8"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "E8 78 08 FF FF 85 C0 75 32 6A 10 68 E4",
                    "E8 78 08 FF FF 85 C0 EB 32 6A 10 68 E4"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "06 FF FF 85 C0 75 54 6A 10 68 E4 14 72",
                    "06 FF FF 85 C0 EB 54 6A 10 68 E4 14 72"
                );

                i_progress += Math.Round( 12.5 );
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "33 73 FE FF 85 C0 74 30 6A 10 68 E4 14",
                    "33 73 FE FF 85 C0 EB 30 6A 10 68 E4 14"
                );

                i_progress = 100;
                StatusBar.Update( string.Format( Lng.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                /*
                    launch WindowFX
                */

                if ( !String.IsNullOrEmpty( WFX_path_fol ) )
                {
                    if ( File.Exists( WFX_path_exe ) )
                    {
                        Console.WriteLine( String.Format( "Patch: [Launch]: {0}", WFX_path_exe ) );
                        System.Diagnostics.Process.Start( WFX_path_exe );
                        StatusBar.Update( string.Format( Lng.status_launch_app, Cfg.Default.app_name ) );
                    }
                }
            }

            /*
                start task explorer.exe

                    disabled because WindowFX doesnt need a shell restart
            */

            // Process.Start( "explorer" );

            /*
                re-enable AutoRestartShell in registry
                AutoRestartShell = 1

            ourKey.SetValue( "AutoRestartShell", 1 );
            ourKey.Close( );

            */

            /*
                re-enable AutoRestartShell in registry
                AutoRestartShell = 1
            */

            MessageBox.Show( new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen }, string.Format( "{0}", Lng.msgbox_patch_compl_msg ),
                Lng.msgbox_patch_compl_title,
                MessageBoxButtons.OK, MessageBoxIcon.Information
            );

            StatusBar.Update( Lng.status_patch_complete );

            return;
        }

        /*
             Modify Bytes

             @arg       : str exe
             @arg       : str a
             @arg       : str b
             @ret       : void
        */

        public void ModifyBytes( string exe, string a, string b )
        {

            byte[] bytes                = File.ReadAllBytes( exe );
            string hex_replace          = BitConverter.ToString( bytes ).Replace( "-", " " );
            string hex_result           = hex_replace.Replace( @a, @b );

            if ( AppInfo.bIsDebug( ) )
            {
                try
                {
                    StreamWriter sw_o   = new StreamWriter( "hex_original.dmp" );
                    sw_o.WriteLine      ( hex_replace );
                    sw_o.Close          ( );

                    StreamWriter sw_p   = new StreamWriter( "hex_patched.dmp" );
                    sw_p.WriteLine      ( hex_result );
                    sw_p.Close          ( );

                    Console.WriteLine( String.Format( "ModifyBytes: [StreamWriter]: {0}\n   Find:     {1}\n   Replace:  {2}\n", exe, a, b ) );

                }
                catch ( Exception e )
                {
                    Console.WriteLine( String.Format( "Hex Dump [Exception]: {0} - {1} ", exe, e.Message ) );
                }
                finally
                {
                    Console.WriteLine( String.Format( "Hex Dump [Complete]: {0}", exe ) );
                }
            }

            // bytes will be separated by a hyphen -, remove hyphen
            string[] hex_patch          = hex_result.Split(' ');
            byte[] bytes_modified       = new byte[ hex_patch.Length ];

            for ( int i = 0 ; i < hex_patch.Length ; i++ )
            {
                bytes_modified [ i ]    = Convert.ToByte( hex_patch [ i ], 16 );
            }

            Console.WriteLine( String.Format( "ModifyBytes: [Save File]: {0}", exe ) );
            File.WriteAllBytes( exe, bytes_modified );
        }

        /*
            Block Host

                A two-part function which
                    - Adds entries to the Windows host file
                    - Adds new Windows firewall rules to block the executable from communicating.
        */

        public void BlockHost( )
        {
            string hostfile_path = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.System ), "drivers\\etc\\hosts" );

            /*
                Windows host file
                    adds a list of blocked dns

                @revised    : 01.14.24
            */

            using ( StreamWriter w = File.AppendText( hostfile_path ) )
            {
                w.WriteLine( Environment.NewLine                    );
                w.WriteLine( "# Stardock WindowFX Host Block"       );
                w.WriteLine( "0.0.0.0 66.79.209.82"                 );
                w.WriteLine( "0.0.0.0 activate.api.stardock.net"    );

                MessageBox.Show
                ( 
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    string.Format( Lng.msgbox_bhost_success_msg, hostfile_path ),
                    Lng.msgbox_bhost_success_title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }

            /*
                full path to exe being searched for
                    X:\Path\To\Folder\WindowFXConfig.exe
            */

            string app_path_exe = Helpers.FindApp( );

            /*
                Found no app file path, no need to continue blocks with the firewall.
                Do host entries only
            */

            if ( !File.Exists( app_path_exe ) )
            {

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    string.Format( Lng.msgbox_bhost_fw_badpath_msg, app_path_exe ), Lng.msgbox_bhost_fw_badpath_title,
                    MessageBoxButtons.OK, MessageBoxIcon.None
                );

                return;
            }

            /*
                splits full path into directory
                    X:\Path\To\Folder\WindowFXConfig.exe ->
                    X:\Path\To\Folder\
            */

            string app_path_dir         = Path.GetDirectoryName( app_path_exe );

            /*
                if for some reason, this utility can't edit the user's host file, we'll use Windows Firewall as a back up.
                Create two new firewall rules for inbound and outbound.
            */

            string fw_id_sha1           = Hash.GetSHA1Hash( app_path_exe );
            fw_id_sha1                  = string.IsNullOrEmpty( fw_id_sha1 ) ? "0" : fw_id_sha1;

            string fw_id_name           = string.Format( "01-WindowFX ({0})", fw_id_sha1 );
            string fw_id_desc           = string.Format( "Blocks WindowFX from communicating with license server. Added by {0}", Cfg.Default.app_url_github );
            string fw_id_exe            = app_path_exe;

            /*
                firewall rules | inbound + outbound
            */

            string fwl_rule_block_in    = "New-NetFirewallRule -Name \"" + fw_id_name + "-Inbound (Auto-added)\" -DisplayName \"" + fw_id_name + "-Inbound (Auto-added)\" -Description \"" + fw_id_desc + "\" -Enabled True -Protocol Any -Profile Any -Direction Inbound -Program \"" + fw_id_exe + "\" -Action Block";
            string fwl_rule_block_out   = "New-NetFirewallRule -Name \"" + fw_id_name + "-Outbound (Auto-added)\" -DisplayName \"" + fw_id_name + "-Outbound (Auto-added)\" -Description \"" + fw_id_desc + "\" -Enabled True -Protocol Any -Profile Any -Direction Outbound -Program \"" + fw_id_exe + "\" -Action Block";

            /*
                run powershell query to add entries to firewall.
            */

            using ( PowerShell ps = PowerShell.Create( ) )
            {

                ps.AddScript( fwl_rule_block_in );
                ps.AddScript( fwl_rule_block_out );

                Collection<PSObject> PSOutput   = ps.Invoke( );
                StringBuilder sb                = new StringBuilder( );

                foreach ( PSObject PSItem in PSOutput )
                {
                    if ( PSItem != null )
                    {
                        //Console.WriteLine( $"Output line: [{PSItem}]" );
                        sb.AppendLine( PSItem.ToString( ) );

                        if ( AppInfo.bIsDebug( ) )
                        {
                            MessageBox.Show
                            (
                                new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                                string.Format( Lng.msgbox_debug_ps_bhost_qry_ok_msg, fwl_rule_block_in, fwl_rule_block_out ),
                                Lng.msgbox_debug_ps_bhost_qry_ok_title,
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation
                            );
                        }

                    }
                }

                if ( ps.Streams.Error.Count > 0 )
                {
                    if ( AppInfo.bIsDebug( ) )
                    {
                        MessageBox.Show
                        (
                            new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                            string.Format( Lng.msgbox_debug_ps_bhost_qry_alert_msg ),
                            Lng.msgbox_debug_ps_bhost_qry_alert_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Error
                        );
                    }
                }
            }

            /*
                Firewall success message
            */

            MessageBox.Show
            (
                new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                string.Format( Lng.msgbox_bhost_fw_success_msg, app_path_exe ),
                Lng.msgbox_bhost_fw_success_title,
                MessageBoxButtons.OK, MessageBoxIcon.None
            );
        }
    }
}

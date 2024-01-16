/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using Res = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;
using System.Xml.Linq;

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
            Define > Actions
        */

        readonly static Action<string> wl = Console.WriteLine;

        /*
            Define > Paths
        */

        static private string patch_launch_fullpath = Process.GetCurrentProcess( ).MainModule.FileName;
        static private string patch_launch_dir      = Path.GetDirectoryName( patch_launch_fullpath );
        static private string patch_launch_exe      = Path.GetFileName( patch_launch_fullpath );
        private static string app_target_exe        = Cfg.Default.app_target_exe;

        /*
             Define > Target Program Search Locations
        */

        private static string find_InAppData        = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.LocalApplicationData ),
                                                        "Stardock",
                                                        "WindowFX",
                                                        app_target_exe
                                                    );

        private static string find_InProg64         = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFiles ),
                                                        "Stardock",
                                                        "WindowFX",
                                                        app_target_exe
                                                    );

        private static string find_InProg86         = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86 ),
                                                        "Stardock",
                                                        "WindowFX",
                                                        app_target_exe
                                                    );

        private static string find_InAppHome        = Path.Combine(
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

            StatusBar.Update( string.Format( Res.status_patch_locating, Cfg.Default.app_name ) );

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

                StatusBar.Update( Res.status_manual_locate );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    string.Format( Res.msgbox_nolocpath_msg, path_compiled ),
                    Res.msgbox_nolocpath_title,
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
                dlg.Title               = Res.dlg_title;
                dlg.InitialDirectory    = ext_default;
                dlg.Filter              = "WindowFX EXE|WindowFXConfig.exe|All files (*.*)|*.*";
                DialogResult result     = dlg.ShowDialog( );

                /*
                    Dialog > User Input > Cancel
                */

                if ( result == DialogResult.Cancel )
                {
                    StatusBar.Update( Res.dlg_cancelled );

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

                    StatusBar.Update( string.Format( Res.status_dlg_loaded, dlg.FileName ) );
                }

                StatusBar.Update( Res.status_wfx_not_found );
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
                 kill tasks
            */

            StatusBar.Update( string.Format( Res.status_task_stop_app, Cfg.Default.app_name ) );

            Helpers.TaskKill( "WindowFXConfig" );
            Helpers.TaskKill( "wfx64" );
            Helpers.TaskKill( "wfx32" );
            Helpers.TaskKill( "WindowFXSRV" );

            /*
                Check skip
            */

            bool bRequireEdit   = false;

            /*
                loop each dll path

                    path_exe returns full path to program exe to back up
                    ->  Stardock\WindowFX\app_target_exe.exe
            */

            foreach ( string WFX_path_exe in paths_arr )
            {

                string WFX_path_fol     = Path.GetDirectoryName( WFX_path_exe );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    WFX_path_exe,
                    "Path",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                /*
                    if full backup path exists
                        x:\path\to\WindowFXConfig.exe.bak
                */

                GenerateBackup( WFX_path_exe );

                /*
                    modify bytes for exe
                */

                double i_progress = 0;

                StatusBar.Update( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "01 00 00 00 00 00 00 C0 4D 00 00 04 00 00 4E BB 4E 00 02 00 40 81 00 00 10 00 00 10 00 00 00 00 10 00 00 10 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 E4 6A 3C 00 F4 01 00 00 00 30 3F 00 F4 84 0A 00 00 00 00 00 00 00 00 00 00 86 4C 00 88 61 01 00 00 C0 49 00 18 F4 03 00 B0 33 31 00 1C 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 20 31 00 B0 0E 00 00",
                    "01 00 00 00 00 00 00 C0 4D 00 00 04 00 00 00 00 00 00 02 00 40 81 00 00 10 00 00 10 00 00 00 00 10 00 00 10 00 00 00 00 00 00 10 00 00 00 00 00 00 00 00 00 00 00 E4 6A 3C 00 F4 01 00 00 00 30 3F 00 F4 84 0A 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 C0 49 00 18 F4 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "FF FF 6A 01 50 E8 6D 60 02 00 83 C4 14 85 C0 78 17 83 BD 58 FF FF FF 0A 7E 0E 33 C0 83 BD",
                    "FF FF 6A 01 50 E8 6D 60 02 00 83 C4 14 85 C0 78 17 C7 85 58 FF FF FF 0B 00 00 00 90 83 BD"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "45 FC C9 C3 55 8B EC 83 EC 7C A1 D8 FA",
                    "45 FC C9 C3 B8 01 00 00 00 C3 A1 D8 FA"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "85 C9 74 06 C7 01 01 00 00 00 83 C0 10",
                    "85 C9 74 06 C7 01 0E 00 00 00 83 C0 10"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "6A 6C 66 89 45 D4 58 6A 69 66 89 45 D8",
                    "6A 6C 66 89 45 D4 58 6A 6F 66 89 45 D8"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "E8 78 08 FF FF 85 C0 75 32 6A 10 68 E4",
                    "E8 78 08 FF FF 85 C0 EB 32 6A 10 68 E4"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "06 FF FF 85 C0 75 54 6A 10 68 E4 14 72",
                    "06 FF FF 85 C0 EB 54 6A 10 68 E4 14 72"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                ModifyBytes(
                    WFX_path_exe,
                    "33 73 FE FF 85 C0 74 30 6A 10 68 E4 14",
                    "33 73 FE FF 85 C0 EB 30 6A 10 68 E4 14"
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                /*
                    Extract modified dll and exe files
                */

                File.WriteAllBytes( Cfg.Default.app_res_file_1, Res.wfx4    );


                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    "Action 1",
                    "1",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    "Action 2",
                    "2",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                File.WriteAllBytes( Cfg.Default.app_res_file_2, Res.wfx4_64 );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_3, Res.wfx32   );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    "Action 3",
                    "3",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );


                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_4, Res.wfx64   );

                i_progress = 100.0;
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    "Action 4",
                    "4",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                /*
                    Move > File 1
                */

                string file_1_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_1 );
                string file_1_sha   = Hash.GetSHA256Hash( file_1_src );

                            GenerateBackup              ( file_1_src, true );

                            /*
                                Move > File 1 > Hash
                            */

                            wl                          ( "" );
                            wl                          ( String.Format( "[ File 1 ]: SHA256*           {0}", file_1_sha ) );
                            wl                          ( String.Format( "[ File 1 ]: SHA256            {0}", Cfg.Default.app_res_hash_1 ) );

                            if ( file_1_sha == Cfg.Default.app_res_hash_1 )
                            {
                                wl                      ( String.Format( "[ File 1 ]: Skip              " ) );
                            }
                            else
                            {

                                bRequireEdit    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_1 ) )
                                {
                                    try
                                    {
                                        wl              ( String.Format( "[ File 1 ]: File.Delete       {0}", file_1_src ) );
                                        File.Delete     ( file_1_src );
                                    }
                                    catch ( Exception e )
                                    {
                                        wl              ( String.Format( "[ File 1 ]: File.Delete       [Failure]: {0}", file_1_src , e.Message ) );

                                        string psq_delete = "Remove-Item -Path \"" + file_1_src + "\" -Force";
                                        wl               ( String.Format( "[ File 1 ]: PSQuery           {0}", psq_delete ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }
                                }

                                wl                      ( String.Format( "[ File 1 ]: Move-Start        {0}", file_1_src ) );
                                File.Move               ( Cfg.Default.app_res_file_1, file_1_src );
                                wl                      ( String.Format( "[ File 1 ]: Move-Complete     {0}", file_1_src ) );
                            }

                /*
                    Move > File 2
                */

                string file_2_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_2 );
                string file_2_sha   = Hash.GetSHA256Hash( file_2_src );

                            GenerateBackup              ( file_2_src, true );

                            /*
                                Move > File 2 > Hash
                            */

                            wl                          ( "" );
                            wl                          ( String.Format( "[ File 2 ]: SHA256*           {0}", file_2_sha ) );
                            wl                          ( String.Format( "[ File 2 ]: SHA256            {0}", Cfg.Default.app_res_hash_2 ) );

                            if ( file_2_sha == Cfg.Default.app_res_hash_2 )
                            {
                                wl                      ( String.Format( "[ File 2 ]: Skip              " ) );
                            }
                            else
                            {

                                bRequireEdit    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_2 ) )
                                {
                                    try
                                    {
                                        wl              ( String.Format( "[ File 2 ]: File.Delete       {0}", file_2_src ) );
                                        File.Delete     ( file_2_src );
                                    }
                                    catch ( Exception e )
                                    {
                                        wl              ( String.Format( "[ File 2 ]: File.Delete       [Failure]: {0}", file_2_src , e.Message ) );

                                        string psq_delete = "Remove-Item -Path \"" + file_2_src + "\" -Force";
                                        wl               ( String.Format( "[ File 2 ]: PSQuery           {0}", psq_delete ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }

                                }

                                wl                      ( String.Format( "[ File 2 ]: Move-Start        {0}", file_2_src ) );
                                File.Move               ( Cfg.Default.app_res_file_2, file_2_src );
                                wl                      ( String.Format( "[ File 2 ]: Move-Complete     {0}", file_2_src ) );
                            }

                /*
                    Move > File 3
                */

                string file_3_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_3 );
                string file_3_sha   = Hash.GetSHA256Hash( file_3_src );

                            GenerateBackup              ( file_3_src, true );

                            /*
                                Move > File 3 > Hash
                            */

                            wl                          ( "" );
                            wl                          ( String.Format( "[ File 3 ]: SHA256*           {0}", file_3_sha ) );
                            wl                          ( String.Format( "[ File 3 ]: SHA256            {0}", Cfg.Default.app_res_hash_3 ) );

                            if ( file_3_sha == Cfg.Default.app_res_hash_3 )
                            {
                                wl                      ( String.Format( "[ File 3 ]: Skip              " ) );
                            }
                            else
                            {

                                bRequireEdit    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_3 ) )
                                {
                                    try
                                    {
                                        wl              ( String.Format( "[ File 3 ]: File.Delete       {0}", file_3_src ) );
                                        File.Delete     ( file_3_src );
                                    }
                                    catch ( Exception e )
                                    {
                                        wl              ( String.Format( "[ File 3 ]: File.Delete       [Failure] {0}", file_3_src , e.Message ) );

                                        string psq_delete = "Remove-Item -Path \"" + file_3_src + "\" -Force";
                                        wl               ( String.Format( "[ File 3 ]: PSQuery           {0}", psq_delete ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }

                                }

                                wl                      ( String.Format( "[ File 3 ]: Move-Start        {0}", file_3_src ) );
                                File.Move               ( Cfg.Default.app_res_file_3, file_3_src );
                                wl                      ( String.Format( "[ File 3 ]: Move-Complete     {0}", file_3_src ) );
                            }

                /*
                    Move > File 4
                */

                string file_4_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_4 );
                string file_4_sha   = Hash.GetSHA256Hash( file_4_src );

                            GenerateBackup              ( file_4_src, true );

                            /*
                                Move > File 4 > Hash
                            */

                            wl                          ( "" );
                            wl                          ( String.Format( "[ File 4 ]: SHA256*           {0}", file_4_sha ) );
                            wl                          ( String.Format( "[ File 4 ]: SHA256            {0}", Cfg.Default.app_res_hash_4 ) );

                            if ( file_4_sha == Cfg.Default.app_res_hash_4 )
                            {
                                wl                      ( String.Format( "[ File 4 ]: Skip              " ) );
                            }
                            else
                            {

                                bRequireEdit    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_4 ) )
                                {
                                    try
                                    {
                                        wl              ( String.Format( "[ File 4 ]: File.Delete       {0}", file_4_src ) );
                                        File.Delete     ( file_4_src );
                                    }
                                    catch ( Exception e )
                                    {
                                        wl              ( String.Format( "[ File 4 ]: File.Delete       [Failure]: {0}", file_4_src , e.Message ) );

                                        string psq_delete = "Remove-Item -Path \"" + file_4_src + "\" -Force";
                                        wl               ( String.Format( "[ File 4 ]: PSQuery           {0}", psq_delete ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }

                                }

                                wl                      ( String.Format( "[ File 4 ]: Move-Start        {0}", file_4_src ) );
                                File.Move               ( Cfg.Default.app_res_file_4, file_4_src );
                                wl                      ( String.Format( "[ File 4 ]: Move-Complete     {0}", file_4_src ) );
                            }
                            wl                          ( "" );

                /*
                    Restart Service
                */

                string wfx_service_restart = "WindowFX";
                StatusBar.Update( string.Format( Res.status_service_restart_begin, wfx_service_restart ) );
                Helpers.RestartService( wfx_service_restart, 500 );

                /*
                    launch WindowFX
                */

                if ( !String.IsNullOrEmpty( WFX_path_fol ) )
                {
                    if ( File.Exists( WFX_path_exe ) )
                    {
                        wl( String.Format( "Patch: [Launch]: {0}", WFX_path_exe ) );
                        System.Diagnostics.Process.Start( WFX_path_exe );
                        StatusBar.Update( string.Format( Res.status_launch_app, Cfg.Default.app_name ) );
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

            if( !bRequireEdit )
            {

                string app_path_full    = Helpers.FindApp( );
                string app_path_dir     = Path.GetDirectoryName( app_path_full );
                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    string.Format( Res.msgbox_patchdll_sha256_nomove_msg, app_path_dir ),
                    Res.msgbox_patchdll_sha256_nomove_title,
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );

                StatusBar.Update( Res.status_patch_aborted );
                return;
            }

            MessageBox.Show
            (
                new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                string.Format( "{0}", Res.msgbox_patch_compl_msg ),
                Res.msgbox_patch_compl_title,
                MessageBoxButtons.OK, MessageBoxIcon.Information
            );

            StatusBar.Update( Res.status_patch_complete );
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
                }
                catch ( Exception e )
                {
                    wl( String.Format( "Hex Dump [Exception]: {0} - {1} ", exe, e.Message ) );
                }
                finally
                {
                    wl( String.Format( "Hex Dump [Complete]: {0}", exe ) );
                }
            }

            wl( String.Format( "[ ModifyBytes ]: Open         {0}", exe ) );
            wl( String.Format( "[ ModifyBytes ]: Write        {0} ->\n                              {1}", a, b ) );

            // bytes will be separated by a hyphen -, remove hyphen
            string[] hex_patch          = hex_result.Split(' ');
            byte[] bytes_modified       = new byte[ hex_patch.Length ];

            for ( int i = 0 ; i < hex_patch.Length ; i++ )
            {
                bytes_modified [ i ]    = Convert.ToByte( hex_patch [ i ], 16 );
            }

            File.WriteAllBytes( exe, bytes_modified );

            wl( String.Format( "[ ModifyBytes ]: Save         {0}", exe ) );
            wl( "" );

        }

        /*
            Generate Backup

                takes source file and checks to see if src.bak exists.
                creates a new src.bak file if one doesnt exist.

            @arg        : str src
                          the file to be backed up
                          full path  x:\path\to\target_file.exe || dll

            @ret        : void
        */

        public void GenerateBackup( string src, bool bMove = false )
        {

            string dest_bak         = src + ".bak";

            /*
                .bak file exists already
            */

            if ( File.Exists( dest_bak ) )
            {
                    MessageBox.Show
                    (
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( ".bak backup file already exists \n\n{0}", dest_bak ),
                        "Debug: Found Existing Backup",
                        MessageBoxButtons.OK, MessageBoxIcon.None
                    );
                
            }
            else
            {


                MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    dest_bak,
                    "Ran Copy",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );


                /*
                    .bak doesnt exist
                */

                File.Copy( src, dest_bak );
            }

            string psq_var          = "$user_current = $env:username";

            string psq_takeown1     = "takeown /f \"" + dest_bak + "\" y";
            string psq_icalcs1      = "icacls \"" + dest_bak + "\" /grant \"${user_current}:F\" /C /L";

            string psq_takeown2     = "takeown /f \"" + src + "\" y";
            string psq_icalcs2      = "icacls \"" + src + "\" /grant \"${user_current}:F\" /C /L";

            /*
                run powershell commands to adjust permissions
            */

            using ( PowerShell ps = PowerShell.Create( ) )
            {

                ps.AddScript( psq_var );

                ps.AddScript( psq_takeown1 );
                ps.AddScript( psq_icalcs1 );

                ps.AddScript( psq_takeown2 );
                ps.AddScript( psq_icalcs2 );

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

            /*
                SET     set attributes on src_app.exe
                COPY    src_app.exe -> src_app.exe.bak
                SET     set attributes on src_app.exe.bak
            */

            if ( File.Exists ( src ) )
                File.SetAttributes      ( src,          FileAttributes.Normal );

            if ( File.Exists ( dest_bak ) )
                File.SetAttributes      ( dest_bak,     FileAttributes.Normal );

            /*
                Backup complete
            */

            StatusBar.Update( string.Format( Res.status_bak_create, dest_bak ) );

            if ( AppInfo.bIsDebug( ) )
            {
                MessageBox.Show
                ( 
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    String.Format( "Backup Complete:\n src: {0}\nBak: {1}", src, dest_bak ),
                    "Backup Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }
        }

        /*
            Block Host

                A two-part function which
                    - Adds entries to the Windows host file
                    - Adds new Windows firewall rules to block the executable from communicating.

            @arg        : void
            @ret        : void
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
                    string.Format( Res.msgbox_bhost_success_msg, hostfile_path ),
                    Res.msgbox_bhost_success_title,
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
                    string.Format( Res.msgbox_bhost_fw_badpath_msg, app_path_exe ),
                    Res.msgbox_bhost_fw_badpath_title,
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
                                string.Format( Res.msgbox_debug_ps_bhost_qry_ok_msg, fwl_rule_block_in, fwl_rule_block_out ),
                                Res.msgbox_debug_ps_bhost_qry_ok_title,
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
                            string.Format( Res.msgbox_debug_ps_bhost_qry_alert_msg ),
                            Res.msgbox_debug_ps_bhost_qry_alert_title,
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
                string.Format( Res.msgbox_bhost_fw_success_msg, app_path_exe ),
                Res.msgbox_bhost_fw_success_title,
                MessageBoxButtons.OK, MessageBoxIcon.None
            );
        }
    }
}

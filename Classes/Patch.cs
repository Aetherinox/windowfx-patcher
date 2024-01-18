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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WFXPatch
{

    /*
        Class > Patch
    */

    class Patch
    {

        #region "Fileinfo"

            /*
                Define > File Name
            */

            readonly static string log_file = "Patch.cs";

        #endregion

        /*
            Define > Dependency Classes
        */

        private Helpers Helpers             = new Helpers( );
        private Perms Perms                 = new Perms( );
        private AppInfo AppInfo             = new AppInfo( );

        /*
            Define > Misc
        */

        readonly static Action<string> wl   = Console.WriteLine;

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

            wl( "" );

            /*
                Check skip
            */

            bool bFilesModified = false;

            /*
                loop each dll path

                    path_exe returns full path to program exe to back up
                    ->  Stardock\WindowFX\app_target_exe.exe
            */

            foreach ( string WFX_path_exe in paths_arr )
            {

                string WFX_path_fol     = Path.GetDirectoryName( WFX_path_exe );

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

                File.WriteAllBytes( Cfg.Default.app_res_file_1, Res.wfx4 );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_2, Res.wfx4_64 );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_3, Res.wfx32   );

                i_progress += Math.Round( 8.0 );
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_4, Res.wfx64   );

                i_progress = 100.0;
                StatusBar.Update    ( string.Format( Res.status_byte_modify, i_progress ) );
                Interface.Progress  ( Convert.ToInt32( i_progress ) );

                /*
                    Move > File 1

                        wfx4.dll

                        OpCodes.LDC:i4 1593
                */

                wl( "" );

                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_1 ) );

                string file_1_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_1 );
                string file_1_sha   = Hash.GetSHA256Hash( file_1_src );

                            GenerateBackup              ( file_1_src, true );

                            /*
                                Move > File 1 > Hash
                            */

                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", file_1_sha ) );
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", Cfg.Default.app_res_hash_1 ) );

                            if ( file_1_sha == Cfg.Default.app_res_hash_1 )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_1, file_1_src ) );
                            }
                            else
                            {
                                bFilesModified    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_1 ) )
                                {
                                    try
                                    {
                                        File.Delete     ( file_1_src );
                                        Log.Send        ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", file_1_src ) );
                                    }
                                    catch ( Exception e )
                                    {
                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", file_1_src ) );
                                        Log.Send( "", 0, "", String.Format( "{0}", e.Message ) );

                                        string[] psq_delete = { "Remove-Item -Path \"" + file_1_src + "\" -Force" };

                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete  ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }
                                }

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Begin", String.Format( "{0}", file_1_src ) );
                                File.Move( Cfg.Default.app_res_file_1, file_1_src );
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Finish", String.Format( "{0}", file_1_src ) );
                            }

                /*
                    Move > File 2

                        wfx4_64.dll

                        OpCodes.LDC:i4 1602

                */

                wl( "" );

                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_2 ) );

                string file_2_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_2 );
                string file_2_sha   = Hash.GetSHA256Hash( file_2_src );

                            GenerateBackup              ( file_2_src, true );

                            /*
                                Move > File 2 > Hash
                            */

                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", file_2_sha ) );
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", Cfg.Default.app_res_hash_2 ) );

                            if ( file_2_sha == Cfg.Default.app_res_hash_2 )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_2, file_2_src ) );
                            }
                            else
                            {
                                bFilesModified    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_2 ) )
                                {
                                    try
                                    {
                                        File.Delete     ( file_2_src );
                                        Log.Send        ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", file_2_src ) );
                                    }
                                    catch ( Exception e )
                                    {
                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", file_2_src ) );
                                        Log.Send( "", 0, "", String.Format( "{0}", e.Message ) );

                                        string[] psq_delete = { "Remove-Item -Path \"" + file_2_src + "\" -Force" };

                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete  ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }
                                }

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), " [ File.Move ] Begin", String.Format( "{0}", file_2_src ) );
                                File.Move( Cfg.Default.app_res_file_2, file_2_src );
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Finish", String.Format( "{0}", file_2_src ) );
                            }

                /*
                    Move > File 3

                        wfx32.exe

                        OpCodes.LDC:i4 1571

                */

                wl( "" );

                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_3 ) );

                string file_3_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_3 );
                string file_3_sha   = Hash.GetSHA256Hash( file_3_src );

                            GenerateBackup              ( file_3_src, true );

                            /*
                                Move > File 3 > Hash
                            */

                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", file_3_sha ) );
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", Cfg.Default.app_res_hash_3 ) );

                            if ( file_3_sha == Cfg.Default.app_res_hash_3 )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_3, file_3_src ) );
                            }
                            else
                            {
                                bFilesModified    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_3 ) )
                                {
                                    try
                                    {
                                        File.Delete     ( file_3_src );
                                        Log.Send        ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", file_3_src ) );
                                    }
                                    catch ( Exception e )
                                    {
                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", file_3_src ) );
                                        Log.Send( "", 0, "", String.Format( "{0}", e.Message ) );

                                        string[] psq_delete = { "Remove-Item -Path \"" + file_3_src + "\" -Force" };

                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete  ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }
                                }

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Begin", String.Format( "{0}", file_3_src ) );
                                File.Move( Cfg.Default.app_res_file_3, file_3_src );
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Finish", String.Format( "{0}", file_3_src ) );
                            }

                /*
                    Move > File 4

                        wfx64.exe

                        OpCodes.LDC:i4 1582

                */

                wl( "" );

                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_4 ) );

                string file_4_src   = Path.Combine( WFX_path_fol, Cfg.Default.app_res_file_4 );
                string file_4_sha   = Hash.GetSHA256Hash( file_4_src );

                            GenerateBackup              ( file_4_src, true );

                            /*
                                Move > File 4 > Hash
                            */

                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", file_4_sha ) );
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", Cfg.Default.app_res_hash_4 ) );

                            if ( file_4_sha == Cfg.Default.app_res_hash_4 )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_4, file_4_src ) );
                            }
                            else
                            {
                                bFilesModified    = true;

                                if ( File.Exists( Cfg.Default.app_res_file_4 ) )
                                {
                                    try
                                    {
                                        File.Delete     ( file_4_src );
                                        Log.Send        ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", file_4_src ) );
                                    }
                                    catch ( Exception e )
                                    {
                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", file_4_src ) );
                                        Log.Send( "", 0, "", String.Format( "{0}", e.Message ) );

                                        string[] psq_delete = { "Remove-Item -Path \"" + file_4_src + "\" -Force" };

                                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete  ) );
                                        Helpers.PowershellQ  ( psq_delete );
                                    }
                                }

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Start", String.Format( "{0}", file_4_src ) );
                                File.Move( Cfg.Default.app_res_file_4, file_4_src );
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Finish", String.Format( "{0}", file_4_src ) );
                            }

                /*
                    Wwomtrust.dll
                */

                string app_dll_wom_file     = "womtrust.dll";
                string app_dll_wom_pathto   = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Windows ), app_dll_wom_file );

                wl( "" );
                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_womdll ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_womdll, Res.womtrust );

                if ( File.Exists( Cfg.Default.app_res_file_womdll ) ) 
                {
                    if ( File.Exists( app_dll_wom_pathto ) )
                    {

                        /*
                            run powershell commands to adjust permissions
                        */

                        string[] psq_perms =
                        {
                            "$user_current = $env:username",
                            "takeown /f \"" + app_dll_wom_pathto + "\"",
                            "icacls \"" + app_dll_wom_pathto + "\" /grant \"${user_current}:F\" /C /L",
                        };

                        Helpers.PowershellQ( psq_perms );


                        File.SetAttributes( app_dll_wom_pathto,   FileAttributes.Normal );

                        /*
                            Wwomtrust.dll > SHA256
                        */

                        string app_dll_wom_sha256_src   = Hash.GetSHA256Hash( Cfg.Default.app_res_file_womdll );
                        string app_dll_wom_sha256_cur   = Hash.GetSHA256Hash( app_dll_wom_pathto );

                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", app_dll_wom_sha256_src ) );
                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", app_dll_wom_sha256_cur ) );

                        if ( app_dll_wom_sha256_src == app_dll_wom_sha256_cur )
                        {
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_womdll, app_dll_wom_pathto ) );
                        }
                        else
                        {

                            /*
                                Wwomtrust.dll > Delete
                            */

                            try
                            {
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", app_dll_wom_pathto ) );
                                File.Delete ( app_dll_wom_pathto );
                            }
                            catch ( Exception e )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", app_dll_wom_pathto ) );
                                Log.Send( " ", 0, " ", String.Format( "{0}", e.Message ) );

                                string[] psq_delete = { "Remove-Item -Path \"" + app_dll_wom_pathto + "\" -Force" };

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete ) );
                                Helpers.PowershellQ( psq_delete );
                            }

                            /*
                                Wwomtrust.dll > Move
                            */

                            try
                            {
                                File.Move   ( Cfg.Default.app_res_file_womdll, app_dll_wom_pathto );
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] OK", String.Format( "{0}", app_dll_wom_pathto ) );
                            }
                            catch ( Exception e )
                            {
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Fail", String.Format( "{0}", app_dll_wom_pathto ) );
                                Log.Send    ( " ", 0, " ", String.Format( "{0}", e.Message ) );

                                MessageBox.Show
                                (
                                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                                    String.Format( Res.msgbox_wotrust_move_error_msg, app_dll_wom_file, app_dll_wom_pathto ),
                                    String.Format( Res.msgbox_wotrust_move_error_title, app_dll_wom_file ),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error
                                );
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Move   ( Cfg.Default.app_res_file_womdll, app_dll_wom_pathto );
                            Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] OK", String.Format( "{0}", app_dll_wom_pathto ) );
                        }
                        catch ( Exception e )
                        {
                            Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Fail", String.Format( "{0}", app_dll_wom_pathto ) );
                            Log.Send    ( " ", 0, " ", String.Format( "{0}", e.Message ) );

                            MessageBox.Show
                            (
                                new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                                String.Format( Res.msgbox_wotrust_move_error_msg, app_dll_wom_file, app_dll_wom_pathto ),
                                String.Format( Res.msgbox_wotrust_move_error_title, app_dll_wom_file ),
                                MessageBoxButtons.OK, MessageBoxIcon.Error
                            );
                        }
                    }
                }

                /*
                    Wwontrust.dll
                */

                string app_dll_won_file     = "wontrust.dll";
                string app_dll_won_pathto   = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.Windows ), app_dll_won_file );

                wl( "" );
                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ Process ]", String.Format( "{0}", Cfg.Default.app_res_file_wondll ) );

                File.WriteAllBytes( Cfg.Default.app_res_file_wondll, Res.wontrust );

                if ( File.Exists( Cfg.Default.app_res_file_wondll ) ) 
                {
                    if ( File.Exists( app_dll_won_pathto ) )
                    {

                        string[] psq_perms =
                        {
                            "$user_current = $env:username",
                            "takeown /f \"" + app_dll_won_pathto + "\"",
                            "icacls \"" + app_dll_won_pathto + "\" /grant \"${user_current}:F\" /C /L",
                        };

                        Helpers.PowershellQ( psq_perms );

                        File.SetAttributes  ( app_dll_won_pathto,   FileAttributes.Normal );

                        /*
                            Wwontrust.dll > SHA256
                        */

                        string app_dll_won_sha256_src   = Hash.GetSHA256Hash( Cfg.Default.app_res_file_wondll );
                        string app_dll_won_sha256_cur   = Hash.GetSHA256Hash( app_dll_won_pathto );

                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ] *", String.Format( "{0}", app_dll_won_sha256_src ) );
                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ SHA256 ]", String.Format( "{0}", app_dll_won_sha256_cur ) );

                        if ( app_dll_won_sha256_src == app_dll_won_sha256_cur )
                        {
                            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Skip", String.Format( "{0} -> {1}", Cfg.Default.app_res_file_wondll, app_dll_won_pathto ) );
                        }
                        else
                        {

                            /*
                                Wwontrust.dll > Delete
                            */

                            try
                            {
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] OK", String.Format( "{0}", app_dll_won_pathto ) );
                                File.Delete ( app_dll_won_pathto );
                            }
                            catch ( Exception e )
                            {
                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Delete ] Fail", String.Format( "{0}", app_dll_won_pathto ) );
                                Log.Send( " ", 0, " ", String.Format( "{0}", e.Message ) );

                                string[] psq_delete = { "Remove-Item -Path \"" + app_dll_won_pathto + "\" -Force" };

                                Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ PSQ.Delete ]", String.Format( "{0}", psq_delete ) );
                                Helpers.PowershellQ( psq_delete );
                            }

                            /*
                                Wwontrust.dll > Move
                            */

                            try
                            {
                                File.Move   ( Cfg.Default.app_res_file_wondll, app_dll_won_pathto );
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] OK", String.Format( "{0}", app_dll_won_pathto ) );
                            }
                            catch ( Exception e )
                            {
                                Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Fail", String.Format( "{0}", app_dll_won_pathto ) );
                                Log.Send    ( " ", 0, " ", String.Format( "{0}", e.Message ) );

                                MessageBox.Show
                                (
                                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                                    String.Format( Res.msgbox_wotrust_move_error_msg, app_dll_wom_file, app_dll_won_pathto ),
                                    String.Format( Res.msgbox_wotrust_move_error_title, app_dll_won_file ),
                                    MessageBoxButtons.OK, MessageBoxIcon.Error
                                );
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            File.Move   ( Cfg.Default.app_res_file_wondll, app_dll_won_pathto );
                            Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] OK", String.Format( "{0}", app_dll_won_pathto ) );
                        }
                        catch ( Exception e )
                        {
                            Log.Send    ( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ File.Move ] Fail", String.Format( "{0}", app_dll_won_pathto ) );
                            Log.Send    ( " ", 0, " ", String.Format( "{0}", e.Message ) );

                            MessageBox.Show
                            (
                                new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                                String.Format( Res.msgbox_wotrust_move_error_msg, app_dll_wom_file, app_dll_won_pathto ),
                                String.Format( Res.msgbox_wotrust_move_error_title, app_dll_won_file ),
                                MessageBoxButtons.OK, MessageBoxIcon.Error
                            );
                        }
                    }
                }

                wl( "" );

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

                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ App ] Launch:", String.Format( "{0}", WFX_path_exe ) );
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

            Cleanup( );

            /*
                Determines if the patch was installed and files were modified.
                This can fail if you have already installed the patch and the SHA256 hashes are the same.
            */

            if( !bFilesModified )
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
                    StreamWriter sw_o   = new StreamWriter( Cfg.Default.app_res_file_dmp_original );
                    sw_o.WriteLine      ( hex_replace );
                    sw_o.Close          ( );

                    StreamWriter sw_p   = new StreamWriter( Cfg.Default.app_res_file_dmp_patched );
                    sw_p.WriteLine      ( hex_result );
                    sw_p.Close          ( );
                }
                catch ( Exception e )
                {
                    Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "Hex Dumb Exception", String.Format( "{0} - {1}", exe, e.Message ) );
                }
                finally
                {
                    Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "Hex Dump Success", exe );
                }
            }

            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ ModifyBytes ] Open", String.Format( "{0}", exe ) );
            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ ModifyBytes ] Write", String.Format( "{0}", a ) );
            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), " ", String.Format( "{0}", b ) );

            // bytes will be separated by a hyphen -, remove hyphen
            string[] hex_patch          = hex_result.Split(' ');
            byte[] bytes_modified       = new byte[ hex_patch.Length ];

            for ( int i = 0 ; i < hex_patch.Length ; i++ )
            {
                bytes_modified [ i ]    = Convert.ToByte( hex_patch [ i ], 16 );
            }

            File.WriteAllBytes( exe, bytes_modified );

            Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ ModifyBytes ] Save", String.Format( "{0}", exe ) );
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
                if( AppInfo.bIsDebug( ) )
                {
                    MessageBox.Show
                    (
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( Res.msgbox_debug_backup_exists_msg, dest_bak ),
                        Res.msgbox_debug_backup_exists_title,
                        MessageBoxButtons.OK, MessageBoxIcon.None
                    );
                }
            }
            else
            {

                /*
                    .bak doesnt exist
                */

                File.Copy( src, dest_bak );
            }

            /*
                run powershell commands to adjust permissions
            */

            string[] psq_perms =
            {
                "$user_current = $env:username",
                "takeown /f \"" + dest_bak + "\"",
                "icacls \"" + dest_bak + "\" /grant \"${user_current}:F\" /C /L",
                "takeown /f \"" + src + "\"",
                "icacls \"" + src + "\" /grant \"${user_current}:F\" /C /L"
            };

            Helpers.PowershellQ  ( psq_perms );

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
                    String.Format( Res.msgbox_debug_backup_compl_msg, src, dest_bak ),
                    Res.msgbox_debug_backup_compl_title,
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

        /*
            Patch > Cleanup
        */

        public static void Cleanup( )
        {

            string[] app_files =
            {
                Cfg.Default.app_res_file_1,
                Cfg.Default.app_res_file_2,
                Cfg.Default.app_res_file_3,
                Cfg.Default.app_res_file_4,
                Cfg.Default.app_res_file_womdll,
                Cfg.Default.app_res_file_wondll,
            };

            foreach ( string file in app_files )
            {   
                if ( File.Exists( file ) )
                {
                    try
                    {
                        File.Delete( file );
                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ App.Cleanup ] Remove", String.Format( "{0}", file ) );
                    }
                    catch ( Exception e )
                    {
                        Log.Send( log_file, new System.Diagnostics.StackTrace( true ).GetFrame( 0 ).GetFileLineNumber( ), "[ App.Cleanup ] Remove", String.Format( "Exception: {0}", e.Message ) );
                    }
                }
            }
        }
    }
}

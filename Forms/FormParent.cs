/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WFXPatch.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using Lng = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;

namespace WFXPatch
{
    public partial class FormParent : Form, IReceiver
    {

        #region "Declarations"

            /*
                Define > Classes
            */

            private Patch Patch         = new Patch( );
            private Helpers Helpers     = new Helpers( );
            private AppInfo AppInfo     = new AppInfo( );

            /*
                Define > Internal > Helper
            */

            internal Helpers Helper
            {
                set     { Helpers = value;  }
                get     { return Helpers;   }
            }

            /*
                patch_launch_fullpath       : Full path to exe
                patch_launch_dir            : Directory only
                patch_launch_exe            : Patcher exe filename only
            */

            static private string patch_launch_fullpath = Process.GetCurrentProcess( ).MainModule.FileName;
            static private string patch_launch_dir      = Path.GetDirectoryName( patch_launch_fullpath );
            static private string patch_launch_exe      = Path.GetFileName( patch_launch_fullpath );

            /*
                Define > Mouse
            */

            private bool mouseDown;
            private Point lastLocation;

            /*
                Form > Register Object

                    used to show / hide form without creating a new instance

                @usage      : FormParent.Object = this;
            */

            private static FormParent Obj;

            public static FormParent Object
            {
                get
                {
                    if ( Obj == null )
                    {
                        Obj = new FormParent( );
                    }
                    return Obj;
                }
                set { Obj = value; }
            }

            /*
                Define > updates
            */

            private bool bUpdateAvailable = false;

            /*
                Manifest > Json
            */

            public class Manifest
            {
                public string version { get; set; }
                public string name { get; set; }
                public string author { get; set; }
                public string description { get; set; }
                public string url { get; set; }
                public string piv { get; set; }
                public string gpg { get; set; }
                public IList<string> products { get; set; }
            }

        #endregion

        #region "Main Window: Initialize"

        /*
            Frame > Parent
        */

            public FormParent( )
            {
                SetStyle( ControlStyles.OptimizedDoubleBuffer, true );
                InitializeComponent( );

                /*
                    Register Form Object
                */

                FormParent.Object           = this;

                /*
                    Initialize Receiver
                */

                StatusBar.InitializeRecv    ( this );
                Interface.InitializeRecv    ( this );

                /*
                    Renderers
                */

                this.status_Strip.Renderer  = new StatusBar_Renderer( );
                this.mnu_Main.Renderer      = new ToolStripProfessionalRenderer( new mnu_ColorTable( ) );

                /*
                    Product, trademark, etc.
                */

                string ver                  = AppInfo.ProductVersionCore.ToString( );
                string product              = AppInfo.Title;
                string tm                   = AppInfo.Trademark;

                /*
                    Form Control Buttons
                */

                btn_Close.Parent            = imgHeader;
                btn_Close.BackColor         = Color.Transparent;

                btn_Minimize.Parent         = imgHeader;
                btn_Minimize.BackColor      = Color.Transparent;

                /*
                    Headers
                */

                lbl_HeaderName.Parent       = imgHeader;
                lbl_HeaderName.BackColor    = Color.Transparent;
                lbl_HeaderName.Text         = product;

                lbl_HeaderSub.Parent        = imgHeader;
                lbl_HeaderSub.BackColor     = Color.Transparent;
                lbl_HeaderSub.Text          = "v" + ver + " by " + tm;

                /*
                    Richtext in body of interface
                */

                string l1                   = Lng.txt_intro_1;
                string l2                   = Lng.txt_intro_2;

                rtxt_Intro.Text             = "";

                rtxt_Intro.AppendText       ( l1 );
                rtxt_Intro.Select           ( 0, l1.Length );
                rtxt_Intro.SelectionColor   = Color.White;

                rtxt_Intro.AppendText       ( "\n\n" );

                rtxt_Intro.AppendText       ( l2 );
                rtxt_Intro.Select           ( l1.Length + 2, l2.Length );
                rtxt_Intro.SelectionColor   = Color.FromArgb( 31, 133, 222 );

                /*
                    Buttons
                */

                btn_Patch.Text              = Lng.btn_patch;
                btn_OpenFolder.Text         = Lng.btn_open_folder;

            }

            /*
                Frame > Parent > Load
            */

            private async void FormParent_Load( object sender, EventArgs e )
            {
                await Task.Run( ( ) => CheckUpdates( Cfg.Default.app_url_manifest ) );
                StatusBar.Update( string.Format( Lng.status_generate ) );
            }

            /*
                Task > Check for Updates

                    views the data stored at https://github.com/Aetherinox/windowfx-patcher/blob/master/Manifest/manifest.json
            */

            private async Task CheckUpdates( string uri )
            {
                try
                {
                    var webClient       = new WebClient( );
                    var json            = await webClient.DownloadStringTaskAsync( uri );

                    JavaScriptSerializer serializer     = new JavaScriptSerializer( ); 
                    Manifest manifest                   = serializer.Deserialize<Manifest>( json );

                    /*
                        Check if update is available for end-user
                    */

                    bool bUpdate        = AppInfo.UpdateAvailable( manifest.version );
                    string ver_curr     = AppInfo.PublishVersion;

                    /*
                        determines if the update notification appears
                    */

                    if ( bUpdate || AppInfo.bIsDebug( ) )
                        bUpdateAvailable = true;

                    /*
                        update checker
                    */

                    #pragma warning disable CS4014
                    Task.Factory.StartNew( () =>
                    {
                        if ( ( bUpdateAvailable && !Cfg.Default.bShowedUpdates ) )
                        {
                            Cfg.Default.bShowedUpdates = true;

                            var result = MessageBox.Show( new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen }, string.Format( Lng.msgbox_update_msg, manifest.version, Cfg.Default.app_name ),
                                string.Format( Lng.msgbox_update_title, ver_curr, manifest.version ),
                                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation
                            );

                            if ( result.ToString( ).ToLower( ) == "yes" )
                                System.Diagnostics.Process.Start( Cfg.Default.app_url_github + "/releases/" );
                        }
                     });
                }
                catch ( WebException e )
                {

                }
            }

        #endregion

        #region "Main Window: Control Buttons"

            /*
                Control Buttons
                    ->  Minimize
                    ->  Maximize
                    ->  Close

                Icons:  http://modernicons.io/segoe-mdl2/cheatsheet/
                Font:   Segoe MDL2 Assets
            */

            /*
                Window > Button > Minimize > Click
            */

            private void btn_Window_Minimize_Click( object sender, EventArgs e )
            {
                this.WindowState = FormWindowState.Minimized;
            }

            /*
                Window > Button > Minimize > Mouse Enter
            */

            private void btn_Window_Minimize_MouseEnter( object sender, EventArgs e )
            {
                btn_Minimize.ForeColor = Color.FromArgb( 243, 41, 101 );
            }

            /*
                Window > Button > Minimize > Mouse Leave
            */

            private void btn_Window_Minimize_MouseLeave( object sender, EventArgs e )
            {
                btn_Minimize.ForeColor = Color.FromArgb( 255, 255, 255 );
            }

            /*
                Window > Button > Close > Click
            */

            private void btn_Window_Close_Click( object sender, EventArgs e )
            {
                Application.Exit( );
            }

            /*
                Window > Button > Close > Mouse Enter
            */

            private void btn_Window_Close_MouseEnter( object sender, EventArgs e )
            {
                btn_Close.ForeColor = Color.FromArgb( 243, 41, 101 );
            }

            /*
                Window > Button > Close > Mouse Leave
            */

            private void btn_Window_Close_MouseLeave( object sender, EventArgs e )
            {
                btn_Close.ForeColor = Color.FromArgb( 255, 255, 255 );
            }

        #endregion

        #region "Main Window: Dragging"

            /*
                Main Form > Mouse Down
                deals with moving form around on screen
            */

            private void MainForm_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown       = true;
                lastLocation    = e.Location;
            }

            /*
                Main Form > Mouse Up
                deals with moving form around on screen
            */

            private void MainForm_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            /*
                Main Form > Mouse Move
                deals with moving form around on screen
            */

            private void MainForm_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        #endregion

        #region "Header"

        /*
            Header Image
        */

            private void imgHeader_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void imgHeader_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void imgHeader_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        /*
            Header > Name Label
        */

            private void lbl_HeaderName_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void lbl_HeaderName_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown = false;
            }

            private void lbl_HeaderName_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        /*
            Header > Sub Label
        */

            private void lbl_HeaderSub_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void lbl_HeaderSub_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown = false;
            }

            private void lbl_HeaderSub_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        #endregion

        #region "Top Menu"

            /*
                Top Menu > Color Overrides
            */

            public class mnu_ColorTable : ProfessionalColorTable
            {
                /*
                    Gets the starting color of the gradient used when
                    a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
                */

                public override Color MenuItemPressedGradientBegin => Color.FromArgb( 255, 55, 55, 55 );

                /*
                    Gets the end color of the gradient used when a top-level
                    System.Windows.Forms.ToolStripMenuItem is pressed.
                */

                public override Color MenuItemPressedGradientEnd => Color.FromArgb( 255, 55, 55, 55 );

                /*
                    Gets the border color to use with a
                    System.Windows.Forms.ToolStripMenuItem.
                */

                public override Color MenuItemBorder => Color.FromArgb( 0, 45, 45, 45 );

                /*
                    Gets the starting color of the gradient used when the
                    System.Windows.Forms.ToolStripMenuItem is selected.
                */

                public override Color MenuItemSelectedGradientBegin => Color.FromArgb( 255, 222, 31, 103 );

                /*
                    Gets the end color of the gradient used when the
                    System.Windows.Forms.ToolStripMenuItem is selected.
                */

                public override Color MenuItemSelectedGradientEnd => Color.FromArgb( 255, 222, 31, 103 );

                /*
                    Gets the solid background color of the
                    System.Windows.Forms.ToolStripDropDown.
                */

                public override Color ToolStripDropDownBackground => Color.FromArgb( 255, 40, 40, 40 );

                /*
                    Top Menu > Image > Start Gradient Color
                */

                public override Color ImageMarginGradientBegin => Color.FromArgb( 255, 222, 31, 103 );

                /*
                    Top Menu > Image > Middle Gradient Color
                */

                public override Color ImageMarginGradientMiddle => Color.FromArgb( 0, 222, 31, 103 );

                /*
                    Top Menu > Image > End Gradient Color
                */

                public override Color ImageMarginGradientEnd => Color.FromArgb( 0, 222, 31, 103 );

                /*
                    Top Menu > Shadow Effect
                */

                public override Color SeparatorDark => Color.FromArgb( 0, 255, 255, 255 );

                /*
                    Top Menu > Border Color
                */

                public override Color MenuBorder => Color.FromArgb( 0, 45, 45, 45 );

                /*
                     Top Menu > Item Hover BG Color
                 */

                public override Color MenuItemSelected => Color.FromArgb( 255, 222, 31, 103 );
            }

            /*
                Top Menu
            */

            private void mnu_Main_Paint( object sender, PaintEventArgs e )
            {
                Graphics g                  = e.Graphics;
                Color backColor             = Color.FromArgb( 35, 255, 255, 255 );
                var imgSize                 = mnu_Main.ClientSize;
                e.Graphics.FillRectangle( new SolidBrush( backColor ), 1, 1, imgSize.Width - 2, 1 );
                e.Graphics.FillRectangle( new SolidBrush( backColor ), 1, imgSize.Height - 2, imgSize.Width - 2, 1 );
            }

            private void mnu_Main_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown       = true;
                lastLocation    = e.Location;
            }

            private void mnu_Main_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void mnu_Main_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

            /*
                Top Menu > File > Exit
            */

            private void mnu_Sub_Exit_Click( object sender, EventArgs e )
            {
                Application.Exit( );
            }

            /*
                Top Menu > Contribute
            */

            private void mnu_Cat_Contribute_Click( object sender, EventArgs e )
            {
                this.Hide( );

                FormContribute to   = new FormContribute( );
                to.TopMost          = true;
                to.Show( );
            }

            /*
                Top Menu > Help > Github Updates
            */

            private void mnu_Sub_Updates_Click( object sender, EventArgs e )
            {
                System.Diagnostics.Process.Start( Cfg.Default.app_url_github );
            }

            /*
                Top Menu > Help > x509 Certificate Validation
            */

            private void mnu_Sub_Validate_Click( object sender, EventArgs e )
            {

                string exe_target = System.AppDomain.CurrentDomain.FriendlyName;
                if ( !File.Exists( exe_target ) )
                {

                    MessageBox.Show(
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( "Could not find executable's location. Aborting validation\n\nFilename: \n{0}", exe_target ),
                        "Integrity Check: Aborted",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );

                    return;
                }

                string x509_cert    = Helper.x509_Thumbprint( exe_target );

                /*
                    x509 certificate

                    Add integrity validation. Ensure the resource DLL has been signed by the developer,
                    otherwise cancel the patching step.
                */

                if ( x509_cert != "0" )
                {

                    /* certificate: resource file  signed */

                    if ( x509_cert.ToLower( ) == Cfg.Default.app_dev_piv_thumbprint.ToLower( ) )
                    {

                        /* certificate: resource file signed and authentic */

                        MessageBox.Show(
                            new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                            string.Format( "Successfully validated that this patch is authentic, continuing...\n\nCertificate Thumbprint: \n{0}", x509_cert ),
                            "Integrity Check Successful",
                            MessageBoxButtons.OK, MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        /* certificate: resource file signed but not by developer */

                        MessageBox.Show(
                            new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                            string.Format( "The fails associated to this patch have a signature, however, it is not by the developer who wrote the patch, aborting...\n\nCertificate Thumbprint: \n{0}", x509_cert ),
                            "Integrity Check Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error
                        );
                    }
                }
                else
                {
                    /* certificate: resource file not signed at all */

                    MessageBox.Show(
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( "The files for this activator are not signed and may be fake from another source. Files from this activator's developer will ALWAYS be signed.\n\nEnsure you downloaded this patch from the developer.\n\nFailed File(s):\n     {0}", exe_target ),
                        "Integrity Check Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                }
            }

            /*
                Top Menu > Separator
                Separates "Exit" from the other items in "About" dropdown.
            */

            private void mnu_Help_Sep_1_Paint( object sender, PaintEventArgs e )
            {
                ToolStripSeparator toolStripSeparator = (ToolStripSeparator)sender;

                int width           = toolStripSeparator.Width;
                int height          = toolStripSeparator.Height;
                Color backColor     = Color.FromArgb( 255, 222, 31, 103 );

                // Fill the background.
                e.Graphics.FillRectangle( new SolidBrush( backColor ), 0, 0, width, height );
            }

            /*
                Top Menu > Help > About
            */

            private void mnu_Sub_About_Click( object sender, EventArgs e )
            {
                this.Hide( );

                FormAbout to    = new FormAbout( );
                to.TopMost      = true;
                to.Show( );
            }

        #endregion

        #region "Body: Intro"

            private void lbl_intro_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void lbl_intro_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void lbl_intro_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

            private void rtxt_Intro_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void rtxt_Intro_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void rtxt_Intro_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        #endregion

        #region "Body: Patch Button"

            /*
                Button > Patch > Click
            */

            private async void btn_Patch_Click( object sender, EventArgs e )
            {
                prog_Bar1.Visible   = true;
                prog_Bar1.Value     = 0;

                StatusBar.Update( string.Format( Lng.status_patch_start ) );
                await Task.Run( ( ) => Patch.Start( ) );
            }

        #endregion

        #region "Body: Open Folder Button"

            /*
                Auto-detects which folder WindowFXConfig.exe is installed in and opens that folder
                in the user's File Explorer.
            */

            private void btn_OpenFolder_Click( object sender, EventArgs e )
            {
                string app_path_full      = Helper.FindApp( );

                if ( String.IsNullOrEmpty( app_path_full ) )
                {
                    MessageBox.Show(
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( Lng.msgbox_nolocopen_msg ),
                        Lng.msgbox_nolocopen_title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                /*
                    target directory
                */

                string app_path_dir       = System.IO.Path.GetDirectoryName( app_path_full );
                if ( Directory.Exists( app_path_dir ) )
                    Process.Start( "explorer.exe", app_path_dir );

                /*
                    cannot locate program. Open dialog in Program Files(86)
                */

                else
                {
                    string path_progfiles = Helpers.ProgramFiles( );
                    Process.Start( "explorer.exe", path_progfiles );

                    MessageBox.Show(
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        string.Format( Lng.msgbox_nolocopen_msg ),
                        Lng.msgbox_nolocopen_title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }

            }
        #endregion

        #region "Status Bar"

            /*
                status bar in lower part of the main interface.
                updated when certain actions are completed to inform the user.
            */

            /*
                Status Bar > Color Table (Override)
            */

            public class StatusBar_ClrTable : ProfessionalColorTable
            {
                public override Color StatusStripGradientBegin => Color.FromArgb( 35, 35, 35 );
                public override Color StatusStripGradientEnd => Color.FromArgb( 35, 35, 35 );
            }

            /*
                Status Bar > Renderer
                Override colors
            */

            public class StatusBar_Renderer : ToolStripProfessionalRenderer
            {
                public StatusBar_Renderer( )
                    : base( new StatusBar_ClrTable( ) ) { }

                protected override void OnRenderToolStripBorder( ToolStripRenderEventArgs e )
                {
                    if ( !( e.ToolStrip is StatusStrip ) )
                        base.OnRenderToolStripBorder( e );
                }
            }

            /*
                Statusbar > Paint
            */

            private void status_Strip_Paint( object sender, PaintEventArgs e )
            {
                Graphics g                  = e.Graphics;
                Color backColor             = Color.FromArgb( 35, 255, 255, 255 );
                var imgSize                 = status_Strip.ClientSize;
                e.Graphics.FillRectangle    ( new SolidBrush( backColor ), 1, 1, imgSize.Width - 2, 2 );
            }

            /*
                Statusbar > Mouse Actions
            */

            private void status_Strip_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown = true;
                lastLocation = e.Location;
            }

            private void status_Strip_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown = false;
            }

            private void status_Strip_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

            /*
                Receiver > Update Status
            */

            public void Status( string message )
            {
                lbl_StatusOutput.Text = message;
                status_Strip.Refresh( );
            }

        #endregion

            private void btn_HostView_Click( object sender, EventArgs e )
            {
                string etc_path = @"C:\Windows\System32\drivers\etc";
                Process.Start( "explorer.exe", etc_path );
            }

            /*
                Button > Block > On Click
            */

            private void btn_DoBlock_Click( object sender, EventArgs e )
            {
                var result = MessageBox.Show
                (
                    new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                    Lng.msgbox_bhost_msg,
                    Lng.msgbox_bhost_title,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                string answer = result.ToString( ).ToLower( );

                /*
                    Block Host > Cancel
                */

                if ( answer != "yes" )
                {
                    MessageBox.Show
                    (
                        new Form( ) { TopMost = true, TopLevel = true, StartPosition = FormStartPosition.CenterScreen },
                        Lng.msgbox_bhost_cancel_msg,
                        Lng.msgbox_bhost_cancel_title,
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );

                    return;
                }

                /*
                    Block Host > Continue
                */

                Patch.BlockHost( );
            }

        #region "Body: Group Boxes"

            /*
                 Group Box > Host Block
            */

            private void box_BlockGroup_1_Paint( object sender, PaintEventArgs e )
            {
                // border color
                Pen p = new Pen( Color.FromArgb( 75, 75, 75 ) );

                e.Graphics.DrawLine( p, 0, 1, 0, e.ClipRectangle.Height - 1 );                                                      // left
                e.Graphics.DrawLine( p, 1, 1, e.ClipRectangle.Width - 1, 1 );                                                       // top
                e.Graphics.DrawLine( p, e.ClipRectangle.Width - 1, 1, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1 );      // right
                e.Graphics.DrawLine( p, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1, 0, e.ClipRectangle.Height - 1 );     // bottom
            }

            private void box_BlockGroup_1_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown       = true;
                lastLocation    = e.Location;
            }

            private void box_BlockGroup_1_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void box_BlockGroup_1_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }


            private void box_BlockGroup_2_Paint( object sender, PaintEventArgs e )
            {
                // border color
                Pen p = new Pen( Color.FromArgb( 75, 75, 75 ) );

                e.Graphics.DrawLine( p, 0, 1, 0, e.ClipRectangle.Height - 1 );                                                      // left
                e.Graphics.DrawLine( p, 1, 1, e.ClipRectangle.Width - 1, 1 );                                                       // top
                e.Graphics.DrawLine( p, e.ClipRectangle.Width - 1, 1, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1 );      // right
                e.Graphics.DrawLine( p, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1, 0, e.ClipRectangle.Height - 1 );     // bottom
            }

            private void box_BlockGroup_2_MouseDown( object sender, MouseEventArgs e )
            {
                mouseDown       = true;
                lastLocation    = e.Location;
            }

            private void box_BlockGroup_2_MouseUp( object sender, MouseEventArgs e )
            {
                mouseDown       = false;
            }

            private void box_BlockGroup_2_MouseMove( object sender, MouseEventArgs e )
            {
                if ( mouseDown )
                {
                    this.Location = new Point(
                        ( this.Location.X - lastLocation.X ) + e.X,
                        ( this.Location.Y - lastLocation.Y ) + e.Y
                    );

                    this.Update( );
                }
            }

        #endregion

        #region "IReceiver"

            /*
                IReceiver > Progress Bar
            */

            public void Progressbar( int i )
            {
                prog_Bar1.Value = i;
            }

        #endregion


    }
}

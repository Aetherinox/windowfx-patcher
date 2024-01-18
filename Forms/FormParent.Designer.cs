using System.Drawing;
using System.Windows.Forms;
using System.Configuration;

namespace WFXPatch
{
    partial class FormParent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose( );
            }
            base.Dispose(disposing);
        }

        protected override bool ShowKeyboardCues => true;

        /*
            Main Form > Rectangle
        */

        protected override void OnPaint(PaintEventArgs e)
        {
            Pen clr_border = new Pen(Color.FromArgb(75, 75, 75));
            e.Graphics.DrawRectangle(clr_border, 0, 0, Width - 1, Height - 1);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( )
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormParent));
            this.btn_Minimize = new System.Windows.Forms.Label();
            this.btn_Close = new System.Windows.Forms.Label();
            this.lbl_HeaderName = new System.Windows.Forms.Label();
            this.mnu_Main = new System.Windows.Forms.MenuStrip();
            this.mnu_Cat_File = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Sub_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Cat_Contribute = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Cat_Help = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Sub_DebugLog = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Sub_Updates = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Sub_Validate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Help_Sep_1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_Sub_About = new System.Windows.Forms.ToolStripMenuItem();
            this.status_Strip = new System.Windows.Forms.StatusStrip();
            this.lbl_StatusOutput = new System.Windows.Forms.ToolStripStatusLabel();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.imgHeader = new System.Windows.Forms.PictureBox();
            this.lbl_HeaderSub = new System.Windows.Forms.Label();
            this.pnl_StatusParent = new System.Windows.Forms.Panel();
            this.box_BlockGroup_1 = new System.Windows.Forms.Panel();
            this.btn_HostView = new WFXPatch.AetherxButton();
            this.btn_DoBlock = new WFXPatch.AetherxButton();
            this.lbl_HostBlocker_Desc = new System.Windows.Forms.Label();
            this.lbl_Group_HostBlocker_Title = new System.Windows.Forms.Label();
            this.box_BlockGroup_2 = new System.Windows.Forms.Panel();
            this.lbl_Group_Patcher_Title = new System.Windows.Forms.Label();
            this.box_BlockGroup_2_Body = new System.Windows.Forms.Panel();
            this.rtxt_Intro = new WFXPatch.Controls.AetherxRTextBox();
            this.prog_Bar1 = new WFXPatch.AetherxProgress();
            this.btn_OpenFolder = new WFXPatch.AetherxButton();
            this.btn_Patch = new WFXPatch.AetherxButton();
            this.mnu_Main.SuspendLayout();
            this.status_Strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgHeader)).BeginInit();
            this.pnl_StatusParent.SuspendLayout();
            this.box_BlockGroup_1.SuspendLayout();
            this.box_BlockGroup_2.SuspendLayout();
            this.box_BlockGroup_2_Body.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Minimize
            // 
            this.btn_Minimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Minimize.Font = new System.Drawing.Font("Segoe MDL2 Assets", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Minimize.Location = new System.Drawing.Point(462, 8);
            this.btn_Minimize.Name = "btn_Minimize";
            this.btn_Minimize.Size = new System.Drawing.Size(13, 32);
            this.btn_Minimize.TabIndex = 8;
            this.btn_Minimize.Text = "";
            this.btn_Minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Minimize.Click += new System.EventHandler(this.btn_Window_Minimize_Click);
            this.btn_Minimize.MouseEnter += new System.EventHandler(this.btn_Window_Minimize_MouseEnter);
            this.btn_Minimize.MouseLeave += new System.EventHandler(this.btn_Window_Minimize_MouseLeave);
            // 
            // btn_Close
            // 
            this.btn_Close.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Close.Font = new System.Drawing.Font("Segoe MDL2 Assets", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Close.Location = new System.Drawing.Point(490, 7);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(24, 32);
            this.btn_Close.TabIndex = 9;
            this.btn_Close.Text = "";
            this.btn_Close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Close.Click += new System.EventHandler(this.btn_Window_Close_Click);
            this.btn_Close.MouseEnter += new System.EventHandler(this.btn_Window_Close_MouseEnter);
            this.btn_Close.MouseLeave += new System.EventHandler(this.btn_Window_Close_MouseLeave);
            // 
            // lbl_HeaderName
            // 
            this.lbl_HeaderName.AutoSize = true;
            this.lbl_HeaderName.Font = new System.Drawing.Font("Myriad Pro Light", 20F);
            this.lbl_HeaderName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(41)))), ((int)(((byte)(101)))));
            this.lbl_HeaderName.Location = new System.Drawing.Point(21, 23);
            this.lbl_HeaderName.Name = "lbl_HeaderName";
            this.lbl_HeaderName.Size = new System.Drawing.Size(217, 32);
            this.lbl_HeaderName.TabIndex = 5;
            this.lbl_HeaderName.Text = "WindowFX Patcher";
            this.lbl_HeaderName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderName_MouseClick);
            this.lbl_HeaderName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderName_MouseDown);
            this.lbl_HeaderName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderName_MouseMove);
            this.lbl_HeaderName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderName_MouseUp);
            // 
            // mnu_Main
            // 
            this.mnu_Main.AutoSize = false;
            this.mnu_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.mnu_Main.Dock = System.Windows.Forms.DockStyle.None;
            this.mnu_Main.GripMargin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.mnu_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Cat_File,
            this.mnu_Cat_Contribute,
            this.mnu_Cat_Help});
            this.mnu_Main.Location = new System.Drawing.Point(1, 100);
            this.mnu_Main.Name = "mnu_Main";
            this.mnu_Main.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.mnu_Main.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mnu_Main.Size = new System.Drawing.Size(528, 38);
            this.mnu_Main.TabIndex = 1;
            this.mnu_Main.Text = "menuStrip1";
            this.mnu_Main.Paint += new System.Windows.Forms.PaintEventHandler(this.mnu_Main_Paint);
            this.mnu_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mnu_Main_MouseDown);
            this.mnu_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mnu_Main_MouseMove);
            this.mnu_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mnu_Main_MouseUp);
            // 
            // mnu_Cat_File
            // 
            this.mnu_Cat_File.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.mnu_Cat_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Sub_Exit});
            this.mnu_Cat_File.ForeColor = System.Drawing.Color.White;
            this.mnu_Cat_File.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.mnu_Cat_File.Name = "mnu_Cat_File";
            this.mnu_Cat_File.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mnu_Cat_File.Size = new System.Drawing.Size(53, 34);
            this.mnu_Cat_File.Text = "File";
            // 
            // mnu_Sub_Exit
            // 
            this.mnu_Sub_Exit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.mnu_Sub_Exit.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnu_Sub_Exit.ForeColor = System.Drawing.Color.White;
            this.mnu_Sub_Exit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mnu_Sub_Exit.Name = "mnu_Sub_Exit";
            this.mnu_Sub_Exit.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.mnu_Sub_Exit.Size = new System.Drawing.Size(95, 21);
            this.mnu_Sub_Exit.Text = "Exit";
            this.mnu_Sub_Exit.Click += new System.EventHandler(this.mnu_Sub_Exit_Click);
            // 
            // mnu_Cat_Contribute
            // 
            this.mnu_Cat_Contribute.ForeColor = System.Drawing.Color.White;
            this.mnu_Cat_Contribute.Name = "mnu_Cat_Contribute";
            this.mnu_Cat_Contribute.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mnu_Cat_Contribute.Size = new System.Drawing.Size(92, 34);
            this.mnu_Cat_Contribute.Text = "Contribute";
            this.mnu_Cat_Contribute.Click += new System.EventHandler(this.mnu_Cat_Contribute_Click);
            // 
            // mnu_Cat_Help
            // 
            this.mnu_Cat_Help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Sub_DebugLog,
            this.mnu_Sub_Updates,
            this.mnu_Sub_Validate,
            this.mnu_Help_Sep_1,
            this.mnu_Sub_About});
            this.mnu_Cat_Help.ForeColor = System.Drawing.Color.White;
            this.mnu_Cat_Help.Name = "mnu_Cat_Help";
            this.mnu_Cat_Help.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.mnu_Cat_Help.Size = new System.Drawing.Size(60, 34);
            this.mnu_Cat_Help.Text = "Help";
            // 
            // mnu_Sub_DebugLog
            // 
            this.mnu_Sub_DebugLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.mnu_Sub_DebugLog.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnu_Sub_DebugLog.ForeColor = System.Drawing.Color.White;
            this.mnu_Sub_DebugLog.Name = "mnu_Sub_DebugLog";
            this.mnu_Sub_DebugLog.Size = new System.Drawing.Size(183, 22);
            this.mnu_Sub_DebugLog.Text = "Debug Log";
            this.mnu_Sub_DebugLog.Click += new System.EventHandler(this.mnu_Sub_DebugLog_Click);
            // 
            // mnu_Sub_Updates
            // 
            this.mnu_Sub_Updates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.mnu_Sub_Updates.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnu_Sub_Updates.ForeColor = System.Drawing.Color.White;
            this.mnu_Sub_Updates.Name = "mnu_Sub_Updates";
            this.mnu_Sub_Updates.Size = new System.Drawing.Size(183, 22);
            this.mnu_Sub_Updates.Text = "Check for Updates";
            this.mnu_Sub_Updates.Click += new System.EventHandler(this.mnu_Sub_Updates_Click);
            // 
            // mnu_Sub_Validate
            // 
            this.mnu_Sub_Validate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.mnu_Sub_Validate.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnu_Sub_Validate.ForeColor = System.Drawing.Color.White;
            this.mnu_Sub_Validate.Name = "mnu_Sub_Validate";
            this.mnu_Sub_Validate.Size = new System.Drawing.Size(183, 22);
            this.mnu_Sub_Validate.Text = "Validate Signature";
            this.mnu_Sub_Validate.Click += new System.EventHandler(this.mnu_Sub_Validate_Click);
            // 
            // mnu_Help_Sep_1
            // 
            this.mnu_Help_Sep_1.AutoSize = false;
            this.mnu_Help_Sep_1.BackColor = System.Drawing.Color.Black;
            this.mnu_Help_Sep_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.mnu_Help_Sep_1.Name = "mnu_Help_Sep_1";
            this.mnu_Help_Sep_1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.mnu_Help_Sep_1.Size = new System.Drawing.Size(180, 1);
            this.mnu_Help_Sep_1.Paint += new System.Windows.Forms.PaintEventHandler(this.mnu_Help_Sep_1_Paint);
            // 
            // mnu_Sub_About
            // 
            this.mnu_Sub_About.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.mnu_Sub_About.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.mnu_Sub_About.ForeColor = System.Drawing.Color.White;
            this.mnu_Sub_About.Name = "mnu_Sub_About";
            this.mnu_Sub_About.Size = new System.Drawing.Size(183, 22);
            this.mnu_Sub_About.Text = "About";
            this.mnu_Sub_About.Click += new System.EventHandler(this.mnu_Sub_About_Click);
            // 
            // status_Strip
            // 
            this.status_Strip.AutoSize = false;
            this.status_Strip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.status_Strip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.status_Strip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status_Strip.ForeColor = System.Drawing.Color.Red;
            this.status_Strip.GripMargin = new System.Windows.Forms.Padding(0);
            this.status_Strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbl_StatusOutput});
            this.status_Strip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.status_Strip.Location = new System.Drawing.Point(1, 0);
            this.status_Strip.Name = "status_Strip";
            this.status_Strip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.status_Strip.Size = new System.Drawing.Size(528, 30);
            this.status_Strip.SizingGrip = false;
            this.status_Strip.TabIndex = 0;
            this.status_Strip.Text = "statusStrip1";
            this.status_Strip.Paint += new System.Windows.Forms.PaintEventHandler(this.status_Strip_Paint);
            this.status_Strip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.status_Strip_MouseDown);
            this.status_Strip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.status_Strip_MouseMove);
            this.status_Strip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.status_Strip_MouseUp);
            // 
            // lbl_StatusOutput
            // 
            this.lbl_StatusOutput.ActiveLinkColor = System.Drawing.Color.White;
            this.lbl_StatusOutput.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbl_StatusOutput.ForeColor = System.Drawing.Color.White;
            this.lbl_StatusOutput.LinkVisited = true;
            this.lbl_StatusOutput.Margin = new System.Windows.Forms.Padding(8, 6, 0, 2);
            this.lbl_StatusOutput.Name = "lbl_StatusOutput";
            this.lbl_StatusOutput.Size = new System.Drawing.Size(96, 22);
            this.lbl_StatusOutput.Text = "Status Output";
            this.lbl_StatusOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
            this.fileToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 28);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            // 
            // aboutToolStripMenuItem2
            // 
            this.aboutToolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.aboutToolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem3});
            this.aboutToolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.aboutToolStripMenuItem2.Name = "aboutToolStripMenuItem2";
            this.aboutToolStripMenuItem2.Size = new System.Drawing.Size(52, 28);
            this.aboutToolStripMenuItem2.Text = "About";
            // 
            // aboutToolStripMenuItem3
            // 
            this.aboutToolStripMenuItem3.Name = "aboutToolStripMenuItem3";
            this.aboutToolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem3.Text = "About";
            // 
            // imgHeader
            // 
            this.imgHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.imgHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imgHeader.BackgroundImage")));
            this.imgHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgHeader.Location = new System.Drawing.Point(1, 1);
            this.imgHeader.Name = "imgHeader";
            this.imgHeader.Size = new System.Drawing.Size(528, 129);
            this.imgHeader.TabIndex = 32;
            this.imgHeader.TabStop = false;
            this.imgHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgHeader_MouseDown);
            this.imgHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imgHeader_MouseMove);
            this.imgHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgHeader_MouseUp);
            // 
            // lbl_HeaderSub
            // 
            this.lbl_HeaderSub.AutoSize = true;
            this.lbl_HeaderSub.Font = new System.Drawing.Font("Segoe UI", 9.2F);
            this.lbl_HeaderSub.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lbl_HeaderSub.Location = new System.Drawing.Point(24, 56);
            this.lbl_HeaderSub.Name = "lbl_HeaderSub";
            this.lbl_HeaderSub.Size = new System.Drawing.Size(51, 17);
            this.lbl_HeaderSub.TabIndex = 33;
            this.lbl_HeaderSub.Text = "Version";
            this.lbl_HeaderSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderSub_MouseDown);
            this.lbl_HeaderSub.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderSub_MouseMove);
            this.lbl_HeaderSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_HeaderSub_MouseUp);
            // 
            // pnl_StatusParent
            // 
            this.pnl_StatusParent.BackColor = System.Drawing.Color.Transparent;
            this.pnl_StatusParent.Controls.Add(this.status_Strip);
            this.pnl_StatusParent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_StatusParent.Location = new System.Drawing.Point(0, 450);
            this.pnl_StatusParent.Name = "pnl_StatusParent";
            this.pnl_StatusParent.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.pnl_StatusParent.Size = new System.Drawing.Size(530, 31);
            this.pnl_StatusParent.TabIndex = 22;
            // 
            // box_BlockGroup_1
            // 
            this.box_BlockGroup_1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.box_BlockGroup_1.Controls.Add(this.btn_HostView);
            this.box_BlockGroup_1.Controls.Add(this.btn_DoBlock);
            this.box_BlockGroup_1.Controls.Add(this.lbl_HostBlocker_Desc);
            this.box_BlockGroup_1.Controls.Add(this.lbl_Group_HostBlocker_Title);
            this.box_BlockGroup_1.Location = new System.Drawing.Point(18, 149);
            this.box_BlockGroup_1.Name = "box_BlockGroup_1";
            this.box_BlockGroup_1.Size = new System.Drawing.Size(494, 116);
            this.box_BlockGroup_1.TabIndex = 34;
            this.box_BlockGroup_1.Paint += new System.Windows.Forms.PaintEventHandler(this.box_BlockGroup_1_Paint);
            this.box_BlockGroup_1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_1_MouseDown);
            this.box_BlockGroup_1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_1_MouseMove);
            this.box_BlockGroup_1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_1_MouseUp);
            // 
            // btn_HostView
            // 
            this.btn_HostView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.btn_HostView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_HostView.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_HostView.FlatAppearance.BorderSize = 0;
            this.btn_HostView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_HostView.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_HostView.Location = new System.Drawing.Point(248, 68);
            this.btn_HostView.Name = "btn_HostView";
            this.btn_HostView.Size = new System.Drawing.Size(111, 29);
            this.btn_HostView.TabIndex = 35;
            this.btn_HostView.Text = "&View Host";
            this.btn_HostView.UseVisualStyleBackColor = false;
            this.btn_HostView.Click += new System.EventHandler(this.btn_HostView_Click);
            // 
            // btn_DoBlock
            // 
            this.btn_DoBlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.btn_DoBlock.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_DoBlock.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_DoBlock.FlatAppearance.BorderSize = 0;
            this.btn_DoBlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_DoBlock.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_DoBlock.Location = new System.Drawing.Point(132, 68);
            this.btn_DoBlock.Name = "btn_DoBlock";
            this.btn_DoBlock.Size = new System.Drawing.Size(111, 29);
            this.btn_DoBlock.TabIndex = 26;
            this.btn_DoBlock.Text = "&Block Host";
            this.btn_DoBlock.UseVisualStyleBackColor = false;
            this.btn_DoBlock.Click += new System.EventHandler(this.btn_DoBlock_Click);
            // 
            // lbl_HostBlocker_Desc
            // 
            this.lbl_HostBlocker_Desc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lbl_HostBlocker_Desc.Location = new System.Drawing.Point(7, 33);
            this.lbl_HostBlocker_Desc.Name = "lbl_HostBlocker_Desc";
            this.lbl_HostBlocker_Desc.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lbl_HostBlocker_Desc.Size = new System.Drawing.Size(484, 32);
            this.lbl_HostBlocker_Desc.TabIndex = 12;
            this.lbl_HostBlocker_Desc.Text = "Perform this step first to block WindowFX from communicating with your computer.." +
    "";
            // 
            // lbl_Group_HostBlocker_Title
            // 
            this.lbl_Group_HostBlocker_Title.AutoSize = true;
            this.lbl_Group_HostBlocker_Title.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Group_HostBlocker_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.lbl_Group_HostBlocker_Title.Location = new System.Drawing.Point(6, 7);
            this.lbl_Group_HostBlocker_Title.Name = "lbl_Group_HostBlocker_Title";
            this.lbl_Group_HostBlocker_Title.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lbl_Group_HostBlocker_Title.Size = new System.Drawing.Size(108, 26);
            this.lbl_Group_HostBlocker_Title.TabIndex = 11;
            this.lbl_Group_HostBlocker_Title.Text = "Host Blocker";
            // 
            // box_BlockGroup_2
            // 
            this.box_BlockGroup_2.BackColor = System.Drawing.Color.Transparent;
            this.box_BlockGroup_2.Controls.Add(this.lbl_Group_Patcher_Title);
            this.box_BlockGroup_2.Controls.Add(this.box_BlockGroup_2_Body);
            this.box_BlockGroup_2.Location = new System.Drawing.Point(18, 278);
            this.box_BlockGroup_2.Name = "box_BlockGroup_2";
            this.box_BlockGroup_2.Size = new System.Drawing.Size(494, 116);
            this.box_BlockGroup_2.TabIndex = 35;
            this.box_BlockGroup_2.Paint += new System.Windows.Forms.PaintEventHandler(this.box_BlockGroup_2_Paint);
            // 
            // lbl_Group_Patcher_Title
            // 
            this.lbl_Group_Patcher_Title.AutoSize = true;
            this.lbl_Group_Patcher_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.lbl_Group_Patcher_Title.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_Group_Patcher_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.lbl_Group_Patcher_Title.Location = new System.Drawing.Point(6, 7);
            this.lbl_Group_Patcher_Title.Name = "lbl_Group_Patcher_Title";
            this.lbl_Group_Patcher_Title.Padding = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.lbl_Group_Patcher_Title.Size = new System.Drawing.Size(72, 26);
            this.lbl_Group_Patcher_Title.TabIndex = 11;
            this.lbl_Group_Patcher_Title.Text = "Patcher";
            // 
            // box_BlockGroup_2_Body
            // 
            this.box_BlockGroup_2_Body.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.box_BlockGroup_2_Body.Controls.Add(this.rtxt_Intro);
            this.box_BlockGroup_2_Body.Dock = System.Windows.Forms.DockStyle.Fill;
            this.box_BlockGroup_2_Body.Location = new System.Drawing.Point(0, 0);
            this.box_BlockGroup_2_Body.Name = "box_BlockGroup_2_Body";
            this.box_BlockGroup_2_Body.Padding = new System.Windows.Forms.Padding(14, 36, 14, 10);
            this.box_BlockGroup_2_Body.Size = new System.Drawing.Size(494, 116);
            this.box_BlockGroup_2_Body.TabIndex = 12;
            this.box_BlockGroup_2_Body.Paint += new System.Windows.Forms.PaintEventHandler(this.box_BlockGroup_2_Paint);
            this.box_BlockGroup_2_Body.MouseDown += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_2_MouseDown);
            this.box_BlockGroup_2_Body.MouseMove += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_2_MouseMove);
            this.box_BlockGroup_2_Body.MouseUp += new System.Windows.Forms.MouseEventHandler(this.box_BlockGroup_2_MouseUp);
            // 
            // rtxt_Intro
            // 
            this.rtxt_Intro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.rtxt_Intro.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxt_Intro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxt_Intro.Enabled = false;
            this.rtxt_Intro.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtxt_Intro.ForeColor = System.Drawing.Color.White;
            this.rtxt_Intro.Location = new System.Drawing.Point(14, 36);
            this.rtxt_Intro.Name = "rtxt_Intro";
            this.rtxt_Intro.ReadOnly = true;
            this.rtxt_Intro.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtxt_Intro.Selectable = false;
            this.rtxt_Intro.ShortcutsEnabled = false;
            this.rtxt_Intro.Size = new System.Drawing.Size(466, 70);
            this.rtxt_Intro.TabIndex = 29;
            this.rtxt_Intro.Text = "This program will patch and activate WindowFX. Ensure that you install Stardock W" +
    "indowFX before running this patch.";
            this.rtxt_Intro.MouseDown += new System.Windows.Forms.MouseEventHandler(this.rtxt_Intro_MouseDown);
            this.rtxt_Intro.MouseMove += new System.Windows.Forms.MouseEventHandler(this.rtxt_Intro_MouseMove);
            this.rtxt_Intro.MouseUp += new System.Windows.Forms.MouseEventHandler(this.rtxt_Intro_MouseUp);
            // 
            // prog_Bar1
            // 
            this.prog_Bar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.prog_Bar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.prog_Bar1.ChannelHeight = 10;
            this.prog_Bar1.ForeColor = System.Drawing.Color.White;
            this.prog_Bar1.Location = new System.Drawing.Point(135, 406);
            this.prog_Bar1.Name = "prog_Bar1";
            this.prog_Bar1.ShowMaximun = false;
            this.prog_Bar1.ShowValue = WFXPatch.TextPosition.None;
            this.prog_Bar1.Size = new System.Drawing.Size(260, 24);
            this.prog_Bar1.SliderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.prog_Bar1.SliderHeight = 8;
            this.prog_Bar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prog_Bar1.SymbolAfter = "";
            this.prog_Bar1.SymbolBefore = "";
            this.prog_Bar1.TabBackColor = System.Drawing.Color.RoyalBlue;
            this.prog_Bar1.TabIndex = 36;
            this.prog_Bar1.Value = 64;
            this.prog_Bar1.Visible = false;
            // 
            // btn_OpenFolder
            // 
            this.btn_OpenFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.btn_OpenFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_OpenFolder.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_OpenFolder.FlatAppearance.BorderSize = 0;
            this.btn_OpenFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OpenFolder.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OpenFolder.Location = new System.Drawing.Point(401, 410);
            this.btn_OpenFolder.Name = "btn_OpenFolder";
            this.btn_OpenFolder.Size = new System.Drawing.Size(111, 29);
            this.btn_OpenFolder.TabIndex = 25;
            this.btn_OpenFolder.Text = "&Open Folder";
            this.btn_OpenFolder.UseVisualStyleBackColor = false;
            this.btn_OpenFolder.Click += new System.EventHandler(this.btn_OpenFolder_Click);
            // 
            // btn_Patch
            // 
            this.btn_Patch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(6)))), ((int)(((byte)(85)))));
            this.btn_Patch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Patch.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btn_Patch.FlatAppearance.BorderSize = 0;
            this.btn_Patch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Patch.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Patch.Location = new System.Drawing.Point(18, 410);
            this.btn_Patch.Name = "btn_Patch";
            this.btn_Patch.Size = new System.Drawing.Size(111, 29);
            this.btn_Patch.TabIndex = 5;
            this.btn_Patch.Text = "&Patch";
            this.btn_Patch.UseVisualStyleBackColor = false;
            this.btn_Patch.Click += new System.EventHandler(this.btn_Patch_Click);
            // 
            // FormParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(530, 481);
            this.Controls.Add(this.prog_Bar1);
            this.Controls.Add(this.box_BlockGroup_2);
            this.Controls.Add(this.box_BlockGroup_1);
            this.Controls.Add(this.pnl_StatusParent);
            this.Controls.Add(this.lbl_HeaderSub);
            this.Controls.Add(this.btn_OpenFolder);
            this.Controls.Add(this.lbl_HeaderName);
            this.Controls.Add(this.btn_Patch);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.btn_Minimize);
            this.Controls.Add(this.mnu_Main);
            this.Controls.Add(this.imgHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnu_Main;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormParent";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WindowFX Patcher";
            this.Load += new System.EventHandler(this.FormParent_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.mnu_Main.ResumeLayout(false);
            this.mnu_Main.PerformLayout();
            this.status_Strip.ResumeLayout(false);
            this.status_Strip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgHeader)).EndInit();
            this.pnl_StatusParent.ResumeLayout(false);
            this.box_BlockGroup_1.ResumeLayout(false);
            this.box_BlockGroup_1.PerformLayout();
            this.box_BlockGroup_2.ResumeLayout(false);
            this.box_BlockGroup_2.PerformLayout();
            this.box_BlockGroup_2_Body.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label btn_Minimize;
        private System.Windows.Forms.Label btn_Close;
        private System.Windows.Forms.Label lbl_HeaderName;
        private System.Windows.Forms.MenuStrip mnu_Main;
        private System.Windows.Forms.ToolStripMenuItem mnu_Cat_File;
        private System.Windows.Forms.ToolStripMenuItem mnu_Sub_Exit;
        private System.Windows.Forms.ToolStripMenuItem mnu_Cat_Help;
        private System.Windows.Forms.ToolStripMenuItem mnu_Sub_About;
        private System.Windows.Forms.StatusStrip status_Strip;
        private System.Windows.Forms.ToolStripStatusLabel lbl_StatusOutput;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem2;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private ToolStripMenuItem aboutToolStripMenuItem3;
        private AetherxButton btn_Patch;
        private AetherxButton btn_OpenFolder;
        private ToolStripMenuItem mnu_Sub_Updates;
        private ToolStripMenuItem mnu_Sub_Validate;
        private ToolStripSeparator mnu_Help_Sep_1;
        private WFXPatch.Controls.AetherxRTextBox rtxt_Intro;
        private ToolStripMenuItem mnu_Cat_Contribute;
        private PictureBox imgHeader;
        private Label lbl_HeaderSub;
        private Panel pnl_StatusParent;
        private Panel box_BlockGroup_1;
        private AetherxButton btn_HostView;
        private AetherxButton btn_DoBlock;
        private Label lbl_HostBlocker_Desc;
        private Label lbl_Group_HostBlocker_Title;
        private Panel box_BlockGroup_2;
        private Label lbl_Group_Patcher_Title;
        private Panel box_BlockGroup_2_Body;
        private AetherxProgress prog_Bar1;
        private ToolStripMenuItem mnu_Sub_DebugLog;
    }
}


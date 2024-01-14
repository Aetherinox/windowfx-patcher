﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFXPatch.Controls
{
    public partial class AetherxRTextBox : RichTextBox
    {

        public AetherxRTextBox()
        {
            Selectable = true;
        }
        const int WM_SETFOCUS = 0x0007;
        const int WM_KILLFOCUS = 0x0008;

        /*
            Enables or disables selection highlight. 
            If you set `Selectable` to `false` then the selection highlight will be disabled.

            enabled by default.
        */

        [DefaultValue(true)]
        public bool Selectable { get; set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETFOCUS && !Selectable)
            {
                m.Msg = WM_KILLFOCUS;

                // set cursor default instead of beam
                this.Cursor = Cursors.Default;
            }

            base.WndProc(ref m);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Makes the richtext background transparent
                CreateParams CP = base.CreateParams;
                CP.ExStyle |= 0x20;
                return CP;
            }
        }
    }
}
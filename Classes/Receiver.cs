/*
    @app        : WindowFX Patcher
    @repo       : https://github.com/Aetherinox/windowfx-patcher
    @author     : Aetherinox
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lng = WFXPatch.Properties.Resources;
using Cfg = WFXPatch.Properties.Settings;

namespace WFXPatch
{

    /*
        OReceiver > Status Bar
    */

    public interface IReceiver
    {
        void Status( string message );

        /*
            Progress bar
        */

        void Progressbar( int i );

    }

    /*
        IReceiver > Interface
    */

    public static class Interface
    {
        private static IReceiver recv = null;

        /*
            Receiver > Initialize
        */

        public static void InitializeRecv( IReceiver f )
        {
            recv = f;
        }

        /*
            Receiver > Send Message
        */

        public static void Progress( int i )
        {
            if ( recv != null ) recv.Progressbar( i );
        }

    }

    /*
        Receiver > Status Bar
    */

    public static class StatusBar
    {
        private static IReceiver recv = null;

        /*
            Receiver > Initialize
        */

        public static void InitializeRecv( IReceiver f )
        {
            recv = f;
        }

        /*
            Receiver > Send Message
        */

        public static void Update( string message )
        {
            if ( recv != null ) recv.Status( message );
        }
    }
}

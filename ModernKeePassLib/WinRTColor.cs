using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernKeePassLib.WinRTAdaptors
{
    // BERT TODO: Whole class is just a non-functional stub 

    public struct Color
    {
        public byte A
        {
            get
            { return 255; }
        }
        public byte R
        {
            get
            {
                return 100;
            }
        }
        public byte G
        {
            get { return 150; }
        }
        public byte B
        {
            get { return 200; }
        }
        public bool isEmpty
        {
            get { return false; }
        }


        public static Color Empty
        {
            get
            {
                Color abc;
                return abc; }
        }

    }
 
}

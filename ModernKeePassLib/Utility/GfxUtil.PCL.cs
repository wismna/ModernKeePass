using System.IO;
using Splat;

namespace ModernKeePassLib.Utility
{
    public class GfxUtil
    {
        public static IBitmap LoadImage(byte[] pb)
        {
            return null;
            //return ScaleImage(pb, null, null);
        }

        public static IBitmap ScaleImage(byte[] pb, int? w, int? h)
        {
            return null;
            /*using (var ms = new MemoryStream(pb, false))
            {
                return BitmapLoader.Current.Load(ms, w, h).Result;
            }*/
        }
    }
}

using System.IO;
using System.Threading.Tasks;
using Splat;

namespace ModernKeePassLib.Utility
{
    public class GfxUtil
    {
        public static async Task<IBitmap> LoadImage(byte[] pb)
        {
            return await ScaleImage(pb, null, null);
        }

        public static async Task<IBitmap> ScaleImage(byte[] pb, int? w, int? h)
        {
            using (var ms = new MemoryStream(pb, false))
            {
                return await BitmapLoader.Current.Load(ms, w, h);
            }
        }
    }
}

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage.Streams;
using ModernKeePassLib.Native;

namespace ModernKeePassLib.Cryptography
{
    public static class ProtectedData
    {
        public static async Task ProtectStream(byte[] buffer, IOutputStream stream)
        {
            //instantiate a DataProtectionProvider for decryption
            var dpp = new DataProtectionProvider("LOCAL=user");

            //Open a stream to load data in
            using (var inputStream = new InMemoryRandomAccessStream())
            {
                //cteate data writer to write data to the input stream
                using (var dw = new DataWriter(inputStream))
                {
                    //write data to the stream
                    dw.WriteBuffer(buffer.AsBuffer());
                    await dw.StoreAsync();

                    //encrypt the intput stream into the file stream
                    await dpp.ProtectStreamAsync(inputStream.GetInputStreamAt(0),
                        stream);
                }
            }
        }

        public static async Task<byte[]> UnprotectStream(IInputStream stream)
        {
            //instantiate a DataProtectionProvider for decryption
            var dpp = new DataProtectionProvider();

            //create a stream to decrypte the data to
            using (var outputStream = new InMemoryRandomAccessStream())
            {
                //decrypt the data
                await dpp.UnprotectStreamAsync(stream, outputStream);

                //fill the data reader with the content of the outputStream,
                //but from position 0
                using (var dr = new DataReader(outputStream.GetInputStreamAt(0)))
                {
                    //load data from the stream to the dataReader
                    await dr.LoadAsync((uint)outputStream.Size);

                    //load the data from the datareader into a buffer
                    IBuffer data = dr.ReadBuffer((uint)outputStream.Size);

                    return data.ToArray();
                }
            }
        }

        public static byte[] Unprotect(byte[] pbEnc, byte[] mPbOptEnt, DataProtectionScope currentUser)
        {
            throw new NotImplementedException();
        }

        public static byte[] Protect(byte[] pbPlain, byte[] mPbOptEnt, DataProtectionScope currentUser)
        {
            throw new NotImplementedException();
        }
    }
}

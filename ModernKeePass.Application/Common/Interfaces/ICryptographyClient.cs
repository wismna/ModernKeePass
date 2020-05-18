using System.Threading.Tasks;

namespace ModernKeePass.Application.Common.Interfaces
{
    public interface ICryptographyClient
    {
        Task<string> Protect(string value);
        Task<string> UnProtect(string value);
        byte[] Random(uint length);
    }
}
namespace ModernKeePass.Domain.Dtos
{
    public class Credentials
    {
        public string Password { get; set; }
        public byte[] KeyFileContents { get; set; }
        // TODO: add Windows Hello
    }
}
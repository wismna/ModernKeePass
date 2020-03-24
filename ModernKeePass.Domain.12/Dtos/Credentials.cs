namespace ModernKeePass.Domain.Dtos
{
    public class Credentials
    {
        public string Password { get; set; }
        public string KeyFilePath { get; set; }
        // TODO: add Windows Hello
    }
}
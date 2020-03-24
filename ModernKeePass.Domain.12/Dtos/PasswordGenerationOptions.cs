namespace ModernKeePass.Domain.Dtos
{
    public class PasswordGenerationOptions
    {
        public int PasswordLength { get; set; }
        public bool UpperCasePatternSelected { get; set; }
        public bool LowerCasePatternSelected { get; set; }
        public bool DigitsPatternSelected { get; set; }
        public bool SpecialPatternSelected { get; set; }
        public bool MinusPatternSelected { get; set; }
        public bool UnderscorePatternSelected { get; set; }
        public bool SpacePatternSelected { get; set; }
        public bool BracketsPatternSelected { get; set; }
        public string CustomChars { get; set; }
    }
}
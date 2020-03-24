using ModernKeePass.Application.Common.Interfaces;
using ModernKeePass.Domain.Dtos;
using ModernKeePassLib.Cryptography;
using ModernKeePassLib.Cryptography.PasswordGenerator;
using ModernKeePassLib.Keys;

namespace ModernKeePass.Infrastructure.KeePass
{
    public class KeePassPasswordClient: IPasswordProxy
    {
        public string GeneratePassword(PasswordGenerationOptions options)
        {
            var pwProfile = new PwProfile
            {
                GeneratorType = PasswordGeneratorType.CharSet,
                Length = (uint)options.PasswordLength,
                CharSet = new PwCharSet()
            };

            if (options.UpperCasePatternSelected) pwProfile.CharSet.Add(PwCharSet.UpperCase);
            if (options.LowerCasePatternSelected) pwProfile.CharSet.Add(PwCharSet.LowerCase);
            if (options.DigitsPatternSelected) pwProfile.CharSet.Add(PwCharSet.Digits);
            if (options.SpecialPatternSelected) pwProfile.CharSet.Add(PwCharSet.Special);
            if (options.MinusPatternSelected) pwProfile.CharSet.Add('-');
            if (options.UnderscorePatternSelected) pwProfile.CharSet.Add('_');
            if (options.SpacePatternSelected) pwProfile.CharSet.Add(' ');
            if (options.BracketsPatternSelected) pwProfile.CharSet.Add(PwCharSet.Brackets);

            pwProfile.CharSet.Add(options.CustomChars);

            PwGenerator.Generate(out var password, pwProfile, null, new CustomPwGeneratorPool());

            return password.ReadString();
        }

        public uint EstimatePasswordComplexity(string password)
        {
            return QualityEstimation.EstimatePasswordBits(password?.ToCharArray());
        }

        public byte[] GenerateKeyFile(byte[] additionalEntropy)
        {
            return KcpKeyFile.Create(additionalEntropy);
        }
    }
}
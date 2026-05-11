using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthOptions
{
    public const string ISSUER = "CRM.Identity"; // издатель токена
    public const string AUDIENCE = "CRM.Client"; // потребитель токена
    const string KEY = "your-super-secret-key-at-least-32-characters-long-here";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}


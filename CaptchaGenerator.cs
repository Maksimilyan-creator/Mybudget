using System;

namespace My_budget;

public class CaptchaGenerator
{
    public static string GenerateCaptcha()
    {
        Random random = new Random();
        const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] captchaChars = new char[4]; // Длина CAPTCHA текста - 4 символов
        for (int i = 0; i < captchaChars.Length; i++)
        {
            captchaChars[i] = characters[random.Next(characters.Length)];
        }
        return new string(captchaChars);
    }
}
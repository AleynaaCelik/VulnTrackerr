using OtpNet;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public static class TwoFactorAutchHelper
{
    /// <summary>
    /// Rastgele bir Secret Key oluşturur.
    /// </summary>
    public static string GenerateSecretKey()
    {
        return KeyGeneration.GenerateRandomKey(20);
    }

    /// <summary>
    /// Kullanıcı bilgisi ve secret key kullanarak OTP URI oluşturur.
    /// </summary>
    public static string GenerateOtpUri(string userEmail, string secretKey)
    {
        return new OtpUri(OtpType.Totp, secretKey, userEmail, "VulnTracker").ToString();
    }

    /// <summary>
    /// OTP URI'sine göre QR kodunu Bitmap olarak oluşturur.
    /// </summary>
    public static Bitmap GenerateQrCode(string otpUri)
    {
        var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(otpUri, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }

    /// <summary>
    /// QR kodunu Base64 string formatında oluşturur.
    /// </summary>
    public static string GenerateQrCodeAsBase64(string otpUri)
    {
        using (var bitmap = GenerateQrCode(otpUri)) // QR kodu Bitmap olarak oluştur
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png); // Bitmap'i PNG formatında MemoryStream'e kaydet
                byte[] qrCodeBytes = ms.ToArray(); // Byte array'e dönüştür
                return Convert.ToBase64String(qrCodeBytes); // Base64 string'e çevir
            }
        }
    }
}

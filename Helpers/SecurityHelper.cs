using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace ProjectFilm.Helpers
{
	public class SecurityHelper
	{
		public static string password = "aulj nrvl foqm zlmv";
		public static string myEmail = "taya13taya@gmail.com";

		public static string GenerateSalt(int nSalt)
		{
			var saltBytes = new byte[nSalt];
			using(var provider = new RNGCryptoServiceProvider())
			{
				provider.GetNonZeroBytes(saltBytes);
			}
			return Convert.ToBase64String(saltBytes);
		}

		public static string HashPassword(string password, string salt, int nIterations, int nHash)
		{
			var saltBytes = Convert.FromBase64String(salt);
			using(var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
			{
				return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
			}
		}

		public static string GenerateRandomCode(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}

		public static void SendRecoveryCode(string email, string recoveryCode)
		{
			SmtpClient smtpClient = new SmtpClient(myEmail);
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(myEmail, password);
			smtpClient.EnableSsl = true;
			MailMessage message = new MailMessage();
			message.From = new MailAddress(myEmail);
			message.To.Add(new MailAddress(email));
			message.Subject = "Password Recovery Code";
			message.Body = $"Your recovery code is: {recoveryCode}";
			smtpClient.Send(message);
		}

		public static void SendNewPassword(string email, string newPassword)
		{
			// Настройка SMTP-клиента
			SmtpClient smtpClient = new SmtpClient(myEmail);
			smtpClient.Port = 587;
			smtpClient.Credentials = new NetworkCredential(myEmail, password);
			smtpClient.EnableSsl = true;

			// Формирование сообщения
			MailMessage message = new MailMessage();
			message.From = new MailAddress(myEmail);
			message.To.Add(new MailAddress(email));
			message.Subject = "New Password";
			message.Body = $"Your new password is: {newPassword}";

			// Отправка сообщения
			smtpClient.Send(message);
		}

		public static string GenerateRandomPassword(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}

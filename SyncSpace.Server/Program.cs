using System.Security.Cryptography;
using System.Text;
using MimeKit;
using Npgsql;

namespace SyncSpace.Server;

public class Class1
{
	public static void Main()
	{
		var connectionString = "Host=localhost;Port=5432;Database=SyncSpace;Username=postgres;Password=1234";
		using var connection = new NpgsqlConnection(connectionString);
		connection.Open();

		using (var cmd = new NpgsqlCommand("LISTEN request_table_change", connection))
		{
			cmd.ExecuteNonQuery();
		}
		connection.Notification += async (sender, e) =>
		{
			Random rand = new();
			string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			char[] code = new char[6];

			for (int i = 0; i < code.Length; i++)
			{
				code[i] = chars[rand.Next(chars.Length)];
			}
			string randomCode = new(code);

			string hashCode;
			hashCode = Convert.ToHexString(SHA512.HashData(Encoding.Unicode.GetBytes(randomCode)));
			Console.WriteLine(randomCode);
			Console.WriteLine(hashCode);

			var body = new TextPart(randomCode);
			var emailMessage = new MimeMessage
			{
				Body = body
			};
			emailMessage.From.Add(new MailboxAddress("Premium Company", "premium.syncspace@gmail.com"));
			emailMessage.Subject = "Ваш код для авторизации";
			using var client = new MailKit.Net.Smtp.SmtpClient();
			await client.ConnectAsync("smtp.gmail.com", 587, false);
			await client.AuthenticateAsync("premium.syncspace@gmail.com", "lymnxziwkndhsqfp");
			await client.SendAsync(emailMessage);
			await client.DisconnectAsync(true);
		};
		while (true)
		{
			connection.Wait();
		}
	}
}
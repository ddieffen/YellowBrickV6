using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using YellowbrickV6Cmd;

namespace YellowbrickV6
{
    public static class SMTPTools
    {
        public static void SendMail(string emailFrom,
            string password,
            string emailTo,
            string subject,
            string body,
            bool isBodyHTML)
        {

            string smtpAddress = "smtp.gmail.com";
            int portNumber = 587;
            bool enableSSL = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = isBodyHTML;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
        }

        internal static void TrySMTP()
        {
            bool haveInfo = false;
            while (!haveInfo)
            {
                if (!String.IsNullOrEmpty(YellowbrickV6Cmd.Properties.Settings.Default.smtpserver)
                       && !String.IsNullOrEmpty(YellowbrickV6Cmd.Properties.Settings.Default.smtpport.ToString())
                       && !String.IsNullOrEmpty(YellowbrickV6Cmd.Properties.Settings.Default.smtpuser)
                       && !String.IsNullOrEmpty(YellowbrickV6Cmd.Properties.Settings.Default.smtppassword))
                {
                    haveInfo = true;
                }
                else
                {
                    Console.WriteLine("Invalid SMTP configuration, please re-enter information");
                    Console.WriteLine("Please enter SMTP server address (example: smtp.gmail.com):");
                    YellowbrickV6Cmd.Properties.Settings.Default.smtpserver = Console.ReadLine();
                    Console.WriteLine("Please enter SMTP server port (example: 587):");
                    YellowbrickV6Cmd.Properties.Settings.Default.smtpport = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Please enter user name (example: name@gmail.com):");
                    YellowbrickV6Cmd.Properties.Settings.Default.smtpuser = Console.ReadLine();
                    Console.WriteLine("Please enter password:");
                    ConsoleKeyInfo key; String pass = "";
                    do
                    {//https://stackoverflow.com/questions/3404421/password-masking-console-application
                        key = Console.ReadKey(true);

                        // Backspace Should Not Work
                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                        {
                            pass += key.KeyChar;
                            Console.Write("*");
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                            {
                                pass = pass.Substring(0, (pass.Length - 1));
                                Console.Write("\b \b");
                            }
                        }
                    }
                    // Stops Receving Keys Once Enter is Pressed
                    while (key.Key != ConsoleKey.Enter);
                    YellowbrickV6Cmd.Properties.Settings.Default.smtppassword = SecurityTools.EncryptString(SecurityTools.ToSecureString(pass));
                    YellowbrickV6Cmd.Properties.Settings.Default.Save();
                }
            }
        }


        internal static void IsValidRecipient()
        {
            bool validEmail = false;
            while (!validEmail)
            {
                try
                {
                    MailAddress addr = new MailAddress(YellowbrickV6Cmd.Properties.Settings.Default.recipient);
                    validEmail = true;
                }
                catch 
                {
                    Console.WriteLine("Invalid email recipient, please re-enter information");
                    Console.WriteLine("Please enter email address (example: teamsorcerer@gmail.com):");
                    YellowbrickV6Cmd.Properties.Settings.Default.recipient = Console.ReadLine();
                    YellowbrickV6Cmd.Properties.Settings.Default.Save();
                }
            }
        }
    }
}

using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Bfar.Extensions
{
    public static class MailExtensions
    {

        /// <summary>
        /// Requires following configurations: networkCredentialUser,networkCredentialPass,externalHost,SendeEMail
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="Recipients">Semicolon (;) separated addresses</param>
        /// <param name="IsHtml"></param>
        public static void SendEmailAsync(this MailMessage mail, string subject, string body, string Recipients, bool IsHtml,string Host,string networkCredentialUser,string networkCredentialPass,string Sender)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    //exchange or smtp server goes here.
                    //if (!Recipients.IsValidEmail())
                    //    return;

                    using (SmtpClient smtp = new SmtpClient(Host))
                    {
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        string[] Recips = Recipients.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < Recips.Length; i++)
                        {
                            mail.To.Add(new MailAddress(Recips[i]));
                        }
                        mail.From = new MailAddress(Sender);
                        mail.IsBodyHtml = IsHtml;
                        mail.Subject = subject.Trim();
                        mail.Body = body.Trim();
                        smtp.Send(mail);
                        //  msg = "پیام شما با موفقیت ارسال گردید";
                    }
                }
                catch (Exception ex)
                {
                    //  msg = "عدم ارسال پیام - لطفا مجددا ارسال فرمایید";
                    throw;
                }
            });
        }
    }
}

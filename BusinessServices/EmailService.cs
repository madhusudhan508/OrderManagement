/*============================================================================
   Namespace        : BusinessServices
   Class            : EmailService
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Sending emails
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessServices
{
    using System.Net.Mail;
    using System.Configuration;
    using Helpers;
    public class EmailService
    {
        /// <summary>
        /// email sending 
        /// </summary>
        /// <param name="_email"></param>
        public void SendEmail(string _email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(ConfigurationManager.AppSettings["UserName"].ToString());
            mail.To.Add(_email);
            mail.Subject = DataHelper.Emails.Subject;
            mail.Body = DataHelper.Emails.Body;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail); 
        }

    }
}

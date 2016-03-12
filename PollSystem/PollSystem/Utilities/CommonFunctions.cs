using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace PollSystem.Utilities
{
    public class CommonFunctions
    {
        public bool SendEmail(string fromEmail, string toEmail, string ccEmail, string bccEmail, string subject, string body, string fileName)
        {
            try
            {
                using (MailMessage mm = new MailMessage(fromEmail, toEmail))
                {
                    if (ccEmail != string.Empty)
                        mm.CC.Add(ccEmail.Trim());
                    if (bccEmail != string.Empty)
                        mm.Bcc.Add(bccEmail.Trim());
                    mm.Subject = subject.Trim();
                    mm.Body = body.Trim();
                    if (fileName != string.Empty)
                        mm.Attachments.Add(new Attachment(fileName.Trim()));
                    mm.IsBodyHtml = true;

                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mm);
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public void SendAsyncMail(string fromEmail, string toEmail, string ccEmail, string bccEmail, string subject, string body, string fileName, string altViewImgPath)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(fromEmail);
                mm.To.Add(new MailAddress(toEmail));
                if (ccEmail != string.Empty)
                    mm.CC.Add(ccEmail.Trim());
                if (bccEmail != string.Empty)
                    mm.Bcc.Add(bccEmail.Trim());
                mm.IsBodyHtml = true;
                mm.Subject = subject;

                if (altViewImgPath != string.Empty)
                {
                    LinkedResource img = new LinkedResource(altViewImgPath, "image/png");
                    img.ContentId = "contentimage";
                    img.TransferEncoding = TransferEncoding.Base64;
                    AlternateView plainTextView = AlternateView.CreateAlternateViewFromString(body, null, "text/plain");
                    // done HTML formatting in the next line to display my logo
                    AlternateView av1 = AlternateView.CreateAlternateViewFromString("<html><body><img src='cid:contentimage' style='width:100px; height:70px;text-align:center;'/><br></body></html>" + body, null, "text/html");
                    av1.LinkedResources.Add(img);
                    mm.AlternateViews.Add(plainTextView);
                    mm.AlternateViews.Add(av1);
                }
                else
                    mm.Body = body;

                mm.Priority = MailPriority.High;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.EnableSsl = true;
                Object state = mm;

                //event handler for asynchronous call
                smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
                try
                {
                    smtpClient.SendAsync(mm, state);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

        void smtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MailMessage mail = e.UserState as MailMessage;

            if (!e.Cancelled && e.Error != null)
            {
                //message.Text = "Mail sent successfully";
            }
        }
    }
}
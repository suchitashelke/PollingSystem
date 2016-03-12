using System;
using System.Net.Mail;
using System.Web.Mvc;

namespace PollSystem
{
    public static class Helper
    {
        public static string IsActive(this HtmlHelper html, string control,string action)
        {
            var routeData = html.ViewContext.RouteData;

            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            var returnActive = control == routeControl && action == routeAction;

            return returnActive ? "active" : "";
        }

        public static bool SendEmail(string fromEmail, string toEmail, string ccEmail, string bccEmail, string subject, string body, string fileName)
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
    }
}
using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Subscription.BLL.Services.FormToMail
{

    public class FormToMail : IFormToMail
    {

        public void SendMail(string subj, string message, string mailTo)
        {
            try
            {  
                var msg = new MailMessage { From = new MailAddress("info@mobarkhotel.com") };
                //var msg = new MailMessage { From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["MailAddress"]) };
                if (mailTo != null)
                {

                    msg.Subject = Regex.Replace(subj, @"\t|\n|\r", ""); 
                    msg.IsBodyHtml = true; 
                    string headerTmp =
                        "<table width=100% ><tr><td align ='center' style='background: #000000'><img src = 'http://gmgportal.azurewebsites.net/Content/images/logo1.png' /></td></tr><br/><tr><td>";

                    string footertmp = "</tr></td></table>";

                    msg.Body = headerTmp + message + footertmp;

                    msg.To.Add(new MailAddress(mailTo));


                    //var mailusername = System.Configuration.ConfigurationManager.AppSettings["Mailusername"];
                    //var mailPassword = System.Configuration.ConfigurationManager.AppSettings["Mailpassword"];
                    var mailusername = "apikey";
                    var mailPassword = "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";

                    SmtpClient client = new SmtpClient
                    {
                        //Host = System.Configuration.ConfigurationManager.AppSettings["MailHost"],
                        //Port = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["MailPort"]),
                        Host = "smtp.sendgrid.net",
                        Port = 587,
                        UseDefaultCredentials = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true,
                        Timeout = 10000,


                        Credentials = new NetworkCredential(mailusername, mailPassword)
                    }; 
                    client.Send(msg);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;

                //  throw;
            }
            return;




        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MailKit.Net.Smtp;
//using MimeKit;
using InventoryManagement.cache;
using MailKit.Security;
using System.Net;
using System.Net.Mail;
using System.Text;


namespace InventoryManagement.Common
{
    public class EmailHelper
    {

        private List<string> _ToList = null;
        private List<string> _CcList = null;
        private string _MailBody = null;
        private string _MailSubject = null;


        private string EmailServer = String.Empty;
        private string EmailServerPort = String.Empty;
        private string EmailUserID = String.Empty;
        private string EmailPassword = String.Empty;
        

        private string _ErrorMessage = String.Empty;


        public string ErrorMsg
        {
            get
            {
                return _ErrorMessage;
            }
        }

        private void LoadEmailServerDetails()
        {
            EmailServer = SettingsCache.Instance.GetValue("EMAIL_SERVER");
            EmailServerPort = SettingsCache.Instance.GetValue("EMAIL_SERVER_PORT");
           

        }

        public EmailHelper(string EmailUser, string EmailPwd,List<string> tolist,string Subject,string Body, List<string> cclist = null)
        {
            _ToList = tolist;
            _CcList = cclist;
            _MailBody = Body;
            _MailSubject = Subject;
            EmailUserID = EmailUser;
            EmailPassword = CryptoManager.DecryptString(EmailPwd);

            LoadEmailServerDetails();
        }

        public bool ProcessEmail()
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(EmailUserID);
                foreach( string tomail in _ToList)
                {
                    message.To.Add(tomail);
                }
                message.Subject =_MailSubject;
                message.Body =_MailBody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(EmailServer, Convert.ToInt32(EmailServerPort)); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(EmailUserID, EmailPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);
                return true;
            }
            catch( Exception eError)
            {
                _ErrorMessage = eError.Message;
                throw eError;
               
            }
            
        }

     
    }
}

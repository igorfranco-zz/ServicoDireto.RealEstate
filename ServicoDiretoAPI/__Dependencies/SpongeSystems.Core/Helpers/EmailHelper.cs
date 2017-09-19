using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Net.Mail;
using System.Web;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using System.IO;

namespace SpongeSystems.Core.Helpers
{
    public class EmailHelper
    {
        /// <summary>
        /// Envio de email baseado em template xsl
        /// </summary>
        /// <param name="to">Para</param>
        /// <param name="from">De</param>
        /// <param name="subject">Assunto</param>
        /// <param name="xslPath">Caminho do template XSL</param>
        /// <param name="xml">XML com dados variaveis do email
        /// </param>
        public static void Send(string to, string subject, string xslPath, XDocument xml)
        {
            MemoryStream write = new MemoryStream();
            MailMessage mail = new MailMessage();
            XslCompiledTransform xsl = new XslCompiledTransform();
            xsl.Load(xslPath);

            xsl.Transform(xml.CreateNavigator(), null, write);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            write.Seek(0, SeekOrigin.Begin);
            mail.Body = new StreamReader(write).ReadToEnd();
            SmtpClient SMTPServer = new SmtpClient();
            SMTPServer.Send(mail);
        }
    }
}

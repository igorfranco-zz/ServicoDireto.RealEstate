using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSolutions.Core
{
    public class Messages
    {

        private enum MessageType : int
        {
            Success = 0,
            Info    = 1,
            Alert   = 2,
            Help    = 3,
            Hint    = 4,
            Error   = 5,
            Total   = 6
        }

        public static void ShowTotalMessage(string message, Label lblMessage)
        {
            ShowMessage(MessageType.Total, message, lblMessage);
        }

        public static void ShowSuccessMessage(string message, Label lblMessage)
        {
            ShowMessage(MessageType.Success, message, lblMessage);
        }

        public static void ShowErrorMessage(string message, Label lblMessage)
        {
            ShowMessage(MessageType.Error, message, lblMessage);
        }

        public static void ShowAlertMessage(string message, Label lblMessage)
        {
            ShowMessage(MessageType.Alert, message, lblMessage);
        }

		public static void ShowInfoMessage( string message, Label lblMessage )
		{
			Messages.ShowMessage( MessageType.Info, message, lblMessage );
		}

        private static void ShowMessage(MessageType messageType, string message, Label lblMessage)
        {
            if (message != null && message.Length > 0)
            {
                switch (messageType)
                {
                    case MessageType.Alert: lblMessage.CssClass = "message message-alert"; break;
                    case MessageType.Success: lblMessage.CssClass = "message message-success"; break;
                    case MessageType.Info: lblMessage.CssClass = "message message-info"; break;
                    case MessageType.Help: lblMessage.CssClass = "message message-help"; break;
                    case MessageType.Hint: lblMessage.CssClass = "message message-hint"; break;
                    case MessageType.Error: lblMessage.CssClass = "message message-error"; break;
                    case MessageType.Total: lblMessage.CssClass = "total"; break;
                    default: lblMessage.CssClass = "message"; break;
                }
                lblMessage.Visible = true;
                lblMessage.Text = message;
            }
        }
    }
}

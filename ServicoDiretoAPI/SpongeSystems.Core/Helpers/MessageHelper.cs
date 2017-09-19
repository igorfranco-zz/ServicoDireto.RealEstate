using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpongeSolutions.Core.Helpers
{
    public class MessageHelper
    {
        public enum MessageType
        {
            Success,
            Warnig,
            Error,
            Information,
        }

        public static void ShowError(Form owner, string title, string message)
        {
            ShowMessage(owner, title, message, MessageType.Error);
        }

        public static void ShowWarning(Form owner, string title, string message)
        {
            ShowMessage(owner, title, message, MessageType.Warnig);
        }

        public static void ShowInformation(Form owner, string title, string message)
        {
            ShowMessage(owner, title, message, MessageType.Information);
        }

        private static void ShowMessage(Form owner, string title, string message, MessageType messageType)
        {
            MessageBoxIcon iconType;
            switch (messageType)
            {
                case MessageType.Success: iconType = MessageBoxIcon.Exclamation; break;
                case MessageType.Warnig: iconType = MessageBoxIcon.Warning; break;
                case MessageType.Error: iconType = MessageBoxIcon.Error; break;
                case MessageType.Information: iconType = MessageBoxIcon.Information; break;
                default: iconType = MessageBoxIcon.None; break;
            }
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, iconType, MessageBoxDefaultButton.Button1);
        }
    }
}

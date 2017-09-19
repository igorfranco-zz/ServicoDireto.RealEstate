using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSolutions.Core.Extension
{
    public class Enums
    {
        public enum PositionToType : short
        {
            Window = 0,
        }
        /// <summary>
        /// Back - To move one step back in history
        /// Dialog - To open link styled as dialog, not tracked in history
        /// External - For linking to another domain
        /// Popup - For opening a popup
        /// </summary>
        public enum RelType : short
        {
            Back = 0,
            External = 1,
            Popup = 2,
            Dialog = 3
        }

        public enum TransitionType : short
        {
            None = 0,
            Pop = 1,
            Dade = 2,
            Flip = 3,
            Turn = 4,
            Flow = 5,
            Slide = 6,
            SlideFade = 7,
            SlidUp = 8,
            SlidDown = 9
        }

        public enum IconType : short
        {
            None = 0,
            Home = 1,
            Delete = 2,
            Back = 3,
            Check = 4,
            Star,
            Grid,
            Gear,
            Lock,
            Plus,
            Arrow_u,
            Arrow_d,
            Carat_l,
            carat_t,
            carat_r,
            carat_b,
            Custom,
            Arrow_r,
            Arrow_l,
            Minus,
            Refresh,
            Forward,
            Alert,
            Info,
            Search,
            Action,
            Bars
        }

        public enum ThemeType
        {
            a = 1,
            b = 2,
            c = 3,
            d = 4,
            e = 5
        }

        public enum ContainerType
        {            
            Header = 2,
            Footer = 3,
            Main = 4,
            NavBar = 5
        }

        public enum PositionDialogType
        {
            None = 0,
            Right = 1,
            Left = 2
        }

        public enum PositionType
        {
            NoText = 0,
            Right = 1,
            Left = 2,
            Top = 3,
            Bottom = 4
        }

        public enum OrientationType
        {
            Horizontal = 1,
            Vertical = 2
        }

        public enum ButtonType
        {
            Submit = 1,
            Reset = 2,
            Button = 3,
            Link = 4,
        }

        public enum InputType
        {
            Text,
            Search,
            TextArea,
            Password,
            Number,
            Date,
            Month,
            Week,
            Time,
            Datetime,
            Telephone,
            Email,
            URL,
            Color,
            File,
            CheckBox,
            Radio
        }

        public enum ComponentType
        {
            Section,
            Button,
        }

        public enum MessageType
        {
            Success,
            Info,
            Warning,
            Danger
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace SpongeSolutions.Core.Extension
{
    public abstract class BaseConfig
    {
        #region [Properties]

        /// <summary>
        /// Default class name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        ///Sets the color scheme (swatch) for the button widget. It accepts a single letter from a-z that maps to the swatches included in your theme.
        ///Possible values: swatch letter (a-z).
        ///This option is also exposed as a data attribute: data-theme="b".
        /// </summary>
        public Enums.ThemeType? Theme { get; set; }

        /// <summary>
        /// Element name
        /// </summary>
        public string Name { get; set; }

        internal IDictionary<string, object> Configuration { get; set; }

        public IDictionary<string, object> HtmlAttributes { get; set; }

        #endregion

        #region [Constructor]

        public BaseConfig()
        {
            this.Theme = null;
            this.Name = null;
            this.ClassName = null;
            this.Configuration = new Dictionary<string, object>();
            this.HtmlAttributes = new Dictionary<string, object>();
        }

        public BaseConfig(string name)
        {
            this.Theme = null;
            this.Name = name;
            this.ClassName = null;
            this.Configuration = new Dictionary<string, object>();
            this.HtmlAttributes = new Dictionary<string, object>();
        }

        #endregion

        #region [SetAttributes]

        public void SetAttributes(TagBuilder tag)
        {
            tag.MergeAttributes(new RouteValueDictionary(GetAttributes()));
        }

        #endregion

        #region [GetAttributes]
        public virtual IDictionary<string, object> GetAttributes()
        {
            if (this.Theme != null)
                this.Configuration.Add("data-theme", this.Theme.ToString());

            if (!string.IsNullOrEmpty(this.Name))
            {
                this.Configuration.Add("name", this.Name);
                this.Configuration.Add("id", this.Name);
            }

            if (!string.IsNullOrEmpty(this.ClassName))
                this.Configuration.Add("class", this.ClassName);

            //if (this.Configuration != null)
            //    this.Configuration.Add((new RouteValueDictionary(this.Configuration), true);
            return this.Configuration;
        }

        #endregion
    }

    public class PageConfig : Config
    {
        #region [Properties]

        /// <summary>
        /// Default: "left"
        /// This option is provided by the dialog extension.
        /// Sets the position of the dialog close button in the header.
        /// Possible values:
        /// "left"
        /// The button will be placed on the left edge of the titlebar.
        /// "right"
        /// The button will be placed on the right edge of the titlebar.
        /// "none"
        /// The dialog will not have a close button.
        /// </summary>
        public Enums.PositionDialogType? CloseBtn { get; set; }

        /// <summary>
        /// This option is provided by the dialog extension.
        /// Customizes the text of the close button which is helpful for translating this into different languages. The close button is displayed as an icon-only button by default so the text isn't visible on-screen, but is read by screen readers so this is an important accessibility feature.
        /// This option is also exposed as a data attribute: data-close-btn-text="Fermer".
        /// </summary>
        public string CloseBtnText { get; set; }

        /// <summary>
        /// This option is provided by the dialog extension.
        ///Sets whether the page should be styled like a dialog.
        ///This option is also exposed as a data attribute: data-dialog="true".
        /// </summary>
        public bool IsDialog { get; set; }

        /// <summary>
        /// This option is provided by the dialog extension.
        /// Dialogs appear to be floating above an overlay layer. This overlay adopts the swatch "a" content color by default, but the data-overlay-theme attribute can be added to the element to set the overlay to any swatch letter.
        /// Possible values: swatch letter (a-z)
        /// This option is also exposed as a data attribute: data-overlay-theme="b".
        /// </summary>
        public Enums.ThemeType? OverlayTheme { get; set; }

        #endregion

        #region [Constructor]

        public PageConfig(string name)
            : base()
        {
            this.Name = name;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            this.Configuration.Add("data-role", "page");

            if (this.CloseBtn != null)
                this.Configuration.Add("data-close-btn", this.CloseBtn.ToString().ToLower());

            if (!string.IsNullOrEmpty(this.CloseBtnText))
                this.Configuration.Add("data-close-btn-text", this.CloseBtnText);

            if (this.IsDialog)
                this.Configuration.Add("data-dialog", "true");

            if (this.OverlayTheme != null)
                this.Configuration.Add("data-overlay-theme", this.OverlayTheme.ToString().ToLower());

            return this.Configuration;
        }

        #endregion
    }

    public class Config : BaseConfig
    {
        #region [Properties]

        public bool DataDefaults { get; set; }

        /// <summary>
        /// Applies the theme button border-radius if set to true. This option is also exposed as a data attribute: data-corners="false".
        /// </summary>
        public bool HasCorner { get; set; }

        /// <summary>
        /// Indicates that the markup necessary for a button widget has been provided as part of the original markup.This option is also exposed as a data attribute: data-enhanced="true".
        /// </summary>
        public bool IsEnhanced { get; set; }

        /// <summary>
        /// If set to true, this will make the button act like an inline button so the width is determined by the button's text. By default, this is null (false) so the button is full width, regardless of the feedback content. Possible values: true, false.This option is also exposed as a data attribute: data-inline="true".
        /// </summary>
        public bool IsInline { get; set; }

        /// <summary>
        /// Disables the button if set to true.This option is also exposed as a data attribute: data-disabled="true".
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        ///If set to true, this will display a more compact version of the button that uses less vertical height by applying the ui-mini class to the outermost element of the button widget.
        ///This option is also exposed as a data attribute: data-mini="true".
        /// </summary>
        public bool IsMini { get; set; }

        /// <summary>
        ///Applies the drop shadow style to the button if set to true.
        ///This option is also exposed as a data attribute: data-shadow="false".]       
        /// </summary>
        public bool HasShadow { get; set; }

        /// <summary>
        ///Allows you to specify CSS classes to be set on the button's wrapper element.
        ///This option is also exposed as a data attribute: data-wrapper-class="custom-class".
        /// </summary>
        public string WrapperClassType { get; set; }

        /// <summary>
        ///Positions the icon in the button. Possible values: left, right, top, bottom, none, notext. The notext value will display an icon-only button with no text feedback. This option is also exposed as a data attribute: data-iconpos="right".
        /// </summary>
        public Enums.PositionType? IconPosition { get; set; }

        /// <summary>
        /// Applies an icon from the icon set.The .buttonMarkup() documentation contains a reference of all the icons available in the default theme. This option is also exposed as a data attribute: data-icon="star".
        /// </summary>
        public Enums.IconType? Icon { get; set; }

        #endregion

        #region [Constructor]

        public Config()
            : base()
        {
            this.DataDefaults = false;
            this.HasCorner = true;
            this.IsEnhanced = false;
            this.IsInline = false;
            this.IsEnabled = true;
            this.IsMini = false;
            this.HasShadow = true;
            this.Icon = null;
            this.IconPosition = null;
            this.WrapperClassType = null;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (this.DataDefaults)
                this.Configuration.Add("data-defaults", "true");

            if (!this.HasCorner)
                this.Configuration.Add("data-corners", "false");

            if (this.IsEnhanced)
                this.Configuration.Add("data-enhanced", "true");

            if (this.IsInline)
                this.Configuration.Add("data-inline", "true");

            if (!this.IsEnabled)
                this.Configuration.Add("data-disabled", "true");

            if (this.IsMini)
                this.Configuration.Add("data-mini", "true");

            if (!this.HasShadow)
                this.Configuration.Add("data-shadow", "false");

            if (this.Icon != null)
                this.Configuration.Add("data-icon", this.Icon.ToString().ToLower());

            if (this.IconPosition != null)
                this.Configuration.Add("data-iconpos", this.IconPosition.ToString().ToLower());

            if (!string.IsNullOrEmpty(this.WrapperClassType))
                this.Configuration.Add("data-wrapper-class", this.WrapperClassType);

            return this.Configuration;
        }

        #endregion
    }

    public class ListViewConfig : BaseConfig
    {
        #region [Properties]
        /// <summary>
        /// This option is provided by the listview.autodividers extension.
        ///When set to true, dividers are automatically created for the listview items.
        ///The function stored in the value of the autodividersSelector option governs the text displayed on the dividers.
        ///This option is also exposed as a data-attribute: data-autodividers="true".
        /// </summary>
        public bool AutoDividers { get; set; }

        /// <summary>
        /// Seting this option to true indicates that other widgets options have default values and causes jQuery Mobile's widget autoenhancement code to omit the step where it retrieves option values from data attributes. This can improve startup time.
        /// This option is also exposed as a data attribute: data-defaults="true".
        /// </summary>
        public bool DataDefaults { get; set; }

        /// <summary>
        /// Disables the button if set to true.This option is also exposed as a data attribute: data-disabled="true".
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Sets the color scheme (swatch) for list dividers. It accepts a single letter from a-z that maps to one of the swatches included in your theme.
        //This option is also exposed as a data attribute: data-divider-theme="b".
        /// </summary>
        public Enums.ThemeType? DividerTheme { get; set; }

        /// <summary>
        /// This option is provided by the listview.hidedividers extension.
        ///When set to true and all list items residing under a given divider become hidden, then the divider itself is hidden.
        ///This option is also exposed as a data-attribute: data-hide-dividers="true".
        /// </summary>
        public bool HideDividers { get; set; }

        /// <summary>
        /// Applies an icon from the icon set.The .buttonMarkup() documentation contains a reference of all the icons available in the default theme. This option is also exposed as a data attribute: data-icon="star".
        /// </summary>
        public Enums.IconType? Icon { get; set; }

        /// <summary>
        /// Adds inset list styles.
        ///This option is also exposed as a data attribute: data-inset="true".
        /// </summary>
        public bool Inset { get; set; }

        /// <summary>
        /// Applies an icon from the icon set to all split list buttons.
        ///This option is also exposed as a data attribute: data-split-icon="star".
        /// </summary>
        public Enums.IconType? SplitIcon { get; set; }

        /// <summary>
        /// Sets the color scheme (swatch) for split list buttons. It accepts a single letter from a-z that maps to one of the swatches included in your theme.
        ///This option is also exposed as a data attribute: data-split-theme="b".
        /// </summary>
        public Enums.ThemeType? SplitTheme { get; set; }

        /// <summary>
        /// Adds a search filter bar to listviews.
        ///This option is also exposed as a data attribute: data-filter="true".
        /// </summary>
        public bool IsFilterable { get; set; }

        public string PlaceHolder  { get; set; }

        #endregion

        #region [Constructor]

        public ListViewConfig(string name)
            : base(name)
        {
            this.IsEnabled = true;
        }

        #endregion

        #region [GetAttributes]

        public virtual IDictionary<string, object> GetAttributes()
        {
            this.Configuration.Add("data-role", "listview");

            if (this.AutoDividers)
                this.Configuration.Add("data-autodividers=", "true");

            if (this.DataDefaults)
                this.Configuration.Add("data-defaults", "true");

            if (!this.IsEnabled)
                this.Configuration.Add("data-disabled", "true");

            if (this.IsFilterable)
                this.Configuration.Add("data-filter", "true");

            if (this.DividerTheme.HasValue)
                this.Configuration.Add("data-divider-theme", this.DividerTheme.ToString());

            if (this.HideDividers)
                this.Configuration.Add("data-hide-dividers", "true");

            if (this.Icon != null)
                this.Configuration.Add("data-icon", this.Icon.ToString().ToLower());

            if (this.Inset)
                this.Configuration.Add("data-inset", "true");

            if (this.SplitIcon.HasValue)
                this.Configuration.Add("data-split-icon", this.Icon.ToString().ToLower());

            if (this.SplitTheme.HasValue)
                this.Configuration.Add("data-split-theme", this.SplitTheme.ToString());

            if (!string.IsNullOrEmpty(this.PlaceHolder))
                this.Configuration.Add("data-filter-placeholder", this.PlaceHolder);

            return this.Configuration;
        }

        #endregion
    }

    public class FlipswitchConfig : Config
    {
        #region [Properties]

        public string OffText { get; set; }
        public string OnText { get; set; }

        #endregion

        #region [Constructor]

        public FlipswitchConfig(string onText, string offText)
            : base()
        {
            this.OffText = offText;
            this.OnText = onText;
        }

        public FlipswitchConfig(string name, string onText, string offText)
            : base()
        {
            this.Name = name;
            this.OffText = offText;
            this.OnText = onText;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (!string.IsNullOrEmpty(this.OnText))
                this.Configuration.Add("data-on-text", this.OnText);

            if (!string.IsNullOrEmpty(this.OffText))
                this.Configuration.Add("data-off-text", this.OffText);

            return this.Configuration;
        }

        #endregion
    }

    public class InputConfig : Config
    {
        #region [Properties]

        public Enums.InputType? InputType { get; set; }

        public object Value { get; set; }

        public bool HasClearButton { get; set; }

        public string ClearButtonText { get; set; }

        public string PlaceHolder { get; set; }

        public string Pattern { get; set; }
        
        public string Mask { get; set; }

        public bool Selected { get; set; }

        #endregion

        #region [Constructor]

        public InputConfig()
        { }

        public InputConfig(Enums.InputType inputType)
        {
            this.InputType = inputType;
        }

        public InputConfig(string name, Enums.InputType inputType)
            : base()
        {
            this.InputType = inputType;
            this.Name = name;
            this.HasClearButton = false;
            this.ClearButtonText = null;
            this.PlaceHolder = null;
            this.Value = null;
            this.Selected = false;
            this.Pattern = null;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (this.InputType.HasValue)
                this.Configuration.Add("type", this.InputType.ToString().ToLower());

            if (this.HasClearButton)
                this.Configuration.Add("data-clear-btn", "true");

            if (!string.IsNullOrEmpty(this.ClearButtonText))
                this.Configuration.Add("data-clear-btn-text", this.ClearButtonText);

            if (!string.IsNullOrEmpty(this.PlaceHolder))
                this.Configuration.Add("placeholder", this.PlaceHolder);

            if (!string.IsNullOrEmpty(this.Pattern))
                this.Configuration.Add("pattern", this.Pattern);

            if (this.Value != null)
                this.Configuration.Add("value", this.Value.ToString());

            if (this.Selected)
                this.Configuration.Add("checked", "true");

            if (!string.IsNullOrEmpty(this.Mask))
                this.Configuration.Add("xmask", this.Mask);

            return this.Configuration;
        }

        #endregion
    }

    public class ButtonConfig : Config
    {
        #region [Properties]
        public object Value { get; set; }
        public Enums.ButtonType ButtonType { get; set; }

        #endregion

        #region [Constructor]
        public ButtonConfig(string name)
        {
            this.Name = name;
        }

        public ButtonConfig(string name, Enums.ButtonType buttonType)
            : base()
        {
            this.Value = null;
            this.Name = name;
            this.ButtonType = buttonType;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();
            if (this.ButtonType == Enums.ButtonType.Reset || this.ButtonType == Enums.ButtonType.Submit)
                this.Configuration.Add("type", this.ButtonType.ToString().ToLower());

            if (this.Value != null)
            {
                if (this.ButtonType == Enums.ButtonType.Reset || this.ButtonType == Enums.ButtonType.Submit)
                    this.Configuration.Add("value", this.Value.ToString());
                //else
                //    tag.SetInnerText(this.Value.ToString());
            }

            return this.Configuration;
        }

        #endregion
    }

    public class LinkConfig : ButtonConfig
    {
        #region [Properties]

        public string Href { get; set; }
        public Enums.RelType? Rel { get; set; }
        public bool Reverse { get; set; }
        public PopupConfig PopupConfig { get; set; }

        #endregion

        #region [Constructor]

        public LinkConfig(string name, string text)
            : base(name, Enums.ButtonType.Link)
        {
            this.ButtonType = Enums.ButtonType.Link;
            this.Value = text;
        }

        public LinkConfig(string name, string text, Enums.RelType rel)
            : base(name, Enums.ButtonType.Link)
        {
            this.ButtonType = Enums.ButtonType.Link;
            this.Value = text;
            this.Rel = rel;
        }

        public LinkConfig(string name, string text, Enums.RelType rel, string href)
            : base(name, Enums.ButtonType.Link)
        {
            this.ButtonType = Enums.ButtonType.Link;
            this.Value = text;
            this.Rel = rel;
            this.Href = href;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (!string.IsNullOrEmpty(this.Href))
                this.Configuration.Add("href", this.Href);

            if (this.Rel.HasValue)
            {
                if (this.Rel.Value == Enums.RelType.External)
                    this.Configuration.Add("rel", this.Rel.ToString().ToLower());
                else
                    this.Configuration.Add("data-rel", this.Rel.ToString().ToLower());
            }

            if (this.Reverse)
                this.Configuration.Add("data-direction", "true");

            if (this.PopupConfig != null)
            {
                foreach (var item in this.PopupConfig.GetAttributes())
                    this.Configuration.Add(item);
            }

            return this.Configuration;
        }

        #endregion
    }


    public class SectionConfig : BaseConfig
    {
        #region [Properties]

        public Enums.ContainerType ContainerType { get; set; }
        public bool IsFixed { get; set; }
        public bool IsFullScreen { get; set; }
        public bool CreateBackButton { get; set; }
        public string BackButtonText { get; set; }

        #endregion

        #region [Constructor]

        public SectionConfig(string name, Enums.ContainerType containerType)
            : base()
        {
            this.ContainerType = containerType;
            base.Name = name;
            this.IsFixed = false;
            this.IsFullScreen = false;
            this.CreateBackButton = false;
            this.BackButtonText = null;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (this.ContainerType == Enums.ContainerType.Main)
                this.Configuration.Add("role", ContainerType.ToString().ToLower());
            else
                this.Configuration.Add("data-role", ContainerType.ToString().ToLower());

            if (this.IsFixed)
                this.Configuration.Add("data-position", "fixed");

            if (this.IsFullScreen)
                this.Configuration.Add("data-fullscreen", "true");

            if (this.CreateBackButton)
            {
                this.Configuration.Add("data-add-back-btn", "true");
                if (string.IsNullOrEmpty(this.BackButtonText))
                    this.Configuration.Add("data-back-btn-text", "Return");
            }

            if (!string.IsNullOrEmpty(this.BackButtonText))
                this.Configuration.Add("data-back-btn-text", this.BackButtonText);

            return this.Configuration;
        }

        #endregion
    }

    public class PopupConfig : Config
    {
        #region [Properties]

        /// <summary>
        /// data-arrow
        /// Sets whether to draw the popup with an arrow. This option is provided by the widgets/popup.arrow extension.
        /// Multiple types supported:
        /// String: A comma-separated list of the letters "l", "t", "r", and "b".
        /// Boolean: A value of true is equivalent to a value of "t,r,b,l", whereas false indicates that no arrow is to be shown.
        /// </summary>
        public string Arrow { get; set; }

        /// <summary>
        /// Sets whether clicking outside the popup or pressing Escape while the popup is open will close the popup. Note: When history support is turned on, pressing the browser's "Back" button will dismiss the popup even if this option is set to false.
        /// </summary>
        public bool Dismissible { get; set; }

        /// <summary>
        /// Sets whether to alter the url when a popup is open to support the back button.
        /// </summary>
        public bool History { get; set; }

        /// <summary>
        /// Sets the color scheme (swatch) for the popup background, which covers the entire window. If not explicitly set, the background will be transparent.
        /// </summary>
        public Enums.ThemeType? OverlayTheme { get; set; }

        /// <summary>
        ///  Sets the element relative to which the popup will be centered. It has the following values:
        ///  "origin" 	When the popup opens, center over the coordinates passed to the open() call (see details on this method).
        ///  "window" 	When the popup opens, center in the window.
        ///  jQuery selector 	When the popup opens, create a jQuery object based on the selector, and center over it. The selector is filtered for elements that are visible with ":visible". If the result is empty, the popup will be centered in the window. 
        /// </summary>
        public Enums.PositionToType? PositionTo { get; set; }

        /// <summary>
        ///        Sets the minimum distance from the edge of the window for the corresponding edge of the popup. By default, the values above will be used for the distance from the top, right, bottom, and left edge of the window, respectively.
        ///You can specify a value for this option in one of four ways:
        ///    Empty string, null, or some other falsy value. This will cause the popup to revert to the above default values.
        ///    A single number. This number will be used for all four edge tolerances.
        ///    Two numbers separated by a comma. The first number will be used for tolerances from the top and bottom edge of the window, and the second number will be used for tolerances from the left and right edge of the window.
        ///    Four comma-separated numbers. The first will be used for tolerance from the top edge, the second for tolerance from the right edge, the third for tolerance from the bottom edge, and the fourth for tolerance from the left edge.
        ///default: "30,15,30,15"
        /// </summary>
        public string Tolerance { get; set; }

        /// <summary>
        ///Sets the default transition for the popup. The default value will result in no transition.
        ///If the popup is opened from a link, and the link has the data-transition attribute set, the value specified therein will override the value of this option at the time the popup is opened from the link.
        /// </summary>
        public Enums.TransitionType? Transition { get; set; }
        #endregion

        #region [Constructor]

        public PopupConfig(string name = null)
        {
            this.Arrow = null;
            this.Dismissible = true;
            this.History = true;
            this.OverlayTheme = null;
            this.PositionTo = null;
            this.Tolerance = null;
            this.Transition = null;
            base.Name = name;
        }

        #endregion

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (!string.IsNullOrEmpty(base.Name))
                this.Configuration.Add("data-role", "popup");

            if (!string.IsNullOrEmpty(this.Arrow))
                this.Configuration.Add("data-arrow", this.Arrow);

            if (!this.Dismissible)
                this.Configuration.Add("data-dismissible", "false");

            if (!this.History)
                this.Configuration.Add("data-history", "false");

            if (this.OverlayTheme != null)
                this.Configuration.Add("data-overlay-theme", this.OverlayTheme.Value.ToString());

            if (this.PositionTo.HasValue)
                this.Configuration.Add("data-position-to", this.PositionTo.ToString().ToLower());

            if (!string.IsNullOrEmpty(this.Tolerance))
                this.Configuration.Add("data-tolerance", this.Tolerance);

            if (this.Transition.HasValue)
                this.Configuration.Add("data-transition", this.Transition.ToString().ToLower());

            return this.Configuration;
        }

        #endregion
    }

    public class ControlGroupConfig : Config
    {
        #region [Properties]

        public string Legend { get; set; }
        public Enums.OrientationType Orientation { get; set; }

        #endregion

        #region [Contructor]

        public ControlGroupConfig(Enums.OrientationType orientation)
        {
            this.Orientation = orientation;
            this.Legend = null;
        }

        #endregion

        #region [GetAttributes]
        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();
            if (!string.IsNullOrEmpty(this.Legend))
            {
                TagBuilder legendTag = new TagBuilder("legend");
                legendTag.SetInnerText(this.Legend);
                //tag.InnerHtml += legendTag.ToString();
            }
            this.Configuration.Add("data-role", "controlgroup");
            this.Configuration.Add("data-type", this.Orientation.ToString().ToLower());

            return this.Configuration;
        }
        #endregion
    }

    public class SelectMenuConfig : Config
    {
        #region [Properties]

        /// <summary>
        /// Texto a ser exibido no botão de fechar
        /// </summary>
        public string CloseText { get; set; }

        /// <summary>
        /// Sets the color scheme (swatch) for the listview dividers that represent the optgroup headers. It accepts a single letter from a-z that maps to the swatches included in your theme. Possible values: swatch letter (a-z).
        /// </summary>
        public Enums.ThemeType? DividerTheme { get; set; }

        /// <summary>
        /// Sets whether placeholder menu items are hidden. When true, the menu item used as the placeholder for the select menu widget will not appear in the list of choices.This option is also exposed as a data attribute: data-hide-placeholder-menu-items="false".
        /// </summary>
        public bool HidePlaceholderMenuItemsType { get; set; }

        /// <summary>
        /// When set to true, clicking the custom-styled select menu will open the native select menu which is best for performance. If set to false, the custom select menu style will be used instead of the native menu.This option is also exposed as a data attribute: data-native-menu="false".
        /// </summary>
        public bool IsNativeMenu { get; set; }

        /// <summary>
        /// Sets the color of the overlay layer for the dialog-based custom select menus and the outer border of the smaller custom menus. It accepts a single letter from a-z that maps to the swatches included in your theme. By default, the content block colors for the overlay will be inherited from the parent of the select.This option is also exposed as a data attribute: data-overlay-theme="a".
        /// </summary>
        public Enums.ThemeType? OverlayThemeType { get; set; }

        /// <summary>
        /// This option disables page zoom temporarily when a custom select is focused, which prevents iOS devices from zooming the page into the select. By default, iOS often zooms into form controls, and the behavior is often unnecessary and intrusive in mobile-optimized layouts.This option is also exposed as a data attribute: data-prevent-focus-zoom="true".
        /// </summary>
        public bool PreventFocusZoomType { get; set; }

        /// <summary>
        /// Allow multiple selection
        /// </summary>
        public bool IsMultiple { get; set; }

        #endregion

        #region [Construtor]

        public SelectMenuConfig()
        {
            this.LoadInitialValues();
        }

        public SelectMenuConfig(string name)
        {
            base.Name = name;
            this.LoadInitialValues();
        }

        #endregion

        private void LoadInitialValues()
        {
            this.CloseText = null;
            this.DividerTheme = null;
            this.HidePlaceholderMenuItemsType = false;
            this.IsNativeMenu = false;
            this.OverlayThemeType = null;
            this.PreventFocusZoomType = false;
            this.IsMultiple = false;
        }

        #region [GetAttributes]

        public override IDictionary<string, object> GetAttributes()
        {
            base.GetAttributes();

            if (this.IsNativeMenu)
                this.Configuration.Add("data-native-menu", "true");
            else
                this.Configuration.Add("data-native-menu", "false");

            if (!string.IsNullOrEmpty(this.CloseText))
                this.Configuration.Add("data-close-text", this.CloseText);

            if (this.DividerTheme != null)
                this.Configuration.Add("data-divider-theme", this.DividerTheme.ToString().ToLower());

            if (this.HidePlaceholderMenuItemsType)
                this.Configuration.Add("data-hide-placeholder-menu-items", "false");

            if (this.OverlayThemeType != null)
                this.Configuration.Add("data-overlay-theme", this.OverlayThemeType.ToString().ToLower());

            if (this.PreventFocusZoomType)
                this.Configuration.Add("data-prevent-focus-zoom", "true");

            if (this.IsMultiple)
                this.Configuration.Add("multiple", "multiple");

            return this.Configuration;
        }

        #endregion
    }

}

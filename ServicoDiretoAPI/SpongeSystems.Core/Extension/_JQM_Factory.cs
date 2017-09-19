using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Reflection;
using SpongeSolutions.Core.Extension;
using SpongeSolutions.Core.Model;
using System.Globalization;

//builder.ToString(TagRenderMode.SelfClosing);
namespace SpongeSolutions.Core.Extension
{
    public static class JQM_Factory
    {
        //public class Helper
        //{
        //    //public string GetIconClass(Enums.IconType icon)
        //    //{ 

        //    //}

        //    /// <summary>
        //    /// Cria container de definição de componente para JQueryMobile
        //    /// </summary>
        //    /// <param name="elements"></param>
        //    /// <returns></returns>
        //    public static MvcHtmlString JQMFieldWrap(params MvcHtmlString[] elements)
        //    {
        //        TagBuilder containerTag = new TagBuilder("div");
        //        containerTag.MergeAttribute("class", "ui-field-contain");
        //        if (elements != null)
        //        {
        //            foreach (var item in elements)
        //                containerTag.InnerHtml += item;
        //        }

        //        return new MvcHtmlString(containerTag.ToString());
        //    }

        //    /// <summary>
        //    /// Cria container para checkbox e radio
        //    /// </summary>
        //    /// <param name="orientation"></param>
        //    /// <param name="iconPosition"></param>
        //    /// <param name="elements"></param>
        //    /// <returns></returns>
        //    //public static MvcHtmlString JQMFieldOptionWrap(
        //    //    Enums.OrientationType orientation,
        //    //    Enums.PositionType iconPosition,
        //    //    params MvcHtmlString[] elements)
        //    //{
        //    //    TagBuilder fieldsetTag = new TagBuilder("fieldset");
        //    //    TagBuilder legendTag = new TagBuilder("legend");
        //    //    fieldsetTag.MergeAttribute("data-role", "controlgroup");
        //    //    fieldsetTag.MergeAttribute("data-type", orientation.ToString().ToLower());
        //    //    fieldsetTag.MergeAttribute("data-iconpos", iconPosition.ToString().ToLower());
        //    //    fieldsetTag.InnerHtml += legendTag.ToString();

        //    //    foreach (var item in elements)
        //    //        fieldsetTag.InnerHtml += item;

        //    //    return new MvcHtmlString(fieldsetTag.ToString());
        //    //}
        //}

        //#region RadioButton

        /////// <summary>
        /////// Criação radio buttn
        /////// </summary>
        /////// <param name="htmlHelper"></param>
        /////// <param name="name"></param>
        /////// <param name="orientation"></param>
        /////// <param name="iconPosition"></param>
        /////// <param name="values"></param>
        /////// <returns></returns>
        ////public static MvcHtmlString JQM_RadioButton(this HtmlHelper htmlHelper, string name, Enums.OrientationType orientation, Enums.PositionType iconPosition, MultiSelectList values)
        ////{
        ////    var elements = new List<MvcHtmlString>();
        ////    foreach (var item in values)
        ////    {
        ////        string id = String.Format("{0}_{1}", name, item.Value);
        ////        elements.Add(JQM_RadioButton(htmlHelper, id, item.Text, item.Selected, item.Text));
        ////    }

        ////    return Helper.JQMFieldOptionWrap(orientation, iconPosition, elements.ToArray());
        ////}

        ///// <summary>
        ///// Criacao de lista de radiobuttons
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="name"></param>
        ///// <param name="value"></param>
        ///// <param name="isChecked"></param>
        ///// <param name="label"></param>
        ///// <param name="hasContainer"></param>
        ///// <returns></returns>
        //public static MvcHtmlString JQM_RadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked = false, string label = null, bool hasContainer = false)
        //{
        //    IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
        //    htmlAttributes.Add(new KeyValuePair<string, object>("value", value.ToString()));
        //    if (isChecked)
        //        htmlAttributes.Add(new KeyValuePair<string, object>("checked", "checked"));

        //    return JQM_Input(htmlHelper, Enums.InputType.Radio, name, null, label, hasContainer, htmlAttributes: htmlAttributes);
        //}

        //#endregion

        //#region Checkbox

        /////// <summary>
        /////// Criação de colecao de checkboxes
        /////// </summary>
        /////// <param name="htmlHelper"></param>
        /////// <param name="name"></param>
        /////// <param name="orientation"></param>
        /////// <param name="iconPosition"></param>
        /////// <param name="values"></param>
        /////// <returns></returns>
        ////public static MvcHtmlString JQM_Checkbox(this HtmlHelper htmlHelper, string name, Enums.OrientationType orientation, Enums.PositionType iconPosition, MultiSelectList values)
        ////{
        ////    var elements = new List<MvcHtmlString>();
        ////    foreach (var item in values)
        ////    {
        ////        string id = String.Format("{0}_{1}", name, item.Value);
        ////        elements.Add(JQM_Checkbox(htmlHelper, id, item.Selected, item.Text));
        ////    }

        ////    return Helper.JQMFieldOptionWrap(orientation, iconPosition, elements.ToArray());
        ////}

        ///// <summary>
        ///// Criação de checkbox
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="name"></param>
        ///// <param name="isChecked"></param>
        ///// <param name="label"></param>
        ///// <param name="hasContainer"></param>
        ///// <returns></returns>
        ////public static MvcHtmlString JQM_Checkbox(this HtmlHelper htmlHelper, string name, bool isChecked = false, string label = null, bool hasContainer = false)
        ////{
        ////    IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
        ////    if (isChecked)
        ////        htmlAttributes.Add(new KeyValuePair<string, object>("checked", "checked"));

        ////    return JQM_Input(htmlHelper, Enums.InputType.Checkbox, name, null, label, hasContainer);
        ////}

        //#endregion

        //#region [Datepicker]

        /////// <summary>
        /////// Componente de seleção de data
        /////// </summary>
        /////// <param name="htmlHelper"></param>
        /////// <param name="name"></param>
        /////// <param name="label"></param>
        /////// <param name="isInline"></param>
        /////// <param name="hasContainer">Agrupar no container padrão de componente</param>
        /////// <param name="htmlAttributes">Atributos adicionais do componente</param>
        /////// <returns></returns>
        ////public static MvcHtmlString JQM_Datepicker(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool isInline = false, bool hasContainer = false)
        ////{
        ////    IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
        ////    htmlAttributes.Add(new KeyValuePair<string, object>("data-role", "date"));
        ////    htmlAttributes.Add(new KeyValuePair<string, object>("data-inline", isInline.ToString().ToLower()));
        ////    return JQM_Input(htmlHelper, Enums.InputType.Text, name, value, label, hasContainer, htmlAttributes: htmlAttributes);
        ////}

        //#endregion

        //#region [Label]

        ///// <summary>
        ///// Criação de label
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="label"></param>
        ///// <param name="forElement"></param>
        ///// <returns></returns>
        //public static MvcHtmlString JQM_Label(this HtmlHelper htmlHelper, string label, string forElement = null)
        //{
        //    return JQM_Label(htmlHelper, label, forElement, null);
        //}

        ///// <summary>
        ///// Componente Label
        ///// </summary>
        ///// <param name="htmlHelper"></param>
        ///// <param name="label">Valor a se exibido pelo Label</param>
        ///// <param name="forElement">Nome do componente a ser referenciado ao label</param>
        ///// <param name="elements">Label atuará como container mantendo os controles</param>
        ///// <returns></returns>
        //public static MvcHtmlString JQM_Label(this HtmlHelper htmlHelper, string name, string label, string forElement = null, params MvcHtmlString[] elements)
        //{
        //    TagBuilder labelTag = new TagBuilder("label");
        //    if (!string.IsNullOrEmpty(forElement))
        //        labelTag.MergeAttribute("for", forElement);

        //    if (elements != null)
        //    {
        //        foreach (var item in elements)
        //            labelTag.InnerHtml += item.ToHtmlString();
        //    }

        //    labelTag.SetInnerText(label);
        //    return new MvcHtmlString(labelTag.ToString());
        //}

        //#endregion

        //#region [Input]

        //public static MvcHtmlString JQM_Color(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Color, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Date(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Date, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Datetime(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Datetime, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Email(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Email, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_File(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.File, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Month(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Month, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Number(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Number, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Search(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Search, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Telephone(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Telephone, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Time(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Time, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_URL(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.URL, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Week(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Week, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_TextArea(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.TextArea, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //public static MvcHtmlString JQM_Textbox(this HtmlHelper htmlHelper, string name, object value = null, string label = null, bool hasContainer = false, bool hasClear = false, string placeholder = null, IDictionary<string, object> htmlAttributes = null)
        //{
        //    return JQM_Input(htmlHelper, Enums.InputType.Text, name, value, label, hasContainer, hasClear, placeholder, htmlAttributes);
        //}

        //#endregion

        //public static MvcHtmlString JQM_Input(this HtmlHelper htmlHelper, string name, object value, string placeHolder)
        //{
        //    Config config = new Config()
        //    {
        //        Name = name,
        //        Value = value,
        //        PlaceHolder = placeHolder
        //    };
        //    return JQM_Input(htmlHelper, config);
        //}

        //public static MvcHtmlString JQM_Input(this HtmlHelper htmlHelper, Config config)
        //{
        //    TagBuilder tag = new TagBuilder("input");
        //    config.SetAttributes(tag);
        //    return new MvcHtmlString(tag.ToString());
        //}

        public static Section JQM_Page(this HtmlHelper htmlHelper, string name, bool isDialog)
        {
            return JQM_Page(htmlHelper, new PageConfig(name) { IsDialog = isDialog });
        }

        public static Section JQM_Page(this HtmlHelper htmlHelper, string name)
        {
            return JQM_Page(htmlHelper, new PageConfig(name));
        }

        public static Section JQM_Page(this HtmlHelper htmlHelper, PageConfig config)
        {
            TagBuilder tag = new TagBuilder("div");
            config.SetAttributes(tag);
            return new Section(htmlHelper.ViewContext, tag);
        }

        public static Section JQM_NavBar(this HtmlHelper htmlHelper, string name = null)
        {
            SectionConfig config = new SectionConfig(name, Enums.ContainerType.NavBar);
            return JQM_Section(htmlHelper, config);
        }

        public static Section JQM_Footer(this HtmlHelper htmlHelper, string name, bool isFixed = false, bool isFullScreen = false)
        {
            SectionConfig config = new SectionConfig(name, Enums.ContainerType.Footer) { IsFixed = isFixed, IsFullScreen = isFullScreen };
            return JQM_Section(htmlHelper, config);
        }

        public static Section JQM_Header(this HtmlHelper htmlHelper, string name, bool isFixed = false, bool isFullScreen = false, bool createBackButton = false, string backButtonText = null)
        {
            SectionConfig config = new SectionConfig(name, Enums.ContainerType.Header) { IsFixed = isFixed, IsFullScreen = isFullScreen, CreateBackButton = createBackButton, BackButtonText = backButtonText };
            return JQM_Section(htmlHelper, config);
        }

        public static Section JQM_Main(this HtmlHelper htmlHelper, string name)
        {
            SectionConfig config = new SectionConfig(name, Enums.ContainerType.Main) { ClassName = "ui-content" };
            return JQM_Section(htmlHelper, config);
        }

        public static Section JQM_Section(this HtmlHelper htmlHelper, SectionConfig config)
        {
            TagBuilder tag = new TagBuilder("div");
            config.SetAttributes(tag);
            return new Section(htmlHelper.ViewContext, tag);
        }

        public static Section JQM_ControlGroup(this HtmlHelper htmlHelper, ControlGroupConfig config)
        {
            TagBuilder fieldsetTag = new TagBuilder("fieldset");
            config.SetAttributes(fieldsetTag);
            return new Section(htmlHelper.ViewContext, fieldsetTag);
        }

        /// <summary>
        /// Exibe uma hint em cima do componente 
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name">Nome do hint</param>
        /// <param name="relatedElement">Nome do componente que terá o hint</param>
        /// <returns></returns>
        public static Section JQM_Hint(this HtmlHelper htmlHelper, string name, string relatedElement)
        {
            System.Text.StringBuilder clientScript = new System.Text.StringBuilder();
            clientScript.AppendFormat("   $.mobile.document.on(\"click\", \"#{0}\", function (evt) {{\n", relatedElement);
            clientScript.AppendFormat("       $(\"#{0}\").popup(\"open\", {{ x: evt.pageX, y: evt.pageY }});\n", name);
            clientScript.AppendLine("         evt.preventDefault();");
            clientScript.AppendLine("     })");
            htmlHelper.ViewDataContainer.ViewData.Add("Script", clientScript.ToString());
            return JQM_Popup(htmlHelper, new PopupConfig(name) { ClassName = "ui-corner-all", Arrow = "true" });
        }

        public static Section JQM_Popup(this HtmlHelper htmlHelper, string name)
        {
            return JQM_Popup(htmlHelper, new PopupConfig(name) { ClassName = "ui-corner-all" });
        }

        public static Section JQM_Popup(this HtmlHelper htmlHelper, PopupConfig config)
        {
            TagBuilder fieldsetTag = new TagBuilder("div");
            config.SetAttributes(fieldsetTag);
            return new Section(htmlHelper.ViewContext, fieldsetTag);
        }

        public static MvcHtmlString JQM_ControlGroup(this HtmlHelper htmlHelper, ControlGroupConfig config, params MvcHtmlString[] elements)
        {
            TagBuilder fieldsetTag = new TagBuilder("fieldset");
            config.SetAttributes(fieldsetTag);
            foreach (var item in elements)
                fieldsetTag.InnerHtml += item.ToHtmlString();

            return new MvcHtmlString(fieldsetTag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_Label(this HtmlHelper htmlHelper, string label, string forElement = null)
        {
            TagBuilder labelTag = new TagBuilder("label");
            labelTag.SetInnerText(label);
            if (!string.IsNullOrEmpty(forElement))
                labelTag.MergeAttribute("for", forElement);

            return new MvcHtmlString(labelTag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_Submit(this HtmlHelper htmlHelper, string name)
        {
            return JQM_Submit(htmlHelper, name, null, Enums.IconType.Action, Enums.PositionType.Top, Enums.ThemeType.a);
        }

        public static MvcHtmlString JQM_Submit(this HtmlHelper htmlHelper, string name, string text)
        {
            return JQM_Submit(htmlHelper, name, text, Enums.IconType.Action, Enums.PositionType.Top, Enums.ThemeType.a);
        }

        private static MvcHtmlString JQM_Submit(this HtmlHelper htmlHelper, string name, string text, Enums.IconType icon, Enums.PositionType iconPosition, Enums.ThemeType theme)
        {
            return JQM_Button(htmlHelper, new ButtonConfig(name, Enums.ButtonType.Submit) { Value = text, Icon = icon, IconPosition = iconPosition, Theme = theme });
        }

        public static MvcHtmlString JQM_Submit(this HtmlHelper htmlHelper, ButtonConfig config)
        {
            config.ButtonType = Enums.ButtonType.Submit;
            return JQM_Button(htmlHelper, config);
        }

        public static MvcHtmlString JQM_Reset(this HtmlHelper htmlHelper, string name)
        {
            return JQM_Button(htmlHelper, new ButtonConfig(name, Enums.ButtonType.Reset));
        }

        public static MvcHtmlString JQM_Button(this HtmlHelper htmlHelper, string name, string text, SpongeSolutions.Core.Extension.Enums.ButtonType buttonType, Enums.IconType icon, Enums.PositionType iconPosition = Enums.PositionType.Left)
        {
            return JQM_Button(htmlHelper, new ButtonConfig(name, buttonType) { Value = text, Icon = icon, IconPosition = iconPosition });
        }

        public static MvcHtmlString JQM_Button(this HtmlHelper htmlHelper, ButtonConfig config)
        {
            TagBuilder tag = null;
            if (config == null || config.Value == null)
                config.Value = string.Empty;

            switch (config.ButtonType)
            {
                case Enums.ButtonType.Button:
                    tag = new TagBuilder("button");
                    tag.SetInnerText(config.Value.ToString());
                    break;
                case Enums.ButtonType.Submit:
                case Enums.ButtonType.Reset:
                    tag = new TagBuilder("input");
                    break;
                case Enums.ButtonType.Link:
                    tag = new TagBuilder("a");
                    tag.SetInnerText(config.Value.ToString());
                    break;
            }
            config.SetAttributes(tag);

            if (config.ButtonType == Enums.ButtonType.Reset || config.ButtonType == Enums.ButtonType.Submit)
                return new MvcHtmlString(tag.ToString());
            else
                return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString JQM_LinkBack(this HtmlHelper htmlHelper, string name, string text, SpongeSolutions.Core.Extension.Enums.PositionType iconPosition = Enums.PositionType.Left)
        {
            return JQM_LinkHelper(htmlHelper, name, text, null, null, null, null, Enums.RelType.Back, SpongeSolutions.Core.Extension.Enums.IconType.Back, iconPosition);
        }

        public static MvcHtmlString JQM_LinkPopup(this HtmlHelper htmlHelper, string name, string text, string href = null, Enums.IconType? icon = null, Enums.PositionType? iconPosition = null, PopupConfig popupConfig = null)
        {
            LinkConfig config = new LinkConfig(name, text)
            {
                Rel = Enums.RelType.Popup,
                Href = href,
                Icon = icon,
                IconPosition = iconPosition,
                PopupConfig = popupConfig
            };

            return JQM_Button(htmlHelper, config);
        }

        public static MvcHtmlString JQM_Link(this HtmlHelper htmlHelper, string name, string text, string href, Enums.IconType icon, Enums.PositionType iconPosition = Enums.PositionType.Left)
        {
            return JQM_LinkHelper(htmlHelper, name, text, href, null, null, null, Enums.RelType.External, icon, iconPosition);
        }

        public static MvcHtmlString JQM_Link(this HtmlHelper htmlHelper, string name, string text, string action, string controller, Enums.IconType icon, Enums.PositionType iconPosition = Enums.PositionType.Left)
        {
            return JQM_LinkHelper(htmlHelper, name, text, null, action, controller, null, null, icon, iconPosition);
        }

        public static MvcHtmlString JQM_Link(this HtmlHelper htmlHelper, string name, string text, string action, string controller, object objectRoute, Enums.IconType icon, Enums.PositionType iconPosition = Enums.PositionType.Left)
        {
            return JQM_LinkHelper(htmlHelper, name, text, null, action, controller, objectRoute, Enums.RelType.External, icon, iconPosition);
        }

        public static MvcHtmlString JQM_LinkHelper(this HtmlHelper htmlHelper, string name, string text, string href, string action, string controller, object objectRoute, SpongeSolutions.Core.Extension.Enums.RelType? rel, Enums.IconType? icon, Enums.PositionType? iconPosition = Enums.PositionType.Left)
        {
            if (!string.IsNullOrEmpty(action) || !string.IsNullOrEmpty(controller))
            {
                UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
                href = urlHelper.Action(action, controller, objectRoute);
            }

            if (string.IsNullOrEmpty(href))
                href = "#";

            LinkConfig config = new LinkConfig(name, text)
            {
                Rel = rel,
                Icon = icon,
                IconPosition = iconPosition,
                Href = href
            };

            return JQM_Button(htmlHelper, config);
        }

        public static MvcHtmlString JQM_Input(this HtmlHelper htmlHelper, InputConfig config)
        {
            TagBuilder tag = null;
            if (config.InputType == Enums.InputType.TextArea)
            {
                tag = new TagBuilder("textarea");
                config.SetAttributes(tag);
                return new MvcHtmlString(tag.ToString());
            }
            else
            {
                tag = new TagBuilder("input");
                config.SetAttributes(tag);
                return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
            }
        }

        public static MvcHtmlString JQM_Color(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Color) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_Date(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Date) { Name = name, Value = value, Mask = "integer" });
        }

        public static MvcHtmlString JQM_Datetime(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Datetime) { Name = name, Value = value, Mask = "datetime" });
        }

        public static MvcHtmlString JQM_Email(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Email) { Name = name, Value = value, Mask = "email" });
        }

        public static MvcHtmlString JQM_File(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.File) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_Month(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Month) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_Number(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Number) { Name = name, Value = value, Mask = "decimal" });
        }

        public static MvcHtmlString JQM_Search(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Search) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_Telephone(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Telephone) { Name = name, Value = value, Mask = "phone" });
        }

        public static MvcHtmlString JQM_Time(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Time) { Name = name, Value = value, Mask = "time" });
        }

        public static MvcHtmlString JQM_URL(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.URL) { Name = name, Value = value, Mask = "url" });
        }

        public static MvcHtmlString JQM_Week(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Week) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_TextArea(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.TextArea) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_TextBox(this HtmlHelper htmlHelper, string name, object value = null)
        {
            return JQM_Input(htmlHelper, new InputConfig(Enums.InputType.Text) { Name = name, Value = value });
        }

        public static MvcHtmlString JQM_FlipSwitch(this HtmlHelper htmlHelper, FlipswitchConfig config)
        {
            TagBuilder tag = new TagBuilder("input");
            config.Configuration.Add("type", "checkbox");
            config.Configuration.Add("data-role", "flipswitch");
            config.SetAttributes(tag);
            return new MvcHtmlString(tag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_CheckboxList(this HtmlHelper htmlHelper, string name, string legend, Enums.OrientationType orientationType, MultiSelectList values, bool isMini = false)
        {
            return JQM_MultiSelection(htmlHelper, Enums.InputType.CheckBox, name, legend, orientationType, values, isMini);
        }

        public static MvcHtmlString JQM_RadioList(this HtmlHelper htmlHelper, string name, string legend, Enums.OrientationType orientationType, MultiSelectList values, bool isMini = false)
        {
            return JQM_MultiSelection(htmlHelper, Enums.InputType.Radio, name, legend, orientationType, values, isMini);
        }

        private static MvcHtmlString JQM_MultiSelection(this HtmlHelper htmlHelper, Enums.InputType inputType, string name, string legend, Enums.OrientationType orientationType, MultiSelectList values, bool isMini = false)
        {
            List<MvcHtmlString> elements = new List<MvcHtmlString>();
            var list = values.ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                string id = String.Format("{0}_{1}", name, i);
                elements.Add(JQM_Label(htmlHelper, item.Text, id));
                elements.Add(JQM_Input(htmlHelper, new InputConfig(id, inputType) { Value = item.Value, Selected = item.Selected }));
            }
            return JQM_ControlGroup(htmlHelper, new ControlGroupConfig(orientationType) { Legend = legend, IsMini = isMini }, elements.ToArray());
        }

        public static MvcHtmlString JQM_SelectMenu(this HtmlHelper htmlHelper, SelectMenuConfig config, MultiSelectList values)
        {
            TagBuilder selectTag = new TagBuilder("select");
            foreach (var item in values)
            {
                var optionTag = new TagBuilder("option");
                optionTag.MergeAttribute("value", item.Value);
                optionTag.SetInnerText(item.Text);
                if (item.Selected)
                    optionTag.MergeAttribute("selected", "true");

                selectTag.InnerHtml += optionTag.ToString();
            }
            config.SetAttributes(selectTag);
            return new MvcHtmlString(selectTag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_GroupedSelectMenu(this HtmlHelper htmlHelper, SelectMenuConfig config, List<CustomSelectListItem> values)
        {
            TagBuilder selectTag = new TagBuilder("select");
            config.IsMultiple = true;
            foreach (var item in values)
            {
                var optgroupTag = new TagBuilder("optgroup");
                optgroupTag.MergeAttribute("label", item.Text);
                foreach (var option in item.Children)
                {
                    var optionTag = new TagBuilder("option");
                    optionTag.MergeAttribute("value", item.Value);
                    optionTag.SetInnerText(item.Text);
                    if (item.Selected)
                        optionTag.MergeAttribute("selected", "true");

                    optgroupTag.InnerHtml += optionTag.ToString();
                }
                selectTag.InnerHtml += optgroupTag.ToString();
            }
            config.SetAttributes(selectTag);
            return new MvcHtmlString(selectTag.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_Image(this HtmlHelper helper, string id, string url, string alternateText)
        {
            return JQM_Image(helper, id, url, alternateText, null);
        }

        public static MvcHtmlString JQM_Image(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        {
            // Create tag builder
            var builder = new TagBuilder("img");

            // Create valid id
            builder.GenerateId(id);

            // Add attributes
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render tag
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JQM_DatePicker(this HtmlHelper htmlHelper, string name, object value = null, bool isInline = false)
        {
            var config = new InputConfig(name, Enums.InputType.Text) { IsInline = isInline };
            config.Configuration.Add("data-role", "date");
            return JQM_Input(htmlHelper, config);
        }

        public static MvcHtmlString JQM_CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, FlipswitchConfig config)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "ui-field-contain");
            config.Configuration.Add("data-role", "flipswitch");
            tagResult.InnerHtml += InputExtensions.CheckBoxFor<TModel>(htmlHelper, expression, config.GetAttributes()).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString JQM_PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string placeHolder = null)
        {
            return JQM_TextBoxFor(htmlHelper, expression, new InputConfig(Enums.InputType.Password) { PlaceHolder = placeHolder });
        }

        public static MvcHtmlString JQM_TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return JQM_TextBoxFor(htmlHelper, expression, new InputConfig(Enums.InputType.Text));
        }

        public static MvcHtmlString JQM_TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, InputConfig config = null, IDictionary<string, object> htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "ui-field-contain");
            if (config == null || (config != null && string.IsNullOrEmpty(config.PlaceHolder)))
                tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();

            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();

            if (config != null)
            {
                foreach (var item in config.GetAttributes())
                {
                    if (htmlAttributes.Count(p => p.Key == item.Key) == 0)
                        htmlAttributes.Add(item);
                }
            }
            tagResult.InnerHtml += InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString JQM_AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string placeHolder, string serviceUrl, string pageName)
        {
            StringBuilder clientScript = new StringBuilder();
            MemberExpression body = expression.Body as MemberExpression;
            string fieldName = body.Member.Name;
            string filterName = String.Format("FilterFor{0}", fieldName);
            ListViewConfig config = new ListViewConfig(filterName) { IsFilterable = true, PlaceHolder = placeHolder, Inset = true };
            TagBuilder tagResult = new TagBuilder("div");
            TagBuilder ulResult = new TagBuilder("ul");
            clientScript.AppendFormat("    function SelectResult{0}(keyField, valueField, key, value) {{\n", filterName);
            clientScript.AppendLine("        $(\"#\" + keyField).val(key);");
            clientScript.AppendLine("        $(\"#\" + valueField).val(value);");
            clientScript.AppendFormat("        $(\"#{0}\").html(\"\");\n", filterName);
            clientScript.AppendLine("    }");

            clientScript.AppendFormat("$(document).on(\"pagecreate\", \"#{0}\", function () {{\n", pageName);
            clientScript.AppendFormat("    $(\"#{0}\").on(\"filterablebeforefilter\", function (e, data) {{\n", filterName);
            clientScript.AppendLine("        var $ul = $(this),");
            clientScript.AppendLine("               $input = $(data.input),");
            clientScript.AppendLine("               value = $input.val(),");
            clientScript.AppendLine("               html = \"\";");
            clientScript.AppendLine("       $ul.html(\"\");");
            clientScript.AppendFormat("     $input.attr('id', 'Description{0}');\n", filterName);
            clientScript.AppendFormat("     $input.attr('name', 'Description{0}');\n", filterName);
            clientScript.AppendLine("       if ($input.val().length >= 4 )");
            clientScript.AppendLine("       {");
            clientScript.AppendLine("        $.ajax({");
            clientScript.AppendFormat("          url: \"{0}\",\n", serviceUrl);
            clientScript.AppendLine("            dataType: \"json\",");
            clientScript.AppendLine("            data: {");
            //clientScript.AppendLine("                minLength: 4,");
            //clientScript.AppendLine("                maxRows: 12,");
            clientScript.AppendLine("                name_startsWith: $input.val()");
            clientScript.AppendLine("            },");
            clientScript.AppendLine("            success: function (data) {");
            clientScript.AppendLine("                $.each(data.ResponseData.Result[0], function (i, val) {");
            clientScript.AppendFormat("                    html +=  \"<li><a href=\\\"javascript:SelectResult{0}('{1}', 'Description{0}', \" + val.id + \", '\" + val.label + \"')\\\" >\" + val.label + \"</a></li>\"", filterName, fieldName);
            clientScript.AppendLine("                });");
            clientScript.AppendLine("                $ul.html(html);");
            clientScript.AppendLine("                $ul.listview(\"refresh\");");
            clientScript.AppendLine("                $ul.trigger(\"updatelayout\");");
            clientScript.AppendLine("            }");
            clientScript.AppendLine("        });");
            clientScript.AppendLine("      }");
            clientScript.AppendLine("    });");
            clientScript.AppendLine("});");

            htmlHelper.ViewDataContainer.ViewData.Add("Script", clientScript.ToString());
            tagResult.MergeAttribute("class", "ui-field-contain");
            ulResult.MergeAttributes(config.GetAttributes());
            ulResult.MergeAttribute("id", filterName);

            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += InputExtensions.HiddenFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += ulResult.ToHtml();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString JQM_SelectMenuFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MultiSelectList values, string optionLabel = null, SelectMenuConfig config = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "ui-field-contain");
            if (config == null)
                config = new SelectMenuConfig();

            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += SelectExtensions.DropDownListFor(htmlHelper, expression, values, optionLabel, (config != null) ? config.GetAttributes() : null).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString JQM_SelectMenu(this HtmlHelper htmlHelper, string name, string label, MultiSelectList values, string optionLabel = null, SelectMenuConfig config = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "ui-field-contain");
            if (config == null)
                config = new SelectMenuConfig();

            tagResult.InnerHtml += LabelExtensions.Label(htmlHelper, "", label).ToHtmlString();
            tagResult.InnerHtml += SelectExtensions.DropDownList(htmlHelper, name, values, optionLabel, (config != null) ? config.GetAttributes() : null).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString JQM_SelectMenuGroupedFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, string optionLabel = null, SelectMenuConfig config = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "ui-field-contain");
            if (config == null)
                config = new SelectMenuConfig();

            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += HtmlHelpers.DropDownGroupListFor(htmlHelper, expression, selectList, optionLabel, (config != null) ? config.GetAttributes() : null).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            return tagResult.ToHtml();
        }
    }
}
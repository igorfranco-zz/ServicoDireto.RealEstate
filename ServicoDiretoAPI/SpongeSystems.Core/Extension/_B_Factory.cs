using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;

namespace SpongeSolutions.Core.Extension
{
    public enum ComponentType : short
    {
        Input = 0,
        TextArea = 1
        //Select = 2
    }

    public enum MessageType : short
    {
        Error = 0,
        Notice = 1,
        Success = 2
    }

    public static class B_Factory
    {
        private static StringBuilder SpongeDropDownScript(string childController, string childAction, string childOptionalLabel, string childFieldName)
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine("<script language='javascript' type='text/javascript'>");
            script.AppendFormat("    function FillChildControl_{0}(params) {{\n", childFieldName);
            script.AppendLine("        if (params != null) {");
            script.AppendFormat("            NBN.REST.Execute('/{0}.aspx/{1}', params, function (data) {{", childController, childAction);
            script.AppendLine("                data = JSON.parse(data);");
            script.AppendLine("                if (data.ResponseData.Status == 200) //Sucesso");
            script.AppendFormat("                    NBN.Helper.FillDropDown('{0}', data, {{ Text: '{1}', Value: '' }})\n", childFieldName, childOptionalLabel);
            script.AppendLine("                else");
            script.AppendFormat("                    NBN.Helper.ShowModalWindowJqueryUI(data.ResponseData.Details, '{0}', {{ width: 300, heigth: 200 }}, false, true);\n", "");
            script.AppendLine("            }, 'POST');");
            script.AppendLine("        }");
            script.AppendLine("        else {            ");
            script.AppendFormat("            $('#{0}').html('');\n", childFieldName);
            script.AppendLine("        }");
            script.AppendLine("    }");
            script.AppendLine("</script>");
            return script;
        }

        public static MvcHtmlString B_TextBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string icon = null, string sectionClass = null, bool useLabel = true, bool usePlaceHolder = false, bool disabled = false, string tooltip = null)
        {
            return InputHelper(htmlHelper, expression, ComponentType.Input, icon, sectionClass, useLabel, usePlaceHolder, disabled, null, null, tooltip);
        }

        public static MvcHtmlString B_Select<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, string optionLabel, string sectionClass = null)
        {
            return B_Select(htmlHelper, expression, items, optionLabel, null, null, null, null, sectionClass, null);
        }

        public static MvcHtmlString B_Select<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items, string optionLabel, string childController, string childAction, string childOptionalLabel, string childFieldName, string sectionClass = null, string parameters = null)
        {
            var attributes = new Dictionary<string, object>();
            attributes.Add("class", "input-sm");            
            StringBuilder output = new StringBuilder();
            TagBuilder section = new TagBuilder("section");
            TagBuilder labelContainer = new TagBuilder("label");
            TagBuilder i = new TagBuilder("i");
            labelContainer.AddCssClass("select");
            if (!string.IsNullOrEmpty(childController))
            {
                attributes.Add("onchange", string.Format("javascript:FillChildControl_{0}({1});", childFieldName, parameters));
                output.Append(SpongeDropDownScript(childController, childAction, childOptionalLabel, childFieldName));
            }

            section.InnerHtml += System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();           
            if (!string.IsNullOrEmpty(sectionClass))
                section.MergeAttribute("class", sectionClass);

            labelContainer.InnerHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();
            output.AppendLine(System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, items, optionLabel, attributes).ToHtmlString());
            labelContainer.InnerHtml += output.ToString();
            section.InnerHtml += labelContainer.ToString();
            return MvcHtmlString.Create(section.ToString());
        }

        public static MvcHtmlString InputHelper<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, ComponentType component, string icon, string sectionClass, bool useLabel, bool usePlaceHolder, bool disabled, IEnumerable<SelectListItem> items, string optionLabel, string tooltip)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            TagBuilder section = new TagBuilder("section");
            TagBuilder labelContainer = new TagBuilder("label");
            TagBuilder i = new TagBuilder("i");
            labelContainer.AddCssClass(component.ToString().ToLower());

            //if (string.IsNullOrEmpty(tooltip))
            //    tooltip = string.Format(Internationalization.Label.Type_Field, metadata.DisplayName);

            if (disabled)
                tooltip = string.Empty;

            if (!string.IsNullOrEmpty(sectionClass))
                section.MergeAttribute("class", sectionClass);

            if (useLabel)
                section.InnerHtml += System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();

            if (component == ComponentType.Input)
            {
                IDictionary<string, object> inputHtmlAttributes = new Dictionary<string, object>();
                if (usePlaceHolder)
                    inputHtmlAttributes.Add("placeholder", metadata.DisplayName);

                if (disabled)
                {
                    labelContainer.AddCssClass("state-disabled");
                    inputHtmlAttributes.Add("disabled", "disabled");
                }
                labelContainer.InnerHtml += System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, inputHtmlAttributes).ToHtmlString();
            }
            //else if (component == ComponentType.Select)
            //{
            //    var attributes = new Dictionary<string, object>();
            //    if (disabled)
            //        attributes.Add("disabled", "disabled");

            //    labelContainer.InnerHtml += System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, items, optionLabel, new { @class = "input-sm" }).ToHtmlString();
            //}

            if (!string.IsNullOrEmpty(tooltip))
            {
                TagBuilder b = new TagBuilder("b");
                b.MergeAttribute("class", "tooltip tooltip-bottom-right");
                b.SetInnerText(tooltip);
                labelContainer.InnerHtml += b.ToString();
            }

            labelContainer.InnerHtml += System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString();

            if (!string.IsNullOrEmpty(icon))
            {
                i.MergeAttribute("class", string.Format("icon-append fa {0}", icon));
                labelContainer.InnerHtml += i.ToString();
            }

            section.InnerHtml += labelContainer.ToString();
            return MvcHtmlString.Create(section.ToString());
        }

        public static MvcHtmlString B_ValidationScript<TModel>(this HtmlHelper<TModel> htmlHelper, string formName)
        {
            StringBuilder stbScript = new StringBuilder();
            StringBuilder stbScriptRules = new StringBuilder();
            StringBuilder stbScriptMessages = new StringBuilder();

            foreach (var property in htmlHelper.ViewData.ModelMetadata.ModelType.GetProperties())
            {
                stbScriptRules.AppendFormat("                {0}: {{\n", property.Name);
                stbScriptMessages.AppendFormat("                {0}: {{\n", property.Name);
                foreach (var item in property.GetCustomAttributes(true))
                {
                    if (item is RequiredAttribute)
                    {
                        stbScriptRules.AppendLine("                    required: true,");
                        stbScriptMessages.AppendFormat("                    required: '{0}',\n", ((RequiredAttribute)item).FormatErrorMessage(""));

                    }
                    else if (item is DisplayAttribute)
                    {

                    }
                    else if (item is StringLengthAttribute)
                    {
                        var stringLengthAttribute = ((StringLengthAttribute)item);
                        stbScriptRules.AppendFormat("                    minlength: {0},\n", stringLengthAttribute.MinimumLength);
                        stbScriptRules.AppendFormat("                    maxlength: {0},\n", stringLengthAttribute.MaximumLength);
                    }
                    //else if (item is EmailAddressAttribute)
                    //{
                    //    stbScriptRules.AppendLine("                    email: true,");
                    //    stbScriptMessages.AppendFormat("                    required: '{0}',\n", ((EmailAddressAttribute)item).FormatErrorMessage(""));
                    //}
                }
                stbScriptRules.AppendLine("                },");
                stbScriptMessages.AppendLine("                },");
            }

            stbScript.AppendLine("<script type='text/javascript'>");
            stbScript.AppendLine("    $(document).ready(function () {");
            stbScript.AppendLine("        var $checkoutForm = $('#" + formName + "').validate({");
            stbScript.AppendLine("            // Rules for form validation");
            stbScript.AppendLine("            rules: {");
            stbScript.Append(stbScriptRules.ToString());
            stbScript.AppendLine("            },");
            stbScript.AppendLine("            // Messages for form validation");
            stbScript.AppendLine("            messages: {");
            stbScript.Append(stbScriptMessages.ToString());
            stbScript.AppendLine("            },");
            stbScript.AppendLine("            // Do not change code below");
            stbScript.AppendLine("            errorPlacement: function (error, element) {");
            stbScript.AppendLine("                error.insertAfter(element.parent());");
            stbScript.AppendLine("            }");
            stbScript.AppendLine("        });");
            stbScript.AppendLine("    });");
            stbScript.AppendLine("</script>");
            return new MvcHtmlString(stbScript.ToString());
        }

        public static MvcHtmlString B_Message(this HtmlHelper htmlHelper, string message, string header, MessageType messageType)
        {
            StringBuilder result = new StringBuilder();
            string baseClass = string.Empty;
            switch (messageType)
            {
                case MessageType.Error:
                    baseClass = "alert-error";
                    break;
                case MessageType.Notice:
                    baseClass = "alert-info";
                    break;
                case MessageType.Success:
                    baseClass = "alert-success";
                    break;
            }

            result.AppendFormat("<div class=\"alert alert-block {0}\">\n", baseClass);
            result.AppendLine("	<a href=\"#\" data-dismiss=\"alert\" class=\"close\">×</a>");
            result.AppendFormat("	<h4 class=\"alert-heading\"><i class=\"fa fa-check-square-o\"></i>{0}</h4>\n", header);
            result.AppendLine("	<p>");
            result.AppendLine(message);
            result.AppendLine("	</p>");
            result.AppendLine("</div>");
            return new MvcHtmlString(result.ToString());
        }

        public static MvcHtmlString B_Header(this HtmlHelper htmlHelper, string header)
        {

            StringBuilder result = new StringBuilder();
            result.AppendLine("	<header role=\"heading\">");
            result.AppendLine("		<div class=\"jarviswidget-ctrls\" role=\"menu\"><a data-placement=\"bottom\" title=\"\" rel=\"tooltip\" class=\"button-icon jarviswidget-toggle-btn\" href=\"#\" data-original-title=\"Collapse\"><i class=\"fa fa-minus \"></i></a><a data-placement=\"bottom\" title=\"\" rel=\"tooltip\" class=\"button-icon jarviswidget-fullscreen-btn\" href=\"javascript:void(0);\" data-original-title=\"Fullscreen\"><i class=\"fa fa-resize-full \"></i></a><a data-placement=\"bottom\" title=\"\" rel=\"tooltip\" class=\"button-icon jarviswidget-delete-btn\" href=\"javascript:void(0);\" data-original-title=\"Delete\"><i class=\"fa fa-times\"></i></a></div>		");
            result.AppendLine("		<div class=\"widget-toolbar\" role=\"menu\">		");
            result.AppendLine("			<a href=\"javascript:void(0);\" class=\"dropdown-toggle color-box selector\" data-toggle=\"dropdown\"></a>	");
            result.AppendLine("			<ul class=\"dropdown-menu arrow-box-up-right color-select pull-right\">	");
            result.AppendLine("				<li><span data-original-title=\"Green Grass\" data-placement=\"left\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-green\" class=\"bg-color-green\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Dark Green\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-greenDark\" class=\"bg-color-greenDark\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Light Green\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-greenLight\" class=\"bg-color-greenLight\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Purple\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-purple\" class=\"bg-color-purple\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Magenta\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-magenta\" class=\"bg-color-magenta\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Pink\" data-placement=\"right\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-pink\" class=\"bg-color-pink\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Fade Pink\" data-placement=\"left\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-pinkDark\" class=\"bg-color-pinkDark\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Light Blue\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-blueLight\" class=\"bg-color-blueLight\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Teal\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-teal\" class=\"bg-color-teal\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Ocean Blue\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-blue\" class=\"bg-color-blue\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Night Sky\" data-placement=\"top\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-blueDark\" class=\"bg-color-blueDark\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Night\" data-placement=\"right\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-darken\" class=\"bg-color-darken\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Day Light\" data-placement=\"left\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-yellow\" class=\"bg-color-yellow\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Orange\" data-placement=\"bottom\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-orange\" class=\"bg-color-orange\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Dark Orange\" data-placement=\"bottom\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-orangeDark\" class=\"bg-color-orangeDark\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Red Rose\" data-placement=\"bottom\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-red\" class=\"bg-color-red\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Light Red\" data-placement=\"bottom\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-redLight\" class=\"bg-color-redLight\"></span></li>");
            result.AppendLine("				<li><span data-original-title=\"Purity\" data-placement=\"right\" rel=\"tooltip\" data-widget-setstyle=\"jarviswidget-color-white\" class=\"bg-color-white\"></span></li>");
            result.AppendLine("				<li><a data-original-title=\"Reset widget color to default\" data-placement=\"bottom\" rel=\"tooltip\" data-widget-setstyle=\"\" class=\"jarviswidget-remove-colors\" href=\"javascript:void(0);\">Remove</a></li>");
            result.AppendLine("			</ul>	");
            result.AppendLine("		</div>		");
            result.AppendLine("		<span class=\"widget-icon\"><i class=\"fa fa-edit\"></i></span>		");
            result.AppendFormat("		<h2>{0}</h2>		", header);
            result.AppendLine("		<span class=\"jarviswidget-loader\"><i class=\"fa fa-refresh fa-spin\"></i></span>		");
            result.AppendLine("	</header>			");
            return new MvcHtmlString(result.ToString());
        }

        //public static MvcHtmlString B_Grid<T>(this HtmlHelper html, IList<T> items, string gridCaption = "", string idGrid = "grid", string tableStyle = "grid", bool isWindowMode = false, List<KeyValuePair<string, object>> parameters = null)
        //{
        //    StringBuilder output = new StringBuilder();
        //    if (items.Count() > 0)
        //    {
        //        List<GridColumnOrder> columns = new List<GridColumnOrder>();
        //        if (parameters == null)
        //            parameters = new List<KeyValuePair<string, object>>();

        //        parameters.Add(new KeyValuePair<string, object>("windowMode", isWindowMode));
        //        var grid = new WebGrid((IEnumerable<dynamic>)items, canPage: false, canSort: false);
        //        foreach (var property in typeof(T).GetProperties())
        //        {
        //            bool showColumn = true;
        //            var attributes = (property.GetCustomAttributes(typeof(GridConfigAttribute), false));
        //            if (attributes != null)
        //            {
        //                foreach (var attribute in attributes)
        //                {
        //                    string propertyName = property.Name;
        //                    var attr = (GridConfigAttribute)attribute;
        //                    string style = attr.Style;

        //                    showColumn = !attr.Hidden;
        //                    if (showColumn)
        //                        showColumn = !(isWindowMode && attr.HiddenOnWindowMode);

        //                    if (showColumn)
        //                        showColumn = attr.Index > 0;

        //                    if (attr.CreateCheckBox && !isWindowMode)
        //                    {
        //                        columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
        //                        {
        //                            Format = (dynamic item) =>
        //                            {
        //                                var entity = (T)(((WebGridRow)item).Value);
        //                                var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        //                                TagBuilder label = new TagBuilder("label");
        //                                TagBuilder i = new TagBuilder("i");
        //                                label.AddCssClass("checkbox");
        //                                TagBuilder input = new TagBuilder("input");
        //                                input.MergeAttribute("type", "checkbox");
        //                                input.MergeAttribute("value", objectValue.ToString());
        //                                input.GenerateId(string.Format("chk{0}", propertyName));
        //                                label.InnerHtml = input.ToString();
        //                                //label.InnerHtml += InputExtensions.CheckBox(html, string.Format("chk{0}", propertyName), new { @class = "checkable", value = objectValue }).ToHtmlString();
        //                                label.InnerHtml += i.ToString();
        //                                return new MvcHtmlString(label.ToString());
        //                            },
        //                            Style = style
        //                        }));
        //                    }

        //                    if (attr.CreateImage && !isWindowMode)
        //                    {
        //                        columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
        //                        {
        //                            //Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(propertyName),
        //                            Format = (dynamic item) =>
        //                            {
        //                                var entity = (T)(((WebGridRow)item).Value);
        //                                var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        //                                objectValue = String.Concat(SpongeSolutions.ServicoDireto.UI.SiteContext.LayoutPath, objectValue);
        //                                return new MvcHtmlString(string.Format("<img src='{0}'/>", objectValue));
        //                            },
        //                            Style = style
        //                        }));
        //                    }

        //                    if (attr.CreateEditLink)
        //                    {
        //                        columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
        //                        {
        //                            Format = (item) =>
        //                            {
        //                                var entity = (T)(((WebGridRow)item).Value);
        //                                try
        //                                {
        //                                    string objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null).ToString();
        //                                    string objectDescription = null;

        //                                    if (attr.RelatedFieldName != null && attr.RelatedFieldName.Count() > 0)
        //                                    {
        //                                        foreach (var fieldName in attr.RelatedFieldName)
        //                                            objectDescription += string.Format("{0}-", entity.GetType().GetProperty(fieldName).GetValue(entity, null));

        //                                        objectDescription = objectDescription.Remove(objectDescription.Length - 1, 1);
        //                                    }
        //                                    if (objectDescription == null)
        //                                        objectDescription = objectValue;

        //                                    if (isWindowMode)
        //                                        return new HtmlString(string.Format("<a href=\"javascript:AddSelected('{0}','{1}')\">{2}</a>", objectValue, objectDescription, SpongeSolutions.ServicoDireto.Internationalization.Label.Select));
        //                                    else
        //                                        return LinkExtensions.ActionLink(html, SpongeSolutions.ServicoDireto.Internationalization.Label.Edit, "create", new { id = objectValue });
        //                                }
        //                                catch (Exception)
        //                                {
        //                                    return string.Empty;
        //                                }
        //                            },
        //                            Style = style
        //                        }));
        //                    }

        //                    if (attr.EnumType != null)
        //                    {
        //                        columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
        //                        {
        //                            Style = style,
        //                            ColumnName = property.Name,
        //                            Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(property.Name),
        //                            Format = (item) =>
        //                            {
        //                                var entity = (T)(((WebGridRow)item).Value);
        //                                var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        //                                //var enumSource = Activator.CreateInstance(attr.EnumType);
        //                                //TODO: COLOCAR DE MODO DINAMICO
        //                                return SpongeSolutions.Core.Translation.EnumTranslator.Translate<Enums.StatusType>(SpongeSolutions.Core.Helpers.EnumHelper.TryParse<Enums.StatusType>(objectValue)).DisplayName;
        //                            }
        //                        }));
        //                        showColumn = false;
        //                    }

        //                    if (showColumn)
        //                        columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
        //                        {
        //                            ColumnName = property.Name,
        //                            Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(property.Name),
        //                            Style = style
        //                        }));
        //                }
        //            }
        //        }

        //        output.AppendLine(MvcHtmlString.Create(grid.GetHtml(
        //           tableStyle: tableStyle,
        //           caption: gridCaption,
        //            //alternatingRowStyle: "alternating",
        //           htmlAttributes: new { id = idGrid },
        //           columns: columns.OrderBy(p => p.Index).Select(p => p.Column).ToArray()).ToHtmlString()).ToHtmlString());
        //    }
        //    else
        //    {
        //        var container = new TagBuilder("div");
        //        container.AddCssClass("notice");
        //        container.SetInnerText(SpongeSolutions.ServicoDireto.Internationalization.Label.Records_Not_Found);
        //        output.Append(container.ToString());
        //    }
        //    return MvcHtmlString.Create(output.ToString());
        //}

        public static MvcHtmlString B_EditorForBaseEntity<TModel>(this HtmlHelper<TModel> htmlHelper, bool showEditor)
        {
            if (showEditor)
            {
                StringBuilder output = new StringBuilder();
                output.AppendLine(System.Web.Mvc.Html.EditorExtensions.EditorForModel(htmlHelper).ToHtmlString());
                return MvcHtmlString.Create(output.ToString());
            }
            else
                return MvcHtmlString.Create(System.Web.Mvc.Html.InputExtensions.Hidden(htmlHelper, "Status", "1").ToHtmlString());
        }

        public static MvcHtmlString B_FindTextBoxFor<TModel, TProperty, TProperty1>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> keySelector, Expression<Func<TModel, TProperty1>> valueSelector, string searchTitle, string searchController, string searchAction, string parameters)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, keySelector).ToHtmlString() + "<br />");
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, valueSelector, new { @class = "xbig", @readonly = "true" }).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, keySelector).ToHtmlString());
            string script = string.Format("NBN.REST.Execute( '/{0}.aspx/{1}', {2}, function (data) {{ NBN.Helper.ShowModalWindowJqueryUI(data, '{3}', {{ width: 680, height: 500 }}, false, true); }} , 'POST');", searchController, searchAction, parameters, searchTitle);
            output.AppendLine(string.Format("<input type=\"button\" id=\"findButton\" class=\"find\" onclick=\"{0}\" />", script));
            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, valueSelector).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString B_AutoCompleteFor<TModel, TProperty, TProperty1>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> keySelector, Expression<Func<TModel, TProperty1>> valueSelector, string url, string icon = null, string sectionClass = null, bool useLabel = true, bool usePlaceHolder = false, bool disabled = false, string tooltip = null, string parameter = null)
        {
            string idField = keySelector.Body.ToString();
            idField = idField.Substring(idField.IndexOf(".") + 1);

            string idValue = valueSelector.Body.ToString();
            idValue = idValue.Substring(idValue.IndexOf(".") + 1);

            StringBuilder output = new StringBuilder();
            var page = htmlHelper.ViewDataContainer as WebViewPage;

            StringBuilder script = new StringBuilder();
            script.AppendLine("<script type=\"text/javascript\">");
            script.AppendLine("    $(function () {");
            script.AppendFormat("        NBN.Helper.LoadAutoComplete(\"{0}\", \"{1}\", \"{2}\"", idField, idValue, url);

            if (!string.IsNullOrEmpty(parameter))
                script.AppendFormat(", {0}", parameter);
            script.AppendFormat(");\n");

            script.AppendLine("  });");
            script.AppendLine("</script>");
            page.ViewBag.Script += script.ToString();
            output.AppendLine(B_TextBox(htmlHelper, valueSelector, icon, sectionClass, useLabel, usePlaceHolder, disabled, tooltip).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, keySelector).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, valueSelector).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }
    }

    public static class SectionExtensions
    {
        private static readonly object _o = new object();

        public static HelperResult RenderSection(this WebPageBase page,
                                string sectionName,
                                Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                return page.RenderSection(sectionName);
            }
            else
            {
                return defaultContent(_o);
            }
        }

        public static HelperResult RedefineSection(this WebPageBase page,
                                string sectionName)
        {
            return RedefineSection(page, sectionName, defaultContent: null);
        }

        public static HelperResult RedefineSection(this WebPageBase page,
                                string sectionName,
                                Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                page.DefineSection(sectionName,
                                   () => page.Write(page.RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                page.DefineSection(sectionName,
                                   () => page.Write(defaultContent(_o)));
            }
            return new HelperResult(_ => { });
        }
    }

    //public class GridColumnOrder
    //{
    //    public int Index { get; set; }
    //    public WebGridColumn Column { get; set; }
    //    public GridColumnOrder(int Index, WebGridColumn Column)
    //    {
    //        this.Index = Index;
    //        this.Column = Column;
    //    }
    //}

}
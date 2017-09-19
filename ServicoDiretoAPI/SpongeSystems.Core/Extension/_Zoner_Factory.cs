using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using SpongeSolutions.Core.Model;

namespace SpongeSolutions.Core.Extension
{
    public static class ZonerFactory
    {
        public static IDictionary<string, object> ToDictionary(this object data)
        {
            if (data == null) return null; // Or throw an ArgumentNullException if you want

            BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            foreach (PropertyInfo property in
                     data.GetType().GetProperties(publicAttributes))
            {
                if (property.CanRead)
                {
                    dictionary.Add(property.Name, property.GetValue(data, null));
                }
            }
            return dictionary;
        }

        public static MvcHtmlString ZN_Submit(this HtmlHelper htmlHelper, string name, string label, object htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("button");
            tagResult.MergeAttribute("type", "submit");
            tagResult.MergeAttribute("name", name);
            tagResult.MergeAttribute("id", name);
            if (htmlAttributes != null)
                tagResult.MergeAttributes(htmlAttributes.ToDictionary());

            tagResult.InnerHtml += label;
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_Message(this HtmlHelper htmlHelper, Enums.MessageType messageType, string message)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", string.Format("alert alert-{0}", messageType.ToString().ToLower()));
            tagResult.MergeAttribute("role", "alert");
            tagResult.InnerHtml += message;
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_Message(this HtmlHelper htmlHelper)
        {
            if (htmlHelper.ViewContext.TempData["Message"] != null)
            {
                ResultMessage resultMessage = (ResultMessage)htmlHelper.ViewContext.TempData["Message"];
                return ZN_Message(htmlHelper, resultMessage.MessageType, resultMessage.Message);
            }
            else
                return new MvcHtmlString("");
        }

        public static MvcHtmlString ZN_TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "form-group");
            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression, "", new { @class = "error" }).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_TextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "form-group");
            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += TextAreaExtensions.TextAreaFor(htmlHelper, expression, htmlAttributes).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression, "", new { @class = "error" }).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "form-group");
            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += InputExtensions.PasswordFor(htmlHelper, expression, htmlAttributes).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression, "", new { @class = "error" }).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel = null, object htmlAttributes = null)
        {
            TagBuilder tagResult = new TagBuilder("div");
            tagResult.MergeAttribute("class", "form-group");
            tagResult.InnerHtml += LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString();
            tagResult.InnerHtml += System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel, htmlAttributes).ToHtmlString();
            tagResult.InnerHtml += ValidationExtensions.ValidationMessageFor(htmlHelper, expression, "", new { @class = "error" }).ToHtmlString();
            return tagResult.ToHtml();
        }

        public static MvcHtmlString ZN_DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> source, string optionLabel, string childController, string childAction, string childOptionalLabel, string childFieldName, string parameters)
        {
            StringBuilder output = new StringBuilder();
            dynamic htmlAttributes = new { onchange = String.Format("javascript:FillChildControl_{0}({1});", childFieldName, parameters) };
            output.Append(SpongeDropDownScript(childController, childAction, childOptionalLabel, childFieldName));
            output.AppendLine(ZN_DropDownListFor(htmlHelper, expression, source, optionLabel, htmlAttributes).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

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

        public static MvcHtmlString ZN_RadionButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            return ZN_RadionButtonList(helper, name, items, null);
        }

        public static MvcHtmlString ZN_RadionButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder output = new StringBuilder();
            foreach (var item in items)
            {
                var span = new TagBuilder("span");
                var div = new TagBuilder("div");
                var label = new TagBuilder("label");
                var input = new TagBuilder("input");

                div.AddCssClass("radio");
                input.MergeAttribute("type", "radio");
                input.MergeAttribute("name", name);
                input.MergeAttribute("value", item.Value);
                
                if (item.Selected)
                    input.MergeAttribute("checked", "checked");

                if (htmlAttributes != null)
                    input.MergeAttributes(htmlAttributes);

                span.SetInnerText(item.Text);
                label.AppendHtml(input);
                label.AppendHtml(span);               
                div.AppendHtml(label);
                output.Append(div);
            }

            return new MvcHtmlString(output.ToString());
        }
    }
}

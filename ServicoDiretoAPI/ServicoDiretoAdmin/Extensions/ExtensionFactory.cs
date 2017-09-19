using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Helpers;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Reflection;

using SpongeSolutions.Core.Attributes;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.Core.Helpers;
using SpongeSolutions.Core.Helpers.Serialization;
using SpongeSolutions.Core.Model;

namespace SpongeSolutions.ServicoDireto.Admin.Extensions
{
    public class ManagedControl
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string ClassName { get; set; }
        public Dictionary<string, string> Events { get; set; }
    }

    public class ManagedButton : ManagedControl
    {
        public ButtonType Type { get; set; }
        public enum ButtonType
        {
            Button = 0,
            Submit = 1
        }
    }

    public static class ExtensionFactory
    {
        public enum RepeatDirection
        {
            GroupHorizontal,
            GroupVertical,
            GroupList,
            NoLineContainer,
        }

        private enum ControlType
        {
            CheckBox,
            Radio,
            Text
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, RepeatDirection direction = RepeatDirection.GroupVertical, IDictionary<string, object> htmlAttributes = null, bool createUsingID = true/*, [Optional] int columns = 1*/)
        {
            return RenderControlList(htmlHelper, name, selectList, ControlType.CheckBox, direction, htmlAttributes, createUsingID);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, RepeatDirection direction = RepeatDirection.GroupVertical, IDictionary<string, object> htmlAttributes = null/*, [Optional] int columns = 1*/)
        {
            return RenderControlList(htmlHelper, name, selectList, ControlType.Radio, direction, htmlAttributes);
        }

        private static MvcHtmlString RenderControlList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, ControlType controlType, RepeatDirection direction, IDictionary<string, object> htmlAttributes = null, bool createUsingID = true)
        {
            string htmlItem;
            TagBuilder container;
            if (direction == RepeatDirection.GroupList)
                container = new TagBuilder("ul");
            else
                container = new TagBuilder("div");

            // container.MergeAttribute("style", "clear: both");
            container.MergeAttribute("id", "checkboxlist-container");

            foreach (var item in selectList)
            {
                TagBuilder lineContainer;
                TagBuilder input = new TagBuilder("input");
                TagBuilder span = new TagBuilder("label");
                var controlID = "";
                if (direction == RepeatDirection.GroupList)
                    lineContainer = new TagBuilder("li");
                else
                    lineContainer = new TagBuilder("div");

                if (createUsingID)
                    controlID = String.Format("{0}_{1}", name, item.Value);
                else
                    controlID = name;

                span.InnerHtml = item.Text;
                span.MergeAttribute("for", controlID);

                input.MergeAttribute("type", controlType.ToString().ToLower());
                input.MergeAttribute("name", name);
                input.MergeAttribute("id", controlID);
                input.MergeAttribute("value", item.Value);
                if (htmlAttributes != null)
                    input.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

                if (item.Selected)
                    input.MergeAttribute("checked", "checked");

                if (direction == RepeatDirection.GroupHorizontal)
                    lineContainer.Attributes.Add("style", "float:left");

                htmlItem = input.ToString() + span.ToString();
                if (direction != RepeatDirection.NoLineContainer)
                {
                    lineContainer.InnerHtml = htmlItem;
                    container.InnerHtml += lineContainer.ToString();
                }
                else
                    container.InnerHtml += htmlItem;
            }
            return MvcHtmlString.Create(container.ToString());
        }

        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optionLabel, int count = 0, int start = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            for (int i = start; i < count + start; i++)
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });

            if (optionLabel != null)
            {
                list.Insert(0, new SelectListItem() { Text = optionLabel, Value = string.Empty, Selected = true });
            }

            return System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, list);
        }

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string optionLabel = null, int count = 0, int start = 0)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            for (int i = start; i < count + start; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlHelper, name, list, optionLabel);
        }

        public static IEnumerable<MvcHtmlString> TextBox(this HtmlHelper htmlHelper, string prefixName, string baseName, object[] values, int start = 0, int count = 0)
        {
            StringBuilder stbHtml = new StringBuilder();
            for (int i = start; i < count + start; i++)
            {
                object value = null;
                string name = null;
                if (prefixName != null)
                    name = String.Format("{0}.{1}_{2}", prefixName, baseName, i);

                if (values.Length > 0 && i < values.Length)
                    value = values[i];

                yield return System.Web.Mvc.Html.InputExtensions.TextBox(htmlHelper, name, value);
            }
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty[]>> expression, MultiSelectList multiSelectList, object htmlAttributes = null)
        {
            //Derive property name for checkbox name
            MemberExpression body = expression.Body as MemberExpression;
            string propertyName = body.Member.Name;

            //Get currently select values from the ViewData model
            TProperty[] list = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Convert selected value list to a List<string> for easy manipulation
            List<string> selectedValues = new List<string>();

            if (list != null)
            {
                selectedValues = new List<TProperty>(list).ConvertAll<string>(delegate(TProperty i) { return i.ToString(); });
            }

            //Create div
            TagBuilder divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add checkboxes
            foreach (SelectListItem item in multiSelectList)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"checkbox\" name=\"{0}\" id=\"{0}_{1}\" " +
                                                    "value=\"{1}\" {2} /><label for=\"{0}_{1}\">{3}</label></div>",
                                                    propertyName,
                                                    item.Value,
                                                    selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "",
                                                    item.Text);
            }
            return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> multiSelectList, object htmlAttributes = null)
        {
            //Derive property name for checkbox name
            MemberExpression body = expression.Body as MemberExpression;
            string propertyName = body.Member.Name;

            //Get currently select values from the ViewData model
            TProperty selectedItem = expression.Compile().Invoke(htmlHelper.ViewData.Model);

            //Convert selected value list to a List<string> for easy manipulation
            List<string> selectedValues = new List<string>();

            //if (list != null)
            //{
            //    selectedValues = new List<TProperty>(list).ConvertAll<string>(delegate(TProperty i) { return i.ToString(); });
            //}

            //Create div
            TagBuilder divTag = new TagBuilder("div");
            divTag.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            //Add radio buttons
            foreach (SelectListItem item in multiSelectList)
            {
                divTag.InnerHtml += String.Format("<div><input type=\"radio\" name=\"{0}\" id=\"{0}\" " +
                                                    "value=\"{1}\" {2} /><label for=\"{0}\">{3}</label></div>",
                                                    propertyName,
                                                    item.Value,
                                                    selectedValues.Contains(item.Value) ? "checked=\"checked\"" : "",
                                                    item.Text);
            }
            return MvcHtmlString.Create(divTag.ToString());
        }

        public static MvcHtmlString RadionButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items)
        {
            return RadionButtonList(helper, name, items, null);
        }

        public static MvcHtmlString RadionButtonList(this HtmlHelper helper, string name, IEnumerable<SelectListItem> items, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder output = new StringBuilder();
            foreach (var item in items)
            {
                output.Append("<div><label>");
                var objectList = new TagBuilder("input");
                objectList.MergeAttribute("type", "radio");
                objectList.MergeAttribute("name", name);
                objectList.MergeAttribute("value", item.Value);

                // Check to see if it’s checked
                if (item.Selected)
                    objectList.MergeAttribute("checked", "checked");

                // Add any attributes
                if (htmlAttributes != null)
                    objectList.MergeAttributes(htmlAttributes);

                objectList.SetInnerText(item.Text);
                output.Append(objectList.ToString(TagRenderMode.SelfClosing));
                output.Append("&nbsp;" + item.Text + "</label></div>");
            }

            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString ManagedButton(this HtmlHelper helper, ManagedButton[] managedButton)
        {
            StringBuilder output = new StringBuilder();
            output.Append("<div class=\"toolbar\">\n");
            output.Append("   <ul>\n");
            output.Append("        <li>\n");
            foreach (var item in managedButton)
                output.AppendFormat("            {0}", ManagedButton(helper, item));

            output.Append("        </li>\n");
            output.Append("    </ul>\n");
            output.Append("</div>\n");
            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString ManagedButton(this HtmlHelper helper, ManagedButton managedButton)
        {
            string controllerName = helper.ViewContext.Controller.ToString(); //helper.ViewContext.RouteData.Values["controller"]
            string actionName = (string)helper.ViewContext.RouteData.Values["action"];
            MvcHtmlString result = new MvcHtmlString("");
            List<Permission> permissions = null;
            Permission permission = null;
            permissions = SpongeSolutions.Core.Cache.CacheManager.GetInsert<List<Permission>>("Permission", () => SpongeSolutions.Core.Helpers.Serialization.SerializationHelper.DeSerialize<List<Permission>>(helper.ViewContext.HttpContext.Server.MapPath("permission1.xml")));
            permission = (from rec in permissions
                          where rec.ControllerInfo.Name.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase)
                          select rec).FirstOrDefault();

            if (permission != null)
            {
                if (permission.ControllerInfo.Roles != null)
                {
                    foreach (var role in permission.ControllerInfo.Roles)
                    {
                        //Verifica se o usuario em questão tem permissao na role ?
                        if (Roles.IsUserInRole(role.Name) && permission.ControllerInfo.Controls != null)
                        {
                            //Buscando permissoes nos botoes vinculado ao controller
                            foreach (var control in permission.ControllerInfo.Controls.Where(
                                                    p => (p.ControlType == Enums.ControlType.Button || p.ControlType == Enums.ControlType.Submit)
                                                    && p.Name.Equals(managedButton.Name, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                if (control.PermissionType == Enums.PermissionType.Permissive)
                                    return CreateButton(managedButton); //Criar botao conforme config
                            }
                        }
                    }
                }
                //Buscando permissoes nos botoes vinculado as actions
                if (permission.ActionInfo != null)
                {
                    var action = permission.ActionInfo.Where(p => p.Name.Equals(actionName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    if (action != null)
                    //Verificando as actions e botões
                    {
                        if (action.Roles != null)
                        {
                            foreach (var actionRole in action.Roles)
                            {
                                if (Roles.IsUserInRole(actionRole.Name) && action.Controls != null)
                                {
                                    var control = action.Controls.Where(
                                                        p => (p.ControlType == Enums.ControlType.Button || p.ControlType == Enums.ControlType.Submit)
                                                        && p.Name.Equals(managedButton.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                                    if (control != null)
                                    {
                                        if (control.PermissionType == Enums.PermissionType.Permissive)
                                            return CreateButton(managedButton); //Criar botao conforme config
                                    }
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                result = CreateButton(managedButton);
            }

            return result;
        }

        private static MvcHtmlString CreateButton(ManagedButton managedButton)
        {
            string events = string.Empty;
            if (managedButton.Events != null)
            {
                foreach (var rec in managedButton.Events)
                    events += String.Format("{0}={1};", rec.Key, rec.Value);
            }

            return new MvcHtmlString(String.Format("<input type=\"{0}\" value=\"{1}\" class=\"{2}\" {3} name={4}/>",
                managedButton.Type,
                managedButton.Value,
                managedButton.ClassName,
                events,
                managedButton.Name));
        }

        public static MvcHtmlString CheckBoxList(this HtmlHelper helper, string name, IEnumerable<CustomSelectListItem> items, IDictionary<string, object> checkboxHtmlAttributes)
        {
            StringBuilder output = new StringBuilder();
            int count = 0;
            foreach (var item in items)
            {
                output.Append("<div class='" + ((count % 2 == 0) ? "alternating" : string.Empty) + "'>");
                output.Append(CheckBox(helper, name, item, checkboxHtmlAttributes));
                output.Append("</div>");
                count++;
            }

            return MvcHtmlString.Create(output.ToString());
        }

        /// <summary>
        /// Paginação
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="totalRowsPage">Total de Linhas por página(25 default)</param>
        /// <param name="totalRows">Total de Linhas a paginar</param>
        /// <param name="activePage">Índice da página ativa</param>
        /// <param name="parameters">Parâmetros adicionais de filtro</param>
        /// <param name="useAjaxPaging">Flag para usar modo ajax</param>
        /// <param name="gridContainer">Campo usado para ser container do resultado da chamada ajax</param>
        /// <returns></returns>
        public static MvcHtmlString CreatePageNavigator(this HtmlHelper helper, int totalRowsPerPage, int totalRows, int activePage = 1, List<KeyValuePair<string, object>> parameters = null, bool useAjaxPaging = true, string gridContainer = "gridResult")
        {
            int totalPages = Convert.ToInt32(totalRows / totalRowsPerPage); //total de páginas
            int totalBatch = 10; // Total de itens de paginacao
            int navNext = 0;
            int navPrevious = 0;
            string url = helper.ViewContext.Controller.ControllerContext.HttpContext.Request.Url.AbsolutePath;
            string additionalParamas = string.Empty;
            StringBuilder builder = new StringBuilder();
            if (totalRows < totalRowsPerPage)
                totalPages = 1;
            else if (totalRows % (totalRowsPerPage * totalPages) > 0)
                totalPages++;

            //totalPages--;
            if (totalPages > 1)
            {
                navNext = activePage + totalBatch;
                navPrevious = activePage - totalBatch;
                if (navNext > totalPages)
                    navNext = totalPages;

                if (parameters != null)
                {
                    foreach (var item in parameters)
                        additionalParamas += String.Format("&{0}={1}", item.Key, HttpUtility.HtmlEncode(item.Value));
                }

                builder.Append("<div class='navigator'>");
                if (totalPages > totalBatch)
                {
                    if (navPrevious < 0) navPrevious = 1;
                    builder.AppendFormat("<div><a href=\"javascript:NBN.Helper.AjaxPaging({0},'{1}?{2}','{3}');\"><<</a></div>", navPrevious, url, additionalParamas, gridContainer);
                }

                if (totalPages <= totalBatch)
                    for (int i = 1; i <= totalPages; i++)
                        builder.AppendFormat("<div class='" + ((activePage == i) ? "selected" : string.Empty) + "'><a href=\"javascript:NBN.Helper.AjaxPaging({0},'{1}?{2}','{3}');\">{0}</a></div>", i, url, additionalParamas, gridContainer);
                else
                    for (int i = activePage; i <= navNext; i++)
                        builder.AppendFormat("<div class='" + ((activePage == i) ? "selected" : string.Empty) + "'><a href=\"javascript:NBN.Helper.AjaxPaging({0},'{1}?{2}','{3}');\">{0}</a></div>", i, url, additionalParamas, gridContainer);

                if (navNext >= totalBatch && totalPages > totalBatch)
                    builder.AppendFormat("<div><a href=\"javascript:NBN.Helper.AjaxPaging({0},'{1}?{2}','{3}');\">>></a></div>", navNext, url, additionalParamas, gridContainer);

                builder.AppendFormat("<span class='total-rows'>{0}: [{1}]</span>", @SpongeSolutions.ServicoDireto.Internationalization.Label.Total_Records, totalRows);

                builder.Append("</div>");
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString CheckBox(this HtmlHelper helper, string name, bool isChecked, IDictionary<string, object> htmlAttributes, string checkedValue = "0", string unCheckedValue = "1")
        {
            StringBuilder output = new StringBuilder();

            var checkbox = new TagBuilder("input");
            checkbox.MergeAttribute("type", "checkbox");
            checkbox.MergeAttribute("name", name);
            checkbox.MergeAttribute("id", name);
            if (isChecked)
                checkbox.MergeAttribute("value", checkedValue);
            else
                checkbox.MergeAttribute("value", unCheckedValue);

            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();

            htmlAttributes.Add(new KeyValuePair<string, object>("onclick", String.Format("javascript:NBN.Helper.SetValueCheckBox(this,'{0}','{1}','{2}');", name, checkedValue, unCheckedValue)));
            checkbox.MergeAttributes(htmlAttributes);
            output.Append(checkbox.ToString(TagRenderMode.SelfClosing));
            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString CheckBox(this HtmlHelper helper, string name, CustomSelectListItem item, IDictionary<string, object> checkboxHtmlAttributes, string prefix = "")
        {
            StringBuilder output = new StringBuilder();
            string text = item.Text;
            if (text.Contains("-"))
                text = text.Remove(text.IndexOf("-"));

            output.Append("<div>");
            output.Append(prefix + "<label>");
            var checkboxList = new TagBuilder("input");
            checkboxList.MergeAttribute("type", "checkbox");
            checkboxList.MergeAttribute("name", name);
            checkboxList.MergeAttribute("value", item.Value);

            // Check to see if it’s checked
            if (item.Selected)
                checkboxList.MergeAttribute("checked", "checked");
            // Add any attributes
            if (checkboxHtmlAttributes != null)
                checkboxList.MergeAttributes(checkboxHtmlAttributes);

            checkboxList.SetInnerText(item.Text);
            output.Append(checkboxList.ToString(TagRenderMode.SelfClosing));
            output.Append("&nbsp; " + text + "</label>");
            //Verificando se há filhos
            foreach (CustomSelectListItem child in item.Children)
            {
                Dictionary<string, object> attributes = new Dictionary<string, object>();
                attributes.Add("relatedRoleName", item.Value);
                output.Append(CheckBox(helper, name, child, attributes, "&nbsp;&nbsp;&nbsp;&nbsp;"));
            }

            output.Append("</div>");
            return MvcHtmlString.Create(output.ToString());
        }

        public static TreeView<T> TreeView<T>(this HtmlHelper html, IEnumerable<T> items)
        {
            return new TreeView<T>(html, items);
        }

        public static MvcHtmlString Grid<T>(this HtmlHelper html, IEnumerable<T> items, string defaultSort, string gridCaption, bool canPage = false, int rowPerPage = 50, int rowCount = 0, int activePage = 1, string idGrid = "grid", bool isWindowMode = false, List<KeyValuePair<string, object>> parameters = null)
        {
            StringBuilder output = new StringBuilder();
            if (items.Count() > 0)
            {
                List<GridColumnOrder> columns = new List<GridColumnOrder>();
                if (parameters == null)
                    parameters = new List<KeyValuePair<string, object>>();

                parameters.Add(new KeyValuePair<string, object>("windowMode", isWindowMode));
                if (rowCount == 0)
                    rowCount = items.Count();

                if (canPage)
                    output.AppendLine(CreatePageNavigator(html, rowPerPage, rowCount, activePage: activePage, parameters: parameters).ToHtmlString());

                var grid = new WebGrid((IEnumerable<dynamic>)items, canPage: false, canSort: false, rowsPerPage: rowPerPage, defaultSort: defaultSort);
                foreach (var property in typeof(T).GetProperties())
                {
                    bool showColumn = true;
                    var attributes = (property.GetCustomAttributes(typeof(GridConfigAttribute), false));
                    if (attributes != null)
                    {
                        foreach (var attribute in attributes)
                        {
                            string propertyName = property.Name;
                            var attr = (GridConfigAttribute)attribute;
                            string style = attr.Style;

                            showColumn = !attr.Hidden;
                            if (showColumn)
                                showColumn = !(isWindowMode && attr.HiddenOnWindowMode);

                            if (showColumn)
                                showColumn = attr.Index > 0;

                            if (attr.CreateCheckBox && !isWindowMode)
                            {
                                columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
                                {
                                    Format = (dynamic item) =>
                                    {
                                        var entity = (T)(((WebGridRow)item).Value);
                                        var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
                                        return InputExtensions.CheckBox(html, String.Format("chk{0}", propertyName), new { @class = "checkable", value = objectValue });
                                    },
                                    Style = style
                                }));
                            }

                            if (attr.CreateImage && !isWindowMode)
                            {
                                columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
                                {
                                    //Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(propertyName),
                                    Format = (dynamic item) =>
                                    {
                                        var entity = (T)(((WebGridRow)item).Value);
                                        var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
                                        objectValue = String.Concat(SpongeSolutions.ServicoDireto.Admin.SiteContext.LayoutPath, objectValue);
                                        return new MvcHtmlString(String.Format("<img src='{0}'/>", objectValue));
                                    },
                                    Style = style
                                }));
                            }

                            if (attr.CreateEditLink)
                            {
                                columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
                                {
                                    Format = (item) =>
                                    {
                                        var entity = (T)(((WebGridRow)item).Value);
                                        try
                                        {
                                            string objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null).ToString();
                                            string objectDescription = null;

                                            if (attr.RelatedFieldName != null && attr.RelatedFieldName.Count() > 0)
                                            {
                                                foreach (var fieldName in attr.RelatedFieldName)
                                                    objectDescription += String.Format("{0}-", entity.GetType().GetProperty(fieldName).GetValue(entity, null));

                                                objectDescription = objectDescription.Remove(objectDescription.Length - 1, 1);
                                            }
                                            if (objectDescription == null)
                                                objectDescription = objectValue;

                                            if (isWindowMode)
                                                return new HtmlString(String.Format("<a href=\"javascript:AddSelected('{0}','{1}')\">{2}</a>", objectValue, objectDescription, SpongeSolutions.ServicoDireto.Internationalization.Label.Select));
                                            else
                                                return LinkExtensions.ActionLink(html, SpongeSolutions.ServicoDireto.Internationalization.Label.Edit, "create", new { id = objectValue });
                                        }
                                        catch (Exception)
                                        {
                                            return string.Empty;
                                        }
                                    },
                                    Style = style
                                }));
                            }

                            if (attr.EnumType != null)
                            {
                                columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
                                {
                                    Style = style,
                                    ColumnName = property.Name,
                                    Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(property.Name),
                                    Format = (item) =>
                                    {
                                        var entity = (T)(((WebGridRow)item).Value);
                                        var objectValue = entity.GetType().GetProperty(propertyName).GetValue(entity, null);
                                        //var enumSource = Activator.CreateInstance(attr.EnumType);
                                        //TODO: COLOCAR DE MODO DINAMICO
                                        return SpongeSolutions.Core.Translation.EnumTranslator.Translate<Enums.StatusType>(SpongeSolutions.Core.Helpers.EnumHelper.TryParse<Enums.StatusType>(objectValue)).DisplayName;
                                    }
                                }));
                                showColumn = false;
                            }

                            if (showColumn)
                                columns.Add(new GridColumnOrder(attr.Index, new WebGridColumn()
                                {
                                    ColumnName = property.Name,
                                    Header = SpongeSolutions.ServicoDireto.Internationalization.Label.ResourceManager.GetString(property.Name)
                                    ,
                                    Style = style
                                }));
                        }
                    }
                }

                output.AppendLine(MvcHtmlString.Create(grid.GetHtml(
                   tableStyle: "grid",
                   caption: gridCaption,
                   alternatingRowStyle: "alternating",
                   htmlAttributes: new { id = idGrid },
                   columns: columns.OrderBy(p => p.Index).Select(p => p.Column).ToArray()).ToHtmlString()).ToHtmlString());

                if (canPage)
                    output.AppendLine(CreatePageNavigator(html, rowPerPage, rowCount, activePage: activePage, parameters: parameters).ToHtmlString());
            }
            else
            {
                var container = new TagBuilder("div");
                container.AddCssClass("notice");
                container.SetInnerText(Internationalization.Label.Records_Not_Found);
                output.Append(container.ToString());
            }
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString FindTextBoxFor<TModel, TProperty, TProperty1>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> keySelector, Expression<Func<TModel, TProperty1>> valueSelector, string searchTitle, string searchController, string searchAction, string parameters)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, keySelector).ToHtmlString() + "<br />");
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, valueSelector, new { @class = "xbig", @readonly = "true" }).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, keySelector).ToHtmlString());
            string script = String.Format("NBN.REST.Execute( '/{0}.aspx/{1}', {2}, function (data) {{ NBN.Helper.ShowModalWindowJqueryUI(data, '{3}', {{ width: 680, height: 500 }}, false, true); }} , 'POST');", searchController, searchAction, parameters, searchTitle);
            output.AppendLine(String.Format("<input type=\"button\" id=\"findButton\" class=\"find\" onclick=\"{0}\" />", script));
            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, valueSelector).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString SpongeAutoCompleteFor<TModel, TProperty, TProperty1>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> keySelector, Expression<Func<TModel, TProperty1>> valueSelector, string url, dynamic htmlAttributes = null, string resultScript = null)
        {
            string idField = keySelector.Body.ToString();
            idField = idField.Substring(idField.IndexOf(".") + 1);

            string idValue = valueSelector.Body.ToString();
            idValue = idValue.Substring(idValue.IndexOf(".") + 1);

            StringBuilder output = new StringBuilder();
            StringBuilder script = new StringBuilder();
            script.AppendLine("<script type=\"text/javascript\">");
            script.AppendLine("    $(function () {");
            script.AppendLine("        NBN.Helper.LoadAutoComplete(\"" + idField + "\", \"" + idValue + "\", '" + url + "');");
            if (!string.IsNullOrEmpty(resultScript))
                script.AppendLine(resultScript);

            script.AppendLine("  });");
            script.AppendLine("</script>");
            output.AppendLine(System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, keySelector).ToHtmlString() + ": <br />");
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, valueSelector, htmlAttributes).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.InputExtensions.HiddenFor(htmlHelper, keySelector).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, valueSelector).ToHtmlString());
            output.AppendLine(script.ToString());
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // build the <img> tag
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            imgBuilder.MergeAttribute("alt", alt);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // build the <a> tag
            var anchorBuilder = new TagBuilder("a");

            anchorBuilder.MergeAttribute("href", url.Action(action, controllerName, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(anchorHtml);
        }

        public static MvcHtmlString SpongeTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, bool isTextArea = false, string initialValue = "")
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString() + ":<br />");
            if (isTextArea)
                output.AppendLine(System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, htmlAttributes).ToHtmlString());
            else
                output.AppendLine(System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, htmlAttributes).ToHtmlString());

            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

        /// <summary>
        /// Criação de combo dependente
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="source"></param>
        /// <param name="optionLabel"></param>
        /// <param name="childController"></param>
        /// <param name="childAction"></param>
        /// <param name="childOptionalLabel"></param>
        /// <param name="childFieldName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static MvcHtmlString SpongeDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> source, string optionLabel, string childController, string childAction, string childOptionalLabel, string childFieldName, string parameters)
        {
            StringBuilder output = new StringBuilder();
            dynamic htmlAttributes = new { onchange = String.Format("javascript:FillChildControl_{0}({1});", childFieldName, parameters) };
            output.Append(SpongeDropDownScript(childController, childAction, childOptionalLabel, childFieldName));
            output.AppendLine(SpongeDropDownListFor(htmlHelper, expression, source, optionLabel, htmlAttributes).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString SpongeDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string childController, string childAction, string childOptionalLabel, string childFieldName, string parameters, string optionLabel = null)
        {
            StringBuilder output = new StringBuilder();
            dynamic htmlAttributes = new { onchange = String.Format("javascript:FillChildControl_{0}({1});", childFieldName, parameters) };
            output.Append(SpongeDropDownScript(childController, childAction, childOptionalLabel, childFieldName));
            output.AppendLine(System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, htmlAttributes).ToHtmlString());
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

        public static MvcHtmlString SpongeDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string optionLabel = null, object htmlAttributes = null)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(System.Web.Mvc.Html.LabelExtensions.LabelFor(htmlHelper, expression).ToHtmlString() + "<br />");
            output.AppendLine(System.Web.Mvc.Html.SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, optionLabel, htmlAttributes).ToHtmlString());
            output.AppendLine(System.Web.Mvc.Html.ValidationExtensions.ValidationMessageFor(htmlHelper, expression).ToHtmlString());
            return MvcHtmlString.Create(output.ToString());
        }

        public static MvcHtmlString SpongeDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optionLabel = null, object htmlAttributes = null)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            return SpongeDropDownListFor(htmlHelper, expression, listItem, optionLabel, null);
        }

        public static MvcHtmlString SpongeEditorForBaseEntity<TModel>(this HtmlHelper<TModel> htmlHelper, bool showEditor)
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
    }

    public class GridColumnOrder
    {
        public int Index { get; set; }
        public WebGridColumn Column { get; set; }

        public GridColumnOrder(int Index, WebGridColumn Column)
        {
            this.Index = Index;
            this.Column = Column;
        }
    }
}

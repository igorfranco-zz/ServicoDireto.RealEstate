using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace SpongeSystems.Core
{
    public class BindingHelper
    {

        #region [Enum]

        public enum FirstItem
        {
            None,
            Custom
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        /// <param name="type"></param>
        /// <param name="firstItemText"></param>
        public static void BindList(ListControl listControl, DataTable dataSource, string dataTextField, string dataValueField, FirstItem type, string firstItemText)
        {
            BindList(listControl, dataSource, dataTextField, dataValueField, type, firstItemText, "");
        }

        /// <summary>
        /// BindList
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="type"></param>
        /// <param name="firstItemText"></param>
        /// <param name="firstItemValue"></param>
        public static void BindList(ListControl listControl, List<string> dataSource, FirstItem type, string firstItemText, string firstItemValue)
        {
            listControl.DataSource = dataSource;
            listControl.DataBind();

            if (type == FirstItem.Custom)
            {
                ListItem item = new ListItem();

                item.Text = firstItemText;
                item.Value = firstItemValue;
                listControl.Items.Insert(0, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        /// <param name="type"></param>
        /// <param name="firstItemText"></param>
        /// <param name="firstItemValue"></param>
        public static void BindList(ListControl listControl, DataTable dataSource, string dataTextField, string dataValueField, FirstItem type, string firstItemText, string firstItemValue)
        {
            listControl.DataSource = dataSource;
            listControl.DataTextField = dataTextField;
            listControl.DataValueField = dataValueField;
            listControl.DataBind();

            if (type == FirstItem.Custom)
            {
                ListItem item = new ListItem();

                item.Text = firstItemText;
                item.Value = firstItemValue;
                listControl.Items.Insert(0, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        /// <param name="separator"></param>
        public static void BindList(ListControl listControl, DataTable dataSource, string[] dataTextField, string dataValueField, string[] separator)
        {
            bool firstTime = true;
            ListItem item;

            for (int i = 0; i < dataSource.Rows.Count; i++)
            {
                item = new ListItem();
                firstTime = true;

                item.Value = dataSource.Rows[i][dataValueField].ToString();

                for (int j = 0; j < dataTextField.Length; j++)
                {
                    if (firstTime)
                    {
                        firstTime = false;
                        item.Text = dataSource.Rows[i][dataTextField[j]].ToString();
                    }
                    else
                    {
                        if (separator.Length > j - 1 && separator[j - 1] != null)
                            item.Text += string.Concat(separator[j - 1], dataSource.Rows[i][dataTextField[j]]);
                        else
                            item.Text += dataSource.Rows[i][dataTextField[j]];

                    }
                }

                listControl.Items.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        /// <param name="type"></param>
        /// <param name="firstItemText"></param>
        /// <param name="firstItemValue"></param>
        public static void BindList<T>(ListControl listControl, IList<T> dataSource, string dataTextField, string dataValueField, FirstItem type, string firstItemText, string firstItemValue)
        {
            listControl.DataSource = dataSource;
            listControl.DataTextField = dataTextField;
            listControl.DataValueField = dataValueField;
            listControl.DataBind();

            if (type == FirstItem.Custom)
            {
                ListItem item = new ListItem();

                item.Text = firstItemText;
                item.Value = firstItemValue;
                listControl.Items.Insert(0, item);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        /// <param name="selectedValue"></param>
        /// <param name="hasFirstItem"></param>
        public static void BindList<T>(ListControl listControl, IList<T> dataSource, string dataTextField, string dataValueField, string selectedValue, bool hasFirstItem)
        {
            BindList<T>(listControl, dataSource, dataTextField, dataValueField, (hasFirstItem) ? FirstItem.Custom : FirstItem.None, "[Selecione]", "-1");

            if (selectedValue != null)
            {
                ListItem item = listControl.Items.FindByValue(selectedValue);
                if (item != null)
                    item.Selected = true;
            }
            else
            {
                if (dataSource.Count() > 0)
                    listControl.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listControl"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataTextField"></param>
        /// <param name="dataValueField"></param>
        public static void BindList<T>(ListControl listControl, IList<T> dataSource, string dataTextField, string dataValueField)
        {
            listControl.DataSource = dataSource;
            listControl.DataTextField = dataTextField;
            listControl.DataValueField = dataValueField;
            listControl.DataBind();
        }

        #endregion

        /// <summary>
        /// Buscar o valor do controle baseado em seu nome
        /// </summary>
        /// <param name="controlName"></param>
        /// <param name="detailsView"></param>
        /// <returns></returns>
        public static string GetTextFieldValue(string controlName, DetailsView detailsView)
        {
            object fieldValue = null;
            object ctl = detailsView.FindControl(controlName);

            if (ctl == null)
                throw new Exception(string.Format("GetTextFieldValue: could not find {0} control!", controlName));

            if (ctl.GetType() == typeof(TextBox))
                fieldValue = ((TextBox)ctl).Text;
            else if (ctl.GetType() == typeof(DropDownList))
                fieldValue = ((DropDownList)ctl).SelectedValue;
            else if (ctl.GetType() == typeof(CheckBox))
                fieldValue = ((CheckBox)ctl).Checked;
            else if (ctl.GetType() == typeof(Label))
                fieldValue = ((Label)ctl).Text;

            return fieldValue.ToString();
        }

        public static decimal GetDecimalFieldValue(string controlName, DetailsView detailsView)
        {
            string fieldValue = GetTextFieldValue(controlName, detailsView);
            if (fieldValue.Length == 0)
                return 0;
            else
                return Convert.ToDecimal(fieldValue);

        }

        public static int GetIntegerFieldValue(string controlName, DetailsView detailsView)
        {
            string fieldValue = GetTextFieldValue(controlName, detailsView);
            if (fieldValue.Length == 0)
                return 0;
            else
                return Convert.ToInt32(fieldValue);

        }

        public static Boolean GetBooleanFieldValue(string controlName, DetailsView detailsView)
        {
            string fieldValue = GetTextFieldValue(controlName, detailsView);
            if (fieldValue.Length == 0)
                return false;
            else
                return Convert.ToBoolean(fieldValue);

        }
    }

}

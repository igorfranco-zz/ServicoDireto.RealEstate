using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpongeSolutions.Core.Translation;

namespace SpongeSolutions.Core.Helpers
{
    public class TranslationHelper
    {
        //public static void LoadListControl<TEnum>(ComboBox control) where TEnum : struct
        //{
        //    control.DisplayMember = "DisplayName";
        //    control.ValueMember = "RawValue";
        //    control.DataSource = EnumTranslator.Translate<TEnum>();
        //}
        //public static void LoadListControl<TEnum>(ComboBox control, TEnum selectedValue) where TEnum : struct
        //{
        //    control.DisplayMember = "DisplayName";
        //    control.ValueMember = "RawValue";
        //    control.DataSource = EnumTranslator.Translate<TEnum>();
        //    control.SelectedValue = selectedValue;
        //}

        //public static void LoadListControl<TEnum>(ComboBox control, TEnum selectedValue)
        //{
        //    PopulateHelper.LoadControl(control, EnumTranslator.Translate<TEnum>().ToList(), "RawValue", "DisplayName", EnumTranslator.Translate<TEnum>(selectedValue), string.Empty);
        //}
        //public static void LoadListControl<TEnum>(ComboBox control, TEnum selectedValue, string initialValue) where TEnum : struct
        //{
        //}

        public static void LoadListControl<TEnum>(ComboBox control, object selectedValue , object initialValue ) where TEnum : struct
        {
            control.BindingContext = new BindingContext();
            control.Items.Clear();
            control.DisplayMember = "DisplayName";
            control.ValueMember = "RawValue";
            if (initialValue != null)
                control.Items.Add(initialValue);

            foreach (var item in EnumTranslator.Translate<TEnum>().ToList())
                control.Items.Add(item);

            if (selectedValue != null)
                control.SelectedItem = selectedValue;
        }
    }
}

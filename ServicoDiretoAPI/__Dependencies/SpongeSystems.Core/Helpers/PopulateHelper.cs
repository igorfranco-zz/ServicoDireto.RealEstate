using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using NBN.Core.Helpers.Reflection;
using NBN.Core.Translation;

namespace NBN.Core.Helpers
{
    public class PopulateHelper
    {
        //public static void Bind<TSource, TDisplay, TValue>(ComboBox control, List<TSource> source, string textFielName, string valueFielName)
        //{
        //    Bind<TSource, TDisplay, TValue>(control, source, textFielName, valueFielName, default(TDisplay), default(TValue));
        //}

        //public static void Bind<TSource, TDisplay, TValue>(ComboBox control, List<TSource> source, string textFielName, string valueFielName, TDisplay initialValue)
        //{
        //    Bind<TSource, TDisplay, TValue>(control, source, textFielName, valueFielName, initialValue, default(TValue));
        //}

        //public static void Bind<TSource, TDisplay, TValue>(ComboBox control, List<TSource> source, string textFielName, string valueFielName, TValue selectedValue)
        //{
        //    Bind<TSource, TDisplay, TValue>(control, source, textFielName, valueFielName, default(TDisplay), selectedValue);
        //}


        //public static void Bind<TEnum, TDisplay, TValue>(ComboBox control, string textFielName, string valueFielName, TValue selectedValue) where TEnum : struct
        //{
        //    Bind<Translation<TEnum>, TDisplay, TValue>(control, EnumTranslator.Translate<TEnum>().ToList(), textFielName, valueFielName, default(TDisplay), selectedValue);
        //}

        public static void Bind<TEnum>(ComboBox control) where TEnum : struct
        {
            Bind<Translation<TEnum>, string, int>(control, EnumTranslator.Translate<TEnum>().ToList(), "DisplayName", "RawValue", "", null);
        }

        public static void Bind<TEnum>(ComboBox control, TEnum selectedValue) where TEnum : struct
        {
            Bind<Translation<TEnum>, string, int>(control, EnumTranslator.Translate<TEnum>().ToList(), "DisplayName", "RawValue", "", EnumTranslator.Translate<TEnum>(selectedValue).DisplayName);
        }

        public static void Bind<TSource, TDisplay, TValue>(ComboBox control, List<TSource> source, string textFielName, string valueFielName, TDisplay initialValue, string selectedText)
        {
            control.Items.Clear();
            control.ValueMember = "Value";
            control.DisplayMember = "Text";

            if (initialValue != null && !initialValue.Equals(default(TDisplay)))
            {
                control.Items.Add(new Wrapper<TDisplay, TValue>() { Text = initialValue });
                control.SelectedIndex = 0;
            }

            foreach (var item in Wrapper<TDisplay, TValue>.ToList<TSource>(source, textFielName, valueFielName))
                control.Items.Add(item);

            control.Text = selectedText;
        }

        public static void Bind<TSource, TDisplay, TValue>(DataGridViewComboBoxColumn control, List<TSource> source, string textFielName, string valueFielName, TDisplay initialValue, string selectedText)
        {
            control.Items.Clear();
            control.ValueMember = "Value";
            control.DisplayMember = "Text";

            if (initialValue != null && !initialValue.Equals(default(TDisplay)))
            {
                control.Items.Add(new Wrapper<TDisplay, TValue>() { Text = initialValue });
            }

            foreach (var item in Wrapper<TDisplay, TValue>.ToList<TSource>(source, textFielName, valueFielName))
                control.Items.Add(item);
        }
    }
    public class Wrapper<TDisplay, TValue>
    {
        private static ReflectionHelper helper = new ReflectionHelper();
        public TDisplay Text { get; set; }
        public TValue Value { get; set; }
        private TValue Selected { get; set; }
        public Wrapper() { }
        public Wrapper(TDisplay text, TValue value)
        {
            this.Text = text;
            this.Value = value;
        }

        public static Wrapper<TDisplay, TValue> GetValue<TSource>(TSource source, string textMember, string valueMember)
        {
            return new Wrapper<TDisplay, TValue>() { Text = (TDisplay)helper.GetValue(source, textMember), Value = (TValue)helper.GetValue(source, valueMember) };
        }

        public static IEnumerable<Wrapper<TDisplay, TValue>> ToList<TSource>(List<TSource> source, string textMember, string valueMember)
        {
            foreach (var item in source)
                yield return GetValue<TSource>(item, textMember, valueMember);
        }
    }
}

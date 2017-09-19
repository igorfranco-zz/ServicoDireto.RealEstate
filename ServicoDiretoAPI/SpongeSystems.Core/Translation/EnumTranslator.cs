using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Resources;
//using SpongeSolutions.Core.Attributes;
using System.Web.Mvc;
using SpongeSolutions.Core.Attributes;

namespace SpongeSolutions.Core.Translation
{
    /// <summary>
    /// Realiza a tradução dos enumerados
    /// </summary>
    public class EnumTranslator
    {
        #region [ Methods ]

        #region [ Public ]
        public static IEnumerable<SelectListItem> Translate<TEnum>(string selectedValue)
        {
            foreach (Translation<TEnum> item in EnumTranslator.Translate<TEnum>(CultureInfo.CurrentUICulture))
                yield return new SelectListItem() { Text = item.DisplayName, Value = item.RawValue.ToString(), Selected = item.DisplayName.ToString().Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase) };
        }

        public static IEnumerable<SelectListItem> Translate<TEnum>(int selectedValue)
        {
            foreach (Translation<TEnum> item in EnumTranslator.Translate<TEnum>(CultureInfo.CurrentUICulture))
                yield return new SelectListItem() { Text = item.DisplayName, Value = item.RawValue.ToString(), Selected = (item.RawValue == selectedValue) };
        }

        /// <summary>
        /// Traduz todos os itens do enum <typeparamref name="TEnum"/> de acordo com o idioma corrente da aplicação 
        /// </summary>
        /// <typeparam name="TEnum">Tipo do enum que será traduzido</typeparam>
        /// <returns></returns>
        public static TanslationCollection<TEnum> Translate<TEnum>()
        {
            return EnumTranslator.Translate<TEnum>(CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Traduz todos os itens do enum <typeparamref name="TEnum"/> de acordo com o idioma <paramref name="culture"/>
        /// </summary>
        /// <typeparam name="TEnum">Tipo do enum que será traduzido</typeparam>
        /// <param name="culture">Idioma que será utilizado para a tradução</param>
        /// <returns></returns>
        public static TanslationCollection<TEnum> Translate<TEnum>(CultureInfo culture)
        {
            TanslationCollection<TEnum> translations = new TanslationCollection<TEnum>();

            Type enumType = typeof(TEnum);
            ResourceManager resManager = EnumTranslator.GetResourceManager(enumType);

            if (resManager != null)
            {
                Array values = Enum.GetValues(enumType);

                for (int index = 0; index < values.Length; index++)
                {
                    TEnum value = (TEnum)values.GetValue(index);
                    string translationDesc = EnumTranslator.GetTranslation<TEnum>(value, resManager, culture);
                    if (translationDesc != null)
                        translations.Add(new Translation<TEnum>(translationDesc, value, culture));
                }
            }

            return translations;
        }

        /// <summary>
        /// Realiza a tradução do item <paramref name="value"/> do enumerador <typeparamref name="TEnum"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Valor que será traduzido</param>
        /// <returns>Retorna a tradução do item <paramref name="value"/></returns>
        /// <remarks>
        /// Para a localização do resource com as traduções é utilizado o idioma da thread em que o código está
        /// executando, propriedade <see cref="System.Globalization.CultureInfo.CurrentUICulture"/>.
        /// </remarks>
        public static Translation<TEnum> Translate<TEnum>(TEnum value)
        {
            return EnumTranslator.Translate<TEnum>(value, CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Realiza a tradução do item <paramref name="value"/> do enumerador <typeparamref name="TEnum"/>
        /// utilizando o idioma <paramref name="culture"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value">Valor que será traduzido</param>
        /// <param name="culture">Idioma que será utilizado para localizar o arquivo *.resx com as traduções</param>
        /// <returns>Retorna a tradução do item <paramref name="value"/></returns>
        public static Translation<TEnum> Translate<TEnum>(TEnum value, CultureInfo culture)
        {
            Type enumType = typeof(TEnum);
            ResourceManager resManager = EnumTranslator.GetResourceManager(enumType);

            if (resManager != null)
                return new Translation<TEnum>(
                    EnumTranslator.GetTranslation<TEnum>(value, resManager, culture),
                    value,
                    culture);
            else
                return new Translation<TEnum>(
                    Enum.GetName(enumType, value),
                    value,
                    culture);
        }

        #endregion

        #region [ Private ]

        /// <summary>
        /// Obtém a tradução de um item do enum
        /// </summary>
        /// <typeparam name="TEnum">Tipo do enum que será traduzido</typeparam>
        /// <param name="enumValue">Item que será traduzido</param>
        /// <param name="resManager">Repositório com as traduções</param>
        /// <param name="culture">Idioma utilizado para a tradução</param>
        /// <returns></returns>
        private static string GetTranslation<TEnum>(TEnum enumValue, ResourceManager resManager, CultureInfo culture)
        {
            Type enumType = typeof(TEnum);
            string enumName = Enum.GetName(enumType, enumValue);

            TranslationAttribute att = (TranslationAttribute)Attribute.GetCustomAttribute(enumType.GetField(enumName), typeof(TranslationAttribute));

            if (att != null)
            {
                if (att.Displayable)
                {
                    return resManager.GetString(att.ResourceName, culture);
                }
                else
                    return null;
            }
            else
                return enumName;
        }

        /// <summary>
        /// Obtém o repositório que será usado para obter as traduções
        /// </summary>
        /// <param name="fromEnum"></param>
        /// <returns></returns>
        private static ResourceManager GetResourceManager(Type fromEnum)
        {
            object[] atts = fromEnum.GetCustomAttributes(typeof(ResourceTypeAttribute), false);

            if (atts != null && atts.Length > 0)
            {
                ResourceTypeAttribute resTypeAtt = (ResourceTypeAttribute)atts[0];

                return new ResourceManager(resTypeAtt.ResourceType);
            }

            return null;
        }

        #endregion

        #endregion
    }
}

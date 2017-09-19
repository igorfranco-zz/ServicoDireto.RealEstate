using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.Core.Attributes;

namespace SpongeSolutions.ServicoDireto.Model.InfraStructure
{
    public class Enums
    {
        [ResourceType(typeof(Internationalization.Enum))]
        public enum ComponentType : short
        {
            [Translation("ComponentType_RangeSlider")]
            RangeSlider = 1,
            [Translation("ComponentType_TextBox")]
            TextBox = 2,
            [Translation("ComponentType_DatePicker")]
            DatePicker = 3
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum FederealIDType : short
        {
            [Translation("FederealIDType_CPF")]
            CPF = 1,
            [Translation("FederealIDType_CNPJ")]
            CNPJ = 2
        }

        public enum ViewType : short
        {
            Map = 1,
            List = 2
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum ChoiceType : short
        {
            [Translation("ChoiceType_Yes")]
            Yes = 1,
            [Translation("ChoiceType_No")]
            No = 0
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum NotifyType : short
        {
            [Translation("NotifyType_SMS")]
            SMS = 1,
            [Translation("NotifyType_Email")]
            Email = 2
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum HierarchyItemType : short
        {
            [Translation("HierarchyItemType_Type")]
            Type = 1,
            [Translation("HierarchyItemType_Category")]
            Category = 2
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum StatusType : short
        {
            [Translation("StatusType_Inactive")]
            Inactive = 0,
            [Translation("StatusType_Active")]
            Active = 1,
            [Translation("StatusType_Pending")]
            Pending = 1
        }

        [ResourceType(typeof(Internationalization.Enum))]
        public enum EmailType : short
        {
            [Translation("EmailType_Excluded")]
            Excluded = 0,
            [Translation("EmailType_Sent")]
            Sent = 1,
            [Translation("EmailType_Read")]
            Read = 2,
        }

        public enum ChartType : short
        {
            LineChart = 1,
            BarChart = 2,
            ColumnChart = 3,
            AreaChart = 4,
            PieChart = 5,
            AnnotatedTimeLine = 6
        }

        public enum ControlType
        {
            Button = 0,
            Submit = 1,
            DropDown = 2,
            TextBox = 3,
            Label = 4
        }

        public enum PermissionType
        {
            NonPermissive = 0,
            Permissive = 1
        }

        public enum ProcessStatusType : short
        {
            Inactive = 0,
            Active = 1,
            NotProcessed = 2,
            Processed = 3,
            Discovered = 4,
            Error = 5
        }

        public enum ActionType : short
        {
            None = 0,
            Insert = 1,
            Update = 2,
            Delete = 3,
            Select = 4,
            Cancel = 5
        }
        //Todo: mudar para ingles
        public enum AttributeType : int
        {
            Caracteristicas = 1,
            Infra_Estrutura = 2,
            Imagem = 3,
            Informacoes_Gerais = 4,
            Filtro = 5,
            Principais_Informacoes = 6
        }
        public enum ObjectType : int
        {
            Apartamento = 16,
            Casa = 17,
            Comercial_Industrial = 18,
            Propriedade_Rural = 19,
            Terreno = 20,
            Aluguel = 29,
            Condominio = 30,
            Pontos_Interesse = 42
        }

        public enum ObjectCategory
        {
            Chacara = 21,
            Sitio = 22,
            Fazenda = 23,
            Casa = 24,
            Conjunto = 25,
            Loteamento = 26,
            Sobrado = 27,
            Cobertura = 28,
            Duplex = 29,
            Flat = 30,
            JK = 31,
            Kitinet = 32,
            Loja = 41,
            Apartamento = 42,
            Academias = 47,
            Farmacias_Drograrias = 48,
            Areas_Verdes_Parques = 49,
            Bancos = 50,
            Escolas = 51,
            Hospitais = 52,
            Panificadoras = 53
        }

        public enum PurposeType : short
        {
            Aluguar = 1,
            Vender = 2,
            Temporada = 3,
            Comprar = 4
        }

    }
}

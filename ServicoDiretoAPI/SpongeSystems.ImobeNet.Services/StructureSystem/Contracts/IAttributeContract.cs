using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using System.Web.Mvc;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IAttributeContract : IBaseService<Model.Attribute>
    {
        /// <summary>
        /// Efetua a inserção de um novo atributo com os valores de cultura
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeCulture"></param>
        void Insert(Model.Attribute entity, ICollection<AttributeCulture> attributeCulture);

        /// <summary>
        /// Efetua a atualização de um atributo e suas culturas
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="attributeCulture"></param>
        void Update(Model.Attribute entity, ICollection<AttributeCulture> attributeCulture);

        IList<AttributeCultureExtended> ListAttributeCulture(int? idAttribute);

        /// <summary>
        /// Lista os atributos baseados no seu status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Model.AttributeExtended> GetByStatus(short status, out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1);

        IList<Model.AttributeExtended> GetAll(out int recordCount, string idCulture, string sortType = null, int startRowIndex = -1, int maximumRows = -1, int idAttribute = -1);

        /// <summary>
        /// Inativar atributos
        /// </summary>
        /// <param name="ids"></param>
        void Inactivate(int[] ids);

        /// <summary>
        /// Listar os atributos disponíveis de acordo com o tipo de atributo
        /// </summary>
        /// <param name="idAttributeType"></param>
        /// <returns></returns>
        IList<Model.AttributeExtended> ListAvailableAttribute(string idCulture, int? idAttributeType = null);

        /// <summary>
        /// Listar os atributos vinculados a um determinado tipo
        /// </summary>
        /// <param name="idAttributeType"></param>
        /// <returns></returns>
        IList<Model.AttributeExtended> ListVinculatedAttribute(string idCulture, int? idAttributeType = null);

        /// <summary>
        /// Lista os atributos de um determinado tipo e vinculado a um determinado elemento
        /// </summary>
        /// <param name="idAttributeType"></param>
        /// <param name="idElement"></param>
        /// <returns></returns>
        IList<Model.AttributeExtended> List(string idCulture, int idAttributeType, long idElement);

        /// <summary>
        /// Listar os atributos vinculados ao elemento em questão
        /// </summary>
        /// <param name="acronym"></param>
        /// <param name="idElement"></param>
        /// <returns></returns>
        IList<Model.AttributeExtended> List(string idCulture, string acronym, long idElement);

        IList<Model.AttributeExtended> List(string idCulture, long idElement);

        IEnumerable<dynamic> AutoComplete(string idCulture, string text);

        //Model.AttributeExtended GetByExId(int id);

        int GetInsert(string attributeName);

        /// <summary>
        /// Buscar o atributo pela sigla dele
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Model.Attribute GetByAcronym(string value);

        /// <summary>
        /// Buscar attributo pelo seu grupo
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Model.Attribute GetByGroup(string value);
    }
}

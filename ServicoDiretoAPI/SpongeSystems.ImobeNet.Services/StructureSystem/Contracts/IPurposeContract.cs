using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using System.Web.Mvc;

namespace SpongeSolutions.ServicoDireto.Services.StructureSystem.Contracts
{
    public interface IPurposeContract : IBaseService<Purpose>
    {
        void Insert(Purpose entity, ICollection<PurposeCulture> purposeCulture, int[] vinculatedStruture);
        
        void Update(Purpose entity, ICollection<PurposeCulture> purposeCulture, int[] vinculatedStruture);
        
        IList<PurposeExtended> GetByStatus(short status, string idCulture);

        IList<PurposeCultureExtended> ListPurposeCulture(int? idPurpose);

        void Inactivate(int[] ids);

        //SelectList ListAvailableCategoryAsSelectList(int[] idPurpose);

        //SelectList ListVinculatedCategoryAsSelectList(int[] idPurpose);

        /// <summary>
        /// Listar as categorias vinculadas a um prop�sito
        /// </summary>
        /// <param name="idPurpose"></param>
        /// <returns></returns>
        IList<Model.HierarchyStructureBasic> ListVinculatedCategory(int[] idPurpose, string idCulture);

        /// <summary>
        /// Listar as categorias que n�o est�o vinculadas a um determinado prop�sito
        /// </summary>
        /// <param name="idPurpose"></param>
        /// <returns></returns>
        IList<Model.HierarchyStructureBasic> ListAvailableCategory(int[] idPurpose, string idCulture);

        /// <summary>
        /// Listar as categorias vinculadas ao prop�sito
        /// </summary>
        /// <param name="idPurpose"></param>
        /// <returns></returns>
        IList<HierarchyStructureExtended> ListCategory(string idCulture, int[] idPurpose);

        IList<Model.HierarchyStructureBasic> ListType(int idHierarhyStructureParent, string idCulture);

        //SelectList ListTypeAsSelectList(int idHierarhyStructureParent);

        //SelectList ListCategoryAsSelectList(int[] idPurpose);

        /// <summary>
        /// Listar os prop�sitos vinculados ao filtro
        /// </summary>
        /// <param name="idFilter"></param>
        /// <returns></returns>
        IList<Model.PurposeExtended> ListVinculatedPurpose(string idCulture, int idFilter);
        
        /// <summary>
        /// Listar os prop�sitos que n�o est�o vinculados ao filtro
        /// </summary>
        /// <param name="idFilter"></param>
        /// <returns></returns>
        IList<Model.PurposeExtended> ListAvailablePurpose(string idCulture, int idFilter);

        //SelectList ListAvailablePurposeAsSelectList(int idFilter);

        //SelectList ListVinculatedPurposeAsSelectList(int idFilter);

        Purpose GetInsert(string purposeName);

        IList<PurposeExtended> ListAll(string idCulture);
    }
}

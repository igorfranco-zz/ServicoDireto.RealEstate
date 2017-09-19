using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.ServicoDireto.Model;
namespace SpongeSolutions.ServicoDireto.Services.ElementSystem.Contracts
{
    public interface IElementContract : IBaseService<Element, long>
    {
        /// <summary>
        /// Atualiza um elemento
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="elementCulture"></param>
        /// <param name="elementAttribute"></param>
        void Update(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute);
        void Update(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute, IList<string> elementImages);

        /// <summary>
        /// Insere um novo elemento
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="elementCulture"></param>
        /// <param name="elementAttribute"></param>
        void Insert(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute);
        void Insert(Element entity, ICollection<ElementCulture> elementCulture, ICollection<ElementAttribute> elementAttribute, IList<string> elementImages);

        /// <summary>
        /// Inativa elementos
        /// </summary>
        /// <param name="ids"></param>
        void Inactivate(int[] ids);

        /// <summary>
        /// Deleta elementos
        /// </summary>
        /// <param name="ids"></param>
        void Delete(long[] ids);

        /// <summary>
        /// Busca um elemento de maneira completa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ElementExtended GetByIdExtended(string idCulture, long id, bool igoreAttributes = false);

        /// <summary>
        /// Listar elementos de um determinado status
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Element> GetByStatus(short status);

        /// <summary>
        /// Listar elementos de uma cliente e com um determinado status
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        IList<Element> GetByCustomer(int idCustomer, short? status);

        /// <summary>
        /// Listar os imóveis em destaque
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        IEnumerable<ElementExtended> ListFeatured(string idCulture, int idCustomer);

        /// <summary>
        /// Listar os elementos similares
        /// </summary>
        /// <param name="idElement"></param>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        IEnumerable<ElementExtended> ListSimilar(string idCulture, long idElement, int idCustomer = 0);

        /// <summary>
        /// Listar os elementos mais visualizados
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        IEnumerable<ElementExtended> ListTopViewed(string idCulture, int idCustomer);

        /// <summary>
        /// Busca todos os imóveis paginando
        /// </summary>
        /// <param name="recordCount"></param>
        /// <param name="idCustomer"></param>
        /// <param name="status"></param>
        /// <param name="sortType"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns></returns>
        IList<ElementExtended> GetAll(string idCulture, out int recordCount, int idCustomer = -1, long idElement = -1, short status = -1, string sortType = null, int startRowIndex = -1, int maximumRows = -1, bool igoreAttributes = false);

        /// <summary>
        /// Listar o Elementos de Acordo com o objeto Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IList<ElementExtended> List(out int recordCount, ElementFilter filter, string sortType = null, int startRowIndex = -1, int maximumRows = -1, bool loadAttributes = false);

        IList<ElementExtended> ListByAdsCategory(string idCulture, int idAdsCategory, int idCustomer);

        /// <summary>
        /// Listar os elementos definidos como favoritos
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        IList<ElementExtended> ListFavorite(string idCulture, List<long> items);

        /// <summary>
        /// Listar Elementos vinculados a um alerta em questão
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <param name="idAlert"></param>
        /// <returns></returns>
        IList<ElementExtended> ListAlert(string idCulture, int idCustomer, long idAlert);

        ///// <summary>
        ///// Listar elementos de um determinado cliente com seleclist
        ///// </summary>
        ///// <param name="idCustomer"></param>
        ///// <returns></returns>
        //SelectList ListByCustomer(int idCustomer);

        /// <summary>
        /// Busca os dados de visualização e detalhes de um cliente
        /// </summary>
        /// <param name="idCustomer"></param>
        /// <returns></returns>
        object[][] ListPageViewXDetailView(long idCustomer);

        /// <summary>
        /// Mudar a imagem padrão
        /// </summary>
        /// <param name="idElement"></param>
        /// <param name="path"></param>
        void ChangeDefaultImage(long idElement, string path);

        /// <summary>
        /// Buscar pelo codigo do site 
        /// </summary>
        /// <param name="idItemSite"></param>
        /// <returns></returns>
        Element GetByIDItemSite(string idItemSite);

        /// <summary>
        /// Insere a imagem ao elemento
        /// </summary>
        /// <param name="idElement"></param>
        /// <param name="thumbImagePathVirtual"></param>
        void InsertImage(long idElement, string path);

        /// <summary>
        /// Deletar todas as imagens de um determinado elemento ou um especifico
        /// </summary>
        /// <param name="idElement"></param>
        /// <param name="idElementAttribute"></param>
        string[] DeleteImage(long idElement, long? idElementAttribute = null);

        /// <summary>
        /// Definir a imagem padrão de exibição
        /// </summary>
        /// <param name="idElement"></param>
        /// <param name="idElementAttribute"></param>
        string SetDefaultImage(long idElement, long idElementAttribute);

        /// <summary>
        /// Listar todas as imagens vinculadas ao elemento
        /// </summary>
        /// <param name="idElement"></param>
        /// <returns></returns>
        IList<Model.ElementAttribute> ListImages(long idElement);

        /// <summary>
        /// Busca o total de imagens de um determinado elemento
        /// </summary>
        /// <param name="idElement"></param>
        /// <returns></returns>
        long GetTotalImages(long idElement);
    }
}

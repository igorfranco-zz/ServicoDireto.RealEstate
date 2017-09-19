using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using int8u = System.Byte;
using int8s = System.SByte;
using int16s = System.Int16;
using int16u = System.UInt16;
using int32s = System.Int32;
using int32u = System.UInt32;

using System.Runtime.InteropServices;
using System.Net;
using Newtonsoft.Json;
using System.Web;
using SpongeSystems.Spider.Entities.ZapImoveis;
using SpongeSystems.Spider.Services.Implementation;
using SpongeSolutions.Core.Helpers;
using MongoDB.Driver;
using MongoDB.Bson;
using SpongeSolutions.ServicoDireto.Model;
using SpongeSolutions.ServicoDireto.Services;
using SpongeSolutions.ServicoDireto.Model.InfraStructure;
using SpongeSolutions.ServicoDireto.Services.InfraStructure;
using SpongeSolutions.Core;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
//using Service = SpongeSolutions.ServicoDireto.Services.ServiceContext;
namespace NBN.STCore.V2.XConsole
{
    public enum TipoExecucao : byte
    {
        Importacao = 0,
        Processamento = 1
    }
    public class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            ZapImoveisService _zapService = new ZapImoveisService();
            TipoExecucao tipo = TipoExecucao.Processamento;
            if (args != null && args.Count() > 0)
                tipo = EnumHelper.TryParse<TipoExecucao>(args[0]);

            log.Info("Carregando");
            string[] states = new string[]
                {
                    "AC",
                    "AL",
                    "AP",
                    "AM",
                    "BA",
                    "CE",
                    "DF",
                    "ES",
                    "GO",
                    "MA",
                    "MT",
                    "MS",
                    "MG",
                    "PA",
                    "PB",
                    "PR",
                    "PE",
                    "PI",
                    "RJ",
                    "RN",
                    "RS",
                    "RO",
                    "RR",
                    "SC",
                    "SP",
                    "SE",
                    "TO"
                };

            foreach (var state in states)
            {
                if (tipo == TipoExecucao.Importacao)
                {
                    ExecutionResult result = null;
                    while (result == null || !result.GotoNext)
                    {
                        if (result == null)
                        {
                            //if (state == "RS")
                            //    result = new ExecutionResult() { GotoNext = false, Page = 3132, State = state };
                            //else
                            result = new ExecutionResult() { GotoNext = false, Page = 1, State = state };
                        }

                        using (Processor proc = new Processor())
                        {
                            result = proc.ImportFromSite(result.State, result.Page);
                        }
                    }
                }
                else
                {
                    using (Processor proc = new Processor())
                    {

                        //proc.UpdateElementAttributes("http://www.zapimoveis.com.br/oferta/venda+casa-de-condominio+3-quartos+ponta-de-baixo+sao-jose+sc+103m2+RS395000/ID-7873567/?paginaoferta=1", 0);
                        //proc.UpdateElementAttributes("http://www.zapimoveis.com.br/lancamento/casa-de-condominio+venda+lagoa-da-conceicao+florianopolis+sc+lagoa-village+alamo-construtora+121m2/ID-11640/?contato=0", 0);

                        int rec = 10;
                        long pages = _zapService.Collection.Count(new BsonDocument()) / rec;
                        if (pages % rec > 0)
                            pages++;

                        log.Info(string.Format("Paginas identificadas [{0}]", pages));

                        for (int i = 1; i < pages; i++)
                        {
                            log.Info(string.Format("Processando página: {0} de [{1}]", i + 1, pages));
                            foreach (var item in _zapService.Collection.Find("{ }").Skip(i * rec).Limit(rec).ToList())
                            {
                                proc.Execute(item);
                            }
                        }

                        Console.ReadKey();
                    }
                }
            }
        }
    }


    public class ExecutionResult
    {
        public int Page { get; set; }
        public string State { get; set; }
        public bool GotoNext { get; set; }
    }

    public class Processor : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ZapImoveisService _zapService = null;
        public void Dispose()
        {
            _zapService = null;
            GC.Collect();
        }

        public Processor()
        {
            Bootstrapper.Initialise();
            //UnityContainerSetup.SetUp();
            _zapService = new ZapImoveisService();
        }

        public ExecutionResult ImportFromSite(string state, int page, bool next = false)
        {
            int totalPages = 0;
            log.Info(string.Format("Carregando pagina {0} - {1}", page, state));
            HashFragment hashFragment = JsonConvert.DeserializeObject<HashFragment>("{\"precomaximo\":\"2147483647\",\"parametrosautosuggest\":[{\"Bairro\":\"\",\"Zona\":\"\",\"Cidade\":\"\",\"Agrupamento\":\"\",\"Estado\":\"" + state + "\"}],\"pagina\":\"" + page + "\",\"ordem\":\"Relevancia\",\"paginaOrigem\":\"ResultadoBusca\",\"semente\":\"0\"}");
            string enc = HttpUtility.UrlEncode(JsonConvert.SerializeObject(hashFragment));
            string url = "http://www.zapimoveis.com.br/Busca/RetornarBuscaAssincrona/";
            string jsonContent = "tipoOferta=1&paginaAtual=1&ordenacaoSelecionada=&pathName=&hashFragment=" + enc;
            Uri target = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);
            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/x-www-form-urlencoded; charset=UTF-8";
            //request.Referer = "http://www.zapimoveis.com.br/venda/apartamentos/rs+porto-alegre/";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:45.0) Gecko/20100101 Firefox/45.0";
            request.KeepAlive = true;
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Dispose();
            }

            try
            {
                ResultContainer result;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                        {
                            StringBuilder json = new StringBuilder();
                            while (reader.Peek() >= 0)
                            {
                                json.Append(reader.ReadLine());
                            }

                            result = JsonConvert.DeserializeObject<ResultContainer>(json.ToString());
                            json.Clear();
                            json = null;
                            totalPages = result.Resultado.QuantidadePaginas;
                            if (page == 1)
                            {
                                log.Info(String.Format("*** Total de Paginas {0} {1}", result.Resultado.QuantidadePaginas, state));
                            }
                            _zapService.Insert(result).Wait();
                            log.Info(String.Format("Pagina {0} de {1} {2} - OK", page, totalPages, state));
                        }
                        responseStream.Dispose();
                        GC.Collect();
                        Thread.Sleep(1000);     //give disk hardware time to recover

                    }
                    response.Dispose();
                    GC.Collect();
                    Thread.Sleep(1000);     //give disk hardware time to recover

                }
                //
                if (page == result.Resultado.QuantidadePaginas)
                    return new ExecutionResult() { GotoNext = true };
                else
                {
                    result = null;
                    page++;
                    return new ExecutionResult() { Page = page, State = state };
                }
            }
            catch (WebException ex)
            {
                log.Error(ex.ToString());
                log.Info(String.Format("Retentando {0} de {1} {2}", page, totalPages, state));
                return new ExecutionResult() { Page = page, State = state };
            }
        }
        //
        public void Execute(ResultContainer resultContainer)
        {
            foreach (var item in resultContainer.Resultado.Resultado_)
                ExecuteItem(item);

            log.Info("FINALIZADO---------------------------------\n\n\n");
        }
        //
        public void ExecuteItem(ResultadoItem item)
        {
            log.Info(string.Format("Processando: CodigoOfertaZAP: {0} Link:{1}", item.CodigoOfertaZAP, item.UrlFicha));
            Element element = SpongeSolutions.ServicoDireto.Services.ServiceContext.ElementService.GetByIDItemSite(item.CodigoOfertaZAP.ToString() + "|ZAP");
            if (element == null)
            {
                List<ElementAttribute> listElementAttribute = new List<ElementAttribute>();
                List<ElementCulture> listElementCulture = new List<ElementCulture>();
                StateProvince stateProvince = SpongeSolutions.ServicoDireto.Services.ServiceContext.StateProvinceService.GetInsert(item.Estado, "pt-BR");
                City city = SpongeSolutions.ServicoDireto.Services.ServiceContext.CityService.GetInsert(item.Cidade, stateProvince.IDStateProvince.Value);
                Purpose purpose = SpongeSolutions.ServicoDireto.Services.ServiceContext.PurposeService.GetInsert(item.Transacao);
                HierarchyStructure hierarchyStructure = SpongeSolutions.ServicoDireto.Services.ServiceContext.HierarchyStructureService.GetInsert(item.SubTipoOferta, purpose.IDPurpose);
                Customer customer;
                string email = null;
                string phone = null;
                if (item.ContatoCampanha != null)
                {
                    email = item.ContatoCampanha.Email;
                    if (item.ContatoCampanha.Telefone != null)
                        phone = string.Format("({0}){1}", item.ContatoCampanha.Telefone.DDD, item.ContatoCampanha.Telefone.Numero);
                }
                customer = SpongeSolutions.ServicoDireto.Services.ServiceContext.CustomerService.GetInsert(item.NomeAnunciante, null, null, email, phone, item.CodigoAnunciante.ToString(), item.UrlLogotipoCliente);

                log.Info("Dado base retornado com sucesso");
                //
                element = new Element()
                {
                    //DefaultPicturePath = "",//????
                    IDHierarchyStructureParent = hierarchyStructure.IDHierarchyStructureParent.Value,
                    Latitude = 0,
                    Longitude = 0,
                    DetailView = 0,
                    AllowRatting = true,
                    IDItemSite = item.CodigoOfertaZAP.ToString() + "|ZAP",
                    IDHierarchyStructure = hierarchyStructure.IDHierarchyStructure.Value,
                    IDPurpose = purpose.IDPurpose.Value,
                    IDCustomer = customer.IDCustomer.Value,
                    Address = string.IsNullOrEmpty(item.Endereco) ? "-" : item.Endereco,
                    Neighborhood = item.Bairro,
                    IDCity = city.IDCity.Value,
                    CreateDate = DateTime.Now,
                    ModifyDate = DateTime.Now,
                    CreatedBy = "AUTO",
                    ModifiedBy = "AUTO",
                    ShowAddress = false,
                    Url = item.UrlFicha,
                    PageView = 0,
                    IsPromoted = false,
                    Status = (short)Enums.StatusType.Active,
                };
                //
                foreach (var culture in SpongeSolutions.ServicoDireto.Services.ServiceContext.CultureService.GetAllActive())
                {
                    listElementCulture.Add(new ElementCulture()
                    {
                        IDCulture = culture.IDCulture,
                        Description = item.DetalhesOferta,
                        Name = string.IsNullOrEmpty(item.Empreendimento) ? item.FormatarSubTipoOferta : item.Empreendimento
                    });
                }
                //colocar AttributeTypeCulture  em basico

                log.Info("Carregando attributos");

                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Quartos/Dormitórios"), Value = item.QuantidadeQuartos.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeQuartosMinima"), Value = item.QuantidadeQuartosMinima.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeQuartosMaxima"), Value = item.QuantidadeQuartosMaxima.ToString() });
                //
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Vagas de garagem"), Value = item.QuantidadeVagas.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeVagasMinima"), Value = item.QuantidadeVagasMinima.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeVagasMaxima"), Value = item.QuantidadeVagasMaxima.ToString() });
                //
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Suites"), Value = item.QuantidadeSuites.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeSuitesMinima"), Value = item.QuantidadeSuitesMinima.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("QuantidadeSuitesMaxima"), Value = item.QuantidadeSuitesMaxima.ToString() });
                //
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Área Útil (m²)"), Value = item.Area.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Àrea Total (m²)"), Value = item.Area.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("AreaMinima"), Value = item.AreaMinima.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("AreaMaxima"), Value = item.AreaMaxima.ToString() });

                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("PrecoVendaMinimo"), Value = item.PrecoVendaMinimo.ToString() });
                listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("PrecoVendaMaximo"), Value = item.PrecoVendaMaximo.ToString() });

                if (!string.IsNullOrEmpty(item.Valor))
                {
                    Regex r = new Regex(ExpressionValidators.GetOnlyNumbers, RegexOptions.IgnoreCase);
                    Match m = r.Match(item.Valor);
                    listElementAttribute.Add(new ElementAttribute() { IDAttribute = SpongeSolutions.ServicoDireto.Services.ServiceContext.AttributeService.GetInsert("Valor Total (R$)"), Value = m.Value.Replace(".", "") });
                }

                log.Info("Efetuando Inserção");
                SpongeSolutions.ServicoDireto.Services.ServiceContext.ElementService.Insert(element, listElementCulture, listElementAttribute);

                this.DownloadImages(item.Fotos, element);
                log.Info("OK---------------------------------\n\n\n");
            }
            else
            {
                log.Info("JA EXISTENTE---------------------------------\n\n\n");
            }

        }
        //
        public void DownloadImages(IList<Foto> fotos, Element element)
        {
            string basePath = string.Format(@"D:\Avantika\Projects\ServicoDireto\trunk\ServivoDiretoAPI\source\ServicoDiretoAdmin\Uploads\{0}\{1}", element.IDCustomer, element.IDElement);
            string virtualBasePath = string.Format(@"/Uploads/{0}/{1}", element.IDCustomer, element.IDElement);
            if (fotos != null)
            {
                foreach (var foto in fotos)
                {
                    try
                    {
                        string newFileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
                        string imagePath = String.Format(@"{0}\{1}", basePath, newFileName);
                        string thumbImagePath = String.Format(@"{0}\Thumb\{1}", basePath, newFileName);
                        string thumbImagePathVirtual = String.Format(@"{0}/Thumb/{1}", virtualBasePath, newFileName);

                        log.Info(string.Format("Baixando foto: {0} ", foto.UrlImagemTamanhoGG));
                        IOHelper.DownloadFile(new Uri(foto.UrlImagemTamanhoGG), imagePath);

                        log.Info(string.Format("Baixando foto thumb: {0} ", foto.UrlImagemTamanhoG));
                        IOHelper.DownloadFile(new Uri(foto.UrlImagemTamanhoG), thumbImagePath);

                        if (foto.Principal)
                        {
                            element.DefaultPicturePath = thumbImagePathVirtual;
                            SpongeSolutions.ServicoDireto.Services.ServiceContext.ElementService.Update(element);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        public void UpdateElementAttributes(string url, long idElement)
        {
            List<LDScript> details;
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("iso-8859-1") };
            HtmlDocument htmlDocument = htmlWeb.Load(url);
            var node = htmlDocument.DocumentNode.SelectSingleNode("//script[@type=\"application/ld+json\"]");
            if (node != null)
            {
                details = JsonConvert.DeserializeObject<List<LDScript>>(node.InnerText);
            }
            else
            {
                var item = new LDScript();
                node = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[1]/section[2]/div[1]/span");
                item.object_ = new SpongeSystems.Spider.Entities.ZapImoveis.Object()
                {
                    description = HttpUtility.HtmlDecode(node.InnerText)
                };

                //node = htmlDocument.DocumentNode.SelectSingleNode();                
            }

            //http://www.zapimoveis.com.br/oferta/venda+casa-de-condominio+3-quartos+ponta-de-baixo+sao-jose+sc+103m2+RS395000/ID-7873567/?paginaoferta=1
        }
    }
}
using HtmlAgilityPack;
using SpongeSystems.Spider.Entities.ServicoDireto;
using SpongeSystems.Spider.Services;
using SpongeSystems.Spider.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpongeSystems.ServidoDiretoSpider
{
    class Program
    {
        static LinkService linkService = new LinkService();
        const string BASE_URL = "http://www.telelistas.net";

        static void Main(string[] args)
        {

            //GetLinkDetail(new Link() { Url = "age: http://www.telelistas.net/AL/arapiraca/reboque+e+socorro+mecanico+24+horas?pagina=1" });
            //return;
            Console.WriteLine("Carregando");
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
                Process(state);
            }
        }

        private static void Process(string state)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("*Iniciando processo do estado: {0}*", state);
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("iso-8859-2") };
            HtmlDocument htmlDocument = null;
            //percorre todos as letras dos indices
            for (int i = 65; i < 90; i++)
            {
                //************************************
                //cidades não esta paginando
                //************************************
                var letter = (char)i;
                string page = string.Format("{0}/{1}-{2}/", BASE_URL, state, letter);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("_Iniciando {0}", page);

                Console.ForegroundColor = ConsoleColor.White;
                htmlDocument = htmlWeb.Load(page);
                var cityContatiner = htmlDocument.DocumentNode.SelectSingleNode("//table[@id=\"ctl00_Content_dataListCidades\"]");
                //verifica se existem cidades a serem verificadas
                if (cityContatiner != null)
                {
                    //percorre todas as cidades identificadas
                    var cities = cityContatiner.SelectNodes(".//a[contains(@id,'ctl00_Content_dataListCidades_ct')]");
                    foreach (var city in cities)
                    {
                        htmlDocument = htmlWeb.Load(string.Format("{0}{1}", BASE_URL, city.Attributes["href"].Value));
                        var categories = htmlDocument.DocumentNode.SelectNodes(".//a[contains(@id,'ctl00_Content_dataListPalavrasChave_ct')]");
                        if (categories != null)
                        {
                            foreach (var category in categories)
                            {
                                string pageToCrawling = string.Format("{0}{1}", BASE_URL, category.Attributes["href"].Value);
                                htmlDocument = htmlWeb.Load(pageToCrawling);
                                //total de registros
                                var totalNode = htmlDocument.DocumentNode.SelectSingleNode("//span[@class=\"texto_caminho_registro\"]");
                                int totalPages = 0;
                                int totalRecords = 0;
                                if (totalNode != null)
                                {
                                    int.TryParse(GetOnlyNumber(totalNode.InnerText), out totalRecords);
                                    totalPages = totalRecords / 25;
                                    if (totalRecords % 25 > 0)
                                        totalPages++;
                                }

                                for (int j = 1; j <= totalPages; j++)
                                {
                                    Console.ForegroundColor = ConsoleColor.White;
                                    page = string.Format("{0}?pagina={1}", pageToCrawling, j);
                                    Console.WriteLine("__Carregando: {0} Page: {1}", j, page);
                                    htmlDocument = htmlWeb.Load(page);
                                    //a[contains(@id,'ctl00_Content_dataListPalavrasChave_ct')]
                                    var content = htmlDocument.DocumentNode.SelectNodes("//td[@class=\"text_resultado_ib\"]/a | //span[contains(@class, 'text_resultado_topo_registro')]");
                                    if (content != null)
                                    {
                                        foreach (HtmlNode item in content)
                                        {
                                            var link = item;
                                            if (item.Name == "span")
                                                link = item.ParentNode;

                                            string id = link.Attributes["href"].Value.Split('/')[7];

                                            Link record = new Spider.Entities.ServicoDireto.Link()
                                            {
                                                LinkID = id,
                                                Name = link.InnerText.Trim(),
                                                Url = link.Attributes["href"].Value,
                                                State = state,
                                                City = city.InnerText,
                                                Category = category.InnerText,
                                                CreateDate = DateTime.Now,
                                                ModifyDate = DateTime.Now,
                                                CreatedBy = "CRAWLER",
                                                ModifiedBy = "CRAWLER",
                                                Status = Spider.Entities.Enums.StatusType.WaitingDetail
                                            };

                                            GetLinkDetail(record);
                                            linkService.InsertUpdate(record);
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine(string.Format("...{0} - [{1}] - {2}", id, link.InnerText.Trim(), link.Attributes["href"].Value));
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Não foram encotradas cidades para o estado {0} indice {1}", state, letter);
                }
            }
        }

        private static string GetOnlyNumber(string value)
        {
            string result = "";
            Regex regex = new Regex(@"\d+");
            var matches = regex.Matches(value);
            foreach (var match in matches)
                result += match;

            return result;
        }

        private static void GetLinkDetail(Link record)
        {
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine("______Buscando informações de {0}", record.Name);
            HtmlWeb htmlWeb = new HtmlWeb() { AutoDetectEncoding = false, OverrideEncoding = Encoding.GetEncoding("iso-8859-2") };
            HtmlDocument htmlDocument = null;
            //htmlDocument = htmlWeb.Load("http://www.telelistas.net/locais/rj/rio+de+janeiro/revendedores+e+concessionarias+de+automoveis/301319490/brilho+car+automoveis");
            //htmlDocument = htmlWeb.Load("http://www.telelistas.net/locais/ms/campo+grande/revendedores+e+concessionarias+de+automoveis/301243477/hc+veiculos+especiais");        
            htmlDocument = htmlWeb.Load(record.Url);

            var phones = htmlDocument.DocumentNode.SelectNodes("//span[@class='content_contato']/div[@class='infoplus_text2 telInfo']/div");
            if(phones == null)
                phones = htmlDocument.DocumentNode.SelectNodes("//div[@class='infoplus_text2 telInfo']/div");
            
            var activities = htmlDocument.DocumentNode.SelectNodes("//h2[@class='linha_atv']/a");
            var prodsservs = htmlDocument.DocumentNode.SelectNodes("//h3[@class='content_informacao']/li");
            var detailNode = htmlDocument.DocumentNode.SelectSingleNode("//h3[@class='content_informacao']");
            var siteNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='url']");
            var addressNode = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='enderecoreg']");
            var addressNumberNode = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='numeroreg']");
            var latitudeNode = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='gradeX']");
            var longitudeNode = htmlDocument.DocumentNode.SelectSingleNode("//input[@id='gradeY']");
            var logoNode = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='logomarca']/img");

            //TODO REVER ESSE ABAIXO
            var paymentMethod = htmlDocument.DocumentNode.SelectSingleNode("//div[@id='content_formapgto']");
            var emails = htmlDocument.DocumentNode.SelectNodes("//select[@id='email_dest']/option");
            if (emails != null)
            {
                foreach (var email in emails)
                    record.Emails.Add(new Email() { Value = email.Attributes["value"].Value.Trim() });
            }

            if (phones != null)
            {
                foreach (var phone in phones)
                {
                    try
                    {
                        Phone result = new Phone();
                        var rawPhone = phone.ChildNodes[0].InnerText.Trim();
                        var prefixPhone = GetOnlyNumber(rawPhone);
                        var fone2 = new Uri(phone.ChildNodes[1].Attributes["src"].Value);
                        var qs = System.Web.HttpUtility.ParseQueryString(fone2.Query);
                        var fone = prefixPhone + GetNumber(qs["t"]);
                        result.Number = fone;
                        result.Type = rawPhone.Substring(0, rawPhone.IndexOf(":"));
                        record.Phones.Add(result);
                    }
                    catch
                    {
                        //Logar nos serviços
                    }
                }
            }

            if (activities != null)
            {
                foreach (var activity in activities)
                    record.Activities.Add(new Activity() { Nome = activity.InnerText });
            }

            if (prodsservs != null)
            {
                foreach (var prodserv in prodsservs)
                    record.ProductsAndServices.Add(new ProductAndService() { Name = prodserv.InnerText });
            }

            record.Logo = (logoNode != null) ? logoNode.Attributes["src"].Value.Trim() : "";
            record.Details = (detailNode != null) ? detailNode.InnerText : "";
            record.Site = (siteNode != null) ? siteNode.InnerText.Trim() : "";
            record.Address = (addressNode != null) ? addressNode.Attributes["value"].Value.Trim() : "";
            record.AddressNumber = (addressNumberNode != null) ? addressNumberNode.Attributes["value"].Value.Trim() : "";
            record.Latitude = (latitudeNode != null) ? latitudeNode.Attributes["value"].Value.Trim() : "";
            record.Longitude = (longitudeNode != null) ? longitudeNode.Attributes["value"].Value.Trim() : "";

            //var contactInfo = htmlDocument.DocumentNode.SelectNodes("//p[@class='infoplus_text1']");
            //if (contactInfo.Count == 3)
            //{
            //    var rawAddress = Regex.Split(contactInfo[0].InnerHtml, "<br>");
            //    record.Address = rawAddress[0].Trim();
            //    var rawNeighborhood = rawAddress[1].Split('-');
            //    record.Neighborhood = rawNeighborhood[0].Trim();
            //}            
        }

        private static string GetNumber(string code)
        {
            string prefixValue = code.Substring(0, 2);
            string sufixValue = code.Substring(2);
            List<KeyValuePair<string, string>> prefix = new List<KeyValuePair<string, string>>();
            List<KeyValuePair<string, string>> sufix = new List<KeyValuePair<string, string>>();
            prefix.Add(new KeyValuePair<string, string>("66", "9"));
            prefix.Add(new KeyValuePair<string, string>("67", "8"));
            prefix.Add(new KeyValuePair<string, string>("68", "7"));
            prefix.Add(new KeyValuePair<string, string>("69", "6"));
            prefix.Add(new KeyValuePair<string, string>("6A", "5"));
            prefix.Add(new KeyValuePair<string, string>("6B", "4"));
            prefix.Add(new KeyValuePair<string, string>("6C", "3"));
            prefix.Add(new KeyValuePair<string, string>("6D", "2"));
            prefix.Add(new KeyValuePair<string, string>("6E", "1"));
            prefix.Add(new KeyValuePair<string, string>("6F", "0"));

            sufix.Add(new KeyValuePair<string, string>("7D", "0"));
            sufix.Add(new KeyValuePair<string, string>("7C", "1"));
            sufix.Add(new KeyValuePair<string, string>("7F", "2"));
            sufix.Add(new KeyValuePair<string, string>("7E", "3"));
            sufix.Add(new KeyValuePair<string, string>("79", "4"));
            sufix.Add(new KeyValuePair<string, string>("78", "5"));
            sufix.Add(new KeyValuePair<string, string>("7B", "6"));
            sufix.Add(new KeyValuePair<string, string>("7A", "7"));
            sufix.Add(new KeyValuePair<string, string>("75", "8"));
            sufix.Add(new KeyValuePair<string, string>("74", "9"));

            var v1 = prefix.First(p => p.Key == prefixValue).Value;
            var v2 = sufix.First(p => p.Key == sufixValue).Value;

            return v1 + v2;
        }
    }
}

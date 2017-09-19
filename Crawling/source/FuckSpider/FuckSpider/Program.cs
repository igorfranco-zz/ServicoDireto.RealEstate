using SpongeSystems.Spider.Entities.Realty;
using SpongeSystems.Spider.Services;
using SpongeSystems.Spider.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpongeSystems.RealtySpider
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrapper.Initialise();

            var property = new Property()
            {
                Bathrooms = 2,
                Bedrooms = 10,
                CreateDate = DateTime.Now,
                CreatedBy = "Igor",
                ModifiedBy = "Teste",
                Details = "Detalhamento...",
                Garages = 1,
                HouseSize = 10,
                LotSize = 10,
                Name = "Propriedade 1",
                ModifyDate = DateTime.Now,
                Price = 150,
                Url = "www.terra.com.br",
                Status = Spider.Entities.Enums.StatusType.Active,
                WebsiteID = "15000"
            };

            property.Addresses.Add(new Spider.Entities.Realty.Address() { City = "Porto Alegre", Country = "Brasil", Latitute = 0, Longitude = 0, Name = "Casa do Zé", Neighborhood = "Rubem Berta", PostalCode = "91250-180", StateProvince = "RS" });
            property.Attributes.Add(new Spider.Entities.Realty.Attribute() { Name = "Teste_1", Value = "50" });
            property.Attributes.Add(new Spider.Entities.Realty.Attribute() { Name = "Teste_2", Value = "60" });
            property.Attributes.Add(new Spider.Entities.Realty.Attribute() { Name = "Teste_3", Value = "70" });
            property.Attributes.Add(new Spider.Entities.Realty.Attribute() { Name = "Teste_4", Value = "80" });

            new PropertyService().Insert(property).Wait();

            ////var item = new AgencyService().Get("55e0a8b6c23ba51b34428981").GetAwaiter().GetResult();
            //var items = new AgencyService().FindAll().GetAwaiter().GetResult();
            //foreach (var item in items)
            //{
            //    new AgencyService().Delete(item).Wait();
            //    Console.WriteLine("Deletando {0}", item.Name);
            //}


            //if (1 == 1)
            //{
            //    for (int i = 0; i < 50; i++)
            //    {
            //        var agency = new Spider.Entities.Realty.Agency()
            //        {
            //            CellPhone = "51-94643433",
            //            CreateDate = DateTime.Now,
            //            CreatedBy = "igor1",
            //            ModifyDate = DateTime.Now,
            //            ModifiedBy = "igor2",
            //            Name = string.Format("RIAL {0}", i),
            //            Phone = "51-33474754",
            //            Url = "www.servicodireto.com",
            //            Status = Spider.Entities.Enums.StatusType.Active,
            //            Email = "igorfranco@gmail.com",
            //            Facebook = "@facebook",
            //            Twitter = "@Twitter",
            //        };

            //        agency.Addresses.Add(new Spider.Entities.Realty.Address() { City = "Porto Alegre", Country = "Brasil", Latitute = 0, Longitude = 0, Name = "Casa do Zé", Neighborhood = "Rubem Berta", PostalCode = "91250-180", StateProvince = "RS" });
            //        agency.Contacts.Add(new Spider.Entities.Realty.Contact() { CellPhone = "51-33666655", Name = "Igor Franco Brum" });
            //        new AgencyService().Save(agency).Wait();
            //        Console.WriteLine("Salvando {0}", agency.Name);
            //    }
            //}
        }
    }
}

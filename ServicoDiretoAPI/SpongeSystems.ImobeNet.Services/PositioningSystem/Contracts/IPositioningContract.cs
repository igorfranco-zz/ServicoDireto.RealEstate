using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Api.Maps.Service;
namespace SpongeSolutions.ServicoDireto.Services.PositioningSystem.Contracts
{
    public interface IPositioningContract
    {
        /// <summary>
        /// Busca a posição geográfica baseda no endereço
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        GeographicPosition GetGeographicPosition(string address);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpongeSolutions.ServicoDireto.Services.PositioningSystem.Contracts;
using Google.Api.Maps.Service.Geocoding;
using Google.Api.Maps.Service;

namespace SpongeSolutions.ServicoDireto.Services.PositioningSystem.Implementation
{
    public class PositioningService : IPositioningContract
    {
        public GeographicPosition GetGeographicPosition(string address)
        {
            var request = new GeocodingRequest();
            request.Address = address;
            request.Sensor = "false";
            var response = GeocodingService.GetResponse(request);
            if (response != null)
            {
                var result = response.Results.FirstOrDefault();
                if (result != null)
                    return result.Geometry.Location;
            }
            return null;
        }
    }
}

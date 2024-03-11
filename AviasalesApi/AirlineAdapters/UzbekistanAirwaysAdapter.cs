using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public class UzbekistanAirwaysAdapter
    {
        public string Endpoint => "https://www.uzairways.uz/api/get";

        public IEnumerable<Flight> GetFlights(GetFlightsDto getFlightsDto)
        {
            throw new NotImplementedException();
        }
    }
}

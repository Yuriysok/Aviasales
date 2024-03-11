using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public class LufthansaAdapter
    {
        public string Endpoint => "https://www.lufthansa.com/api/get";

        public IEnumerable<Flight> GetFlights(GetFlightsDto getFlightsDto)
        {
            throw new NotImplementedException();
        }
    }
}

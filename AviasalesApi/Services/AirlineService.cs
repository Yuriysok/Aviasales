using AviasalesApi.Models;
using AviasalesApi.Services.AirlineAdapters;
namespace AviasalesApi.Services
{
    public class AirlineService(IEnumerable<IAirlineAdapter> adapters) : IAirlineService
    {
        private readonly HttpClient Http = new HttpClient(new MockHttpHandler());
        private readonly IEnumerable<IAirlineAdapter> _adapters = adapters;

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync(GetFlightsDto getFlightsDto)
        {
            var tasks = new List<Task<List<Flight>>>();

            foreach (var adapter in _adapters)
            {
                tasks.Add(GetFlights(adapter, getFlightsDto));
            }

            var flights = await Task.WhenAll(tasks);

            var result = flights.SelectMany(x => x);

            return result;
        }

        private async Task<List<Flight>> GetFlights(IAirlineAdapter adapter, GetFlightsDto getFlightsDto)
        {
            var url = adapter.ConstructRequestUrl(getFlightsDto);
            var response = await Http.GetAsync(url);
            var airlineFlightList = await response.Content.ReadFromJsonAsync(adapter.ResponseType);
            return (List<Flight>)adapter.Mapper.Map(airlineFlightList, adapter.ResponseType, typeof(List<Flight>));
        }
    }
}

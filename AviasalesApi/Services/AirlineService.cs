using AviasalesApi.AirlineAdapters;
using AviasalesApi.Models;
using Moq;
namespace AviasalesApi.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly HttpClient Http;
        private readonly IEnumerable<IAirlineAdapter> _adapters;

        public AirlineService(IEnumerable<IAirlineAdapter> adapters)
        {
            _adapters = adapters;
            Http = new HttpClient(new MockHttpHandler());
        }

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
            var dtos = await response.Content.ReadFromJsonAsync(adapter.ResponseType);
            return (List<Flight>)adapter.Mapper.Map(dtos, adapter.ResponseType, typeof(List<Flight>));
        }
    }
}

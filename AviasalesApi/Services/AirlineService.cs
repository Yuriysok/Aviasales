using AviasalesApi.Models;
using AviasalesApi.Services.AirlineAdapters;
namespace AviasalesApi.Services
{
    public class AirlineService(IEnumerable<IAirlineAdapter> adapters) : IAirlineService
    {
        private readonly HttpClient Http = new(new MockHttpHandler());
        private readonly IEnumerable<IAirlineAdapter> _adapters = adapters;

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync(GetFlightsDto getFlightsDto)
        {
            var flightTasks = new List<Task<List<Flight>>>();

            foreach (var adapter in _adapters)
            {
                flightTasks.Add(GetFlights(adapter, getFlightsDto));
            }

            var allFlights = await Task.WhenAll(flightTasks);

            var result = allFlights.SelectMany(flights => flights);

            return result;
        }

        private async Task<List<Flight>> GetFlights(IAirlineAdapter adapter, GetFlightsDto getFlightsDto)
        {
            var requestUrl = adapter.ConstructRequestUrl(getFlightsDto);
            var response = await Http.GetAsync(requestUrl);
            var airlineFlightList = await response.Content.ReadFromJsonAsync(adapter.ResponseType);
            return (List<Flight>)adapter.ResponseMapper.Map(airlineFlightList, adapter.ResponseType, typeof(List<Flight>));
        }
    }
}

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
                tasks.Add(adapter.GetFlightsAsync(getFlightsDto, Http));
            }

            var flights = await Task.WhenAll(tasks);

            var result = flights.SelectMany(x => x);

            return result;

            //var stubFlights = new List<Flight>
            //{
            //    new Flight
            //    {
            //        Airline = Airline.Aeroflot,
            //        Departure = new DateTime(2024, 1, 1),
            //        Arrival = new DateTime(2024, 1, 2),
            //        PriceUsd = 20

            //    }
            //};
            //return flights;
        }
    }
}

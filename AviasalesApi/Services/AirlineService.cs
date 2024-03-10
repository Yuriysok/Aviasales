using AviasalesApi.AirlineAdapters;
using AviasalesApi.Models;
using Moq;
using Moq.Protected;
using System.Text.Json;

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

        public async Task<List<Flight>> GetFlightsAsync(GetFlightsDto getFlightsDto)
        {
            var tasks = new List<Task<HttpResponseMessage>>();

            foreach (var adapter in _adapters)
            {
                tasks.Add(Http.GetAsync(adapter.Endpoint));
            }

            var messages = await Task.WhenAll(tasks);

            var flights = new List<Flight>();
            foreach (var message in messages)
            {
                flights.Add(await message.Content.ReadFromJsonAsync<Flight>());
            }

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
            return flights;
        }

        private static Mock<HttpClient> GetHttpMock()
        {
            Mock<HttpClient> httpMock = new();
            var path = "Services/MockAirlineResponses/";
            SetupFromUrlAndPath(httpMock, "aeroflot.ru/api/get", path+"Aeroflot.json", 300);
            SetupFromUrlAndPath(httpMock, "lufthansa.com/api/get", path+"MockAirlineResponses/Lufthansa.json", 400);
            SetupFromUrlAndPath(httpMock, "uzairways.uz/api/get", path+"MockAirlineResponses/UzbekistanAirways.json", 500);
            return httpMock;
        }

        private static void SetupFromUrlAndPath(Mock<HttpClient> mock, string url, string path, int delayMs)
        {
            var httpMessage = new HttpResponseMessage();
            var aeroflotJson = File.ReadAllText(path);
            httpMessage.Content = new StringContent(aeroflotJson);

            mock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(httpMessage, TimeSpan.FromMilliseconds(delayMs));

            //mock.Setup(x => x.GetAsync(url)).ReturnsAsync(httpMessage, TimeSpan.FromMilliseconds(delayMs));
        }
    }
}

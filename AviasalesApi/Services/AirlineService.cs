using AviasalesApi.Models;
using AviasalesApi.Services.AirlineAdapters;

namespace AviasalesApi.Services
{
    public class AirlineService(IEnumerable<IAirlineAdapter> adapters) : IAirlineService
    {
        private readonly HttpClient Http = new(new MockHttpHandler());
        private readonly IEnumerable<IAirlineAdapter> _adapters = adapters;

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync(
            GetFlightsDto getFlightsDto,
            FilterOptions? filterOptions = null,
            SortOptions sortOptions = SortOptions.None
            )
        {
            var flightTasks = new List<Task<List<Flight>>>();

            foreach (var adapter in _adapters)
            {
                flightTasks.Add(GetFlights(adapter, getFlightsDto));
            }

            var allFlights = await Task.WhenAll(flightTasks);

            var result = allFlights.SelectMany(flights => flights);

            result = FilterResult(result, filterOptions);

            result = SortResult(result, sortOptions);

            return result;
        }
        private async Task<List<Flight>> GetFlights(IAirlineAdapter adapter, GetFlightsDto getFlightsDto)
        {
            var requestUrl = adapter.ConstructRequestUrl(getFlightsDto);
            var response = await Http.GetAsync(requestUrl);
            var airlineFlightList = await response.Content.ReadFromJsonAsync(adapter.ResponseType);
            return (List<Flight>)adapter.ResponseMapper.Map(airlineFlightList, adapter.ResponseType, typeof(List<Flight>));
        }

        private static IEnumerable<Flight> FilterResult(IEnumerable<Flight> result, FilterOptions? optionsNullable)
        {
            if (optionsNullable == null)
                return result;

            var options = optionsNullable.Value;

            if (options.PriceFrom != 0)
                result = result.Where(x => x.PriceUsd >= options.PriceFrom);

            if (options.PriceTo != default)
                result = result.Where(x => x.PriceUsd <= options.PriceTo);

            if (options.ArrivalTimeFrom != default)
                result = result.Where(x => x.Arrival >= options.ArrivalTimeFrom);

            if (options.ArrivalTimeFrom != default)
                result = result.Where(x => x.Arrival >= options.ArrivalTimeTo);

            if (options.DepartureTimeFrom != default)
                result = result.Where(x => x.Departure >= options.DepartureTimeFrom);

            if (options.DepartureTimeFrom != default)
                result = result.Where(x => x.Departure >= options.DepartureTimeTo);

            if (options.Airlines != null)
                result = result.Where(x => options.Airlines.Contains(x.Airline));

            return result;
        }

        private static IEnumerable<Flight> SortResult(IEnumerable<Flight> result, SortOptions options) =>
            options switch
            {
                SortOptions.None => result,
                SortOptions.OrderByPriceAsc => result.OrderBy(x => x.PriceUsd),
                SortOptions.OrderByPriceDesc => result.OrderByDescending(x => x.PriceUsd),
                SortOptions.OrderByAirlineAsc => result.OrderBy(x => x.Airline),
                SortOptions.OrderByAirlineDesc => result.OrderByDescending(x => x.Airline),
                SortOptions.OrderByDepartureDateAsc => result.OrderBy(x => x.Departure),
                SortOptions.OrderByDepartureDateDesc => result.OrderByDescending(x => x.Departure),
                SortOptions.OrderByArrivalDateAsc => result.OrderBy(x => x.Arrival),
                SortOptions.OrderByArrivalDateDesc => result.OrderByDescending(x => x.Arrival),
                _ => throw new ArgumentOutOfRangeException(nameof(options))
            };  
    }
}

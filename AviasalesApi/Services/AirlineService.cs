﻿using AviasalesApi.Models;
using AviasalesApi.Services.AirlineAdapters;
using System.Net;

namespace AviasalesApi.Services
{
    public class AirlineService(IEnumerable<IAirlineAdapter> adapters) : IAirlineService
    {
        private readonly HttpClient Http = new(new MockHttpHandler());
        private readonly IEnumerable<IAirlineAdapter> _adapters = adapters;

        public async Task<IEnumerable<Flight>> GetAllFlightsAsync(
            FlightInfo flightInfo,
            FilterOptions filterOptions,
            SortOptions sortOptions
            )
        {
            var flightTasks = new List<Task<List<Flight>>>();

            foreach (var adapter in _adapters)
            {
                flightTasks.Add(GetFlights(adapter, flightInfo));
            }

            var allFlights = await Task.WhenAll(flightTasks);

            var result = allFlights.SelectMany(flights => flights);

            result = FilterResult(result, filterOptions);

            result = SortResult(result, sortOptions);

            return result;
        }

        public async Task<HttpStatusCode> BookFlightAsync(BookFlightDto bookFlightDto)
        {
            var adapter = _adapters.Single(x => x.Airline == bookFlightDto.Airline);
            var bookingRequestUrl = adapter.BookingUrl;
            var dto = adapter.GetBookingJson(bookFlightDto);
            var response = await Http.PostAsJsonAsync(bookingRequestUrl, dto);
            return response.StatusCode;
        }

        private async Task<List<Flight>> GetFlights(IAirlineAdapter adapter, FlightInfo flightInfo)
        {
            var requestUrl = adapter.ConstructRequestUrl(flightInfo);
            var response = await Http.GetAsync(requestUrl);
            var airlineFlightList = await response.Content.ReadFromJsonAsync(adapter.ResponseType);
            return (List<Flight>)adapter.ResponseMapper.Map(airlineFlightList, adapter.ResponseType, typeof(List<Flight>));
        }

        private static IEnumerable<Flight> FilterResult(IEnumerable<Flight> result, FilterOptions options)
        {
            if (options.PriceFrom != default)
                result = result.Where(x => x.PriceUsd >= options.PriceFrom);

            if (options.PriceTo != default)
                result = result.Where(x => x.PriceUsd <= options.PriceTo);

            if (options.ArrivalTimeFrom != default)
                result = result.Where(x => TimeOnly.FromDateTime(x.Arrival) >= options.ArrivalTimeFrom);

            if (options.ArrivalTimeTo != default)
                result = result.Where(x => TimeOnly.FromDateTime(x.Arrival) <= options.ArrivalTimeTo);

            if (options.DepartureTimeFrom != default)
                result = result.Where(x => TimeOnly.FromDateTime(x.Departure) >= options.DepartureTimeFrom);

            if (options.DepartureTimeTo != default)
                result = result.Where(x => TimeOnly.FromDateTime(x.Departure) <= options.DepartureTimeTo);

            if (options.TransitFlightsMax != default)
                result = result.Where(x => x.NumberOfFlights <= options.TransitFlightsMax);

            if (options.Airlines!.Count != 0)
                result = result.Where(x => options.Airlines.Contains(x.Airline));

            return result;
        }

        private static IEnumerable<Flight> SortResult(IEnumerable<Flight> result, SortOptions options)
        {
            result = result.OrderBy(x => x.NumberOfFlights);
            return options switch
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
}

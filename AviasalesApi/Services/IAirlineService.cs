using AviasalesApi.Models;
using System.Net;

namespace AviasalesApi.Services
{
    public interface IAirlineService
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync(
            FlightInfo flightInfo,
            FilterOptions filterOptions,
            SortOptions sortOptions
            );

        Task<HttpStatusCode> BookFlightAsync(BookFlightDto bookFlightDto);
    }
}
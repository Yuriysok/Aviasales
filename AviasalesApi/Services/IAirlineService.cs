using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    public interface IAirlineService
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync(
            FlightInfo flightInfo,
            FilterOptions filterOptions,
            SortOptions sortOptions
            );
    }
}
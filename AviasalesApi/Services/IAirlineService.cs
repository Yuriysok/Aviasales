using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    public interface IAirlineService
    {
        Task<IEnumerable<Flight>> GetAllFlightsAsync(
            GetFlightsDto getFlightsDto,
            FilterOptions filterOptions,
            SortOptions sortOptions
            );
    }
}
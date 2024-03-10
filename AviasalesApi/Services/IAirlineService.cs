using AviasalesApi.Models;

namespace AviasalesApi.Services
{
    public interface IAirlineService
    {
        Task<List<Flight>> GetFlightsAsync(GetFlightsDto getFlightsDto);
    }
}
using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public interface IAirlineAdapter
    {
        string Endpoint { get; }

        Task<List<Flight>> GetFlightsAsync(GetFlightsDto getFlightsDto, HttpClient http);

        string ConstructRequestUrl(GetFlightsDto getFlightsDto);
    }
}
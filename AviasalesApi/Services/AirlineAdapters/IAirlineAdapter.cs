using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public interface IAirlineAdapter
    {
        Type ResponseType { get; }
        string Endpoint { get; }
        IMapper Mapper { get; }
        string ConstructRequestUrl(GetFlightsDto dto);
    }
}
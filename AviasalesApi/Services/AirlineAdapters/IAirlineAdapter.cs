using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public interface IAirlineAdapter
    {
        Type ResponseType { get; }
        IMapper ResponseMapper { get; }
        string ConstructRequestUrl(FlightInfo flightInfo);
    }
}
using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public interface IAirlineAdapter
    {
        Airline Airline { get; }
        string BookingUrl { get; }
        Type ResponseType { get; }
        IMapper ResponseMapper { get; }
        string ConstructRequestUrl(FlightInfo flightInfo);
        object GetBookingJson(BookFlightDto dto);
    }
}
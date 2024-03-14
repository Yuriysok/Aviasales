using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class LufthansaAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<LufthansaFlight>);
        private const string Endpoint = "https://www.lufthansa.com/api/get";
        public IMapper ResponseMapper { get; } =
            new MapperConfiguration(config => config.CreateMap<LufthansaFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.Lufthansa)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceUsd))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.TakeoffTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.LandingTime))
            ).CreateMapper();
        public string ConstructRequestUrl(FlightInfo flightInfo) =>
            $"{Endpoint}?departurecity={flightInfo.FromCity}&destinationcity={flightInfo.ToCity}&flightdate={flightInfo.Date:yyyy-MM-dd}";

        public record struct LufthansaFlight(float PriceUsd, DateTime TakeoffTime, DateTime LandingTime);
    }
}

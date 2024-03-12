using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class LufthansaAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<LufthansaFlight>);
        public string Endpoint { get; } = "https://www.lufthansa.com/api/get";
        public IMapper Mapper { get; } =
            new MapperConfiguration(config => config.CreateMap<LufthansaFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.Lufthansa)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceUsd))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.TakeoffTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.LandingTime))
            ).CreateMapper();
        public string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?destinationcity={dto.ToCity}&departurecity={dto.FromCity}&flightdate={dto.Date}";

        private record struct LufthansaFlight(float PriceUsd, DateTime TakeoffTime, DateTime LandingTime);
    }
}

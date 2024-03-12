using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class AeroflotAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<AeroflotFlight>);
        public string Endpoint { get; } = "https://www.aeroflot.ru/api/get";
        public IMapper Mapper { get; } =
            new MapperConfiguration(config => config.CreateMap<AeroflotFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.Aeroflot)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceDollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.DepartureTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.ArrivalTime))
            ).CreateMapper();
        public string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?tocity={dto.ToCity}&fromcity={dto.FromCity}&date={dto.Date}";

        private record struct AeroflotFlight(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
    }
}

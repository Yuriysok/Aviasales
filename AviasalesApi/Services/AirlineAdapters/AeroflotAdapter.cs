using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class AeroflotAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<AeroflotFlight>);
        private const string Endpoint = "https://www.aeroflot.ru/api/get";
        public IMapper ResponseMapper { get; } =
            new MapperConfiguration(config => config.CreateMap<AeroflotFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.Aeroflot)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceDollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.DepartureTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.ArrivalTime))
            ).CreateMapper();
        public string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?fromcity={dto.FromCity}&tocity={dto.ToCity}&date={dto.Date:yyyyMMdd}";

        public record struct AeroflotFlight(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime);
    }
}

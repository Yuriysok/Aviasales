using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class UzAirwaysAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<UzAirwaysFlight>);
        private const string Endpoint = "https://www.uzairways.com/api/get";
        public IMapper ResponseMapper { get; } =
            new MapperConfiguration(config => config.CreateMap<UzAirwaysFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.UzAirways)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.Dollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.StartTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.FinishTime))
            ).CreateMapper();
        public string ConstructRequestUrl(FlightInfo flightInfo) =>
            $"{Endpoint}?departurecity={flightInfo.FromCity}&arrivalcity={flightInfo.ToCity}&flightday={flightInfo.Date:dd-MM-yyyy}";

        public record struct UzAirwaysFlight(float Dollars, DateTime StartTime, DateTime FinishTime);
    }
}

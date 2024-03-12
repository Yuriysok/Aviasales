using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class UzAirwaysAdapter : IAirlineAdapter
    {
        public Type ResponseType { get; } = typeof(List<UzAirwaysFlight>);
        public string Endpoint { get; } = "https://www.uzairways.com/api/get";
        public IMapper Mapper { get; } =
            new MapperConfiguration(config => config.CreateMap<UzAirwaysFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline.UzAirways)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.Dollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.StartTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.FinishTime))
            ).CreateMapper();
        public string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?arrivalcity={dto.ToCity}&departurecity={dto.FromCity}&flightday={dto.Date}";

        private record struct UzAirwaysFlight(float Dollars, DateTime StartTime, DateTime FinishTime);
    }
}

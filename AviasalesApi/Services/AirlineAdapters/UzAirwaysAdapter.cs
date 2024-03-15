using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class UzAirwaysAdapter : IAirlineAdapter
    {
        public Airline Airline { get; } = Airline.UzAirways;
        public Type ResponseType { get; } = typeof(List<UzAirwaysFlight>);
        private const string Endpoint = "https://www.uzairways.com/api/get";
        public string BookingUrl { get; } = "https://www.uzairways.com/api/book";
        public IMapper ResponseMapper { get; }

        public UzAirwaysAdapter()
        {
            ResponseMapper = new MapperConfiguration(config => {
                config.CreateMap<UzAirwaysFlight, Flight>()
                    .BeforeMap((_, dest) => dest.Airline = Airline)
                    .ForMember(dest => dest.FlightId, src => src.MapFrom(x => x.FlightId))
                    .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.Dollars))
                    .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.StartTime))
                    .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.FinishTime))
                    .ForMember(dest => dest.NumberOfFlights, src => src.MapFrom(x => x.AmountOfFlights));

                config.CreateMap<BookFlightDto, UzAirwaysBookingDto>()
                .ForMember(dest => dest.FlightId, src => src.MapFrom(x => x.FlightId))
                .ForMember(dest => dest.DocumentInfo, src => src.MapFrom(x => x.PassportSerialNumber))
                .ForMember(dest => dest.PassengerName, src => src.MapFrom(x => x.Name));
            }).CreateMapper();
        }
        public string ConstructRequestUrl(FlightInfo flightInfo) =>
            $"{Endpoint}?departurecity={flightInfo.FromCity}&arrivalcity={flightInfo.ToCity}&flightday={flightInfo.Date:dd-MM-yyyy}";

        public object GetBookingJson(BookFlightDto dto) =>
            ResponseMapper.Map<UzAirwaysBookingDto>(dto);

        public record struct UzAirwaysFlight(string FlightId, float Dollars, DateTime StartTime, DateTime FinishTime, int AmountOfFlights);

        private record struct UzAirwaysBookingDto(string FlightId, string PassengerName, string DocumentInfo);
    }
}

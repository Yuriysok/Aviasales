using AutoMapper;
using AviasalesApi.Models;
using static AviasalesApi.Services.AirlineAdapters.AeroflotAdapter;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class LufthansaAdapter : IAirlineAdapter
    {
        public Airline Airline { get; } = Airline.Lufthansa;
        public Type ResponseType { get; } = typeof(List<LufthansaFlight>);
        private const string Endpoint = "https://www.lufthansa.com/api/get";
        public IMapper ResponseMapper { get; }

        public string BookingUrl { get; } = "https://www.lufthansa.com/api/book";

        public LufthansaAdapter()
        {
            ResponseMapper = new MapperConfiguration(config => {
                config.CreateMap<LufthansaFlight, Flight>()
                    .BeforeMap((_, dest) => dest.Airline = Airline)
                    .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceUsd))
                    .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.TakeoffTime))
                    .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.LandingTime))
                    .ForMember(dest => dest.NumberOfFlights, src => src.MapFrom(x => x.FlightNum));

                config.CreateMap<BookFlightDto, LufthansaBookingDto>()
                .ForMember(dest => dest.FlightId, src => src.MapFrom(x => x.FlightId))
                .ForMember(dest => dest.PassportSerial, src => src.MapFrom(x => x.PassportSerialNumber))
                .ForMember(dest => dest.PassengerName, src => src.MapFrom(x => x.Name));
            }).CreateMapper();
        }
        public string ConstructRequestUrl(FlightInfo flightInfo) =>
            $"{Endpoint}?departurecity={flightInfo.FromCity}&destinationcity={flightInfo.ToCity}&flightdate={flightInfo.Date:yyyy-MM-dd}";

        public object GetBookingJson(BookFlightDto dto) =>
            ResponseMapper.Map<LufthansaBookingDto>(dto);

        public record struct LufthansaFlight(float PriceUsd, DateTime TakeoffTime, DateTime LandingTime, int FlightNum);

        private record struct LufthansaBookingDto(Guid FlightId, string PassengerName, string PassportSerial);
    }
}

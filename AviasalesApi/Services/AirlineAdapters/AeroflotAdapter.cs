using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.Services.AirlineAdapters
{
    public class AeroflotAdapter : IAirlineAdapter
    {
        public Airline Airline { get; } = Airline.Aeroflot;
        public Type ResponseType { get; } = typeof(List<AeroflotFlight>);
        private const string Endpoint = "https://www.aeroflot.ru/api/get";
        public IMapper ResponseMapper { get; }

        public string BookingUrl { get; } = "https://www.aeroflot.ru/api/book";

        public AeroflotAdapter()
        {
            ResponseMapper = new MapperConfiguration(config =>
            {
                config.CreateMap<AeroflotFlight, Flight>()
                .BeforeMap((_, dest) => dest.Airline = Airline)
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceDollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.DepartureTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.ArrivalTime))
                .ForMember(dest => dest.NumberOfFlights, src => src.MapFrom(x => x.FlightsNumber));

                config.CreateMap<BookFlightDto, AeroflotBookingDto>()
                .ForMember(dest => dest.FlightId, src => src.MapFrom(x => x.FlightId))
                .ForMember(dest => dest.PassportData, src => src.MapFrom(x => x.PassportSerialNumber))
                .ForMember(dest => dest.PersonName, src => src.MapFrom(x => x.Name));
            }).CreateMapper();
        }

        public object GetBookingJson(BookFlightDto dto) =>
            ResponseMapper.Map<AeroflotBookingDto>(dto);

        public string ConstructRequestUrl(FlightInfo flightInfo) =>
            $"{Endpoint}?fromcity={flightInfo.FromCity}&tocity={flightInfo.ToCity}&date={flightInfo.Date:yyyyMMdd}";

        public record struct AeroflotFlight(float PriceDollars, DateTime DepartureTime, DateTime ArrivalTime, int FlightsNumber);

        private record struct AeroflotBookingDto(Guid FlightId, string PersonName, string PassportData);
    }
}

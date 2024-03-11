using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public class AeroflotAdapter : IAirlineAdapter
    {
        public string Endpoint => "https://www.aeroflot.ru/api/get";

        public string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?tocity={dto.ToCity}&fromcity={dto.FromCity}&date={dto.Date}";

        private class FlightResponseDto
        {
            public float PriceDollars { get; set; }
            public DateTime DepartureTime { get; set; }
            public DateTime ArrivalTime { get; set; }
        }

        public MapperConfiguration MapperConfiguration =>
            new MapperConfiguration(config => config.CreateMap<FlightResponseDto, Flight>()
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceDollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.DepartureTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.ArrivalTime)));

        public IMapper Mapper => MapperConfiguration.CreateMapper();

        public async Task<List<Flight>> GetFlightsAsync(GetFlightsDto getFlightsDto, HttpClient http)
        {
            var url = ConstructRequestUrl(getFlightsDto);
            var result = await http.GetAsync(url);
            return await ParseResponseAsync(result);
        }

        public async Task<List<Flight>> ParseResponseAsync(HttpResponseMessage msg)
        {
            var flightResponses = await msg.Content.ReadFromJsonAsync<List<FlightResponseDto>>();
            return Mapper.Map<List<FlightResponseDto>, List<Flight>>(flightResponses!);
        }

    }
}

using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public class AeroflotAdapter : AirlineAdapterBase
    {
        public override string Endpoint { get; } = "https://www.aeroflot.ru/api/get";

        public override MapperConfiguration MapperConfiguration { get; } =
            new(config => config.CreateMap<AeroflotResponse, Flight>()
                .ForMember(dest => dest.PriceUsd, src => src.MapFrom(x => x.PriceDollars))
                .ForMember(dest => dest.Departure, src => src.MapFrom(x => x.DepartureTime))
                .ForMember(dest => dest.Arrival, src => src.MapFrom(x => x.ArrivalTime)));

        public override string ConstructRequestUrl(GetFlightsDto dto) =>
            $"{Endpoint}?tocity={dto.ToCity}&fromcity={dto.FromCity}&date={dto.Date}";


        //Code is the same in all implementations
        public override async Task<List<Flight>> ParseResponseAsync(HttpResponseMessage msg)
        {
            var flightResponses = await msg.Content.ReadFromJsonAsync<List<AeroflotResponse>>();
            return Mapper.Map<List<AeroflotResponse>, List<Flight>>(flightResponses!);
        }
    }
}

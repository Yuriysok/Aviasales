using AutoMapper;
using AviasalesApi.Models;

namespace AviasalesApi.AirlineAdapters
{
    public abstract class AirlineAdapterBase : IAirlineAdapter
    {
        public abstract string Endpoint { get; }
        public abstract string ConstructRequestUrl(GetFlightsDto dto);
        public abstract Task<List<Flight>> ParseResponseAsync(HttpResponseMessage msg);
        public abstract MapperConfiguration MapperConfiguration { get; }
        public IMapper Mapper => MapperConfiguration.CreateMapper();

        public async Task<List<Flight>> GetFlightsAsync(GetFlightsDto getFlightsDto, HttpClient http)
        {
            var url = ConstructRequestUrl(getFlightsDto);
            var result = await http.GetAsync(url);
            return await ParseResponseAsync(result);
        }
    }
}
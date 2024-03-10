namespace AviasalesApi.AirlineAdapters
{
    public class AeroflotAdapter : IAirlineAdapter
    {
        public string Endpoint => "https://www.aeroflot.ru/api/get";
    }
}

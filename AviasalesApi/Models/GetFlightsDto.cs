using AviasalesApi.Attributes;

namespace AviasalesApi.Models
{
    public record struct GetFlightsDto
    {
        [ComplexProperty]
        public FlightInfo FlightInfo { get; set; }

        [ComplexProperty]
        public FilterOptions FilterOptions { get; set; }

        public SortOptions SortOptions { get; set; }

        public static ValueTask<GetFlightsDto?> BindAsync(HttpContext context)
        {
            var airlinesFromQuery = GetQueryParameter(nameof(FilterOptions.Airlines));
            var airlines = new List<Airline>();
            if (airlinesFromQuery != null)
            {
                foreach (var airline in airlinesFromQuery.Split(','))
                {
                    if (Enum.TryParse<Airline>(airline, out var value))
                        airlines.Add(value);
                }
            }

            var filterOptions = new FilterOptions()
            {
                ArrivalTimeFrom = GetTimeOnlyParameter(nameof(FilterOptions.ArrivalTimeFrom)),
                ArrivalTimeTo = GetTimeOnlyParameter(nameof(FilterOptions.ArrivalTimeTo)),
                DepartureTimeFrom = GetTimeOnlyParameter(nameof(FilterOptions.DepartureTimeFrom)),
                DepartureTimeTo = GetTimeOnlyParameter(nameof(FilterOptions.DepartureTimeTo)),
                PriceFrom = ParseFromQueryOrDefault(nameof(FilterOptions.PriceFrom), float.Parse),
                PriceTo = ParseFromQueryOrDefault(nameof(FilterOptions.PriceTo), float.Parse),
                Airlines = airlines
            };

            var flightInfo = new FlightInfo
            {
                FromCity = GetQueryParameter(nameof(FlightInfo.FromCity)),
                ToCity = GetQueryParameter(nameof(FlightInfo.ToCity)),
                Date = ParseFromQueryOrDefault(nameof(FlightInfo.Date), DateTime.Parse),
            };

            var result = new GetFlightsDto
            {
                FlightInfo = flightInfo,
                FilterOptions = filterOptions,
                SortOptions = ParseFromQueryOrDefault(nameof(SortOptions), Enum.Parse<SortOptions>)
            };

            return ValueTask.FromResult<GetFlightsDto?>(result);

            string GetQueryParameter(string name) =>
                context.Request.Query[name]!;

            T ParseFromQueryOrDefault<T>(string name, Func<string, T> parseMethod)
            {
                var parameter = GetQueryParameter(name);
                return parameter != null
                    ? parseMethod.Invoke(parameter)
                    : default!;
            }

            TimeOnly GetTimeOnlyParameter(string name) =>
                ParseFromQueryOrDefault(name, TimeOnly.Parse);
        }
    }
}

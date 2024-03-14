using AviasalesApi.Attributes;
using AviasalesApi.Services;

namespace AviasalesApi.Models
{
    public record GetFlightsDto
    {
        public required string FromCity { get; set; }
        public required string ToCity { get; set; }
        public required DateTime Date { get; set; }
        public SortOptions SortOptions { get; set; }
        [ComplexProperty]
        public FilterOptions FilterOptions { get; set; }

        public static ValueTask<GetFlightsDto?> BindAsync(HttpContext context)
        {
            var sortOptionsFromQuery = GetQueryParameter(nameof(SortOptions));

            var sortOptions = sortOptionsFromQuery == null
                ? SortOptions.None
                : Enum.Parse<SortOptions>(sortOptionsFromQuery);

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

            var result = new GetFlightsDto
            {
                FromCity = GetQueryParameter(nameof(FromCity)),
                ToCity = GetQueryParameter(nameof(ToCity)),
                Date = ParseFromQueryOrDefault(nameof(Date), DateTime.Parse),
                SortOptions = sortOptions,
                FilterOptions = filterOptions
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

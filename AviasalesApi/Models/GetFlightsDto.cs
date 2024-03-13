using AviasalesApi.Attributes;
using AviasalesApi.Services;
using System.ComponentModel.DataAnnotations.Schema;

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
                var airlinesStr = airlinesFromQuery.Split(',');

                var parsedAirlines = airlinesStr.Select(x => 
                    Enum.TryParse<Airline>(x, out var value) 
                    ? value
                    : Airline.Undefined
                ).Where(x => x != Airline.Undefined);

                airlines.AddRange(parsedAirlines);
            }

            var filterOptions = new FilterOptions()
            {
                ArrivalTimeFrom = GetDateTimeParameter(nameof(FilterOptions.ArrivalTimeFrom)),
                ArrivalTimeTo = GetDateTimeParameter(nameof(FilterOptions.ArrivalTimeTo)),
                DepartureTimeFrom = GetDateTimeParameter(nameof(FilterOptions.DepartureTimeFrom)),
                DepartureTimeTo = GetDateTimeParameter(nameof(FilterOptions.DepartureTimeTo)),
                PriceFrom = float.Parse(GetQueryParameter(nameof(FilterOptions.PriceFrom))),
                PriceTo = float.Parse(GetQueryParameter(nameof(FilterOptions.PriceTo))),
                Airlines = airlines
            };

            var result = new GetFlightsDto
            {
                FromCity = GetQueryParameter(nameof(FromCity)),
                ToCity = GetQueryParameter(nameof(ToCity)),
                Date = GetDateTimeParameter(nameof(Date)),
                SortOptions = sortOptions,
                FilterOptions = filterOptions

            };

            return ValueTask.FromResult<GetFlightsDto?>(result);

            string GetQueryParameter(string name) =>
                context.Request.Query[name]!;

            DateTime GetDateTimeParameter(string name)
            {
                var parameter = GetQueryParameter(name);
                return parameter != null
                    ? DateTime.Parse(parameter)
                    : default;
            }
        }
    }
}

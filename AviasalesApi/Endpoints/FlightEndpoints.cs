using AviasalesApi.Extensions;
using AviasalesApi.Models;
using AviasalesApi.Services;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AviasalesApi.Endpoints
{
    public class FlightEndpoints(IAirlineService _airlineService) : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("api/flights");
            userGroup.MapGet("", GetFlights)
                .WithOpenApiCustomParameters(typeof(GetFlightsDto));
            userGroup.MapPost("", BookFlight);
        }

        private async Task<Ok<IEnumerable<Flight>>> GetFlights(DataContext context, GetFlightsDto getFlightsDto)
        {
            var flights = await _airlineService.GetAllFlightsAsync(getFlightsDto.FlightInfo, getFlightsDto.FilterOptions, getFlightsDto.SortOptions);
            return TypedResults.Ok(flights);
        }

        private async Task<StatusCodeHttpResult> BookFlight(DataContext context, [AsParameters]BookFlightDto bookFlightDto)
        {
            var result = await _airlineService.BookFlightAsync(bookFlightDto);
            return TypedResults.StatusCode((int)result);
        }
    }
}

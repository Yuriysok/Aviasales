using AviasalesApi.Extensions;
using AviasalesApi.Models;
using AviasalesApi.Services;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AviasalesApi.Endpoints
{
    public class FlightEndpoints(IAirlineService _airlineService) : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("api/flights");
            userGroup.MapGet("", GetFlights)
                .AllowAnonymous()
                .WithOpenApiCustomParameters(typeof(GetFlightsDto));
            userGroup.MapPost("", BookFlight)
                //.RequireAuthorization()
                .AllowAnonymous();
        }

        private async Task<Ok<IEnumerable<Flight>>> GetFlights(DataContext context, GetFlightsDto getFlightsDto)
        {
            var flights = await _airlineService.GetAllFlightsAsync(getFlightsDto.FlightInfo, getFlightsDto.FilterOptions, getFlightsDto.SortOptions);
            return TypedResults.Ok(flights);
        }

        private async Task<StatusCodeHttpResult> BookFlight(DataContext context, [FromBody]BookFlightDto bookFlightDto)
        {
            var result = await _airlineService.BookFlightAsync(bookFlightDto);
            return TypedResults.StatusCode((int)result);
        }
    }
}

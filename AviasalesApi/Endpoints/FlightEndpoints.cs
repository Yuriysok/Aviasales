using AviasalesApi.Endpoints.Helpers;
using AviasalesApi.Models;
using AviasalesApi.Services;
using Carter;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace AviasalesApi.Endpoints
{
    public class FlightEndpoints(IAirlineService _airlineService) : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var userGroup = app.MapGroup("api/flights");
            userGroup.MapGet("", GetFlights)
                .WithOpenApiCustomParameters(typeof(GetFlightsDto));
        }

        private async Task<Ok<IEnumerable<Flight>>> GetFlights(DataContext context, GetFlightsDto getFlightsDto)
        {
            var flights = await _airlineService.GetAllFlightsAsync(getFlightsDto, getFlightsDto.FilterOptions, getFlightsDto.SortOptions);
            return TypedResults.Ok(flights);
        }
    }
}

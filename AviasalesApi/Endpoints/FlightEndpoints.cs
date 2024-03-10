﻿using AviasalesApi.Models;
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
            userGroup.MapGet("", GetFlights);
        }

        private async Task<Ok<List<Flight>>> GetFlights(DataContext context, [AsParameters]GetFlightsDto getFlightsDto)
        {

            var flights = await _airlineService.GetFlightsAsync(getFlightsDto);
            return TypedResults.Ok(flights);
        }
    }
}

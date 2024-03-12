using AviasalesApi.Models;
using AviasalesApi.Services.AirlineAdapters;
using FluentAssertions;
using static AviasalesApi.Services.AirlineAdapters.AeroflotAdapter;
using static AviasalesApi.Services.AirlineAdapters.LufthansaAdapter;
using static AviasalesApi.Services.AirlineAdapters.UzAirwaysAdapter;

namespace Aviasales.Test
{
    [TestClass]
    public class AdapterTest
    {
        [DataTestMethod]
        [DataRow(typeof(AeroflotAdapter), "https://www.aeroflot.ru/api/get?fromcity=Tashkent&tocity=Moscow&date=20240101")]
        [DataRow(typeof(LufthansaAdapter), "https://www.lufthansa.com/api/get?departurecity=Tashkent&destinationcity=Moscow&flightdate=2024-01-01")]
        [DataRow(typeof(UzAirwaysAdapter), "https://www.uzairways.com/api/get?departurecity=Tashkent&arrivalcity=Moscow&flightday=01-01-2024")]
        public void ConstructRequestUrl_Test(Type adapterType, string expectedResult)
        {
            // Arrange
            GetFlightsDto dto = new()
            {
                Date = DateTime.Parse("2024/01/01"),
                FromCity = "Tashkent",
                ToCity = "Moscow"
            };

            IAirlineAdapter adapter = (IAirlineAdapter)Activator.CreateInstance(adapterType)!;

            // Act
            var result = adapter.ConstructRequestUrl(dto);

            //Assert
            result.Should().Be(expectedResult);
        }

        [DataTestMethod]
        [DataRow(typeof(AeroflotAdapter), typeof(AeroflotFlight), Airline.Aeroflot)]
        [DataRow(typeof(LufthansaAdapter), typeof(LufthansaFlight), Airline.Lufthansa)]
        [DataRow(typeof(UzAirwaysAdapter), typeof(UzAirwaysFlight), Airline.UzAirways)]
        public void MapperTest(Type adapterType, Type flightType, Airline resultAirline)
        {
            // Arrange
            var departureDate = DateTime.Parse("2024/01/01 10:00:00");
            var arrivalDate = DateTime.Parse("2024/01/01 13:00:00");
            var priceUsd = 10;

            var airlineFlight = Activator.CreateInstance(flightType, priceUsd, departureDate, arrivalDate);

            var expectedResult = new Flight
            {
                Airline = resultAirline,
                Departure = departureDate,
                Arrival = arrivalDate,
                PriceUsd = priceUsd
            };

            IAirlineAdapter adapter = (IAirlineAdapter)Activator.CreateInstance(adapterType)!;

            // Act
            var result = (Flight)adapter.ResponseMapper.Map(airlineFlight, flightType, typeof(Flight));

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
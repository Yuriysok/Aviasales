using System.Net;

namespace AviasalesApi.Services
{
    public class MockHttpHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = "Services/MockAirlineResponses/";
            var apiPath = request.RequestUri!.AbsolutePath;
            var host = request.RequestUri!.Host;
            int delay;
            HttpResponseMessage httpMessage;
            switch (apiPath)
            {
                case "/api/get":
                    (delay, var fileName) = GetDelayAndFileName(host);
                    httpMessage = GetFlightResponse(path + fileName);
                    break;
                case "/api/book":
                    (delay, var httpStatus) = GetDelayAndBookingResponseStatus(host);
                    httpMessage = new HttpResponseMessage(httpStatus);
                    break;
                default:
                    throw new ArgumentException(nameof(request.RequestUri.AbsoluteUri));
            }
            await Task.Delay(delay, cancellationToken);
            return httpMessage;
        }
        private static HttpResponseMessage GetFlightResponse(string path)
        {
            var httpMessage = new HttpResponseMessage();
            var flightJson = File.ReadAllText(path);
            httpMessage.Content = new StringContent(flightJson);
            return httpMessage;
        }

        private static (int, string) GetDelayAndFileName(string uri) =>
            uri switch
            {
                "www.aeroflot.ru" => (200, "Aeroflot.json"),
                "www.lufthansa.com" => (250, "Lufthansa.json"),
                "www.uzairways.com" => (300, "UzAirways.json"),
                _ => throw new NotSupportedException()
            };

        private static (int, HttpStatusCode) GetDelayAndBookingResponseStatus(string uri) =>
            uri switch
            {
                "www.aeroflot.ru" => (200, HttpStatusCode.Accepted),
                "www.lufthansa.com" => (250, HttpStatusCode.OK),
                "www.uzairways.com" => (300, HttpStatusCode.Gone),
                _ => throw new NotSupportedException()
            };
    }
}

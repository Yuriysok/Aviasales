namespace AviasalesApi.Services
{
    public class MockHttpHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = "Services/MockAirlineResponses/";
            var (delay, fileName) = GetDelayAndFileName(request.RequestUri!.Host);
            var httpMessage = GetHttpResponseMessage(path + fileName);
            await Task.Delay(delay, cancellationToken);
            return httpMessage;
        }
        private HttpResponseMessage GetHttpResponseMessage(string path)
        {
            var httpMessage = new HttpResponseMessage();
            var aeroflotJson = File.ReadAllText(path);
            httpMessage.Content = new StringContent(aeroflotJson);
            return httpMessage;
        }

        private static (int, string) GetDelayAndFileName(string host) =>
            host switch
            {
                "www.aeroflot.ru" => (200, "Aeroflot.json"),
                "www.lufthansa.com" => (250, "Lufthansa.json"),
                "www.uzairways.com" => (300, "UzAirways.json"),
                _ => throw new NotSupportedException()
            };

    }
}

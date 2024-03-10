
using Moq;
using System.IO;
using System.Security.Policy;

namespace AviasalesApi.Services
{
    public class MockHttpHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var path = "Services/MockAirlineResponses/";
            var httpMessage = GetHttpResponseMessage(path + "Aeroflot.json");
            await Task.Delay(200, cancellationToken);
            return httpMessage;
        }

        private HttpResponseMessage GetHttpResponseMessage(string path)
        {
            var httpMessage = new HttpResponseMessage();
            var aeroflotJson = File.ReadAllText(path);
            httpMessage.Content = new StringContent(aeroflotJson);
            return httpMessage;
        }
    }
}

using Moq;
using Moq.Protected;
using Org.BouncyCastle.Crypto.Prng;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApiTesting.Model
{
    public class MockHttpHandler<T>
    {
        public static Mock<HttpMessageHandler> SetupGetRequest(List<T> response)
        {
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(response))
            };
            mockResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var mockhandler = new Mock<HttpMessageHandler>();
            mockhandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);
            return mockhandler;

        }
        public static Mock<HttpMessageHandler> SetUpGetNotFound(List<T> response)
        {
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
            {
                Content = new StringContent("Not found please try again later")
            };
            mockResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var mockhandler = new Mock<HttpMessageHandler>();
            mockhandler.Protected().Setup<Task<HttpResponseMessage>>(
           "SendAsync",
           It.IsAny<HttpRequestMessage>(),
           It.IsAny<CancellationToken>())
           .ReturnsAsync(mockResponse);
            return mockhandler;
        }
    }
}

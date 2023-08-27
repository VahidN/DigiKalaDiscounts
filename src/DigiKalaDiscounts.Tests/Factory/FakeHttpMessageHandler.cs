namespace DigiKalaDiscounts.Tests.Factory;

public class FakeHttpMessageHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                           CancellationToken cancellationToken)
    {
        var content = File.ReadAllText("Resources/api.digikala.com.1402.05.31.04.json");
        var fakeResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content) };
        return Task.FromResult(fakeResponse);
    }
}

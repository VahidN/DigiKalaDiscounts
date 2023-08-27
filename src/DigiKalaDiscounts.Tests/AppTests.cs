using DigiKalaDiscounts.Services.Contracts;
using DigiKalaDiscounts.Tests.Factory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigiKalaDiscounts.Tests;

[TestClass]
public class AppTests
{
    [TestMethod]
    public async Task TestAppRunnerServiceWorks()
    {
        var appRunnerService = TestsAppFactory.GetRequiredService<IAppRunnerService>();
        var results = await appRunnerService.StartAsync(new[]
                                                        {
                                                            "-t tt",
                                                            "-i ii",
                                                        });
        Assert.AreEqual(4000, results.TotalItemsCount);
    }
}

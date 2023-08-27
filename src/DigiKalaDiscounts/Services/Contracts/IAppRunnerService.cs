using DigiKalaDiscounts.Models;

namespace DigiKalaDiscounts.Services.Contracts;

public interface IAppRunnerService
{
    Task<AppRunnerResults> StartAsync(string[] args);
}

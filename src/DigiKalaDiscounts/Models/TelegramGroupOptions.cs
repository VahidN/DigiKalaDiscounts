using CommandLine;

namespace DigiKalaDiscounts.Models;

public class TelegramGroupOptions
{
    [Option('t', "AccessToken", Required = true, HelpText = "Telegram's AccessToken")]
    public required string AccessToken { get; set; }

    [Option('i', "ChatId", Required = true, HelpText = "Telegram group's ID")]
    public required string ChatId { get; set; }
}

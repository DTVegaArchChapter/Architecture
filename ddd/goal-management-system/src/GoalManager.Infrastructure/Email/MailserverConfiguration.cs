namespace GoalManager.Infrastructure.Email;

public sealed class MailserverConfiguration
{
  public string Hostname { get; set; } = "localhost";
  public int Port { get; set; } = 25;
}

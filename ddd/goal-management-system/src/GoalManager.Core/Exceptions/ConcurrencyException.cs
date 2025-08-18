namespace GoalManager.Core.Exceptions;

public class ConcurrencyException : Exception
{
  public ConcurrencyException() { }
  public ConcurrencyException(string message) : base(message) { }
  public ConcurrencyException(string message, Exception innerException) : base(message, innerException) { }
}

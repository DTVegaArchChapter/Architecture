namespace GoalManager.Core.Organisation;

public readonly record struct TeamName
{
  public string Value { get; }
  
  public TeamName(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
    }

    Value = value;
  }

  public bool Equals(TeamName name)
  {
    return string.Equals(Value, name.Value, StringComparison.CurrentCultureIgnoreCase);
  }

  public override int GetHashCode()
  {
    return Value.GetHashCode();
  }

  public static Result<TeamName> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return Result<TeamName>.Error("Team Name is required");
    }

    return new TeamName(value);
  }
}

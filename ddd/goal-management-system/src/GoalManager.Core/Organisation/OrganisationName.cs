namespace GoalManager.Core.Organisation;

public readonly record struct OrganisationName
{
  public string Value { get; }
  
  public OrganisationName(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
    }

    Value = value;
  }

  public override string ToString()
  {
    return Value;
  }

  public static Result<OrganisationName> Create(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
    {
      return Result<OrganisationName>.Error("Organisation Name is required");
    }

    return new OrganisationName(value);
  }
}

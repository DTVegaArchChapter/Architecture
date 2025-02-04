namespace TransactionalOutboxPatternApp.Infrastructure.Entity;

public class Order
{
    public int Id { get; set; }

    public DateTime CreateDate { get; set; }
}
public class TransactionEntry
{
    public Stock stock;
    public int quantity;
    public float pricePerUnit;
    public bool isBought;

    public TransactionEntry(Stock stock, int quantity, float pricePerUnit, bool isBought)
    {
        this.stock = stock;
        this.quantity = quantity;
        this.pricePerUnit = pricePerUnit;
        this.isBought = isBought;
    }
}


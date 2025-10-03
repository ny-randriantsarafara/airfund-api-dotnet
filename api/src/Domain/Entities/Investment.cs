public class Investment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal CommittedCapital { get; set; }
    public decimal DistributedCapital { get; set; }

    public decimal CurrentNetAssetValue { get; set; }

    public decimal CalculateTVPI()
    {
        if (CommittedCapital == 0)
        {
            throw new InvalidOperationException("Committed Capital cannot be zero when calculating TVPI.");
        }
        return (CurrentNetAssetValue + DistributedCapital) / CommittedCapital;
    }
}
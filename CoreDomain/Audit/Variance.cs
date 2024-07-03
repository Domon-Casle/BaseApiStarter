namespace CoreDomain.Audit
{
    public record class Variance(string property, string oldValue, string newValue)
    {
    }
}

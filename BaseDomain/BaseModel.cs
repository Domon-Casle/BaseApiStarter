namespace BaseDomain
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}

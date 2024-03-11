namespace Producer.Entities
{
    public class ProductRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
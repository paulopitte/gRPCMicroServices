namespace GrpcCatalog.Domain
{
    public class Product
    {

        public int Id { get; set; } = 0;
        public string? Sku { get; set; }
        public string? Title { get; set; }
        public float Price { get; set; } = 0;
        public StatusProduct Status { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow.AddHours(-3);
    }

    public enum StatusProduct : short
    {
        Actived = 1,
        Inatived = 2,
    }
}

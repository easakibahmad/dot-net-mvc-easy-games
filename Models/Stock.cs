namespace EasyGames.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Book, Game, Toy
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

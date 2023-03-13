namespace DotNetLearn.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Category> Categories { get; set; }
        
    }
}

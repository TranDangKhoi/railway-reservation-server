using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetLearn.Models
{
    public class Product_Category
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; } = new();
    }
}

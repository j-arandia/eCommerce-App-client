using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CaseStudyAPI.DAL.DomainClasses
{
    public class Products
    {

        
        [Key]
        [StringLength(15)]
        public string? Id { get; set; }
        [ForeignKey("BrandId")]
        public Brands? Brands { get; set; } // generates FK
        [Required]
        public int BrandId { get; set; }
        [Required]
        [StringLength(50)]
        public string? ProductName { get; set; }
        [Required]
        [StringLength(20)]
        public string? GraphicName { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public Decimal CostPrice { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public Decimal MSRP { get; set; }
        [Required]
        public int QtyOnHand { get; set; }
        [Required]
        public int QtyOnBackOrder { get; set; }
        [Required]
        [StringLength(2000)]
        public string? Description { get; set; }
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[]? Timer { get; set; }
    }
}

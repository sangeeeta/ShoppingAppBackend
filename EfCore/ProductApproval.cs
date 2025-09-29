using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.EfCore
{
    public class ProductApproval
    {
        [Key, Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovalId { get; set; }
        public int ProductId { get; set; }
        public int AdminId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ApproedAt { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get;set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal {  get; set; }
        public string? OrderStatus {  get; set; }
        public string? PaymentStatus {  get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get;set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress {  get; set; }
        public string? City { get; set; }
        public string? Name { get; set; }
    }
}

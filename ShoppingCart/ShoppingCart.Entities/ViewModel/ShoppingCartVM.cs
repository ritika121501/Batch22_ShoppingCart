using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingKart> ShoppingKartsList {  get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}

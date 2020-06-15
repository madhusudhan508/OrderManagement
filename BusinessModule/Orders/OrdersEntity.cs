/*============================================================================
   Namespace        : BusinessModule
   Class            : Orders
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Get properties for Buyer and Product
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessModule
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class OrdersEntity
    {
        public OrdersEntity()
        {
            BuyerInfoDetail = new BuyerEntity();
            ProductInfoDetail = new List<ProductEntity>();
        }
        [Key]
        public int OrderId { get; set; }
        [Key]
        public int BuyerId { get; set; }
        public string OrderStatus { get; set; }
        public BuyerEntity BuyerInfoDetail { get; set; }
        public List<ProductEntity> ProductInfoDetail { get; set; }
 
    }
}

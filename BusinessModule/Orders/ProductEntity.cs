/*============================================================================
   Namespace        : BusinessModule
   Class            : ProductEntity
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Product Properties
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessModule
{
    using System.ComponentModel.DataAnnotations;
    public class ProductEntity
    {
        [Key]
        public string Name { get; set; }
        public int SKUID { get; set; }
        public decimal Weight { get; set; }
        public int Quantity { get; set; }
        public decimal Height { get; set; }
        public byte Barcode { get; set; }
        public string Image { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
}

/*============================================================================
   Namespace        : BusinessModule
   Class            : BuyerEntity
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Buyer Properties
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
    public class BuyerEntity
    {
        [Key]
        public int BuyerId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}

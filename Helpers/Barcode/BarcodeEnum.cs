/*============================================================================
   Namespace        : Helpers
   Class            : BarcodeEnum
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Barcode Enums
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/


namespace Helpers
{
    public class BarcodeEnum
    {
        public enum TYPES : int { DYNAMIC, A, B, C };
        public enum BatchType { OPEN, CLOSE };
        public enum TYPE { CODE128, UNSPECIFIED }
    }
}

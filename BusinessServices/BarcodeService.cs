/*============================================================================
   Namespace        : BusinessServices
   Class            : BusinessServices
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Generate Barcode
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/

namespace BusinessServices
{
    using System.Drawing;
    using Helpers;
    public class BarcodeService
    {

        /// <summary>
        /// Generating Barcode Label
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        byte[] GenerateBarcodeLabel(string pathType, string ID)
        {
            BarcodeWriter b = new BarcodeWriter();
            BarcodeEnum.TYPE type = BarcodeEnum.TYPE.CODE128;
            b.BarCodeData = ID;
            b.ImgWidth = 120;
            b.ImgHeight = 40;
            System.Drawing.Image img = b.Encode(type);
            img = b.Generate_Labels(img);

            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
            //return img;

        }
    }
}

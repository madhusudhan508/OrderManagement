/*============================================================================
   Namespace        : Helpers
   Class            : BarcodeWriter
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Barcode Write logic
   Revision History : 
   ----------------------------------------------------------------------------
 *  Author:            Date:          Description:
 * 
 * 
   ----------------------------------------------------------------------------
================================================================================*/


namespace Helpers
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class BarcodeWriter
    {
        private string _barCodeData = "";
        private string _encodedValue = "";
        private BarcodeEnum.TYPE _encodedType = BarcodeEnum.TYPE.UNSPECIFIED;
        private Image _encodedImage = null;
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.White;

        public int ImgWidth;
        public int ImgHeight;

        public string BarCodeData
        {
            get { return _barCodeData; }
            set { _barCodeData = value; }
        }

        public Image Encode(BarcodeEnum.TYPE iType)
        {
            _encodedType = iType;
            return Encode();
        }

        /// <summary>
        /// Encodes the raw data into binary form representing bars and spaces.
        /// </summary>
        public Image Encode()
        {
            //make sure there is something to encode
            if (_barCodeData.Trim() == "")
                throw new Exception("EENCODE-1: Input data not allowed to be blank.");

            if (this._encodedType == BarcodeEnum.TYPE.UNSPECIFIED)
                throw new Exception("EENCODE-2: Symbology type not allowed to be unspecified.");

            this._encodedValue = "";

            Code128 code128 = new Code128(_barCodeData, BarcodeEnum.TYPES.DYNAMIC);
            this._encodedValue = code128.Encoded_Value;

            return (Image)Generate_Image();
        }

        private Bitmap Generate_Image()
        {
            return Generate_Image(this._foreColor, this._backColor);
        }

        private Bitmap Generate_Image(Color DrawColor, Color BackColor)
        {

            if (_encodedValue == "") throw new Exception("EGENERATE_IMAGE-1: Must be encoded first.");
            Bitmap b = null;

            _encodedValue = "0000000000" + _encodedValue + "0000000000";

            b = new Bitmap(_encodedValue.Length * 2, ImgHeight);

            //draw image
            Color c = DrawColor;

            int pos = 0;
            using (Graphics g = Graphics.FromImage(b))
            {
                int intEC = 0;
                while (intEC < _encodedValue.Length)
                {
                    if (pos < _encodedValue.Length)
                    {
                        if (_encodedValue[intEC] == '1')
                            c = DrawColor;
                        if (_encodedValue[intEC] == '0')
                            c = BackColor;
                    }
                    //lines are 2px wide so draw the appropriate color line vertically
                    g.DrawLine(new Pen(c, (float)2), new Point(pos * 2 + 1, 0), new Point(pos * 2 + 1, b.Height));
                    pos++;
                    intEC++;
                }
            }
            _encodedImage = (Image)b;
            return b;
        }


        #region Label Generation
        public Image Generate_Labels(Image img)
        {
            return Label_Generic(img);
        }

        private Image Label_Generic(Image img)
        {
            System.Drawing.Font font = new Font("Arial", 9, FontStyle.Bold);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(img, (float)0, (float)0);

                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                //color a white box at the bottom of the barcode to hold the string of data
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, img.Height - 16, img.Width, 16));

                //draw datastring under the barcode image
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;

                string barcodeDataForCode128 = this._barCodeData;

                //g.DrawString(barcodeDataForCode128, font, new SolidBrush(this._foreColor), (float)(img.Width / 2), img.Height - 16, f);

                g.Save();
            }//using
            return img;
        }
        #endregion


        #region Misc
        /// <summary>
        /// This function takes a string of data and breaks it into parts and trys to do Int64.TryParse
        /// This will verify that only numeric data is contained in the string passed in.  The complexity below
        /// was done to ensure that the minimum number of interations and checks could be performed.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static bool CheckNumericOnly(string Data)
        {

            //9223372036854775808 is the largest number a 64bit number(signed) can hold so ... make sure its less than that by one place
            int STRING_LENGTHS = 18;

            string temp = Data;
            string[] strings = new string[(Data.Length / STRING_LENGTHS) + ((Data.Length % STRING_LENGTHS == 0) ? 0 : 1)];

            int i = 0;
            while (i < strings.Length)
                if (temp.Length >= STRING_LENGTHS)
                {
                    strings[i++] = temp.Substring(0, STRING_LENGTHS);
                    temp = temp.Substring(STRING_LENGTHS);
                }
                else
                    strings[i++] = temp.Substring(0);

            foreach (string s in strings)
            {
                long value = 0;
                if (!Int64.TryParse(s, out value))
                    return false;
            }

            return true;
        }
        #endregion


    }
}

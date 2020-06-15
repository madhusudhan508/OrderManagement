/*============================================================================
   Namespace        : Helpers
   Class            : TypeConversion
   Author           : Madhusudhan Chakali                           
   Date             : Sunday, Jun 14th 2020
   Description      : Type conversion 
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

    public static class TypeConversion
    {
        #region TypeConversions
        public static bool ToBoolean(this Object obj)
        {
            if (obj == DBNull.Value)
                return false;
            else
                return (bool)obj;
        }

        public static int ToInt(this object str)
        {
            int retValue;
            if (str == null || str == DBNull.Value)
                retValue = 0;   
            else
            {
                int.TryParse(str.ToStr(), out retValue);
            }
            return retValue;
        }

        public static int ToInt(this string str)
        {
            int retValue;
            int.TryParse(str, out retValue);
            return retValue;
        }

        public static int ToInt32(this Object str)
        {
            if (str == DBNull.Value || str == null)
            {
                return 0;
            }
            else
            {
                int retValue;
                int.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }
        public static int? ToNullInt(this object str)
        {
            int retValue;
            if (str != null || str != DBNull.Value)
            {
                int.TryParse(str.ToStr(), out retValue);
                return retValue;
            }
            return null;
        }
        public static string ToStrToNull(this string val)
        {
            if (val == "" || val == null || val == "null")
                return null;
            else
                return val.ToString();
        }

        public static string ToStrToNull(this object val)
        {
            if (val == null)
                return null;
            else
                return val.ToString();
        }
        
        public static DateTime? ToDateTime(this object str)
        {
            if (str == DBNull.Value || str == null)
            {
                return null;
            }
            else
            {
                DateTime retValue;
                DateTime.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }       

        public static object ToObject(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return DBNull.Value;
            else
                return val;
        }

        public static object ToObject(this int? val)
        {
            if (val == null)
                return DBNull.Value;
            else
                return val;
        }
        public static object ToObject(this DateTime? val)
        {
            if (val == null)
                return DBNull.Value;
            else
                return val;
        }

        public static object ToObject(this int val)
        {
            if (val == 0)
                return DBNull.Value;
            else
                return val;
        }

        public static string ToStr(this object val)
        {
            if (val == DBNull.Value || val == null)
                return "";
            else
                return val.ToString().Trim();
        }
        public static string ToStr(this object val, bool returnEmptyIfDBNull)
        {
            if (val == DBNull.Value)
                return returnEmptyIfDBNull ? string.Empty : null;
            if (val == null)
                return string.Empty;
            else
                return val.ToString();
        }
        public static string ToBoolStr(this object val)
        {
            if (val == DBNull.Value || val == null)
                return "N";
            else
                return val.ToString().Trim().ToLower() == "true" ? "Y" : "N";
        }


        public static bool ToBoolFromBit(this object val)
        {
            if (val.ToInt() == 1)
                return true;
            else
                return false;
        }



        public static string ToStrWhiteSpace(this object val)
        {
            if (val == null)
                return "";
            else
                return val.ToString().Trim();
        }

        public static object ToStrDBNUll(this object val)
        {
            if (val.ToStr() == "" || val == null)
                return DBNull.Value;
            else if (val.ToStr() == "False")
                return false;
            else if (val.ToStr() == "True")
                return true;
            else
                return val.ToString();
        }

        public static string ToStrInitCap(this object val)
        {
            if (val == DBNull.Value)
                return "";
            else
                return new System.Globalization.CultureInfo("en").TextInfo.ToTitleCase(val.ToString().ToLower());
        }

        public static bool ToBool(this object val)
        {
            if (val == DBNull.Value || val==null)
                return false;
            else if (val.ToString().ToUpper() == "TRUE"|| val.ToString().ToUpper() == "1")
                return true;
            else
                return false;
        }
        public static string ToDateStr(this string date)
        {
            if (date != null && date.ToStr() != "null" && date.ToStr() != string.Empty)
            {
                return Convert.ToDateTime(date).ToString("MM/dd/yyyy");
            }
            else
            {
                return null;
            }
        }

        public static string ToDateStrYYYYMMDD(this string date)
        {
            if (date != null && date.ToStr() != "null" && date.ToStr() != string.Empty)
            {
                return Convert.ToDateTime(date).ToString("yyyy/MM/dd");
            }
            else
            {
                return null;
            }
        }
        public static string DateToStr(this DateTime? date)
        {
            if (date != null)
            {
                return date?.ToString("MM/dd/yyyy");
            }
            else
            {
                return "";
            }
        }


        public static string FromBase(Int64 number, int target_base)
        {
            if (target_base < 2 || target_base > 36) return "";
            int n = target_base;
            Int64 q = number;
            Int64 r;
            string rtn = "";
            while (q >= n)
            {
                r = q % n;
                q = q / n;
                if (r < 10)
                    rtn = r.ToString() + rtn;
                else
                    rtn = Convert.ToChar(r + 55).ToString() + rtn;
            }
            if (q < 10)
                rtn = q.ToString() + rtn;
            else
                rtn = Convert.ToChar(q + 55).ToString() + rtn;
            return rtn;
        }

        public static double ToDouble(this object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return 0.0;
            }
            else
            {
                double retValue;
                double.TryParse(obj.ToString(), out retValue);
                return retValue;
            }
        }

        public static decimal ToDecimal(this object str)
        {
            if (str == DBNull.Value || str == null)
            {
                return 0;
            }
            else
            {
                decimal retValue;
                decimal.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }
        public static decimal? ToDecimalNull(this object str)
        {
            if (str == DBNull.Value || str == null)
            {
                return null;
            }
            else
            {
                decimal retValue;
                decimal.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }

        //public static T ToJsonDeserialize<T>(this string value)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer jsSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    jsSerializer.MaxJsonLength = Int32.MaxValue;
        //    return jsSerializer.Deserialize<T>(value);
        //}

        public static DateTime ToDateTimeWithOutNull(this object str)
        {
            if (str == DBNull.Value)
            {
                return DateTime.Now;
            }
            else
            {
                DateTime retValue;
                DateTime.TryParse(str.ToString(), out retValue);
                return retValue;
            }
        }
        
        public static string ToReplaceNumberWithCommas(this string value)
        {
            decimal decmalValue = Convert.ToDecimal(value);
            return string.Format("{0:#,##0.#0}", decmalValue);
        }

        public static string ToNormalize4(this decimal? value)
        {
            if (value != null)
            {
                string result = Convert.ToDecimal(value).ToString("0.####");
                if (result.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
                {
                    result = Convert.ToDecimal(value).ToString("0.00");
                }
                else
                {
                    if (result.Split('.').Length > 1 && result.Split('.')[1].Length == 1)
                    {
                        result = Convert.ToDecimal(value).ToString("0.#0");
                    }
                }

                return result;
            }
            else
            {
                return "";
            }
        }
        
        public static string ToNormalize4(this decimal value)
        {
            string result = Convert.ToDecimal(value).ToString("0.####");
            if (result.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                result = Convert.ToDecimal(value).ToString("0.00");
            }
            else
            {
                if (result.Split('.').Length > 1 && result.Split('.')[1].Length == 1)
                {
                    result = Convert.ToDecimal(value).ToString("0.#0");
                }
            }

            return result;
        }

        public static string ToNormalizeFull(this decimal value)
        {
            string result = Convert.ToDecimal(value).ToString("0.###########");
            if (result.IndexOf(".", StringComparison.OrdinalIgnoreCase) == -1)
            {
                result = Convert.ToDecimal(value).ToString("0.00");
            }
            else
            {
                if (result.Split('.').Length > 1 && result.Split('.')[1].Length == 1)
                {
                    result = Convert.ToDecimal(value).ToString("0.#0");
                }
            }

            return result;
        }


        public static string ToNormalizeUpTo2Decs(this decimal value)
        {
            string result = value.ToString("0.##");
            if (result.Split('.').Length > 1 && result.Split('.')[1].Length == 1)
            {
                result = value.ToString("0.#0");
            }
            return result;
        }
        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceDealerService.Extensions
{
        public static class StringExtensions
    {
        /// <summary>
        /// Get substring of specified number of characters from right, like VB.Right()
        /// </summary>
        public static string ClassicRight(this string value, int length)
        {
            return value[^length..];
        }

        /// <summary>
        /// Get substring of specified number of characters from right, like VB.Mid()
        /// CAUTION: This emulates a VB Function and starts at 1 NOT 0
        /// </summary>
        public static string Mid(this string value, int startIndex, int length)
        {
            //if (startIndex > 0)
            //  startIndex--;

            if (length > 0)
                return value.Substring(startIndex, length);

            return value[startIndex..];
        }

        public static T FromJson<T>(this string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Get substring of specified number of characters from left, like a VB.Left()
        /// </summary>
        /// <param name="v"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ClassicLeft(this string v, int len)
        {
            return v[..len];
        }

        public static bool ContainsInValidCharacters(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Contains("\\"))
                    return true;
                if (value.Contains("/"))
                    return true;
                if (value.Contains(" "))
                    return true;
                if (value.Contains(":"))
                    return true;
            }
            return false;
        }

        public static string RemoveByteOrderMarkUtf8(this string value)
        {

            if (!string.IsNullOrEmpty(value))
            {
                if (!value.Substring(0, 1).Equals("<"))
                {
                    string _byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                    if (value.StartsWith(_byteOrderMarkUtf8))
                    {
                        value = value.Remove(0, _byteOrderMarkUtf8.Length);
                    }
                }
            }
            return value;
        }

        public static byte[] HexStringToByteArray(this string value)
        {
            byte[] r = new byte[value.Length / 2 - 1 + 1];

            for (int i = 0; i <= value.Length / 2 - 1; i++)
                r[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);

            return r;
        }
        public static HashSet<string> ToHashSetLower(this IEnumerable<string> values)
        {
            var ret = new HashSet<string>();
            foreach (var item in values)
                ret.Add(item.ToLower());
            return ret;
        }

        public static string ToJoinedString(this HashSet<string> hashSet)
        {
            if (hashSet == null)
                return "";

            var ret = "";
            foreach (var item in hashSet)
                ret += item + " ";
            return ret.TrimEnd();
        }

        public static string IStr(this string value)
        {
            return value.Replace(",", ".");
        }

        public static string ToHexColor(this string value)
        {
            var ret = "#";

            if (value.StartsWith("#"))
                return value;
            if (value.Contains('/'))
            {
                var s = value.Split('/');
                foreach (var item in s)
                {
                    if (int.TryParse(item, out int i))
                        ret = ret + string.Format("{0:X2}", i);
                }
            }
            return ret;
        }

        public static string HexStringToString(this string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            var b = HexStringToByteArray(value);
            return encoding.GetString(b, 0, b.Length);
        }

        public static string AliasWithoutVariantID(string Setalias)
        {
            string sa = Setalias;

            if (!string.IsNullOrEmpty(sa))
            {
                if (sa.Contains("@"))
                    return sa.Substring(0, sa.IndexOf("@"));
            }

            return sa;
        }

        public static string EncodeB64(this string value)
        {
            byte[] b;
            var encoding = new UTF8Encoding();
            b = encoding.GetBytes(value);
            return Convert.ToBase64String(b);
        }

        public static string DecodeB64(this string value)
        {
            byte[] B = Convert.FromBase64String(value);
            var enc = new UTF8Encoding();
            return enc.GetString(B);
        }

        public static string DeNullify(this string value)
        {
            if (value == null) return "";
            else return value;
        }

        public static string UrlDecode(this string value)
        {
            return System.Web.HttpUtility.UrlDecode(value);
        }

        public static string UrlEncode(this string value)
        {
            return System.Web.HttpUtility.UrlEncode(value);
        }

        public static string HtmlEncode(this string value)
        {
            return System.Web.HttpUtility.HtmlEncode(value);
        }

        public static string HtmlDecode(this string value)
        {
            return System.Web.HttpUtility.HtmlDecode(value);
        }

        public static string UnitizeUrl(this string value)
        {
            if (value == null) return "";
            if (value.EndsWith("/"))
                return value;
            return value + "/";
        }

        public static string CorrectDirectorySeperator(this string value)
        {
            if (value.Contains("\\"))
                return value.Replace("\\", Path.DirectorySeparatorChar.ToString());
            else
                return value;
        }

        public static string UnitizePath(this string value)
        {
            if (value == null) return "";
            if (value.EndsWith(Path.DirectorySeparatorChar))
                return value;
            return value + Path.DirectorySeparatorChar;
        }

        public static string RemoveTrailingDirectorySeparator(this string value)
        {
            if (value == null) return "";
            if (!value.EndsWith(Path.DirectorySeparatorChar))
                return value;
            return value.TrimEnd(Path.DirectorySeparatorChar);
        }

        public static string RemoveStartingDirectorySeparator(this string value)
        {
            if (value == null) return "";
            if (!value.StartsWith(Path.DirectorySeparatorChar))
                return value;
            return value.TrimStart(Path.DirectorySeparatorChar);
        }

        public static bool IsHexString(this string v)
        {
            var pattern = @"[A-Fa-f0-9]";
            var rg = new Regex(pattern);
            return rg.IsMatch(v);
        }

    }
}

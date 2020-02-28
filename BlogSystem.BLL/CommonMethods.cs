using System;
using System.Text.RegularExpressions;

namespace BlogSystem.BLL
{
    public class CommonMethods
    {
        /// <summary>
        /// 截取指定长度字符串，并替换
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="maxLength"></param>
        /// <param name="endWith"></param>
        /// <returns></returns>
        public static string SplitString(string Str, int maxLength, string endWith)
        {
            if (string.IsNullOrEmpty(Str))
            {
                return string.Empty;
            }
            if (maxLength < 1)
            {
                return Str;
            }
            if (Str.Length > maxLength)
            {
                string strTmp = Str.Substring(0, maxLength);
                if (string.IsNullOrEmpty(endWith))
                    return strTmp;
                else
                    return strTmp + endWith;
            }
            return Str;
        }
        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="bingTime"></param>
        /// <returns></returns>
        public static string ComputeTime(DateTime bingTime)
        {
            DateTime NowTime = DateTime.Now;
            TimeSpan ts = NowTime - bingTime;
            return ts.Days.ToString() + "天" + ts.Hours + "时" + ts.Minutes + "分前";

        }
        /// <summary>
        /// 取得HTML中所有图片的URL
        /// </summary>
        /// <param name="sHtmlText"></param>
        /// <returns></returns>
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签   
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串   
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            if (matches.Count==0)
            {
                sUrlList = new string[1];
                sUrlList[0] = "0.png";
            }
            // 取得匹配项列表   
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;

            return sUrlList;

        }
    }
}
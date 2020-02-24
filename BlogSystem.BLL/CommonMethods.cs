using System;

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
            return ts.Days.ToString() +"天"+ ts.Hours + "时" +ts.Minutes + "分前" ;

        }
    }
}
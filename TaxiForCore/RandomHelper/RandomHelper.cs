using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TaxiForCore.RandomHelper
{
    public static class RandomHelper
    {

        private static char[] Lowercase = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static char[] Uppercase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] Numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] Symbols = { '!', '\\', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '[', ']', '~', '^', '_', '{', '}', '|' };


        /// <summary>
        /// Char[] 拼接
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static char[] CharArrayHandler(List<char[]> values)
        {
            var c = new ArrayList();
            foreach (char[] item in values)
            {
                c.AddRange(item);
            }
            return c.ToArray(typeof(char)) as char[];
        }


        /// <summary>
        /// 生成随机数字,取值范围为(0-9)
        /// </summary>
        /// <param name="randomStringLength">生成数字长度</param>
        /// <returns>返回取值范围为(0-9)的数字</returns>
        public static long GenerateRandomNumber(int randomStringLength)
        {
            return Convert.ToInt64(GetRandomString(Numbers, randomStringLength));
        }


        /// <summary>
        /// 生成随机字符串,字符范围为(0-9 a-b A-B)
        /// </summary>
        /// <param name="randomStringLength">生成字符串长度</param>
        /// <returns>返回字符范围为(0-9 a-b A-B)的字符串</returns>
        public static string GenerateRandomString(int randomStringLength)
        {
            var constant = CharArrayHandler(new List<char[]> { Lowercase, Uppercase, Numbers });
            return GetRandomString(constant, randomStringLength);
        }



        /// <summary>
        /// 生成随机字符串,字符范围为(a-b)或者(A-B)
        /// </summary>
        /// <param name="randomStringLength">生成字符串长度</param>
        /// <param name="isUpper">是否大写</param>
        /// <returns>返回字符范围为(a-b)或者(A-B)的字符串</returns>
        public static string GenerateRandomString(int randomStringLength, bool isUpper)
        {
            var constant = CharArrayHandler(new List<char[]> { Lowercase, Uppercase });
            string result = GetRandomString(constant, randomStringLength);
            return isUpper ? result.ToUpper() : result;
        }

        
        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="randomStringLength">密码长度</param>
        /// <param name="includeLetters">是否包含字母</param>
        /// <param name="includeNumber">是否包含数字</param>
        /// <param name="includeMixedCase">是否包含混合大小写</param>
        /// <param name="includePunctuation">是否包含符号</param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int randomStringLength,bool includeLetters=true, bool includeNumber = true,bool includeMixedCase=true,bool includePunctuation= true)
        {
            var temp = new List<char[]>();
            // 是否包含字母，默认包含了大小写
            if (includeLetters)
            {
                temp.Add(Lowercase);
                temp.Add(Uppercase);
            }

            // 包含数字
            if (includeNumber)
            {
                temp.Add(Numbers);
            }

            // 包含符号
            if (includePunctuation)
            {
                temp.Add(Symbols);
            }

            // 如果不允许混合大小写，则剔除大写字母集
            if (includeMixedCase==false)
            {
                temp.Remove(Uppercase);
            }

            var constant = CharArrayHandler(temp);
            if (temp.Count == 0) return string.Empty;
            return GetRandomString(constant, randomStringLength);
        }


        /// <summary>
        /// 根据字符串数组生成随机内容和长度的字符串
        /// </summary>
        /// <param name="constant">字符取值数组</param>
        /// <param name="randomStringLength">生成随机字符串长度</param>
        /// <returns>返回指定长度的随机字符串</returns>
        public static string GetRandomString(char[] constant, int randomStringLength)
        {
            StringBuilder sb = new StringBuilder(randomStringLength);
            Random rd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < randomStringLength; i++)
            {
                sb.Append(constant[rd.Next(constant.Length)]);
            }
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using TaxiForCore.StringHelper;

namespace TaxiForCore.EncryptsAndDecrypts
{
    public static class TokenHelper
    {

        /// <summary>
        /// 只生成TOKEN不返回GUID
        /// </summary>
        /// <returns></returns>
        public static string GenTokenString()
        {
            var a = GenToken();
            return a.Values.First();
        }

        /// <summary>
        /// 只返回校验结果不返回GUID
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeOutHour"></param>
        /// <returns></returns>
        public static bool CheckTokenBool(string token, int timeOutHour = 24)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("token");
            }
            var a = CheckToken(token, timeOutHour);
            return a.Values.First();
        }

        /// <summary>
        /// 用于生成TOKEN
        /// </summary>
        /// <returns>返回key为guid,value为token</returns>
        public static Dictionary<string, string> GenToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var guid = Guid.NewGuid();
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            return new Dictionary<string, string>() { { guid.ToString(), token } };
        }

        /// <summary>
        /// 用于校验TOKEN是否过期
        /// </summary>
        /// <param name="token">Token</param>
        /// <param name="timeOutHour">超时时间</param>
        /// <returns>返回key为guid,value为是否过期</returns>
        public static Dictionary<string, bool> CheckToken(string token, int timeOutHour = 24)
        {
            if (token.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("token");
            }

            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            var decodeToken = new Guid(data.Skip(8).ToArray()).ToString();
            if (when < DateTime.UtcNow.AddHours(timeOutHour * -1))
            {
                return new Dictionary<string, bool>() { { decodeToken, false } };
            }
            return new Dictionary<string, bool>() { { decodeToken, true } };
        }
    }
}

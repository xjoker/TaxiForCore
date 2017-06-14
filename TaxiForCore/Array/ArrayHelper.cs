using System.Collections;
using System.Text;

namespace TaxiForCore.Array
{
    public static class ArrayHelper
    {
        /// <summary>
        /// 数组转为String类型
        /// </summary>
        /// <param name="arr">数组</param>
        /// <param name="separator">分隔符,默认为","</param>
        /// <returns></returns>
        public static string JoinToString(this IEnumerable arr, string separator=",")
        {
            StringBuilder stringBuilder = new StringBuilder();
            string value = string.Empty;
            foreach (object current in arr)
            {
                stringBuilder.Append(value);
                stringBuilder.Append(current.ToString());
                value = separator;
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 慢速比较byte类型
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool SlowEquals(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            uint num = (uint)(a.Length ^ b.Length);
            for (int i = 0; i < a.Length; i++)
            {
                num |= (uint)(a[i] ^ b[i]);
            }
            return num == 0u;
        }
    }
}

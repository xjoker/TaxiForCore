using System.Collections.Generic;
using System.Linq;

namespace TaxiForCore.List
{
    public static class ListHelper
    {
        /// <summary>
        /// 获取List内最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetLast<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// List 搜索功能
        /// </summary>
        /// <param name="t"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static List<string> Search(this List<string> t,string word)
        {
            return t.Where(e => e.ToLower().Contains(word.ToLower())).ToList();
        }

        /// <summary>
        /// List 对比相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list1"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        public static bool Compare<T>(this List<T> list1,List<T> list2)
        {
            return !list1.Except(list2).ToList().Any() && !list2.Except(list1).ToList().Any();
        }
    }
}

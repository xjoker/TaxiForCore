using System.Collections.Generic;
using System.Linq;

namespace TaxiForCore.Dictionary
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// 字典合并拓展
        /// 如果不存在key才添加，存在则不报错直接忽略
        /// </summary>
        /// <typeparam name="TKey">字典的Key类型</typeparam>
        /// <typeparam name="TValue">字典的Value类型</typeparam>
        /// <param name="first">合并的字典1</param>
        /// <param name="second">合并的字典2</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> MergeDictionaryAdd<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            if (first == null) first = new Dictionary<TKey, TValue>();
            if (second == null) return first;

            foreach (var key in second.Keys)
            {
                if (!first.ContainsKey(key))
                    first.Add(key, second[key]);
            }
            return first;
        }

        /// <summary>
        /// 字典合并拓展
        /// 如果不存在key则添加，存在则替换,替换法则为second替换first
        /// </summary>
        /// <typeparam name="TKey">字典的Key类型</typeparam>
        /// <typeparam name="TValue">字典的Value类型</typeparam>
        /// <param name="first">合并的字典1</param>
        /// <param name="second">合并的字典2</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> MergeDictionaryReplace<TKey, TValue>(this Dictionary<TKey, TValue> first, Dictionary<TKey, TValue> second)
        {
            if (first == null) first = new Dictionary<TKey, TValue>();
            if (second == null) return first;

            foreach (var key in second.Keys)
            {
                if (first.ContainsKey(key))
                {
                    first[key] = second[key];
                }
                else
                {
                    first.Add(key, second[key]);
                }

            }
            return first;
        }

        /// <summary>
        /// 获取字典key的值，如果没有结果则返回空或者指定的值
        /// </summary>
        /// <typeparam name="TKey">字典的Key类型</typeparam>
        /// <typeparam name="TValue">字典的Value类型</typeparam>
        /// <param name="dict">查询的字典</param>
        /// <param name="key">key名称</param>
        /// <param name="defaultValue">未找到返回的值</param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 向字典中批量添加键值对
        /// </summary>
        /// <param name="replaceExisted">如果已存在，是否替换</param>
        public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) == false || replaceExisted)
                    dict[item.Key] = item.Value;
            }
            return dict;
        }

        /// <summary>
        /// 字典简单排序(默认以值做基准正向排序)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict">字典</param>
        /// <param name="desc">排序模式，默认正向</param>
        /// <param name="byKey">以KEY为基准排序</param>
        /// <param name="byValue">以值为基准排序</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> DictionarySort<TKey, TValue>(this Dictionary<TKey, TValue> dict, bool desc = false, bool byKey = false, bool byValue = true)
        {
            if (dict == null) return dict;
            IOrderedEnumerable<KeyValuePair<TKey, TValue>> temp = null;
            if (desc)
            {
                if (byValue)
                {
                    temp = from objDict in dict orderby objDict.Value descending select objDict;
                }
                else
                {
                    temp = from objDict in dict orderby objDict.Key descending select objDict;
                }

            }
            else
            {
                if (byValue)
                {
                    temp = from objDict in dict orderby objDict.Value select objDict;
                }
                else
                {
                    temp = from objDict in dict orderby objDict.Key select objDict;
                }
            }

            Dictionary<TKey, TValue> t = new Dictionary<TKey, TValue>();
            foreach (var item in temp)
            {
                t.Add(item.Key, item.Value);
            }
            return t;
        }

        /// <summary>
        /// 字典相等比较
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            return first.DictionaryEqual(second, null);
        }

        /// <summary>
        /// 字典相等比较
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="valueComparer"></param>
        /// <returns></returns>
        public static bool DictionaryEqual<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second,IEqualityComparer<TValue> valueComparer)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            foreach (var kvp in first)
            {
                if (!second.TryGetValue(kvp.Key, out TValue secondValue))
                {
                    return false;
                }

                if (!valueComparer.Equals(kvp.Value, secondValue))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

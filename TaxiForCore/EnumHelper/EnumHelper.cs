using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaxiForCore.StringHelper;

namespace TaxiForCore.EnumHelper
{
    public static class EnumHelper
    {

        /// <summary>
        /// String类型转为对应的Enum类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="str">枚举项字符串</param>
        /// <returns></returns>
        public static T ToEnum<T>(string str)
        {
            if (str.IsNullOrWhiteSpace())
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), str, true);
        }

        /// <summary>
        /// 将枚举类型的枚举项转换为List<String>
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns></returns>
        public static List<string> ToEnumName<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        /// <summary>
        /// 获得Enum项对应的数值
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static int GetIntValue(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        /// <summary>
        /// 获取Enum项的描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            var description = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .Select(x => x.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true)
                .FirstOrDefault()).FirstOrDefault() as System.ComponentModel.DescriptionAttribute;
            return description == null ? enumValue.ToString() : description.Description;
        }

        /// <summary>
        /// Enum 转换为字典类型 Dictionary<int, string>
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static IDictionary<int, string> ToDictionary<T>()
        {
            return Enum.GetValues(typeof(T)).OfType<Enum>()
                .ToDictionary(x => x.GetIntValue(), x => x.GetDescription());
        }
    }
}

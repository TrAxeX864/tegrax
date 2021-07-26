using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TgVozderzhansBot.Models
{
    public enum AbsItemType
    {
        [Description("Онанизм")]
        Онанизм = 1,
        [Description("Курение")]
        Курение = 2,
        [Description("Игромания")]
        Игромания = 3,
        [Description("Алкоголь")]
        Алкоголь = 4,
        [Description("Секс")]
        Секс = 5,
        [Description("Наркомания")]
        Наркомания = 6,
        [Description("Голодание")]
        Голодание = 7,
        [Description("Музыка")]
        Музыка = 8,
        [Description("Фильмы")]
        Фильмы = 9
    }
    
    public static class Extensions
    {
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
        
        public static string ToString(this AbsItemType absItemType)
        {
            string str = absItemType.ToString();

            str = Regex.Replace(str, "_", "");
            
            return str;
        }
    }

    public class AbsItem
    {
        public long Id { get; set; }
        
        public AbsItemType AbsItemType { get; set; }
        
        public DateTime Started { get; set; }
        
        public DateTime? Finished { get; set; }
        
        public User User { get; set; }
    }
}
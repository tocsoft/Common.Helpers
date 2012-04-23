﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Tocsoft.Common.Helpers.TypeConverters
{
    public class EnumToStringUsingDescription : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType.Equals(typeof(Enum)));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType.Equals(typeof(String)));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (!destinationType.Equals(typeof(String)))
            {
                throw new ArgumentException("Can only convert to string.", "destinationType");
            }

            if (!value.GetType().BaseType.Equals(typeof(Enum)))
            {
                throw new ArgumentException("Can only convert an instance of enum.", "value");
            }


            if (Enum.IsDefined(value.GetType(), value))
            {
                string name = value.ToString();
                object[] attrs =
                    value.GetType().GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attrs.Length > 0) ? ((DescriptionAttribute)attrs[0]).Description : name;
            }
            else
            {

                object[] objAttr = value.GetType().GetCustomAttributes(typeof(DefaultValueAttribute), false);
                return (objAttr.Length > 0) ? ((DefaultValueAttribute)objAttr[0]).Value : "";

            }
        }
    }
}

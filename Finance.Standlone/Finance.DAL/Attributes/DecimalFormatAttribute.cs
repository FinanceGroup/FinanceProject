using System;

namespace Finance.DAL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DecimalFormatAttribute : Attribute
    {
        public DecimalFormatAttribute(int precision, int scale)
        {
            Precision = precision;
            Scale = scale;
        }

        public int Precision { get; set; }
        public int Scale { get; set; }
    }
}

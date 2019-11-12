using System;

namespace Travel.Shop.Back.Common.Domain
{
    public class SqlDbEntity : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

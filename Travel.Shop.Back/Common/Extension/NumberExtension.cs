namespace Travel.Shop.Back.Common.Extension
{
    public static class NumberExtension
    {
        public static bool IsEmpty(this int? id)
        {
            return id == null || id == 0;
        }
    }
}

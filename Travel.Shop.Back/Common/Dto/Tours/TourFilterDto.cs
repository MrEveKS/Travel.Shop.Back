namespace Travel.Shop.Back.Common.Dto.Tours
{
    public class TourFilterDto : EntityDto, IQueryName
    {
        public string QueryName { get; set; }
    }
}

namespace Travel.Shop.Back.Common.Domain
{
    /// <summary>
    /// Базовая сущность c целочисленным идентификатором
    /// </summary>
    public class EntityBase : SqlDbEntity
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Сущность раннее не сохранялась
        /// </summary>
        public bool IsTransient => Id == 0;
    }
}

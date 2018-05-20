namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public interface IEnemySettings : IMechanicSettings
    {
        float MaxHealth { get; }
        bool HasPhases { get; }
        float PhasingHealth { get; }
    }
}
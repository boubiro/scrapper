using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrapper.Scrapper.Entities.Mechanics.Enemies
{
    public interface IEnemySettings : IMechanicSettings
    {
        float MaxHealth { get; }
        bool HasPhases { get; }
        float PhasingHealth { get; }
    }
}

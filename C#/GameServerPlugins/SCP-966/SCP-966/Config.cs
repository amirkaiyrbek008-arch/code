using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace SCP966 
{
    public class Config
    {
        [Description("Количество Игроков Требуемое Для Спавна SCP966")]
        public int RequiredPlayers { get; set; } = 8;
    }
}

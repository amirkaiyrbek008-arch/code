using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCP343
{
    public class Config
    {
        [Description("Количество Игроков Требуемое Для Спавна SCP343")]
        public static int RequiredPlayers { get; set; } = 8;
    }
}

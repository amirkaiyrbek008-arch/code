using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainRPCorePlugin.Features
{
    public class BlackCode
    {

        public void Init()
        {
            AudioClipStorage.LoadClip("blackcode_snd.ogg", "blackcode_snd");
        }
    }
}

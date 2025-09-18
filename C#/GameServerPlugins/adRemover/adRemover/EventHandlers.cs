using System;
using LabApi.Events.CustomHandlers;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using static System.Net.Mime.MediaTypeNames;

namespace adRemover
{
    public class EventHandlers : CustomEventsHandler
    {
        public EventHandlers()
        {
            Timing.RunCoroutine(enumerator());
        }
        List<string> bannedAds = new List<string>()
        {
            "#oldrust",
            "#rave",
            "#bulprust",
            "#stormrust",
            "#blaze",
            "#bloodrust",
            "#burprust",
            "#distrorting",
            "#magmarust",
            "tt:",
            "#eta",
            "t.me/",
            "#companyrust",
            "#pranik"
        };

        Dictionary<int, string> newNames = new Dictionary<int, string>();

        IEnumerator<float> enumerator()
        {
            while (true)
            {
                foreach (var k in newNames)
                {
                    Player plr = Player.Get(k.Key);
                    if (plr != null && plr.DisplayName != k.Value)
                    {
                        plr.DisplayName = k.Value;
                    }
                }
                yield return Timing.WaitForSeconds(2f);
            }
        }

        //public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
        //{
         //   foreach (string a in bannedAds)
       //     {
                
     //           string loweredA = ev.Player.Nickname.ToLower();
   //             int index = loweredA.IndexOf(a);
  //              if (index != -1)
 //               {
//                    string res = ev.Player.Nickname.Replace(a, "").Replace("  ", " ").Trim();

                    //newNames.Add(ev.Player.PlayerId, res);
                  //  ev.Player.DisplayName = res;
                //}
                
  //          }

//        }

        public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
        {
            string loweredName = ev.Player.Nickname.ToLower();
            string res = ev.Player.Nickname;

            foreach (string a in bannedAds)
            {
                int index = loweredName.IndexOf(a);
                if (index != -1)
                {
                    res = res.Remove(index, a.Length);
                    res = res.Replace("  ", " ").Trim();

                    newNames[ev.Player.PlayerId] = res;
                    ev.Player.DisplayName = res;

                    break; 
                }
            }
        }
        public override void OnServerRoundRestarted()
        {
            Timing.KillCoroutines();
        }
    }
}



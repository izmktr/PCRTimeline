using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class Battler
    {
        public Avatar avatar;
        public List<ISkill> timeline = new List<ISkill>();


        public Battler(Avatar avatar)
        {
            for (float time = 0.0f; time < 120f;)
            {
                timeline.Add(new CustomSkill()
                {
                    acttime = 4f,
                    interval = 3.5f
                }
                );

                time += 7.5f;
            }
        }
    }
}

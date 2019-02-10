using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public class Battler
    {
        public Avatar avatar = null;
        public List<CustomSkill> timeline = new List<CustomSkill>();


        public Battler(Avatar avatar)
        {
            this.avatar = avatar;

            float time = 0.0f;
            foreach(var skill in avatar.ActionOrder())
            {
                if (skill == null) break;
                if (120f < time) break;

                timeline.Add(new CustomSkill(skill));
                time += skill.acttime + skill.interval;
            }
        }
    }
}

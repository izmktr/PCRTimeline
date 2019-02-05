using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    interface ISkill
    {
        float acttime { get; set; }
        float interval { get; set; }
        SkillType Type { get; }

        bool darty { get; }
    }

    public class OriginalSkill : ISkill
    {
        public Buff buff;

        public float acttime { get; set; }
        public float interval { get; set; }

        public float AllTime { get { return acttime + interval; } }

        public SkillType Type { get { return SkillType.Bind; } }

        public bool darty { get { return false; } }
    }

    public class CustomSkill : ISkill
    {
       public float acttime { get; set; }
        public float interval { get; set; }

        public float AllTime { get { return acttime + interval; } }

        public bool darty { get; private set; }

        public SkillType Type { get { return skill.type; } }

        public OriginalSkill original;
        private Skill skill;

        public CustomSkill(Skill skill)
        {
            this.skill = skill;
            Reset();
        }

        private void Reset()
        {
            this.acttime = skill.acttime;
            this.interval = skill.interval;
            this.darty = false;
        }
    }

    /*
    public class AdditionalSkill : ISkill
    {
        public float acttime { get; set; }
        public float interval { get; set; }

        public float AllTime { get { return acttime + interval; } }

        public bool darty { get { return true; } }
    }
    */


}

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

        bool darty { get; set; }
        bool basic { get; }
    }

    public class CustomSkill : ISkill
    {
        public float acttime { get; set; }
        public float interval { get; set; }

        public float AllTime { get { return acttime + interval; } }

        public bool darty { get; set; }
        public SkillType Type { get { return skill.type; } }
        public bool basic { get { return true; } }

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

    public class AdditionalSkill : ISkill
    {
        public float acttime { get; set; }
        public float interval { get; set; }

        public float AllTime { get { return acttime + interval; } }

        public bool darty { get {return true; } set { } }
        public SkillType Type { get; set; }
        public bool basic { get { return false; } }


    }


}

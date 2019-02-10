using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public class CustomSkill
    {
        Skill original;
        public Skill modify;

        public void CreateModify()
        {
            if (modify == null) modify = original.Copy();
        }

        public float acttime => modify != null ? modify.acttime : original.acttime;
        public float interval => modify != null ? modify.interval : original.interval;

        public float AllTime { get { return acttime + interval; } }

        public bool darty => modify != null;
        public SkillType Type => original != null ? original.type : modify.type;
        public bool basic => original != null;

        public int skillNo
        {
            get
            {
                switch (Type)
                {
                    case SkillType.Skill1:
                        return 1;
                    case SkillType.Skill2:
                        return 2;
                    case SkillType.Skill3:
                        return 3;
                    case SkillType.Skill4:
                        return 4;
                    case SkillType.Skill5:
                        return 5;
                    default:
                        return 0;
                }
            }
        }

        public bool IsSkill
        {
            get
            {
                switch (Type)
                {
                    case SkillType.Skill1:
                    case SkillType.Skill2:
                    case SkillType.Skill3:
                    case SkillType.Skill4:
                    case SkillType.Skill5:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public string name => original.name;

        public Effect effect => modify != null ? modify.effect : original.effect;

        public CustomSkill(Skill skill)
        {
            original = skill;
        }

        public void Reset()
        {
            modify = null;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRTimeline.Data;

namespace PCRTimeline
{
    public class CustomSkill
    {
        Skill original = null;
        ActTime act = null;
        public float adjustment = default(float);

        public float interval => act.interval - adjustment;

        public bool darty => adjustment != default(float);
        public SkillType Type => original.type;
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

        public Effect effect => original.effect;

        public CustomSkill(Skill skill, SkillType nexttype = SkillType.Default)
        {
            original = skill;
            act = skill.GetActTime(nexttype);
        }

        public void Reset()
        {
            adjustment = default(float);
        }
    }

}

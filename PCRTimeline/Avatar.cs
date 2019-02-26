using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    namespace Data
    {
        public class Effect
        {
            public BuffEffectType type = BuffEffectType.None;
            public int value;
            public float delay;
            public float duration;

            internal Effect Copy()
            {
                var ret = new Effect();
                ret.type = type;
                ret.value = value;
                ret.delay = delay;
                ret.duration = duration;
                return ret;
            }
        }

        public class ActTime
        {
            public SkillType nexttype = SkillType.Default;
            public float interval = 0f;
            public float solidtime = 0f;
        }

        public class Skill
        {
            public SkillType type;
            public string name;

            public Effect effect;

            public List<ActTime> acttimelist = new List<ActTime>();

            internal Skill Copy()
            {
                var ret = new Skill();
                ret.type = type;
                ret.name = name;
                ret.effect = effect?.Copy();

                return ret;
            }

            public ActTime GetActTime(SkillType nexttype) 
            {
                var getindex = acttimelist.FindIndex(n => n.nexttype == nexttype);
                return 0 <= getindex ? acttimelist[getindex] : acttimelist.Find(n => n.nexttype == SkillType.Default);
            }

            static Dictionary<SkillType, Skill> genraldic = new Dictionary<SkillType, Skill>();
            static Skill General(SkillType type)
            {
                Skill skill;
                if (genraldic.TryGetValue(type, out skill)) { return skill; }
                skill = new Skill();
                skill.type = type;
                var acttime = new ActTime();
                skill.acttimelist.Add(acttime);
                switch (type)
                {
                    case SkillType.Bind:
                        acttime.interval = 1.0f;
                        break;
                    case SkillType.Dead:
                        acttime.interval = 3.0f;
                        break;
                }
                genraldic.Add(type, skill);

                return skill;
            }
        }

        public enum SkillType
        {
            Default,
            Opening,
            Attack,
            Skill1,
            Skill2,
            Skill3,
            Skill4,
            Skill5,
            UnionBurst,
            Bind,
            Dead,
        }

        public enum BuffEffectType
        {
            None,
            AtkFriend,
            AtkEnemy,
            DefFriend,
            DefEnemy,
            SpdFriend,
            SpdEnemy,
            HealFriend,
            OtherEnemy,
        }

        public class Avatar
        {
            public string name;
            public string aliasName;
            public int position;

            public string icon;

            public List<Skill> skill = new List<Skill>();

            public string actionOrder;

            [System.Xml.Serialization.XmlIgnore]
            public Image image;

            public Avatar Copy()
            {
                var ret = new Avatar();
                ret.name = name;
                ret.aliasName = aliasName;
                ret.position = position;
                ret.skill = skill;
                ret.actionOrder = actionOrder;
                return ret;
            }

            public IEnumerable<Skill> ActionOrder()
            {
                int loopindex = 0;

                for (int index = 0; index < actionOrder.Length; index++)
                {
                    var c = actionOrder[index];
                    switch (c)
                    {
                        case '[':
                            loopindex = index;
                            break;
                        case ']':
                            index = loopindex;
                            break;
                        default:
                            yield return ConvertSkill(c);
                            break;
                    }
                }
            }

            public Skill GetSkill(SkillType type)
            {
                switch (type)
                {
                    case SkillType.Opening:
                        break;
                    case SkillType.Attack:
                        break;
                    case SkillType.Skill1:
                        break;
                    case SkillType.Skill2:
                        break;
                    case SkillType.Skill3:
                        break;
                    case SkillType.Skill4:
                        break;
                    case SkillType.Skill5:
                        break;
                    case SkillType.UnionBurst:
                        break;
                    case SkillType.Bind:
                        break;
                    case SkillType.Dead:
                        break;
                    default:
                        break;
                }
                return null;
            }

            private Skill ConvertSkill(char c)
            {
                switch (Char.ToLower(c))
                {
                    case '1':
                        return skill.First(n => n.type == SkillType.Skill1);
                    case '2':
                        return skill.First(n => n.type == SkillType.Skill2);
                    case '3':
                        return skill.First(n => n.type == SkillType.Skill3);
                    case '4':
                        return skill.First(n => n.type == SkillType.Skill4);
                    case '5':
                        return skill.First(n => n.type == SkillType.Skill5);
                    case 'o':
                        return skill.First(n => n.type == SkillType.Opening);
                    case 'a':
                    case '0':
                        return skill.First(n => n.type == SkillType.Attack);
                }

                return null;
            }
        }
    }
}

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

                skill = new Skill
                {
                    type = type
                };
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

            public List<Tuple<SkillType, SkillType>> SkillPair()
            {
                HashSet<Tuple<SkillType, SkillType>> hash = new HashSet<Tuple<SkillType, SkillType>>();
                int loopindex = 0;

                bool endstep = false;

                SkillType before = SkillType.Default;
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
                            endstep = true;
                            break;
                        default:
                            var skilltype = ConvertSkillType(c);
                            if (skilltype == SkillType.Default)
                            {
                                index = actionOrder.Length;
                            }
                            else if (before != SkillType.Default)
                            {
                                hash.Add(new Tuple<SkillType, SkillType>(before, skilltype));
                                if (endstep) index = actionOrder.Length;
                            }
                            before = skilltype;
                            break;
                    }
                }

                return hash.ToList();
            }

            public void SkillDefault()
            {
                var list = SkillPair();

                foreach (var item in list)
                {
                    var s = skill.FirstOrDefault(n => n.type == item.Item1);
                    if (s == null)
                    {
                        s = new Skill() { type = item.Item1, name = item.Item1.ToString() };
                        s.acttimelist.Add(new ActTime() { nexttype = item.Item2, interval = 2f });
                    }
                    else
                    {
                        if (s.acttimelist.All(n => n.nexttype != item.Item2))
                        {
                            s.acttimelist.Add(new ActTime() { nexttype = item.Item2, interval = 2f });
                        }
                    }
                }

            }

            public Skill GetSkill(SkillType type)
            {
                return skill.FirstOrDefault(n => n.type == type);
            }

            SkillType ConvertSkillType(char c)
            {
                switch (Char.ToLower(c))
                {
                    case '1':
                        return SkillType.Skill1;
                    case '2':
                        return SkillType.Skill2;
                    case '3':
                        return SkillType.Skill3;
                    case '4':
                        return SkillType.Skill4;
                    case '5':
                        return SkillType.Skill5;
                    case 'o':
                        return SkillType.Opening;
                    case 'a':
                    case '0':
                        return SkillType.Attack;
                }
                return SkillType.Default;
            }

            private Skill ConvertSkill(char c)
            {
                var type = ConvertSkillType(c);
                if (type != SkillType.Default)
                {
                    return skill.First(n => n.type == type);
                }
                return null;
            }

            public void Save(string filename)
            {
                System.Xml.Serialization.XmlSerializer serializer1 = 
                    new System.Xml.Serialization.XmlSerializer(typeof(Avatar));

                System.IO.StreamWriter sw = new System.IO.StreamWriter(
                    filename, false, new System.Text.UTF8Encoding(false));

                serializer1.Serialize(sw, this);
                sw.Close();
            }
        }

    }
}

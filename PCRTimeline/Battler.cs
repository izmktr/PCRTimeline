using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRTimeline.Data;

namespace PCRTimeline
{
    public class BuffValuePair{
        public float time;
        public int deltaValue;
        public int totalValue;

        public BuffValuePair(float time, int delta)
        {
            this.time = time;
            this.deltaValue = delta;
            this.totalValue = 0;
        }
    };

    public class Battler
    {
        public Avatar avatar = null;
        public List<CustomSkill> timeline = new List<CustomSkill>();
        public Dictionary<BuffEffectType, List<BuffValuePair>> buff;

        public void CreateBuffline()
        {
            buff = new Dictionary<BuffEffectType, List<BuffValuePair>>();

            float time = 0.0f;
            foreach (var skill in timeline)
            {
                if (skill.effect == null) continue;
                if (0 < skill.effect.duration && skill.effect.type == BuffEffectType.None)
                {
                    float starttime = time + skill.interval + skill.effect.delay;
                    float endtime = starttime + skill.effect.duration;

                    var buffline = GetBuffLine(buff, skill.effect.type);

                    buffline.Add(new BuffValuePair(starttime, skill.effect.value));
                    buffline.Add(new BuffValuePair(endtime, -skill.effect.value));
                }
            }

            foreach (var buffkey in buff.Keys.ToArray())
            {
                var buffline = GetBuffLine(buff, buffkey);
                buffline.Sort((a, b) => FloatCompare(a.time, b.time));

                int value = 0;
                foreach (var item in buffline)
                {
                    value += item.deltaValue;
                    item.totalValue = value;
                }
            }
        }

        static int FloatCompare(float a, float b)
        {
            return a < b ? 1 : b < a ? -1 : 0;
        }

        private static List<BuffValuePair> GetBuffLine(Dictionary<BuffEffectType, List<BuffValuePair>> buff, BuffEffectType type)
        {
            List<BuffValuePair> buffline;
            if (buff.TryGetValue(type, out buffline)) return buffline;
            buffline = new List<BuffValuePair>();
            buff.Add(type, buffline);
            return buffline;
        }

        public Battler(Avatar avatar)
        {
            this.avatar = avatar;

            float time = 0.0f;

            Skill skill = null;
            foreach (var next in avatar.ActionOrder())
            {
                if (next == null) break;
                if (120f < time) break;

                if (skill != null)
                {
                    CustomSkill cskill = new CustomSkill(skill, next.type);
                    timeline.Add(cskill);
                    var acttime = skill.GetActTime(next.type);
                    time += acttime.interval;
                }

                skill = next;
            }
            CreateBuffline();
        }
    }
}

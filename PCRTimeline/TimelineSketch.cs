using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class SkillSketch
    {
        public int index;
        public Skill skill;

        public SkillSketch(int index, ISkill skill1)
        {
            this.index = index;
  //          this.skill1 = skill1;
        }
    }

    class BattlerSketch
    {
        public string aliasName;
        public List<SkillSketch> skillsketch = new List<SkillSketch>();
    }

    class TimelineSketch
    {
        public List<BattlerSketch> blist = new List<BattlerSketch>();
        
        [System.Xml.Serialization.XmlIgnore]
        public List<Avatar> avatarlist;

        public void Serialize(List<Battler> battlelist)
        {
            blist.Clear();

            foreach (var battler in battlelist)
            {
                BattlerSketch bsketch = new BattlerSketch() { aliasName = battler.avatar.aliasName };
                blist.Add(bsketch);

                int index = 0;
                foreach (var skill in battler.timeline)
                {
                    if (skill.darty)
                    {
                        bsketch.skillsketch.Add(new SkillSketch(index, skill));
                    }
                    if (!skill.basic) index++;
                }
            }


        }

        public List<Battler> DeSerialize()
        {
            var battlelist = new List<Battler>();

            return battlelist;
        }

    }
}

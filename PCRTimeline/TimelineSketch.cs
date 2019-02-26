using PCRTimeline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public class SkillSketch
    {
        public int index = 0;
        public SkillType type = SkillType.Default;
        public float adjustment = default(float);

        public SkillSketch(int index, CustomSkill skill)
        {
            this.index = index;
            this.type = skill.Type;
            this.adjustment = skill.adjustment;
        }

        public SkillSketch()
        {
        }
    }

    public class BattlerSketch
    {
        public string aliasName;
        public List<SkillSketch> skillsketch = new List<SkillSketch>();
    }

    public class TimelineSketch
    {
        public List<BattlerSketch> blist = new List<BattlerSketch>();
        
        [System.Xml.Serialization.XmlIgnore]
        public List<Avatar> avatarlist = null;

        public TimelineSketch()
        {
        }

        public static void Save(TimelineSketch sketch, string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(TimelineSketch));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                filename, false, new System.Text.UTF8Encoding(false));
            serializer.Serialize(sw, sketch);
            sw.Close();
        }

        public static TimelineSketch Load(string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(TimelineSketch));
            System.IO.StreamReader sr = new System.IO.StreamReader(
                filename, new System.Text.UTF8Encoding(false));
            TimelineSketch sketch = (TimelineSketch)serializer.Deserialize(sr);
            sr.Close();

            return sketch;
        }

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
                    index++;
                }
            }
        }

        public List<Battler> DeSerialize()
        {
            var battlelist = new List<Battler>();

            foreach (var item in blist)
            {
                var avatar = avatarlist.Find(n => n.aliasName == item.aliasName);
                var battler = new Battler(avatar);
                battlelist.Add(battler);

                foreach (var ssketch in item.skillsketch)
                {
                    var battlerskill = battler.timeline[ssketch.index];
                    if (ssketch.type == battlerskill.Type)
                    {
                        battlerskill.adjustment = ssketch.adjustment;
                    }
                    else
                    {
                        var skill = avatar.GetSkill(ssketch.type);
                        var insertskill = new CustomSkill(skill, SkillType.Default);
                        insertskill.adjustment = ssketch.adjustment;
                        battler.timeline.Insert(ssketch.index, insertskill);
                    }
                }
            }
            return battlelist;
        }

    }
}

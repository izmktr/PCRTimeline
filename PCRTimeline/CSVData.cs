using CsvHelper;
using CsvHelper.Configuration;
using PCRTimeline.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class CSVBasicStatus
    {
        public int princessIndex { get; set; }
        public string name { get; set; }
        public string aliasName { get; set; }
        public int position { get; set; }
        public string actionOrder { get; set; }
    }

    class CSVBattleStatus
    {
        public string aliasName { get; set; }
        public int Star { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public int Equip { get; set; }
        public int PAtk { get; set; }
        public int PDef { get; set; }
        public int MaxHp { get; set; }
        public int Avoid { get; set; }
        public int Accuracy { get; set; }
        public int HPAuto { get; set; }
        public int TPAuto { get; set; }
        public int HPDrain { get; set; }
        public int HealUp { get; set; }
        public int TPUp { get; set; }
        public int TPReduction { get; set; }
        public int Move { get; set; }
        public int MAtk { get; set; }
        public int MDef { get; set; }
        public int PCri { get; set; }
        public int MCri { get; set; }
        public int Reach { get; set; }
    }

    class CSVSkillName
    {
        public string aliasName { get; set; }
        public SkillType skillalias { get; set; }
        public string name { get; set; }
        public float preparation { get; set; }
    }

    class CSVSkillTime
    {
        public string aliasName { get; set; }
        public SkillType skillalias { get; set; }
        public SkillType nextskillalias { get; set; }
        public float intervalTime { get; set; }
    }

    class CSVSkillEffect
    {
        public string aliasName { get; set; }
        public SkillType skillalias { get; set; }
        public TargetType target { get; set; }
        public SkillEffectType effecttype { get; set; }
        public float delay { get; set; }
        public float duration { get; set; }
        public string expression { get; set; }
    }

    class CSVData
    {
        Dictionary<string, CSVBasicStatus> princesslist = new Dictionary<string, CSVBasicStatus>();

        public void ReadBasicStatus(string filename, List<Avatar> avatarlist)
        {
            using (var streamReader = new StreamReader(filename))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.HasHeaderRecord = false;
                var people = csv.GetRecords<CSVBasicStatus>();
                foreach (var p in people)
                {
                    princesslist[p.aliasName] = p;
                    avatarlist.Add(new Avatar()
                    {
                        aliasName = p.aliasName,
                        name = p.name,
                        position = p.position,
                        actionOrder = p.actionOrder,
                        icon = $@"Data\Icon\{p.aliasName}.png",
                    });
                }
            }
        }

        public void ReadSkillName(string filename, List<Avatar> avatarlist)
        {
            using (var streamReader = new StreamReader(filename))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.HasHeaderRecord = false;

                var skill = csv.GetRecords<CSVSkillName>();
                foreach (var p in skill)
                {
                    var first = avatarlist.FirstOrDefault(n => n.aliasName == p.aliasName);
                    if (first != null)
                    {
                        first.skill.Add(new Skill()
                        {
                            name = p.name,
                            type = p.skillalias,
                        });
                    }
                }
            }
        }

        public void ReadSkillTime(string filename, List<Avatar> avatarlist)
        {
            using (var streamReader = new StreamReader(filename))
            using (var csv = new CsvReader(streamReader))
            {
                csv.Configuration.HasHeaderRecord = false;
                var skilltime = csv.GetRecords<CSVSkillTime>();
                foreach (var p in skilltime)
                {
                    var avatar = avatarlist.FirstOrDefault(n => n.aliasName == p.aliasName);
                    if (avatar == null) continue;

                    var skill = avatar.skill.FirstOrDefault(n => n.type == p.skillalias);

                    if (skill == null)
                    {
                        skill = new Skill()
                        {
                            type = p.skillalias,
                            name = p.skillalias.ToString(),
                        };

                        avatar.skill.Add(skill);
                    }

                    skill.acttimelist.Add(new ActTime()
                    {
                        nexttype = p.nextskillalias,
                        interval = p.intervalTime,
                        solidtime = 0f,
                    });
                }
            }
        }

    }
}

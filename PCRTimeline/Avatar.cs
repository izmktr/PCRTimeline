﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public class Buff
    {
        public float delay;
        public float time;
    }

    public class Skill
    {
        public SkillType type;
        public string name;
        public float acttime;
        public float interval;
    }

    public enum SkillType
    {
        Opening,
        Attack,
        Skill1,
        Skill2,
        UnionBurst,
        Bind,
    }

    public class Avatar
    {
        public string name;
        public string aliasName;
        public int position;

        public string prevavatar;
        public string nextavatar;

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

        private Skill ConvertSkill(char c)
        {
            switch (Char.ToLower(c))
            {
                case '1':
                    return skill.First(n => n.type == SkillType.Skill1);
                case '2':
                    return skill.First(n => n.type == SkillType.Skill2);
                case 'o':
                    return skill.First(n => n.type == SkillType.Opening);
                case 'a':
                    return skill.First(n => n.type == SkillType.Attack);
            }

            return null;
        }
    }
}

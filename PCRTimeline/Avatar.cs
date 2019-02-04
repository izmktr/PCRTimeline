using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public class Skill
    {
        public float acttime;
        public float dalay;
        public Buff buff;

        public float AllTime { get { return acttime + dalay; } }
    }

    public class Buff
    {
        public float delay;
        public float time;
    }

    public class Avatar
    {
        public string name;
        public string aliasName;
        public Image image;
        public int position;

        public List<Skill> timeline = new List<Skill>();

        public void SetSkill()
        {
            float time = 0.0f;
            for (; time < 90f; )
            {
                timeline.Add(new Skill()
                {
                    acttime = 4f,
                    dalay = 3.5f                    
                }
                );

                time += 7.5f;
            }
        }

    }
}

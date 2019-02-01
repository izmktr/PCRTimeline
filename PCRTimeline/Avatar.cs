using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class Avatar
    {
        public string name;
        public string aliasName;
        public Image image;
        public int position;



        public class Skill
        {
            public float acttime;
            public float dalay;
            public float bufftime;

            public float AllTime { get { return acttime + dalay; } }
        }

        public List<Skill> timeline = new List<Skill>();

        public void SetSkill()
        {
            float time = 0.0f;
            for (; time < 90f; )
            {
                timeline.Add(new Skill()
                {
                    acttime = 4f,
                    dalay = 3.5f,
                    bufftime = 0f                    
                }
                );

                time += 7.5f;
            }
        }

    }
}

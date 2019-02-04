using System;
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
        public string type;
        public string name;
        public int acttime;
        public int interval;
    }

    public class Avatar
    {
        public string name;
        public string aliasName;
        public int position;

        public string prevavatar;
        public string nextavatar;

        public List<Skill> Skill = new List<Skill>();

        public string actionOrder;

        [System.Xml.Serialization.XmlIgnore]
        public Image image;
    }
}

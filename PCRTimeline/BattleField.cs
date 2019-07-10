using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline.Battle
{
    public class BuffUnit
    {
        public string name;
        public int value;
    }

    public class Command
    {
        public float delay;
    }

    public class Buff : Command
    {
        public List<BuffUnit> bufflist = new List<BuffUnit>();

        public float duration;

        public void Add(string name, int value)
        {
            bufflist.Add(new BuffUnit() { name = name, value = value });
        }
    }

    public class Attack : Command
    {
        public float duration;

        IEnumerable<Avatar> avatars(Field field, Team team, Avatar avatar)
        {
            yield return team.Opponent.GetNear(avatar);
        }

    }

    public class AvatarCondition
    {
        public float time;
        public int hp;
        public int tp;
        public int positon;
    }

    public class Avatar
    {
        List<AvatarCondition> conditions = new List<AvatarCondition>();


    }

    public class Team
    {
        List<Avatar> avatars = new List<Avatar>();

        public Team Opponent { get; set; }

        public Avatar GetNear(Avatar avatar)
        {
            return avatar;
//            return avatars.FindMin(n => Math.Abs(n.positon - avatar.positon));
        }


    }

    class Field
    {
        public Team[] team = new Team[2];

    }
}

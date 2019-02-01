using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class ActionOrder
    {
        static IEnumerable<char> Attack(string attack)
        {
            int loopindex = 0;

            for (int index = 0; index < attack.Length; index++)
            {
                var c = attack[index];
                switch (c)
                {
                    case '[':
                        loopindex = index;
                        break;
                    case ']':
                        index = loopindex;
                        break;
                    default:
                        yield return c;
                        break;
                }
            }
        }
    }
}

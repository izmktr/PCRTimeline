using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class Utility
    {
        public static uint Scramble(uint v)
        {
            // 奇数その1の乗算
            v *= 0x1ca7bc5b;

            // ビット逆順
            v = ((v >> 1) & 0x55555555) | ((v & 0x55555555) << 1);
            v = ((v >> 2) & 0x33333333) | ((v & 0x33333333) << 2);
            v = ((v >> 4) & 0x0F0F0F0F) | ((v & 0x0F0F0F0F) << 4);
            v = ((v >> 8) & 0x00FF00FF) | ((v & 0x00FF00FF) << 8);
            v = (v >> 16) | (v << 16);

            // 奇数その2（奇数その1の逆数）の乗算
            v *= 0x6b5f13d3;
            return v;
        }
    }
}

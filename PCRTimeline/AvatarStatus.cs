using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    public enum SkillEffectType
    {
        None,
        MaxHp,
        PAtk,
        PDef,
        PCri,
        MAtk,
        MDef,
        MCri,
        Hit,
        Avoid,
        HealUp,
        TPUp,
        HPAuto,
        TPAuto,

        PDamage,
        MDamage,
        HPHeal,
        TPHeal,
        HealOnTime,
        Bind,
        Charm,
        PBarrior,
        MBarrior,
        WBarrior,
        Knockback,

        EnshuredHit,
        EnshuredCri,
        EnshuredAvoid,
        Invincible,

    }


    class AvatarStatus
    {
        public int Level = 0;
        public int Rank = 0;
        public int Equip = 0;

        public int MaxHp = 0;
        public int PAtk = 0;
        public int PDef = 0;
        public int PCri = 0;
        public int MAtk = 0;
        public int MDef = 0;
        public int MCri = 0;
        public int Hit = 0;
        public int Avoid = 0;
        public int HealUp = 0;
        public int TPUp = 0;
        public int HPAuto = 0;
        public int TPAuto = 0;

        public int NameToValue(SkillEffectType status)
        {
            switch (status)
            {
                case SkillEffectType.MaxHp:
                    return MaxHp;
                case SkillEffectType.PAtk:
                    return PAtk;
                case SkillEffectType.PDef:
                    return PDef;
                case SkillEffectType.PCri:
                    return PCri;
                case SkillEffectType.MAtk:
                    return MAtk;
                case SkillEffectType.MDef:
                    return MDef;
                case SkillEffectType.MCri:
                    return MCri;
                case SkillEffectType.Hit:
                    return Hit;
                case SkillEffectType.Avoid:
                    return Avoid;
                case SkillEffectType.HealUp:
                    return HealUp;
                case SkillEffectType.TPUp:
                    return TPUp;
                case SkillEffectType.HPAuto:
                    return HPAuto;
                case SkillEffectType.TPAuto:
                    return TPAuto;
                default:
                    return 0;
            }
        }
    }
}

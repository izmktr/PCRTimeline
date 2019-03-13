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

    static class AttackFlag {
        public const uint Psysical = 1 << 0;
        public const uint Magic = 1 << 1;
        public const uint EnshuredHit = 1 << 2;
        public const uint EnshuredCri = 1 << 3;
        public const uint Invincible = 1 << 4;
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
        public int HPDrain = 0;
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


        public static AttackResult Battle(AvatarStatus attack, AvatarStatus defence, int damage, uint flag)
        {
            var result = new AttackResult();

            if ((flag & AttackFlag.Psysical) != 0)
            {
                result.damage = damage * 100 / (100 + defence.PDef);
                result.criRate = attack.PCri * 0.05f * Math.Max(attack.Level - defence.Level, 0);
            }
            else if ((flag & AttackFlag.Magic) != 0)
            {
                result.damage = damage * 100 / (100 + defence.MDef);
                result.criRate = attack.MCri * 0.05f * Math.Max(attack.Level - defence.Level, 0);
            }

            int avoid = Math.Max(defence.Avoid - attack.Hit, 0);
            result.avoidRate = 1f * avoid / (100 + avoid);
            result.hpDrain = result.damage * attack.HPDrain / (attack.HPDrain + defence.Level + 100);

            result.tpGain = 90 * (100 + attack.TPUp) / 100;

            if ((flag & AttackFlag.EnshuredHit) != 0) result.avoidRate = 0;
            if ((flag & AttackFlag.EnshuredCri) != 0) result.criRate = 100;
            if ((flag & AttackFlag.Invincible) != 0) result.avoidRate = 100;

            result.tpEnemyGain = (result.damage * 500 / defence.MaxHp) * (100 + defence.TPUp) / 100;

            return result;
        }
    }

    class AttackResult
    {
        public int damage;
        public float criRate;
        public float avoidRate;
        public int hpDrain;
        public int tpGain;
        public int tpEnemyGain;

        public int avgDamage { get { return (int)(damage * (1 + criRate) * (1 - avoidRate)); } }
    }

    
}

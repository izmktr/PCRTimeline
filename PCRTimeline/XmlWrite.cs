using PCRTimeline.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTimeline
{
    class XmlWrite
    {
        public static void SampleWrite()
        {
//             Avatar avatar = new Avatar()
//             {
//                 name = "ペコリーヌ",
//                 aliasName = "peko",
//                 position = 1,
// 
//                 prevavatar = "kaori",
//                 nextavatar = "nozomi",
// 
//                 actionOrder = "O12[A12]"
// 
//             };
// 
//             avatar.skill.Add(new Skill()
//             {
//                 type = SkillType.Opening,
//                 name = "開幕",
//                 acttime = 0,
//                 interval = 4,
//             });
// 
//             avatar.skill.Add(new Skill()
//             {
//                 type = SkillType.Attack,
//                 name = "通常攻撃",
//                 acttime = 2.5f,
//                 interval = 4,
//             });
// 
//             avatar.skill.Add(new Skill()
//             {
//                 type = SkillType.Skill1,
//                 name = "ランチタイム",
//                 acttime = 4.5f,
//                 interval = 4f,
//             });
// 
//             avatar.skill.Add(new Skill()
//             {
//                 type = SkillType.Skill2,
//                 name = "フォールスラッシュ",
//                 acttime = 2.5f,
//                 interval = 4,
//             });
// 
//             avatar.skill.Add(new Skill()
//             {
//                 type = SkillType.UnionBurst,
//                 name = "プリンセスストライク",
//                 acttime = 0.1f,
//                 interval = 4f,
//             });

            //XMLファイルに保存する
            System.Xml.Serialization.XmlSerializer serializer1 =
                new System.Xml.Serialization.XmlSerializer(typeof(Avatar));
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                @"Data\test.xml", false, new System.Text.UTF8Encoding(false));
//            serializer1.Serialize(sw, avatar);
            sw.Close();
            //*/
        }
    }

}

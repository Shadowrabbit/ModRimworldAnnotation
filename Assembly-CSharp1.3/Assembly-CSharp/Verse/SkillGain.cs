using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F2 RID: 242
	public class SkillGain
	{
		// Token: 0x06000678 RID: 1656 RVA: 0x000033AC File Offset: 0x000015AC
		public SkillGain()
		{
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001F93D File Offset: 0x0001DB3D
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001F954 File Offset: 0x0001DB54
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured SkillGain: " + xmlRoot.OuterXml);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.xp = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x040005AC RID: 1452
		public SkillDef skill;

		// Token: 0x040005AD RID: 1453
		public int xp;
	}
}

using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000166 RID: 358
	public class SkillGain
	{
		// Token: 0x060008F7 RID: 2295 RVA: 0x00006B8B File Offset: 0x00004D8B
		public SkillGain()
		{
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0000D187 File Offset: 0x0000B387
		public SkillGain(SkillDef skill, int xp)
		{
			this.skill = skill;
			this.xp = xp;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00097C2C File Offset: 0x00095E2C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured SkillGain: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.xp = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400079D RID: 1949
		public SkillDef skill;

		// Token: 0x0400079E RID: 1950
		public int xp;
	}
}

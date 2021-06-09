using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000165 RID: 357
	public class SkillRequirement
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x0000D0CA File Offset: 0x0000B2CA
		public string Summary
		{
			get
			{
				if (this.skill == null)
				{
					return "";
				}
				return string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0000D0FF File Offset: 0x0000B2FF
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0000D12C File Offset: 0x0000B32C
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0000D157 File Offset: 0x0000B357
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel;
		}

		// Token: 0x0400079B RID: 1947
		public SkillDef skill;

		// Token: 0x0400079C RID: 1948
		public int minLevel;
	}
}

using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F1 RID: 241
	public class SkillRequirement
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001F880 File Offset: 0x0001DA80
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

		// Token: 0x06000674 RID: 1652 RVA: 0x0001F8B5 File Offset: 0x0001DAB5
		public bool PawnSatisfies(Pawn pawn)
		{
			return pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001F8E2 File Offset: 0x0001DAE2
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001F90D File Offset: 0x0001DB0D
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel;
		}

		// Token: 0x040005AA RID: 1450
		public SkillDef skill;

		// Token: 0x040005AB RID: 1451
		public int minLevel;
	}
}

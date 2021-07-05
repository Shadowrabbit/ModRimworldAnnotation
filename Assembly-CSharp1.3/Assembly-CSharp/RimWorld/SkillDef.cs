using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AC1 RID: 2753
	public class SkillDef : Def
	{
		// Token: 0x06004126 RID: 16678 RVA: 0x0015ECF1 File Offset: 0x0015CEF1
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0015ED08 File Offset: 0x0015CF08
		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != WorkTags.None)
			{
				return true;
			}
			if (this.neverDisabledBasedOnWorkTypes)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			bool result = false;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				for (int j = 0; j < workTypeDef.relevantSkills.Count; j++)
				{
					if (workTypeDef.relevantSkills[j] == this)
					{
						if (!disabledWorkTypes.Contains(workTypeDef))
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x040026AB RID: 9899
		[MustTranslate]
		public string skillLabel;

		// Token: 0x040026AC RID: 9900
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x040026AD RID: 9901
		public bool pawnCreatorSummaryVisible;

		// Token: 0x040026AE RID: 9902
		public WorkTags disablingWorkTags;

		// Token: 0x040026AF RID: 9903
		public float listOrder;

		// Token: 0x040026B0 RID: 9904
		public bool neverDisabledBasedOnWorkTypes;
	}
}

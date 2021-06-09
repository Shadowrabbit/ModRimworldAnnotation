using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FE2 RID: 4066
	public class SkillDef : Def
	{
		// Token: 0x060058B2 RID: 22706 RVA: 0x0003DA23 File Offset: 0x0003BC23
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x060058B3 RID: 22707 RVA: 0x001D0830 File Offset: 0x001CEA30
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

		// Token: 0x04003AF3 RID: 15091
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04003AF4 RID: 15092
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04003AF5 RID: 15093
		public bool pawnCreatorSummaryVisible;

		// Token: 0x04003AF6 RID: 15094
		public WorkTags disablingWorkTags;

		// Token: 0x04003AF7 RID: 15095
		public float listOrder;

		// Token: 0x04003AF8 RID: 15096
		public bool neverDisabledBasedOnWorkTypes;
	}
}

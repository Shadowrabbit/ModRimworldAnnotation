using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020000B0 RID: 176
	public class RecipeMakerProperties
	{
		// Token: 0x0400034D RID: 845
		public int productCount = 1;

		// Token: 0x0400034E RID: 846
		public int targetCountAdjustment = 1;

		// Token: 0x0400034F RID: 847
		public int bulkRecipeCount = -1;

		// Token: 0x04000350 RID: 848
		public bool useIngredientsForColor = true;

		// Token: 0x04000351 RID: 849
		public int workAmount = -1;

		// Token: 0x04000352 RID: 850
		public StatDef workSpeedStat;

		// Token: 0x04000353 RID: 851
		public StatDef efficiencyStat;

		// Token: 0x04000354 RID: 852
		public ThingDef unfinishedThingDef;

		// Token: 0x04000355 RID: 853
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000356 RID: 854
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000357 RID: 855
		public SkillDef workSkill;

		// Token: 0x04000358 RID: 856
		public float workSkillLearnPerTick = 1f;

		// Token: 0x04000359 RID: 857
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x0400035A RID: 858
		public EffecterDef effectWorking;

		// Token: 0x0400035B RID: 859
		public SoundDef soundWorking;

		// Token: 0x0400035C RID: 860
		public List<ThingDef> recipeUsers;

		// Token: 0x0400035D RID: 861
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x0400035E RID: 862
		public List<MemeDef> memePrerequisitesAny;

		// Token: 0x0400035F RID: 863
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000360 RID: 864
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000361 RID: 865
		public bool fromIdeoBuildingPreceptOnly;
	}
}

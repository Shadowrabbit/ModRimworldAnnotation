using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000118 RID: 280
	public class RecipeMakerProperties
	{
		// Token: 0x04000537 RID: 1335
		public int productCount = 1;

		// Token: 0x04000538 RID: 1336
		public int targetCountAdjustment = 1;

		// Token: 0x04000539 RID: 1337
		public int bulkRecipeCount = -1;

		// Token: 0x0400053A RID: 1338
		public bool useIngredientsForColor = true;

		// Token: 0x0400053B RID: 1339
		public int workAmount = -1;

		// Token: 0x0400053C RID: 1340
		public StatDef workSpeedStat;

		// Token: 0x0400053D RID: 1341
		public StatDef efficiencyStat;

		// Token: 0x0400053E RID: 1342
		public ThingDef unfinishedThingDef;

		// Token: 0x0400053F RID: 1343
		public ThingFilter defaultIngredientFilter;

		// Token: 0x04000540 RID: 1344
		public List<SkillRequirement> skillRequirements;

		// Token: 0x04000541 RID: 1345
		public SkillDef workSkill;

		// Token: 0x04000542 RID: 1346
		public float workSkillLearnPerTick = 1f;

		// Token: 0x04000543 RID: 1347
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x04000544 RID: 1348
		public EffecterDef effectWorking;

		// Token: 0x04000545 RID: 1349
		public SoundDef soundWorking;

		// Token: 0x04000546 RID: 1350
		public List<ThingDef> recipeUsers;

		// Token: 0x04000547 RID: 1351
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x04000548 RID: 1352
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000549 RID: 1353
		[NoTranslate]
		public List<string> factionPrerequisiteTags;
	}
}

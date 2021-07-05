using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A80 RID: 2688
	public class InspirationDef : Def
	{
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0015BD8B File Offset: 0x00159F8B
		public InspirationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InspirationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040024A8 RID: 9384
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x040024A9 RID: 9385
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x040024AA RID: 9386
		public float baseCommonality = 1f;

		// Token: 0x040024AB RID: 9387
		public float baseDurationDays = 1f;

		// Token: 0x040024AC RID: 9388
		public bool allowedOnAnimals;

		// Token: 0x040024AD RID: 9389
		public bool allowedOnNonColonists;

		// Token: 0x040024AE RID: 9390
		public bool allowedOnDownedPawns = true;

		// Token: 0x040024AF RID: 9391
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x040024B0 RID: 9392
		public List<SkillRequirement> requiredSkills;

		// Token: 0x040024B1 RID: 9393
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x040024B2 RID: 9394
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x040024B3 RID: 9395
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x040024B4 RID: 9396
		public WorkTags requiredNonDisabledWorkTags;

		// Token: 0x040024B5 RID: 9397
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x040024B6 RID: 9398
		public List<SkillDef> associatedSkills;

		// Token: 0x040024B7 RID: 9399
		public List<StatModifier> statOffsets;

		// Token: 0x040024B8 RID: 9400
		public List<StatModifier> statFactors;

		// Token: 0x040024B9 RID: 9401
		[MustTranslate]
		public string beginLetter;

		// Token: 0x040024BA RID: 9402
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x040024BB RID: 9403
		public LetterDef beginLetterDef;

		// Token: 0x040024BC RID: 9404
		[MustTranslate]
		public string endMessage;

		// Token: 0x040024BD RID: 9405
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x040024BE RID: 9406
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}

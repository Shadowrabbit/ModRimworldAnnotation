using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FAB RID: 4011
	public class InspirationDef : Def
	{
		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x060057CA RID: 22474 RVA: 0x0003CDE0 File Offset: 0x0003AFE0
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

		// Token: 0x04003993 RID: 14739
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x04003994 RID: 14740
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x04003995 RID: 14741
		public float baseCommonality = 1f;

		// Token: 0x04003996 RID: 14742
		public float baseDurationDays = 1f;

		// Token: 0x04003997 RID: 14743
		public bool allowedOnAnimals;

		// Token: 0x04003998 RID: 14744
		public bool allowedOnNonColonists;

		// Token: 0x04003999 RID: 14745
		public bool allowedOnDownedPawns = true;

		// Token: 0x0400399A RID: 14746
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x0400399B RID: 14747
		public List<SkillRequirement> requiredSkills;

		// Token: 0x0400399C RID: 14748
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x0400399D RID: 14749
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x0400399E RID: 14750
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x0400399F RID: 14751
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x040039A0 RID: 14752
		public List<SkillDef> associatedSkills;

		// Token: 0x040039A1 RID: 14753
		public List<StatModifier> statOffsets;

		// Token: 0x040039A2 RID: 14754
		public List<StatModifier> statFactors;

		// Token: 0x040039A3 RID: 14755
		[MustTranslate]
		public string beginLetter;

		// Token: 0x040039A4 RID: 14756
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x040039A5 RID: 14757
		public LetterDef beginLetterDef;

		// Token: 0x040039A6 RID: 14758
		[MustTranslate]
		public string endMessage;

		// Token: 0x040039A7 RID: 14759
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x040039A8 RID: 14760
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}

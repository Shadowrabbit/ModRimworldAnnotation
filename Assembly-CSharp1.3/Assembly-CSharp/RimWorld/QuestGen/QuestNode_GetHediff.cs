using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001672 RID: 5746
	public class QuestNode_GetHediff : QuestNode
	{
		// Token: 0x060085CE RID: 34254 RVA: 0x002FFB9A File Offset: 0x002FDD9A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x002FFBA4 File Offset: 0x002FDDA4
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x002FFBB4 File Offset: 0x002FDDB4
		private void SetVars(Slate slate)
		{
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		// Token: 0x040053A3 RID: 21411
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		// Token: 0x040053A4 RID: 21412
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		// Token: 0x040053A5 RID: 21413
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		// Token: 0x02002919 RID: 10521
		public class Option
		{
			// Token: 0x04009ADF RID: 39647
			public HediffDef def;

			// Token: 0x04009AE0 RID: 39648
			public List<BodyPartDef> partsToAffect;

			// Token: 0x04009AE1 RID: 39649
			public float weight = 1f;
		}
	}
}

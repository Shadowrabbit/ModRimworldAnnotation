using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F20 RID: 7968
	public class QuestNode_GetHediff : QuestNode
	{
		// Token: 0x0600AA6C RID: 43628 RVA: 0x0006FA6F File Offset: 0x0006DC6F
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA6D RID: 43629 RVA: 0x0006FA79 File Offset: 0x0006DC79
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA6E RID: 43630 RVA: 0x0031BA2C File Offset: 0x00319C2C
		private void SetVars(Slate slate)
		{
			QuestNode_GetHediff.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetHediff.Option x) => x.weight);
			slate.Set<HediffDef>(this.storeHediffAs.GetValue(slate), option.def, false);
			if (this.storePartsToAffectAs.GetValue(slate) != null)
			{
				slate.Set<List<BodyPartDef>>(this.storePartsToAffectAs.GetValue(slate), option.partsToAffect, false);
			}
		}

		// Token: 0x040073CE RID: 29646
		[NoTranslate]
		public SlateRef<string> storeHediffAs;

		// Token: 0x040073CF RID: 29647
		[NoTranslate]
		public SlateRef<string> storePartsToAffectAs;

		// Token: 0x040073D0 RID: 29648
		public SlateRef<List<QuestNode_GetHediff.Option>> options;

		// Token: 0x02001F21 RID: 7969
		public class Option
		{
			// Token: 0x040073D1 RID: 29649
			public HediffDef def;

			// Token: 0x040073D2 RID: 29650
			public List<BodyPartDef> partsToAffect;

			// Token: 0x040073D3 RID: 29651
			public float weight = 1f;
		}
	}
}

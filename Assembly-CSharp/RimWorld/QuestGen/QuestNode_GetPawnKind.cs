using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F3E RID: 7998
	public class QuestNode_GetPawnKind : QuestNode
	{
		// Token: 0x0600AAD8 RID: 43736 RVA: 0x0006FDBA File Offset: 0x0006DFBA
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AAD9 RID: 43737 RVA: 0x0006FDC4 File Offset: 0x0006DFC4
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AADA RID: 43738 RVA: 0x0031D90C File Offset: 0x0031BB0C
		private void SetVars(Slate slate)
		{
			QuestNode_GetPawnKind.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetPawnKind.Option x) => x.weight);
			PawnKindDef var;
			if (option.kindDef != null)
			{
				var = option.kindDef;
			}
			else if (option.anyAnimal)
			{
				var = (from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Animal && (option.onlyAllowedFleshType == null || x.RaceProps.FleshType == option.onlyAllowedFleshType)
				select x).RandomElement<PawnKindDef>();
			}
			else
			{
				var = null;
			}
			slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x04007435 RID: 29749
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007436 RID: 29750
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		// Token: 0x02001F3F RID: 7999
		public class Option
		{
			// Token: 0x04007437 RID: 29751
			public PawnKindDef kindDef;

			// Token: 0x04007438 RID: 29752
			public float weight;

			// Token: 0x04007439 RID: 29753
			public bool anyAnimal;

			// Token: 0x0400743A RID: 29754
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}

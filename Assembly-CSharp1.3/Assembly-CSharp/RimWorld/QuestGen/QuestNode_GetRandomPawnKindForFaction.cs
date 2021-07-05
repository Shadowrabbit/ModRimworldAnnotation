using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016C2 RID: 5826
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
	{
		// Token: 0x060086F9 RID: 34553 RVA: 0x0030541B File Offset: 0x0030361B
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x060086FA RID: 34554 RVA: 0x00305424 File Offset: 0x00303624
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x060086FB RID: 34555 RVA: 0x00305434 File Offset: 0x00303634
		private bool SetVars(Slate slate)
		{
			Thing value = this.factionOf.GetValue(slate);
			if (value == null)
			{
				return false;
			}
			Faction faction = value.Faction;
			if (faction == null)
			{
				return false;
			}
			List<QuestNode_GetRandomPawnKindForFaction.Choice> value2 = this.choices.GetValue(slate);
			for (int i = 0; i < value2.Count; i++)
			{
				PawnKindDef var;
				if (((value2[i].factionDef != null && faction.def == value2[i].factionDef) || (!value2[i].categoryTag.NullOrEmpty() && value2[i].categoryTag == faction.def.categoryTag)) && value2[i].pawnKinds.TryRandomElement(out var))
				{
					slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
					return true;
				}
			}
			if (this.fallback.GetValue(slate) != null)
			{
				slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.fallback.GetValue(slate), false);
				return true;
			}
			return false;
		}

		// Token: 0x040054D7 RID: 21719
		public SlateRef<Thing> factionOf;

		// Token: 0x040054D8 RID: 21720
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040054D9 RID: 21721
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		// Token: 0x040054DA RID: 21722
		public SlateRef<PawnKindDef> fallback;

		// Token: 0x02002947 RID: 10567
		public class Choice
		{
			// Token: 0x04009B47 RID: 39751
			public FactionDef factionDef;

			// Token: 0x04009B48 RID: 39752
			public string categoryTag;

			// Token: 0x04009B49 RID: 39753
			public List<PawnKindDef> pawnKinds;
		}
	}
}

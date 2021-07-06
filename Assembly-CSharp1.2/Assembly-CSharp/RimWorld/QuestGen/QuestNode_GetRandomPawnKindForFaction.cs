using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F98 RID: 8088
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
	{
		// Token: 0x0600AC01 RID: 44033 RVA: 0x00070792 File Offset: 0x0006E992
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x0600AC02 RID: 44034 RVA: 0x0007079B File Offset: 0x0006E99B
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AC03 RID: 44035 RVA: 0x00320B38 File Offset: 0x0031ED38
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

		// Token: 0x04007557 RID: 30039
		public SlateRef<Thing> factionOf;

		// Token: 0x04007558 RID: 30040
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04007559 RID: 30041
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		// Token: 0x0400755A RID: 30042
		public SlateRef<PawnKindDef> fallback;

		// Token: 0x02001F99 RID: 8089
		public class Choice
		{
			// Token: 0x0400755B RID: 30043
			public FactionDef factionDef;

			// Token: 0x0400755C RID: 30044
			public string categoryTag;

			// Token: 0x0400755D RID: 30045
			public List<PawnKindDef> pawnKinds;
		}
	}
}

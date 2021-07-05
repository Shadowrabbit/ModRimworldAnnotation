using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FBE RID: 8126
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		// Token: 0x0600AC83 RID: 44163 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600AC84 RID: 44164 RVA: 0x00322584 File Offset: 0x00320784
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		// Token: 0x040075F7 RID: 30199
		public SlateRef<Pawn> pawn;

		// Token: 0x040075F8 RID: 30200
		public SlateRef<RoyalTitleDef> royalTitle;

		// Token: 0x040075F9 RID: 30201
		public SlateRef<Faction> faction;
	}
}

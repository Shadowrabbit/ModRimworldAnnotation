using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016E4 RID: 5860
	public class QuestNode_SetRoyalTitle : QuestNode
	{
		// Token: 0x0600876C RID: 34668 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0600876D RID: 34669 RVA: 0x00307208 File Offset: 0x00305408
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Pawn value = this.pawn.GetValue(slate);
			if (value.royalty != null)
			{
				value.royalty.SetTitle(this.faction.GetValue(slate), this.royalTitle.GetValue(slate), false, false, true);
			}
		}

		// Token: 0x0400557E RID: 21886
		public SlateRef<Pawn> pawn;

		// Token: 0x0400557F RID: 21887
		public SlateRef<RoyalTitleDef> royalTitle;

		// Token: 0x04005580 RID: 21888
		public SlateRef<Faction> faction;
	}
}

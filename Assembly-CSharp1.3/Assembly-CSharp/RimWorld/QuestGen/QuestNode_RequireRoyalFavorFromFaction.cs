using System;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A1 RID: 5793
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		// Token: 0x06008693 RID: 34451 RVA: 0x00303F15 File Offset: 0x00302115
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate) != null && this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		// Token: 0x06008694 RID: 34452 RVA: 0x0000313F File Offset: 0x0000133F
		protected override void RunInt()
		{
		}

		// Token: 0x0400546B RID: 21611
		public SlateRef<Faction> faction;
	}
}

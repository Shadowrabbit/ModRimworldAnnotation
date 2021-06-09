using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F73 RID: 8051
	public class QuestNode_RequireRoyalFavorFromFaction : QuestNode
	{
		// Token: 0x0600AB90 RID: 43920 RVA: 0x0007061D File Offset: 0x0006E81D
		protected override bool TestRunInt(Slate slate)
		{
			return this.faction.GetValue(slate).allowRoyalFavorRewards;
		}

		// Token: 0x0600AB91 RID: 43921 RVA: 0x00006A05 File Offset: 0x00004C05
		protected override void RunInt()
		{
		}

		// Token: 0x040074EB RID: 29931
		public SlateRef<Faction> faction;
	}
}

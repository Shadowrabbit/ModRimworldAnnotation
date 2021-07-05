using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA1 RID: 2977
	public class QuestPart_ReserveFaction : QuestPart
	{
		// Token: 0x06004583 RID: 17795 RVA: 0x00170913 File Offset: 0x0016EB13
		public override bool QuestPartReserves(Faction f)
		{
			return f != null && f == this.faction;
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x00170923 File Offset: 0x0016EB23
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x00170935 File Offset: 0x0016EB35
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04002A55 RID: 10837
		public Faction faction;
	}
}

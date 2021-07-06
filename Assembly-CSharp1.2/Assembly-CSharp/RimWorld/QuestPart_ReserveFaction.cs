using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001109 RID: 4361
	public class QuestPart_ReserveFaction : QuestPart
	{
		// Token: 0x06005F4A RID: 24394 RVA: 0x00041ED1 File Offset: 0x000400D1
		public override bool QuestPartReserves(Faction f)
		{
			return f != null && f == this.faction;
		}

		// Token: 0x06005F4B RID: 24395 RVA: 0x00041EE1 File Offset: 0x000400E1
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x00041EF3 File Offset: 0x000400F3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x04003FBE RID: 16318
		public Faction faction;
	}
}

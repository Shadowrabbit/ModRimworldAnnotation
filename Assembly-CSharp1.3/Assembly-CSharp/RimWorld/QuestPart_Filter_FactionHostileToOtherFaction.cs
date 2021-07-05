using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B38 RID: 2872
	public class QuestPart_Filter_FactionHostileToOtherFaction : QuestPart_Filter
	{
		// Token: 0x06004342 RID: 17218 RVA: 0x00167119 File Offset: 0x00165319
		protected override bool Pass(SignalArgs args)
		{
			return this.faction != null && this.other != null && this.faction != this.other && this.faction.HostileTo(this.other);
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x0016714C File Offset: 0x0016534C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Faction>(ref this.other, "other", false);
		}

		// Token: 0x040028E9 RID: 10473
		public Faction faction;

		// Token: 0x040028EA RID: 10474
		public Faction other;
	}
}

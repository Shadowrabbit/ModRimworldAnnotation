using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105F RID: 4191
	public class QuestPart_Filter_FactionHostileToOtherFaction : QuestPart_Filter
	{
		// Token: 0x06005B3A RID: 23354 RVA: 0x0003F417 File Offset: 0x0003D617
		protected override bool Pass(SignalArgs args)
		{
			return this.faction != null && this.other != null && this.faction != this.other && this.faction.HostileTo(this.other);
		}

		// Token: 0x06005B3B RID: 23355 RVA: 0x0003F44A File Offset: 0x0003D64A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Faction>(ref this.other, "other", false);
		}

		// Token: 0x04003D40 RID: 15680
		public Faction faction;

		// Token: 0x04003D41 RID: 15681
		public Faction other;
	}
}

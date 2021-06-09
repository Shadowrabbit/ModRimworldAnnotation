using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CB RID: 4299
	public class ExtraFaction : IExposable
	{
		// Token: 0x06005DBD RID: 23997 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ExtraFaction()
		{
		}

		// Token: 0x06005DBE RID: 23998 RVA: 0x00040FEA File Offset: 0x0003F1EA
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		// Token: 0x06005DBF RID: 23999 RVA: 0x00041000 File Offset: 0x0003F200
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		// Token: 0x04003EB3 RID: 16051
		public Faction faction;

		// Token: 0x04003EB4 RID: 16052
		public ExtraFactionType factionType;
	}
}

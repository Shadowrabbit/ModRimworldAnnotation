using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B78 RID: 2936
	public class ExtraFaction : IExposable
	{
		// Token: 0x060044A1 RID: 17569 RVA: 0x000033AC File Offset: 0x000015AC
		public ExtraFaction()
		{
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x0016C0A9 File Offset: 0x0016A2A9
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x0016C0BF File Offset: 0x0016A2BF
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		// Token: 0x040029A7 RID: 10663
		public Faction faction;

		// Token: 0x040029A8 RID: 10664
		public ExtraFactionType factionType;
	}
}

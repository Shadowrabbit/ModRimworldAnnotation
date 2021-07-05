using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8C RID: 2956
	public class QuestPart_PawnKilled : QuestPart
	{
		// Token: 0x06004520 RID: 17696 RVA: 0x0016EAA0 File Offset: 0x0016CCA0
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent == this.mapParent)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal));
			}
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x0016EAF3 File Offset: 0x0016CCF3
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x0016EB05 File Offset: 0x0016CD05
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x04002A07 RID: 10759
		public Faction faction;

		// Token: 0x04002A08 RID: 10760
		public MapParent mapParent;

		// Token: 0x04002A09 RID: 10761
		public string outSignal;
	}
}

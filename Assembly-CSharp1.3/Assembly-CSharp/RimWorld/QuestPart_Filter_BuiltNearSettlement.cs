using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B36 RID: 2870
	public class QuestPart_Filter_BuiltNearSettlement : QuestPart_Filter
	{
		// Token: 0x0600433C RID: 17212 RVA: 0x00167089 File Offset: 0x00165289
		protected override bool Pass(SignalArgs args)
		{
			return this.settlementFaction != null && args.GetArg<Thing>("SUBJECT").MapHeld.Parent == this.mapParent;
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001670B3 File Offset: 0x001652B3
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.settlementFaction == faction)
			{
				this.settlementFaction = null;
			}
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x001670CC File Offset: 0x001652CC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Faction>(ref this.settlementFaction, "settlementFaction", false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
		}

		// Token: 0x040028E6 RID: 10470
		public MapParent mapParent;

		// Token: 0x040028E7 RID: 10471
		public Faction settlementFaction;

		// Token: 0x040028E8 RID: 10472
		public float radius;
	}
}

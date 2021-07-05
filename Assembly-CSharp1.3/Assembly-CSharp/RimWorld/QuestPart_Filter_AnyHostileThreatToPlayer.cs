using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2E RID: 2862
	public class QuestPart_Filter_AnyHostileThreatToPlayer : QuestPart_Filter
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x001669C9 File Offset: 0x00164BC9
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.Map != null && GenHostility.AnyHostileActiveThreatToPlayer(this.mapParent.Map, this.countDormantPawnsAsHostile, true);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x001669F9 File Offset: 0x00164BF9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "parent", false);
			Scribe_Values.Look<bool>(ref this.countDormantPawnsAsHostile, "countDormantPawnsAsHostile", false, false);
		}

		// Token: 0x040028D3 RID: 10451
		public MapParent mapParent;

		// Token: 0x040028D4 RID: 10452
		public bool countDormantPawnsAsHostile;
	}
}

using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2B RID: 2859
	public class QuestPart_Filter_AnyColonistAlive : QuestPart_Filter
	{
		// Token: 0x06004316 RID: 17174 RVA: 0x001668D7 File Offset: 0x00164AD7
		protected override bool Pass(SignalArgs args)
		{
			return this.mapParent != null && this.mapParent.HasMap && this.mapParent.Map.mapPawns.ColonistCount != 0;
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x00166908 File Offset: 0x00164B08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x040028D1 RID: 10449
		public MapParent mapParent;
	}
}

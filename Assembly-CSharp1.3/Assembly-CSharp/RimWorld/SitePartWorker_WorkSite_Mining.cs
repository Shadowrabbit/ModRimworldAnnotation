using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF6 RID: 4086
	public class SitePartWorker_WorkSite_Mining : SitePartWorker_WorkSite
	{
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x0600602A RID: 24618 RVA: 0x0020CA3F File Offset: 0x0020AC3F
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowMiningCamps
				select p;
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x0600602B RID: 24619 RVA: 0x0020CA6A File Offset: 0x0020AC6A
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Miners;
			}
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x0020CA74 File Offset: 0x0020AC74
		public override bool CanSpawnOn(int tile)
		{
			Hilliness hilliness = Find.WorldGrid[tile].hilliness;
			return hilliness >= Hilliness.LargeHills && hilliness < Hilliness.Impassable;
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x0020CA9C File Offset: 0x0020AC9C
		protected override void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
			site.customLabel = sitePart.def.label.Formatted(loot.thing.Named("THING"));
		}
	}
}

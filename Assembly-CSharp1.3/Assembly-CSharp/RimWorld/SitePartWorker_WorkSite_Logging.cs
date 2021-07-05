using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF7 RID: 4087
	public class SitePartWorker_WorkSite_Logging : SitePartWorker_WorkSite
	{
		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x0600602F RID: 24623 RVA: 0x0020CAC9 File Offset: 0x0020ACC9
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowLoggingCamps
				select p;
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06006030 RID: 24624 RVA: 0x0020CAF4 File Offset: 0x0020ACF4
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Loggers;
			}
		}

		// Token: 0x06006031 RID: 24625 RVA: 0x0020CAFB File Offset: 0x0020ACFB
		public override bool CanSpawnOn(int tile)
		{
			return Find.WorldGrid[tile].biome.TreeDensity >= BiomeDefOf.Tundra.TreeDensity;
		}
	}
}

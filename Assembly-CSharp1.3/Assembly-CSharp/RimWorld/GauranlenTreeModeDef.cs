using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A6E RID: 2670
	public class GauranlenTreeModeDef : Def
	{
		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x0600401A RID: 16410 RVA: 0x0015B3DC File Offset: 0x001595DC
		public string Description
		{
			get
			{
				if (this.cachedDescription == null)
				{
					this.cachedDescription = this.description;
					PawnKindDef pawnKindDef = this.pawnKindDef;
					CompProperties_Spawner compProperties_Spawner = (pawnKindDef != null) ? pawnKindDef.race.GetCompProperties<CompProperties_Spawner>() : null;
					if (compProperties_Spawner != null)
					{
						this.cachedDescription = this.cachedDescription + "\n\n" + "DryadProducesResourcesDesc".Translate(this.pawnKindDef.Named("DRYAD"), GenLabel.ThingLabel(compProperties_Spawner.thingToSpawn, null, compProperties_Spawner.spawnCount).Named("RESOURCES"), compProperties_Spawner.spawnIntervalRange.max.ToStringTicksToPeriod(true, false, true, true).Named("DURATION")).Resolve().CapitalizeFirst();
					}
				}
				return this.cachedDescription;
			}
		}

		// Token: 0x04002454 RID: 9300
		public GauranlenTreeModeDef previousStage;

		// Token: 0x04002455 RID: 9301
		public PawnKindDef pawnKindDef;

		// Token: 0x04002456 RID: 9302
		public List<MemeDef> requiredMemes;

		// Token: 0x04002457 RID: 9303
		public Vector2 drawPosition;

		// Token: 0x04002458 RID: 9304
		public List<StatDef> displayedStats;

		// Token: 0x04002459 RID: 9305
		public List<DefHyperlink> hyperlinks;

		// Token: 0x0400245A RID: 9306
		private string cachedDescription;
	}
}

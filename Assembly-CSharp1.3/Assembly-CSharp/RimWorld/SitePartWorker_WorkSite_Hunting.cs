using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF4 RID: 4084
	public class SitePartWorker_WorkSite_Hunting : SitePartWorker_WorkSite
	{
		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06006020 RID: 24608 RVA: 0x0020C932 File Offset: 0x0020AB32
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowHuntingCamps
				select p;
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06006021 RID: 24609 RVA: 0x0020C95D File Offset: 0x0020AB5D
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Hunters;
			}
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0020C964 File Offset: 0x0020AB64
		public override bool CanSpawnOn(int tile)
		{
			return Find.WorldGrid[tile].biome.animalDensity > BiomeDefOf.Desert.animalDensity && base.CanSpawnOn(tile);
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0020C990 File Offset: 0x0020AB90
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(int tile)
		{
			IEnumerable<ThingDef> enumerable = from a in Find.WorldGrid[tile].biome.AllWildAnimals
			select a.RaceProps.leatherDef;
			float leatherWeight = 1f / (float)enumerable.Count<ThingDef>();
			foreach (ThingDef thing in enumerable)
			{
				yield return new SitePartWorker_WorkSite.CampLootThingStruct
				{
					thing = thing,
					thing2 = ThingDefOf.Pemmican,
					weight = leatherWeight
				};
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF5 RID: 4085
	public class SitePartWorker_WorkSite_Farming : SitePartWorker_WorkSite
	{
		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06006025 RID: 24613 RVA: 0x0020C9A8 File Offset: 0x0020ABA8
		public override IEnumerable<PreceptDef> DisallowedPrecepts
		{
			get
			{
				return from p in DefDatabase<PreceptDef>.AllDefs
				where p.disallowFarmingCamps
				select p;
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06006026 RID: 24614 RVA: 0x0020C9D3 File Offset: 0x0020ABD3
		public override PawnGroupKindDef WorkerGroupKind
		{
			get
			{
				return PawnGroupKindDefOf.Farmers;
			}
		}

		// Token: 0x06006027 RID: 24615 RVA: 0x0020C9DC File Offset: 0x0020ABDC
		public override bool CanSpawnOn(int tile)
		{
			return Find.WorldGrid[tile].biome.allowFarmingCamps && this.LootThings(tile).Where(delegate(SitePartWorker_WorkSite.CampLootThingStruct thing)
			{
				float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(tile);
				return seasonalTemp >= 6f && seasonalTemp <= 42f;
			}).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x06006028 RID: 24616 RVA: 0x0020CA36 File Offset: 0x0020AC36
		public override IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(int tile)
		{
			IEnumerable<ThingDef> enumerable = from t in DefDatabase<ThingDef>.AllDefsListForReading.Where(new Func<ThingDef, bool>(SitePartWorker_WorkSite_Farming.<>c.<>9.<LootThings>g__IsFoodCrop|5_0))
			select t.plant.harvestedThingDef;
			float cropWeight = 1f / (float)enumerable.Count<ThingDef>();
			foreach (ThingDef thing in enumerable)
			{
				yield return new SitePartWorker_WorkSite.CampLootThingStruct
				{
					thing = thing,
					weight = cropWeight
				};
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}

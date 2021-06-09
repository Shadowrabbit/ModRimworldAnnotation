using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001285 RID: 4741
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x0600674B RID: 26443 RVA: 0x000468A8 File Offset: 0x00044AA8
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x0600674C RID: 26444 RVA: 0x001FC7E8 File Offset: 0x001FA9E8
		public override void Generate(Map map, GenStepParams parms)
		{
			map.regionAndRoomUpdater.Enabled = false;
			float currentPlantDensity = map.wildPlantSpawner.CurrentPlantDensity;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensity, currentWholeMapNumDesiredPlants, true);
				}
			}
			map.regionAndRoomUpdater.Enabled = true;
		}

		// Token: 0x040044B2 RID: 17586
		private const float ChanceToSkip = 0.001f;
	}
}

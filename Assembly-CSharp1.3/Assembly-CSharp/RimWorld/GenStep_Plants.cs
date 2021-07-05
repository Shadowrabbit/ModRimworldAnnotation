﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA3 RID: 3235
	public class GenStep_Plants : GenStep
	{
		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06004B77 RID: 19319 RVA: 0x00190E28 File Offset: 0x0018F028
		public override int SeedPart
		{
			get
			{
				return 578415222;
			}
		}

		// Token: 0x06004B78 RID: 19320 RVA: 0x00190E30 File Offset: 0x0018F030
		public override void Generate(Map map, GenStepParams parms)
		{
			float currentPlantDensity = map.wildPlantSpawner.CurrentPlantDensity;
			float currentWholeMapNumDesiredPlants = map.wildPlantSpawner.CurrentWholeMapNumDesiredPlants;
			foreach (IntVec3 c in map.cellsInRandomOrder.GetAll())
			{
				if (!Rand.Chance(0.001f))
				{
					map.wildPlantSpawner.CheckSpawnWildPlantAt(c, currentPlantDensity, currentWholeMapNumDesiredPlants, true);
				}
			}
		}

		// Token: 0x04002DB3 RID: 11699
		private const float ChanceToSkip = 0.001f;
	}
}

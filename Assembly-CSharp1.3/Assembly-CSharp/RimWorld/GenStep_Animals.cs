using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA0 RID: 3232
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x06004B6E RID: 19310 RVA: 0x001909D0 File Offset: 0x0018EBD0
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x001909D8 File Offset: 0x0018EBD8
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.");
					return;
				}
				IntVec3 loc = RCellFinder.RandomAnimalSpawnCell_MapGen(map);
				if (!map.wildAnimalSpawner.SpawnRandomWildAnimalAt(loc))
				{
					break;
				}
			}
		}
	}
}

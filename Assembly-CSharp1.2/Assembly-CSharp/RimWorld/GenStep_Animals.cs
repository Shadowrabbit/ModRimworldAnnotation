using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001276 RID: 4726
	public class GenStep_Animals : GenStep
	{
		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x0600670C RID: 26380 RVA: 0x000466C7 File Offset: 0x000448C7
		public override int SeedPart
		{
			get
			{
				return 1298760307;
			}
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x001FAFCC File Offset: 0x001F91CC
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			while (!map.wildAnimalSpawner.AnimalEcosystemFull)
			{
				num++;
				if (num >= 10000)
				{
					Log.Error("Too many iterations.", false);
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

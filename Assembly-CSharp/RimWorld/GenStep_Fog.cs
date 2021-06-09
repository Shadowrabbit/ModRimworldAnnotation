using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001284 RID: 4740
	public class GenStep_Fog : GenStep
	{
		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06006748 RID: 26440 RVA: 0x000468A1 File Offset: 0x00044AA1
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x06006749 RID: 26441 RVA: 0x001FC790 File Offset: 0x001FA990
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("GenerateInitialFogGrid");
			map.fogGrid.SetAllFogged();
			FloodFillerFog.FloodUnfog(MapGenerator.PlayerStartSpot, map);
			List<IntVec3> rootsToUnfog = MapGenerator.rootsToUnfog;
			for (int i = 0; i < rootsToUnfog.Count; i++)
			{
				FloodFillerFog.FloodUnfog(rootsToUnfog[i], map);
			}
			DeepProfiler.End();
		}
	}
}

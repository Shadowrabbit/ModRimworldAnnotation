using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C8F RID: 3215
	public class GenStep_Fog : GenStep
	{
		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004AFE RID: 19198 RVA: 0x0018CE78 File Offset: 0x0018B078
		public override int SeedPart
		{
			get
			{
				return 1568957891;
			}
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x0018CE80 File Offset: 0x0018B080
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

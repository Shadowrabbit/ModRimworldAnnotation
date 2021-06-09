using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001282 RID: 4738
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06006743 RID: 26435 RVA: 0x00046889 File Offset: 0x00044A89
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x06006744 RID: 26436 RVA: 0x001FC738 File Offset: 0x001FA938
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		// Token: 0x040044B0 RID: 17584
		private const int MinRoomCellCount = 10;
	}
}

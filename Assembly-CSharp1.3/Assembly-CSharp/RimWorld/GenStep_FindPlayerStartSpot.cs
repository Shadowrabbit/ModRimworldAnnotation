using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA2 RID: 3234
	public class GenStep_FindPlayerStartSpot : GenStep
	{
		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06004B74 RID: 19316 RVA: 0x00190DC8 File Offset: 0x0018EFC8
		public override int SeedPart
		{
			get
			{
				return 1187186631;
			}
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x00190DD0 File Offset: 0x0018EFD0
		public override void Generate(Map map, GenStepParams parms)
		{
			DeepProfiler.Start("RebuildAllRegions");
			map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
			DeepProfiler.End();
			MapGenerator.PlayerStartSpot = CellFinderLoose.TryFindCentralCell(map, 7, 10, (IntVec3 x) => !x.Roofed(map));
		}

		// Token: 0x04002DB2 RID: 11698
		private const int MinRoomCellCount = 10;
	}
}

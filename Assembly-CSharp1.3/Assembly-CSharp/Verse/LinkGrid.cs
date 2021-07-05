using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000362 RID: 866
	public class LinkGrid
	{
		// Token: 0x0600188D RID: 6285 RVA: 0x0009138D File Offset: 0x0008F58D
		public LinkGrid(Map map)
		{
			this.map = map;
			this.linkGrid = new LinkFlags[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x000913B2 File Offset: 0x0008F5B2
		public LinkFlags LinkFlagsAt(IntVec3 c)
		{
			return this.linkGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000913CC File Offset: 0x0008F5CC
		public void Notify_LinkerCreatedOrDestroyed(Thing linker)
		{
			CellIndices cellIndices = this.map.cellIndices;
			foreach (IntVec3 c in linker.OccupiedRect())
			{
				LinkFlags linkFlags = LinkFlags.None;
				List<Thing> list = this.map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.graphicData != null)
					{
						linkFlags |= list[i].def.graphicData.linkFlags;
					}
				}
				this.linkGrid[cellIndices.CellToIndex(c)] = linkFlags;
			}
		}

		// Token: 0x040010A6 RID: 4262
		private Map map;

		// Token: 0x040010A7 RID: 4263
		private LinkFlags[] linkGrid;
	}
}

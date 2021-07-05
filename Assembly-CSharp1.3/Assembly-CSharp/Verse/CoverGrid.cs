using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001AE RID: 430
	public sealed class CoverGrid
	{
		// Token: 0x1700024B RID: 587
		public Thing this[int index]
		{
			get
			{
				return this.innerArray[index];
			}
		}

		// Token: 0x1700024C RID: 588
		public Thing this[IntVec3 c]
		{
			get
			{
				return this.innerArray[this.map.cellIndices.CellToIndex(c)];
			}
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0004129A File Offset: 0x0003F49A
		public CoverGrid(Map map)
		{
			this.map = map;
			this.innerArray = new Thing[map.cellIndices.NumGridCells];
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x000412C0 File Offset: 0x0003F4C0
		public void Register(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, null);
				}
			}
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00041320 File Offset: 0x0003F520
		public void DeRegister(Thing t)
		{
			if (t.def.Fillage == FillCategory.None)
			{
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculateCell(c, t);
				}
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00041380 File Offset: 0x0003F580
		private void RecalculateCell(IntVec3 c, Thing ignoreThing = null)
		{
			Thing thing = null;
			float num = 0.001f;
			List<Thing> list = this.map.thingGrid.ThingsListAtFast(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				if (thing2 != ignoreThing && !thing2.Destroyed && thing2.Spawned && thing2.def.fillPercent > num)
				{
					thing = thing2;
					num = thing2.def.fillPercent;
				}
			}
			this.innerArray[this.map.cellIndices.CellToIndex(c)] = thing;
		}

		// Token: 0x040009F2 RID: 2546
		private Map map;

		// Token: 0x040009F3 RID: 2547
		private Thing[] innerArray;
	}
}

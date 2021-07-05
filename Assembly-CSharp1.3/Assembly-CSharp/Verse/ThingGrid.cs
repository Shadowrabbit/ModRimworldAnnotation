using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001B7 RID: 439
	public sealed class ThingGrid
	{
		// Token: 0x06000C94 RID: 3220 RVA: 0x00042FC4 File Offset: 0x000411C4
		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00043018 File Offset: 0x00041218
		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.RegisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0004309C File Offset: 0x0004129C
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Warning(string.Concat(new object[]
				{
					t,
					" tried to register out of bounds at ",
					c,
					". Destroying."
				}));
				t.Destroy(DestroyMode.Vanish);
				return;
			}
			this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0004310C File Offset: 0x0004130C
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
			{
				return;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
				return;
			}
			CellRect cellRect = t.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					this.DeregisterInCell(t, new IntVec3(j, 0, i));
				}
			}
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0004319C File Offset: 0x0004139C
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(t + " tried to de-register out of bounds at " + c);
				return;
			}
			int num = this.map.cellIndices.CellToIndex(c);
			if (this.thingGrid[num].Contains(t))
			{
				this.thingGrid[num].Remove(t);
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000431FF File Offset: 0x000413FF
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				yield break;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			int num;
			for (int i = 0; i < list.Count; i = num + 1)
			{
				yield return list[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00043218 File Offset: 0x00041418
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.ErrorOnce("Got ThingsListAt out of bounds: " + c, 495287);
				return ThingGrid.EmptyThingList;
			}
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0004326B File Offset: 0x0004146B
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00043285 File Offset: 0x00041485
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0004328F File Offset: 0x0004148F
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0004329C File Offset: 0x0004149C
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.category == cat)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00043300 File Offset: 0x00041500
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x00043310 File Offset: 0x00041510
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			if (!c.InBounds(this.map))
			{
				return null;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x00043370 File Offset: 0x00041570
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			if (!c.InBounds(this.map))
			{
				return default(T);
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x04000A0E RID: 2574
		private Map map;

		// Token: 0x04000A0F RID: 2575
		private List<Thing>[] thingGrid;

		// Token: 0x04000A10 RID: 2576
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C63 RID: 3171
	public class ComplexRoom : IExposable
	{
		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x00187248 File Offset: 0x00185448
		public int Area
		{
			get
			{
				return this.rects.Sum((CellRect r) => r.Area);
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004A0F RID: 18959 RVA: 0x00187274 File Offset: 0x00185474
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				return this.rects.SelectMany((CellRect r) => r.Corners);
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x001872A0 File Offset: 0x001854A0
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.rects.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.rects[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004A11 RID: 18961 RVA: 0x001872B0 File Offset: 0x001854B0
		public ComplexRoom()
		{
		}

		// Token: 0x06004A12 RID: 18962 RVA: 0x001872C3 File Offset: 0x001854C3
		public ComplexRoom(List<CellRect> rects)
		{
			this.rects = rects;
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x001872E0 File Offset: 0x001854E0
		public bool IsCorner(IntVec3 position)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				if (this.rects[i].IsCorner(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x00187320 File Offset: 0x00185520
		public bool IsAdjacentTo(ComplexRoom room, int minAdjacencyScore = 1)
		{
			foreach (CellRect rect in this.rects)
			{
				foreach (CellRect other in room.rects)
				{
					if (rect.GetAdjacencyScore(other) >= minAdjacencyScore)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004A15 RID: 18965 RVA: 0x001873BC File Offset: 0x001855BC
		public bool Contains(IntVec3 position)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				if (this.rects[i].Contains(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004A16 RID: 18966 RVA: 0x001873F9 File Offset: 0x001855F9
		public void ExposeData()
		{
			Scribe_Collections.Look<CellRect>(ref this.rects, "rects", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<ComplexRoomDef>(ref this.def, "def");
		}

		// Token: 0x04002CFA RID: 11514
		public List<CellRect> rects = new List<CellRect>();

		// Token: 0x04002CFB RID: 11515
		public ComplexRoomDef def;
	}
}

using System;

namespace Verse.AI
{
	// Token: 0x02000600 RID: 1536
	public class Pathing
	{
		// Token: 0x06002C20 RID: 11296 RVA: 0x00106FEE File Offset: 0x001051EE
		public Pathing(Map map)
		{
			this.normal = new PathingContext(map, new PathGrid(map, true));
			this.fenceBlocked = new PathingContext(map, new PathGrid(map, false));
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x0010701C File Offset: 0x0010521C
		public PathingContext Normal
		{
			get
			{
				return this.normal;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002C22 RID: 11298 RVA: 0x00107024 File Offset: 0x00105224
		public PathingContext FenceBlocked
		{
			get
			{
				return this.fenceBlocked;
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x0010702C File Offset: 0x0010522C
		public PathingContext For(TraverseParms parms)
		{
			if (!parms.fenceBlocked || parms.canBashFences)
			{
				return this.normal;
			}
			return this.fenceBlocked;
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x0010704B File Offset: 0x0010524B
		public PathingContext For(Pawn pawn)
		{
			if (pawn != null && pawn.ShouldAvoidFences && (pawn.CurJob == null || !pawn.CurJob.canBashFences))
			{
				return this.fenceBlocked;
			}
			return this.normal;
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x0010707A File Offset: 0x0010527A
		public void RecalculateAllPerceivedPathCosts()
		{
			this.normal.pathGrid.RecalculateAllPerceivedPathCosts();
			this.fenceBlocked.pathGrid.RecalculateAllPerceivedPathCosts();
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x0010709C File Offset: 0x0010529C
		public void RecalculatePerceivedPathCostUnderThing(Thing thing)
		{
			if (thing.def.size == IntVec2.One)
			{
				this.RecalculatePerceivedPathCostAt(thing.Position);
				return;
			}
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					this.RecalculatePerceivedPathCostAt(c);
				}
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x00107114 File Offset: 0x00105314
		public void RecalculatePerceivedPathCostAt(IntVec3 c)
		{
			bool flag = false;
			this.normal.pathGrid.RecalculatePerceivedPathCostAt(c, ref flag);
			this.fenceBlocked.pathGrid.RecalculatePerceivedPathCostAt(c, ref flag);
		}

		// Token: 0x04001ADD RID: 6877
		private readonly PathingContext normal;

		// Token: 0x04001ADE RID: 6878
		private readonly PathingContext fenceBlocked;
	}
}

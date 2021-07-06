using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002AF RID: 687
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x06001184 RID: 4484 RVA: 0x000C29D8 File Offset: 0x000C0BD8
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!this.allowInWaterBiome && map.TileInfo.WaterCovered)
			{
				return;
			}
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					return;
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000C2A3C File Offset: 0x000C0C3C
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearWith(map.Center, (IntVec3 x) => this.CanScatterAt(x, map), map, out result, 3, 2147483647))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.", false);
			}
			return false;
		}

		// Token: 0x06001186 RID: 4486
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x06001187 RID: 4487 RVA: 0x000C2B04 File Offset: 0x000C0D04
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				return false;
			}
			if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				return false;
			}
			if (this.NearUsedSpot(loc, this.minSpacing))
			{
				return false;
			}
			if ((map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
			{
				return false;
			}
			if (this.spotMustBeStandable && !loc.Standable(map))
			{
				return false;
			}
			if (!this.allowFoggedPositions && loc.Fogged(map))
			{
				return false;
			}
			if (this.validators != null)
			{
				for (int i = 0; i < this.validators.Count; i++)
				{
					if (!this.validators[i].Allows(loc, map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000C2BDC File Offset: 0x000C0DDC
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x00012BF7 File Offset: 0x00010DF7
		protected int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
			}
			return this.count;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x000C2C24 File Offset: 0x000C0E24
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x000C2C5C File Offset: 0x000C0E5C
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x04000E28 RID: 3624
		public int count = -1;

		// Token: 0x04000E29 RID: 3625
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04000E2A RID: 3626
		public bool nearPlayerStart;

		// Token: 0x04000E2B RID: 3627
		public bool nearMapCenter;

		// Token: 0x04000E2C RID: 3628
		public float minSpacing = 10f;

		// Token: 0x04000E2D RID: 3629
		public bool spotMustBeStandable;

		// Token: 0x04000E2E RID: 3630
		public int minDistToPlayerStart;

		// Token: 0x04000E2F RID: 3631
		public int minEdgeDist;

		// Token: 0x04000E30 RID: 3632
		public int extraNoBuildEdgeDist;

		// Token: 0x04000E31 RID: 3633
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04000E32 RID: 3634
		public bool allowInWaterBiome = true;

		// Token: 0x04000E33 RID: 3635
		public bool allowFoggedPositions = true;

		// Token: 0x04000E34 RID: 3636
		public bool warnOnFail = true;

		// Token: 0x04000E35 RID: 3637
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04000E36 RID: 3638
		private const int ScatterNearPlayerRadius = 20;
	}
}

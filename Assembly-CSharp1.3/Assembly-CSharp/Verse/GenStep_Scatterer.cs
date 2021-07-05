using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x06000DD2 RID: 3538 RVA: 0x0004E1FC File Offset: 0x0004C3FC
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

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0004E260 File Offset: 0x0004C460
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => this.CanScatterAt(x, map), map, out result))
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
				Log.Warning("Scatterer " + this.ToString() + " could not find cell to generate at.");
			}
			return false;
		}

		// Token: 0x06000DD4 RID: 3540
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0004E314 File Offset: 0x0004C514
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
			if (!this.allowRoofed && loc.Roofed(map))
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

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0004E400 File Offset: 0x0004C600
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

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0004E446 File Offset: 0x0004C646
		protected int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1);
			}
			return this.count;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0004E46C File Offset: 0x0004C66C
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0004E4A4 File Offset: 0x0004C6A4
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x04000B48 RID: 2888
		public int count = -1;

		// Token: 0x04000B49 RID: 2889
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04000B4A RID: 2890
		public bool nearPlayerStart;

		// Token: 0x04000B4B RID: 2891
		public bool nearMapCenter;

		// Token: 0x04000B4C RID: 2892
		public float minSpacing = 10f;

		// Token: 0x04000B4D RID: 2893
		public bool spotMustBeStandable;

		// Token: 0x04000B4E RID: 2894
		public int minDistToPlayerStart;

		// Token: 0x04000B4F RID: 2895
		public int minEdgeDist;

		// Token: 0x04000B50 RID: 2896
		public int extraNoBuildEdgeDist;

		// Token: 0x04000B51 RID: 2897
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04000B52 RID: 2898
		public bool allowInWaterBiome = true;

		// Token: 0x04000B53 RID: 2899
		public bool allowFoggedPositions = true;

		// Token: 0x04000B54 RID: 2900
		public bool allowRoofed = true;

		// Token: 0x04000B55 RID: 2901
		public bool warnOnFail = true;

		// Token: 0x04000B56 RID: 2902
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04000B57 RID: 2903
		private const int ScatterNearPlayerRadius = 20;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020012B5 RID: 4789
	public class RiverMaker
	{
		// Token: 0x060067F4 RID: 26612 RVA: 0x0020138C File Offset: 0x001FF58C
		public RiverMaker(Vector3 center, float angle, RiverDef riverDef)
		{
			this.surfaceLevel = riverDef.widthOnMap / 2f;
			this.coordinateX = new AxisAsValueX();
			this.coordinateZ = new AxisAsValueZ();
			this.coordinateX = new Rotate(0.0, (double)(-(double)angle), 0.0, this.coordinateX);
			this.coordinateZ = new Rotate(0.0, (double)(-(double)angle), 0.0, this.coordinateZ);
			this.coordinateX = new Translate((double)(-(double)center.x), 0.0, (double)(-(double)center.z), this.coordinateX);
			this.coordinateZ = new Translate((double)(-(double)center.x), 0.0, (double)(-(double)center.z), this.coordinateZ);
			ModuleBase x = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			ModuleBase z = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			ModuleBase moduleBase = new Const(8.0);
			x = new Multiply(x, moduleBase);
			z = new Multiply(z, moduleBase);
			this.coordinateX = new Displace(this.coordinateX, x, new Const(0.0), z);
			this.coordinateZ = new Displace(this.coordinateZ, x, new Const(0.0), z);
			this.generator = this.coordinateX;
			this.shallowizer = new Perlin(0.029999999329447746, 2.0, 0.5, 3, Rand.Range(0, int.MaxValue), QualityMode.Medium);
			this.shallowizer = new Abs(this.shallowizer);
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x00201598 File Offset: 0x001FF798
		public TerrainDef TerrainAt(IntVec3 loc, bool recordForValidation = false)
		{
			float value = this.generator.GetValue(loc);
			float num = this.surfaceLevel - Mathf.Abs(value);
			if (num > 2f && this.shallowizer.GetValue(loc) > this.shallowFactor)
			{
				return TerrainDefOf.WaterMovingChestDeep;
			}
			if (num > 0f)
			{
				if (recordForValidation)
				{
					if (value < 0f)
					{
						this.lhs.Add(loc);
					}
					else
					{
						this.rhs.Add(loc);
					}
				}
				return TerrainDefOf.WaterMovingShallow;
			}
			return null;
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x00046D1E File Offset: 0x00044F1E
		public Vector3 WaterCoordinateAt(IntVec3 loc)
		{
			return new Vector3(this.coordinateX.GetValue(loc), 0f, this.coordinateZ.GetValue(loc));
		}

		// Token: 0x060067F7 RID: 26615 RVA: 0x00201618 File Offset: 0x001FF818
		public void ValidatePassage(Map map)
		{
			IntVec3 intVec = (from loc in this.lhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			IntVec3 intVec2 = (from loc in this.rhs
			where loc.InBounds(map) && loc.GetTerrain(map) == TerrainDefOf.WaterMovingShallow
			select loc).RandomElementWithFallback(IntVec3.Invalid);
			if (intVec == IntVec3.Invalid || intVec2 == IntVec3.Invalid)
			{
				Log.Error("Failed to find river edges in order to verify passability", false);
				return;
			}
			while (!map.reachability.CanReach(intVec, intVec2, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings))
			{
				if (this.shallowFactor > 1f)
				{
					Log.Error("Failed to make river shallow enough for passability", false);
					return;
				}
				this.shallowFactor += 0.1f;
				foreach (IntVec3 intVec3 in map.AllCells)
				{
					if (intVec3.GetTerrain(map) == TerrainDefOf.WaterMovingChestDeep && this.shallowizer.GetValue(intVec3) <= this.shallowFactor)
					{
						map.terrainGrid.SetTerrain(intVec3, TerrainDefOf.WaterMovingShallow);
					}
				}
			}
		}

		// Token: 0x0400453C RID: 17724
		private ModuleBase generator;

		// Token: 0x0400453D RID: 17725
		private ModuleBase coordinateX;

		// Token: 0x0400453E RID: 17726
		private ModuleBase coordinateZ;

		// Token: 0x0400453F RID: 17727
		private ModuleBase shallowizer;

		// Token: 0x04004540 RID: 17728
		private float surfaceLevel;

		// Token: 0x04004541 RID: 17729
		private float shallowFactor = 0.2f;

		// Token: 0x04004542 RID: 17730
		private List<IntVec3> lhs = new List<IntVec3>();

		// Token: 0x04004543 RID: 17731
		private List<IntVec3> rhs = new List<IntVec3>();
	}
}

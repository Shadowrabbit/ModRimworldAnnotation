using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCE RID: 3278
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004C64 RID: 19556 RVA: 0x001972B1 File Offset: 0x001954B1
		protected override float DesiredPowerOutput
		{
			get
			{
				if (this.cacheDirty)
				{
					this.RebuildCache();
				}
				if (!this.waterUsable)
				{
					return 0f;
				}
				if (this.waterDoubleUsed)
				{
					return base.DesiredPowerOutput * 0.3f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06004C65 RID: 19557 RVA: 0x001972EA File Offset: 0x001954EA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			this.RebuildCache();
			this.ForceOthersToRebuildCache(this.parent.Map);
		}

		// Token: 0x06004C66 RID: 19558 RVA: 0x0019731F File Offset: 0x0019551F
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.ForceOthersToRebuildCache(map);
		}

		// Token: 0x06004C67 RID: 19559 RVA: 0x0019732F File Offset: 0x0019552F
		private void ClearCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x06004C68 RID: 19560 RVA: 0x00197338 File Offset: 0x00195538
		private void RebuildCache()
		{
			this.waterUsable = true;
			foreach (IntVec3 c in this.WaterCells())
			{
				if (c.InBounds(this.parent.Map) && !this.parent.Map.terrainGrid.TerrainAt(c).affordances.Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					this.waterUsable = false;
					break;
				}
			}
			this.waterDoubleUsed = false;
			IEnumerable<Building> enumerable = this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator);
			foreach (IntVec3 c2 in this.WaterUseCells())
			{
				if (c2.InBounds(this.parent.Map))
				{
					foreach (Building building in enumerable)
					{
						if (building != this.parent && building.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(c2))
						{
							this.waterDoubleUsed = true;
							break;
						}
					}
				}
			}
			if (!this.waterUsable)
			{
				this.spinRate = 0f;
				return;
			}
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
			if (this.waterDoubleUsed)
			{
				this.spinRate *= 0.5f;
			}
			this.cacheDirty = false;
		}

		// Token: 0x06004C69 RID: 19561 RVA: 0x00197598 File Offset: 0x00195798
		private void ForceOthersToRebuildCache(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
			{
				building.GetComp<CompPowerPlantWater>().ClearCache();
			}
		}

		// Token: 0x06004C6A RID: 19562 RVA: 0x001975F4 File Offset: 0x001957F4
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + 0.006666667f * this.spinRate + 6.2831855f) % 6.2831855f;
			}
		}

		// Token: 0x06004C6B RID: 19563 RVA: 0x0019762E File Offset: 0x0019582E
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06004C6C RID: 19564 RVA: 0x0019764B File Offset: 0x0019584B
		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc + rot.FacingCell * 3;
			yield return loc + rot.FacingCell * 3 - perpOffset;
			yield return loc + rot.FacingCell * 3 - perpOffset * 2;
			yield return loc + rot.FacingCell * 3 + perpOffset;
			yield return loc + rot.FacingCell * 3 + perpOffset * 2;
			yield break;
		}

		// Token: 0x06004C6D RID: 19565 RVA: 0x00197662 File Offset: 0x00195862
		public CellRect WaterUseRect()
		{
			return CompPowerPlantWater.WaterUseRect(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06004C6E RID: 19566 RVA: 0x00197680 File Offset: 0x00195880
		public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
		{
			int width = rot.IsHorizontal ? 7 : 13;
			int height = rot.IsHorizontal ? 13 : 7;
			return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
		}

		// Token: 0x06004C6F RID: 19567 RVA: 0x001976C5 File Offset: 0x001958C5
		public IEnumerable<IntVec3> WaterUseCells()
		{
			return CompPowerPlantWater.WaterUseCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06004C70 RID: 19568 RVA: 0x001976E2 File Offset: 0x001958E2
		public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
		{
			foreach (IntVec3 intVec in CompPowerPlantWater.WaterUseRect(loc, rot))
			{
				yield return intVec;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004C71 RID: 19569 RVA: 0x001976F9 File Offset: 0x001958F9
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x06004C72 RID: 19570 RVA: 0x00197716 File Offset: 0x00195916
		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc - rot.FacingCell;
			yield return loc - rot.FacingCell - perpOffset;
			yield return loc - rot.FacingCell + perpOffset;
			yield return loc;
			yield return loc - perpOffset;
			yield return loc + perpOffset;
			yield return loc + rot.FacingCell;
			yield return loc + rot.FacingCell - perpOffset;
			yield return loc + rot.FacingCell + perpOffset;
			yield break;
		}

		// Token: 0x06004C73 RID: 19571 RVA: 0x00197730 File Offset: 0x00195930
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * 2.36f;
			for (int i = 0; i < 9; i++)
			{
				float num = this.spinPosition + 6.2831855f * (float)i / 9f;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.2831855f < 3.1415927f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.04054054f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Token: 0x06004C74 RID: 19572 RVA: 0x00197850 File Offset: 0x00195A50
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.waterUsable && this.waterDoubleUsed)
			{
				text += "\n" + "Watermill_WaterUsedTwice".Translate();
			}
			return text;
		}

		// Token: 0x04002E2D RID: 11821
		private float spinPosition;

		// Token: 0x04002E2E RID: 11822
		private bool cacheDirty = true;

		// Token: 0x04002E2F RID: 11823
		private bool waterUsable;

		// Token: 0x04002E30 RID: 11824
		private bool waterDoubleUsed;

		// Token: 0x04002E31 RID: 11825
		private float spinRate = 1f;

		// Token: 0x04002E32 RID: 11826
		private const float PowerFactorIfWaterDoubleUsed = 0.3f;

		// Token: 0x04002E33 RID: 11827
		private const float SpinRateFactor = 0.006666667f;

		// Token: 0x04002E34 RID: 11828
		private const float BladeOffset = 2.36f;

		// Token: 0x04002E35 RID: 11829
		private const int BladeCount = 9;

		// Token: 0x04002E36 RID: 11830
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");
	}
}

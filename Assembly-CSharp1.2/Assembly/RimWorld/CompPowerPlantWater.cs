using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E9 RID: 4841
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x060068E4 RID: 26852 RVA: 0x00047793 File Offset: 0x00045993
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

		// Token: 0x060068E5 RID: 26853 RVA: 0x000477CC File Offset: 0x000459CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			this.RebuildCache();
			this.ForceOthersToRebuildCache(this.parent.Map);
		}

		// Token: 0x060068E6 RID: 26854 RVA: 0x00047801 File Offset: 0x00045A01
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			this.ForceOthersToRebuildCache(map);
		}

		// Token: 0x060068E7 RID: 26855 RVA: 0x00047811 File Offset: 0x00045A11
		private void ClearCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x060068E8 RID: 26856 RVA: 0x00204D08 File Offset: 0x00202F08
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

		// Token: 0x060068E9 RID: 26857 RVA: 0x00204F68 File Offset: 0x00203168
		private void ForceOthersToRebuildCache(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
			{
				building.GetComp<CompPowerPlantWater>().ClearCache();
			}
		}

		// Token: 0x060068EA RID: 26858 RVA: 0x0004781A File Offset: 0x00045A1A
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + 0.006666667f * this.spinRate + 6.2831855f) % 6.2831855f;
			}
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x00047854 File Offset: 0x00045A54
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x00047871 File Offset: 0x00045A71
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

		// Token: 0x060068ED RID: 26861 RVA: 0x00047888 File Offset: 0x00045A88
		public CellRect WaterUseRect()
		{
			return CompPowerPlantWater.WaterUseRect(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060068EE RID: 26862 RVA: 0x00204FC4 File Offset: 0x002031C4
		public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
		{
			int width = rot.IsHorizontal ? 7 : 13;
			int height = rot.IsHorizontal ? 13 : 7;
			return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
		}

		// Token: 0x060068EF RID: 26863 RVA: 0x000478A5 File Offset: 0x00045AA5
		public IEnumerable<IntVec3> WaterUseCells()
		{
			return CompPowerPlantWater.WaterUseCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x000478C2 File Offset: 0x00045AC2
		public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
		{
			foreach (IntVec3 intVec in CompPowerPlantWater.WaterUseRect(loc, rot))
			{
				yield return intVec;
			}
			yield break;
			yield break;
		}

		// Token: 0x060068F1 RID: 26865 RVA: 0x000478D9 File Offset: 0x00045AD9
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x060068F2 RID: 26866 RVA: 0x000478F6 File Offset: 0x00045AF6
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

		// Token: 0x060068F3 RID: 26867 RVA: 0x0020500C File Offset: 0x0020320C
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
				matrix.SetTRS(a + Vector3.up * 0.042857144f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Token: 0x060068F4 RID: 26868 RVA: 0x0020512C File Offset: 0x0020332C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.waterUsable && this.waterDoubleUsed)
			{
				text += "\n" + "Watermill_WaterUsedTwice".Translate();
			}
			return text;
		}

		// Token: 0x040045C0 RID: 17856
		private float spinPosition;

		// Token: 0x040045C1 RID: 17857
		private bool cacheDirty = true;

		// Token: 0x040045C2 RID: 17858
		private bool waterUsable;

		// Token: 0x040045C3 RID: 17859
		private bool waterDoubleUsed;

		// Token: 0x040045C4 RID: 17860
		private float spinRate = 1f;

		// Token: 0x040045C5 RID: 17861
		private const float PowerFactorIfWaterDoubleUsed = 0.3f;

		// Token: 0x040045C6 RID: 17862
		private const float SpinRateFactor = 0.006666667f;

		// Token: 0x040045C7 RID: 17863
		private const float BladeOffset = 2.36f;

		// Token: 0x040045C8 RID: 17864
		private const int BladeCount = 9;

		// Token: 0x040045C9 RID: 17865
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");
	}
}

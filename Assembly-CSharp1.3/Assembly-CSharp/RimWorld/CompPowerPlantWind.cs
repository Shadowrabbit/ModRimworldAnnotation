using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000CCF RID: 3279
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x06004C77 RID: 19575 RVA: 0x001978C0 File Offset: 0x00195AC0
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x06004C78 RID: 19576 RVA: 0x001978C8 File Offset: 0x00195AC8
		private float PowerPercent
		{
			get
			{
				return base.PowerOutput / (-base.Props.basePowerConsumption * 1.5f);
			}
		}

		// Token: 0x06004C79 RID: 19577 RVA: 0x001978E4 File Offset: 0x00195AE4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)this.parent.def.size.z - 0.95f, 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		// Token: 0x06004C7A RID: 19578 RVA: 0x0019793E File Offset: 0x00195B3E
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06004C7B RID: 19579 RVA: 0x00197967 File Offset: 0x00195B67
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

		// Token: 0x06004C7C RID: 19580 RVA: 0x00197998 File Offset: 0x00195B98
		public override void CompTick()
		{
			base.CompTick();
			if (!base.PowerOn)
			{
				this.cachedPowerOutput = 0f;
				return;
			}
			this.ticksSinceWeatherUpdate++;
			if (this.ticksSinceWeatherUpdate >= this.updateWeatherEveryXTicks)
			{
				float num = Mathf.Min(this.parent.Map.windManager.WindSpeed, 1.5f);
				this.ticksSinceWeatherUpdate = 0;
				this.cachedPowerOutput = -(base.Props.basePowerConsumption * num);
				this.RecalculateBlockages();
				if (this.windPathBlockedCells.Count > 0)
				{
					float num2 = 0f;
					for (int i = 0; i < this.windPathBlockedCells.Count; i++)
					{
						num2 += this.cachedPowerOutput * 0.2f;
					}
					this.cachedPowerOutput -= num2;
					if (this.cachedPowerOutput < 0f)
					{
						this.cachedPowerOutput = 0f;
					}
				}
			}
			if (this.cachedPowerOutput > 0.01f)
			{
				this.spinPosition += this.PowerPercent * CompPowerPlantWind.SpinRateFactor;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				this.sustainer = SoundDefOf.WindTurbine_Ambience.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
			this.sustainer.Maintain();
			this.sustainer.externalParams["PowerOutput"] = this.PowerPercent;
		}

		// Token: 0x06004C7D RID: 19581 RVA: 0x00197B00 File Offset: 0x00195D00
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = new GenDraw.FillableBarRequest
			{
				center = this.parent.DrawPos + Vector3.up * 0.1f,
				size = CompPowerPlantWind.BarSize,
				fillPercent = this.PowerPercent,
				filledMat = CompPowerPlantWind.WindTurbineBarFilledMat,
				unfilledMat = CompPowerPlantWind.WindTurbineBarUnfilledMat,
				margin = 0.15f
			};
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			Vector3 vector = this.parent.TrueCenter();
			vector += this.parent.Rotation.FacingCell.ToVector3() * CompPowerPlantWind.VerticalBladeOffset;
			vector += this.parent.Rotation.RighthandCell.ToVector3() * CompPowerPlantWind.HorizontalBladeOffset;
			vector.y += 0.04054054f;
			float num = CompPowerPlantWind.BladeWidth * Mathf.Sin(this.spinPosition);
			if (num < 0f)
			{
				num *= -1f;
			}
			bool flag = this.spinPosition % 3.1415927f * 2f < 3.1415927f;
			Vector2 vector2 = new Vector2(num, 1f);
			Vector3 s = new Vector3(vector2.x, 1f, vector2.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
			vector.y -= 0.08108108f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

		// Token: 0x06004C7E RID: 19582 RVA: 0x00197D08 File Offset: 0x00195F08
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (this.windPathBlockedCells.Count > 0)
			{
				stringBuilder.AppendLine();
				Thing thing = null;
				if (this.windPathBlockedByThings != null)
				{
					thing = this.windPathBlockedByThings[0];
				}
				if (thing != null)
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedBy".Translate() + " " + thing.Label);
				}
				else
				{
					stringBuilder.Append("WindTurbine_WindPathIsBlockedByRoof".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004C7F RID: 19583 RVA: 0x00197DA0 File Offset: 0x00195FA0
		private void RecalculateBlockages()
		{
			if (this.windPathCells.Count == 0)
			{
				IEnumerable<IntVec3> collection = WindTurbineUtility.CalculateWindCells(this.parent.Position, this.parent.Rotation, this.parent.def.size);
				this.windPathCells.AddRange(collection);
			}
			this.windPathBlockedCells.Clear();
			this.windPathBlockedByThings.Clear();
			for (int i = 0; i < this.windPathCells.Count; i++)
			{
				IntVec3 intVec = this.windPathCells[i];
				if (this.parent.Map.roofGrid.Roofed(intVec))
				{
					this.windPathBlockedByThings.Add(null);
					this.windPathBlockedCells.Add(intVec);
				}
				else
				{
					List<Thing> list = this.parent.Map.thingGrid.ThingsListAt(intVec);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (thing.def.blockWind)
						{
							this.windPathBlockedByThings.Add(thing);
							this.windPathBlockedCells.Add(intVec);
							break;
						}
					}
				}
			}
		}

		// Token: 0x04002E37 RID: 11831
		public int updateWeatherEveryXTicks = 250;

		// Token: 0x04002E38 RID: 11832
		private int ticksSinceWeatherUpdate;

		// Token: 0x04002E39 RID: 11833
		private float cachedPowerOutput;

		// Token: 0x04002E3A RID: 11834
		private List<IntVec3> windPathCells = new List<IntVec3>();

		// Token: 0x04002E3B RID: 11835
		private List<Thing> windPathBlockedByThings = new List<Thing>();

		// Token: 0x04002E3C RID: 11836
		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		// Token: 0x04002E3D RID: 11837
		private float spinPosition;

		// Token: 0x04002E3E RID: 11838
		private Sustainer sustainer;

		// Token: 0x04002E3F RID: 11839
		private const float MaxUsableWindIntensity = 1.5f;

		// Token: 0x04002E40 RID: 11840
		[TweakValue("Graphics", 0f, 0.1f)]
		private static float SpinRateFactor = 0.035f;

		// Token: 0x04002E41 RID: 11841
		[TweakValue("Graphics", -1f, 1f)]
		private static float HorizontalBladeOffset = -0.02f;

		// Token: 0x04002E42 RID: 11842
		[TweakValue("Graphics", 0f, 1f)]
		private static float VerticalBladeOffset = 0.7f;

		// Token: 0x04002E43 RID: 11843
		[TweakValue("Graphics", 4f, 8f)]
		private static float BladeWidth = 6.6f;

		// Token: 0x04002E44 RID: 11844
		private const float PowerReductionPercentPerObstacle = 0.2f;

		// Token: 0x04002E45 RID: 11845
		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		// Token: 0x04002E46 RID: 11846
		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		// Token: 0x04002E47 RID: 11847
		private static Vector2 BarSize;

		// Token: 0x04002E48 RID: 11848
		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04002E49 RID: 11849
		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		// Token: 0x04002E4A RID: 11850
		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");
	}
}

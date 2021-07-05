using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020012ED RID: 4845
	[StaticConstructorOnStartup]
	public class CompPowerPlantWind : CompPowerPlant
	{
		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06006910 RID: 26896 RVA: 0x000479F7 File Offset: 0x00045BF7
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.cachedPowerOutput;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06006911 RID: 26897 RVA: 0x000479FF File Offset: 0x00045BFF
		private float PowerPercent
		{
			get
			{
				return base.PowerOutput / (-base.Props.basePowerConsumption * 1.5f);
			}
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x002056D0 File Offset: 0x002038D0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			CompPowerPlantWind.BarSize = new Vector2((float)this.parent.def.size.z - 0.95f, 0.14f);
			this.RecalculateBlockages();
			this.spinPosition = Rand.Range(0f, 15f);
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x00047A1A File Offset: 0x00045C1A
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x00047A43 File Offset: 0x00045C43
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceWeatherUpdate, "updateCounter", 0, false);
			Scribe_Values.Look<float>(ref this.cachedPowerOutput, "cachedPowerOutput", 0f, false);
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x0020572C File Offset: 0x0020392C
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

		// Token: 0x06006916 RID: 26902 RVA: 0x00205894 File Offset: 0x00203A94
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
			vector.y += 0.042857144f;
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
			vector.y -= 0.08571429f;
			matrix.SetTRS(vector, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(flag ? MeshPool.plane10Flip : MeshPool.plane10, matrix, CompPowerPlantWind.WindTurbineBladesMat, 0);
		}

		// Token: 0x06006917 RID: 26903 RVA: 0x00205A9C File Offset: 0x00203C9C
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

		// Token: 0x06006918 RID: 26904 RVA: 0x00205B34 File Offset: 0x00203D34
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

		// Token: 0x040045E2 RID: 17890
		public int updateWeatherEveryXTicks = 250;

		// Token: 0x040045E3 RID: 17891
		private int ticksSinceWeatherUpdate;

		// Token: 0x040045E4 RID: 17892
		private float cachedPowerOutput;

		// Token: 0x040045E5 RID: 17893
		private List<IntVec3> windPathCells = new List<IntVec3>();

		// Token: 0x040045E6 RID: 17894
		private List<Thing> windPathBlockedByThings = new List<Thing>();

		// Token: 0x040045E7 RID: 17895
		private List<IntVec3> windPathBlockedCells = new List<IntVec3>();

		// Token: 0x040045E8 RID: 17896
		private float spinPosition;

		// Token: 0x040045E9 RID: 17897
		private Sustainer sustainer;

		// Token: 0x040045EA RID: 17898
		private const float MaxUsableWindIntensity = 1.5f;

		// Token: 0x040045EB RID: 17899
		[TweakValue("Graphics", 0f, 0.1f)]
		private static float SpinRateFactor = 0.035f;

		// Token: 0x040045EC RID: 17900
		[TweakValue("Graphics", -1f, 1f)]
		private static float HorizontalBladeOffset = -0.02f;

		// Token: 0x040045ED RID: 17901
		[TweakValue("Graphics", 0f, 1f)]
		private static float VerticalBladeOffset = 0.7f;

		// Token: 0x040045EE RID: 17902
		[TweakValue("Graphics", 4f, 8f)]
		private static float BladeWidth = 6.6f;

		// Token: 0x040045EF RID: 17903
		private const float PowerReductionPercentPerObstacle = 0.2f;

		// Token: 0x040045F0 RID: 17904
		private const string TranslateWindPathIsBlockedBy = "WindTurbine_WindPathIsBlockedBy";

		// Token: 0x040045F1 RID: 17905
		private const string TranslateWindPathIsBlockedByRoof = "WindTurbine_WindPathIsBlockedByRoof";

		// Token: 0x040045F2 RID: 17906
		private static Vector2 BarSize;

		// Token: 0x040045F3 RID: 17907
		private static readonly Material WindTurbineBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x040045F4 RID: 17908
		private static readonly Material WindTurbineBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);

		// Token: 0x040045F5 RID: 17909
		private static readonly Material WindTurbineBladesMat = MaterialPool.MatFrom("Things/Building/Power/WindTurbine/WindTurbineBlades");
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CCC RID: 3276
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x00197041 File Offset: 0x00195241
		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, 1700f, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004C5C RID: 19548 RVA: 0x00197070 File Offset: 0x00195270
		private float RoofedPowerOutputFactor
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.parent.OccupiedRect())
				{
					num++;
					if (this.parent.Map.roofGrid.Roofed(c))
					{
						num2++;
					}
				}
				return (float)(num - num2) / (float)num;
			}
		}

		// Token: 0x06004C5D RID: 19549 RVA: 0x001970F4 File Offset: 0x001952F4
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.parent.DrawPos + Vector3.up * 0.1f;
			r.size = CompPowerPlantSolar.BarSize;
			r.fillPercent = base.PowerOutput / 1700f;
			r.filledMat = CompPowerPlantSolar.PowerPlantSolarBarFilledMat;
			r.unfilledMat = CompPowerPlantSolar.PowerPlantSolarBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}

		// Token: 0x04002E26 RID: 11814
		private const float FullSunPower = 1700f;

		// Token: 0x04002E27 RID: 11815
		private const float NightPower = 0f;

		// Token: 0x04002E28 RID: 11816
		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		// Token: 0x04002E29 RID: 11817
		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x04002E2A RID: 11818
		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
	}
}

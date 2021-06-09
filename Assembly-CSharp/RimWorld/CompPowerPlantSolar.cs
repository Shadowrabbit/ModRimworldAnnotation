using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E7 RID: 4839
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x060068DB RID: 26843 RVA: 0x00047727 File Offset: 0x00045927
		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, 1700f, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x060068DC RID: 26844 RVA: 0x00204B08 File Offset: 0x00202D08
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

		// Token: 0x060068DD RID: 26845 RVA: 0x00204B8C File Offset: 0x00202D8C
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

		// Token: 0x040045B9 RID: 17849
		private const float FullSunPower = 1700f;

		// Token: 0x040045BA RID: 17850
		private const float NightPower = 0f;

		// Token: 0x040045BB RID: 17851
		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		// Token: 0x040045BC RID: 17852
		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x040045BD RID: 17853
		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
	}
}

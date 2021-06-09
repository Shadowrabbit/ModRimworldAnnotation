using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016A2 RID: 5794
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x06007EB7 RID: 32439 RVA: 0x0005527B File Offset: 0x0005347B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x0025A910 File Offset: 0x00258B10
		public override void Draw()
		{
			base.Draw();
			CompPowerBattery comp = base.GetComp<CompPowerBattery>();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.DrawPos + Vector3.up * 0.1f;
			r.size = Building_Battery.BarSize;
			r.fillPercent = comp.StoredEnergy / comp.Props.storedEnergyMax;
			r.filledMat = Building_Battery.BatteryBarFilledMat;
			r.unfilledMat = Building_Battery.BatteryBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = base.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
			if (this.ticksToExplode > 0 && base.Spawned)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			}
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x0025A9DC File Offset: 0x00258BDC
		public override void Tick()
		{
			base.Tick();
			if (this.ticksToExplode > 0)
			{
				if (this.wickSustainer == null)
				{
					this.StartWickSustainer();
				}
				else
				{
					this.wickSustainer.Maintain();
				}
				this.ticksToExplode--;
				if (this.ticksToExplode == 0)
				{
					IntVec3 randomCell = this.OccupiedRect().RandomCell;
					float radius = Rand.Range(0.5f, 1f) * 3f;
					GenExplosion.DoExplosion(randomCell, base.Map, radius, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, false, null, 0f, 1, 0f, false, null, null);
					base.GetComp<CompPowerBattery>().DrawPower(400f);
				}
			}
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x0025AA9C File Offset: 0x00258C9C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x0025AB08 File Offset: 0x00258D08
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x0400526E RID: 21102
		private int ticksToExplode;

		// Token: 0x0400526F RID: 21103
		private Sustainer wickSustainer;

		// Token: 0x04005270 RID: 21104
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x04005271 RID: 21105
		private const float MinEnergyToExplode = 500f;

		// Token: 0x04005272 RID: 21106
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x04005273 RID: 21107
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x04005274 RID: 21108
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x04005275 RID: 21109
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}

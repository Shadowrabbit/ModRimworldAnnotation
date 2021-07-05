using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200106C RID: 4204
	[StaticConstructorOnStartup]
	public class Building_Battery : Building
	{
		// Token: 0x06006399 RID: 25497 RVA: 0x0021A3DB File Offset: 0x002185DB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToExplode, "ticksToExplode", 0, false);
		}

		// Token: 0x0600639A RID: 25498 RVA: 0x0021A3F8 File Offset: 0x002185F8
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

		// Token: 0x0600639B RID: 25499 RVA: 0x0021A4C4 File Offset: 0x002186C4
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

		// Token: 0x0600639C RID: 25500 RVA: 0x0021A584 File Offset: 0x00218784
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && this.ticksToExplode == 0 && dinfo.Def == DamageDefOf.Flame && Rand.Value < 0.05f && base.GetComp<CompPowerBattery>().StoredEnergy > 500f)
			{
				this.ticksToExplode = Rand.Range(70, 150);
				this.StartWickSustainer();
			}
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x0021A5F0 File Offset: 0x002187F0
		private void StartWickSustainer()
		{
			SoundInfo info = SoundInfo.InMap(this, MaintenanceType.PerTick);
			this.wickSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(info);
		}

		// Token: 0x04003862 RID: 14434
		private int ticksToExplode;

		// Token: 0x04003863 RID: 14435
		private Sustainer wickSustainer;

		// Token: 0x04003864 RID: 14436
		private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

		// Token: 0x04003865 RID: 14437
		private const float MinEnergyToExplode = 500f;

		// Token: 0x04003866 RID: 14438
		private const float EnergyToLoseWhenExplode = 400f;

		// Token: 0x04003867 RID: 14439
		private const float ExplodeChancePerDamage = 0.05f;

		// Token: 0x04003868 RID: 14440
		private static readonly Material BatteryBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f), false);

		// Token: 0x04003869 RID: 14441
		private static readonly Material BatteryBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}

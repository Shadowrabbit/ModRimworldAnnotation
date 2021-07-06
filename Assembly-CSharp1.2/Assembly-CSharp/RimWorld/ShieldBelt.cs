using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200172B RID: 5931
	[StaticConstructorOnStartup]
	public class ShieldBelt : Apparel
	{
		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x060082C3 RID: 33475 RVA: 0x00057CB1 File Offset: 0x00055EB1
		private float EnergyMax
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
			}
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x060082C4 RID: 33476 RVA: 0x00057CBF File Offset: 0x00055EBF
		private float EnergyGainPerTick
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x060082C5 RID: 33477 RVA: 0x00057CD3 File Offset: 0x00055ED3
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x060082C6 RID: 33478 RVA: 0x00057CDB File Offset: 0x00055EDB
		public ShieldState ShieldState
		{
			get
			{
				if (this.ticksToReset > 0)
				{
					return ShieldState.Resetting;
				}
				return ShieldState.Active;
			}
		}

		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x060082C7 RID: 33479 RVA: 0x0026C6A4 File Offset: 0x0026A8A4
		private bool ShouldDisplay
		{
			get
			{
				Pawn wearer = base.Wearer;
				return wearer.Spawned && !wearer.Dead && !wearer.Downed && (wearer.InAggroMentalState || wearer.Drafted || (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks);
			}
		}

		// Token: 0x060082C8 RID: 33480 RVA: 0x0026C720 File Offset: 0x0026A920
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x060082C9 RID: 33481 RVA: 0x00057CE9 File Offset: 0x00055EE9
		public override IEnumerable<Gizmo> GetWornGizmos()
		{
			foreach (Gizmo gizmo in base.GetWornGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Find.Selector.SingleSelectedThing == base.Wearer)
			{
				yield return new Gizmo_EnergyShieldStatus
				{
					shield = this
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x060082CA RID: 33482 RVA: 0x00057CF9 File Offset: 0x00055EF9
		public override float GetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x060082CB RID: 33483 RVA: 0x0026C770 File Offset: 0x0026A970
		public override void Tick()
		{
			base.Tick();
			if (base.Wearer == null)
			{
				this.energy = 0f;
				return;
			}
			if (this.ShieldState == ShieldState.Resetting)
			{
				this.ticksToReset--;
				if (this.ticksToReset <= 0)
				{
					this.Reset();
					return;
				}
			}
			else if (this.ShieldState == ShieldState.Active)
			{
				this.energy += this.EnergyGainPerTick;
				if (this.energy > this.EnergyMax)
				{
					this.energy = this.EnergyMax;
				}
			}
		}

		// Token: 0x060082CC RID: 33484 RVA: 0x0026C7F4 File Offset: 0x0026A9F4
		public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			if (this.ShieldState != ShieldState.Active)
			{
				return false;
			}
			if (dinfo.Def == DamageDefOf.EMP)
			{
				this.energy = 0f;
				this.Break();
				return false;
			}
			if (dinfo.Def.isRanged || dinfo.Def.isExplosive)
			{
				this.energy -= dinfo.Amount * this.EnergyLossPerDamage;
				if (this.energy < 0f)
				{
					this.Break();
				}
				else
				{
					this.AbsorbedDamage(dinfo);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060082CD RID: 33485 RVA: 0x00057D08 File Offset: 0x00055F08
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060082CE RID: 33486 RVA: 0x0026C884 File Offset: 0x0026AA84
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = base.Wearer.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			MoteMaker.MakeStaticMote(loc, base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				MoteMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x060082CF RID: 33487 RVA: 0x0026C974 File Offset: 0x0026AB74
		private void Break()
		{
			SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			MoteMaker.MakeStaticMote(base.Wearer.TrueCenter(), base.Wearer.Map, ThingDefOf.Mote_ExplosionFlash, 12f);
			for (int i = 0; i < 6; i++)
			{
				MoteMaker.ThrowDustPuff(base.Wearer.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.energy = 0f;
			this.ticksToReset = this.StartingTicksToReset;
		}

		// Token: 0x060082D0 RID: 33488 RVA: 0x0026CA4C File Offset: 0x0026AC4C
		private void Reset()
		{
			if (base.Wearer.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
				MoteMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.EnergyOnReset;
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x0026CAC4 File Offset: 0x0026ACC4
		public override void DrawWornExtras()
		{
			if (this.ShieldState == ShieldState.Active && this.ShouldDisplay)
			{
				float num = Mathf.Lerp(1.2f, 1.55f, this.energy);
				Vector3 vector = base.Wearer.Drawer.DrawPos;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				int num2 = Find.TickManager.TicksGame - this.lastAbsorbDamageTick;
				if (num2 < 8)
				{
					float num3 = (float)(8 - num2) / 8f * 0.05f;
					vector += this.impactAngleVect * num3;
					num -= num3;
				}
				float angle = (float)Rand.Range(0, 360);
				Vector3 s = new Vector3(num, 1f, num);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(vector, Quaternion.AngleAxis(angle, Vector3.up), s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, ShieldBelt.BubbleMat, 0);
			}
		}

		// Token: 0x060082D2 RID: 33490 RVA: 0x00057D1A File Offset: 0x00055F1A
		public override bool AllowVerbCast(IntVec3 root, Map map, LocalTargetInfo targ, Verb verb)
		{
			return !(verb is Verb_LaunchProjectile) || ReachabilityImmediate.CanReachImmediate(root, targ, map, PathEndMode.Touch, null);
		}

		// Token: 0x040054C5 RID: 21701
		private float energy;

		// Token: 0x040054C6 RID: 21702
		private int ticksToReset = -1;

		// Token: 0x040054C7 RID: 21703
		private int lastKeepDisplayTick = -9999;

		// Token: 0x040054C8 RID: 21704
		private Vector3 impactAngleVect;

		// Token: 0x040054C9 RID: 21705
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x040054CA RID: 21706
		private const float MinDrawSize = 1.2f;

		// Token: 0x040054CB RID: 21707
		private const float MaxDrawSize = 1.55f;

		// Token: 0x040054CC RID: 21708
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x040054CD RID: 21709
		private const int JitterDurationTicks = 8;

		// Token: 0x040054CE RID: 21710
		private int StartingTicksToReset = 3200;

		// Token: 0x040054CF RID: 21711
		private float EnergyOnReset = 0.2f;

		// Token: 0x040054D0 RID: 21712
		private float EnergyLossPerDamage = 0.033f;

		// Token: 0x040054D1 RID: 21713
		private int KeepDisplayingTicks = 1000;

		// Token: 0x040054D2 RID: 21714
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x040054D3 RID: 21715
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}

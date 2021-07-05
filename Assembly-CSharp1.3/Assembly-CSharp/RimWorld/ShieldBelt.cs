using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010C9 RID: 4297
	[StaticConstructorOnStartup]
	public class ShieldBelt : Apparel
	{
		// Token: 0x170011A1 RID: 4513
		// (get) Token: 0x060066C4 RID: 26308 RVA: 0x0022B729 File Offset: 0x00229929
		private float EnergyMax
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldEnergyMax, true);
			}
		}

		// Token: 0x170011A2 RID: 4514
		// (get) Token: 0x060066C5 RID: 26309 RVA: 0x0022B737 File Offset: 0x00229937
		private float EnergyGainPerTick
		{
			get
			{
				return this.GetStatValue(StatDefOf.EnergyShieldRechargeRate, true) / 60f;
			}
		}

		// Token: 0x170011A3 RID: 4515
		// (get) Token: 0x060066C6 RID: 26310 RVA: 0x0022B74B File Offset: 0x0022994B
		public float Energy
		{
			get
			{
				return this.energy;
			}
		}

		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x060066C7 RID: 26311 RVA: 0x0022B753 File Offset: 0x00229953
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

		// Token: 0x170011A5 RID: 4517
		// (get) Token: 0x060066C8 RID: 26312 RVA: 0x0022B764 File Offset: 0x00229964
		private bool ShouldDisplay
		{
			get
			{
				Pawn wearer = base.Wearer;
				return wearer.Spawned && !wearer.Dead && !wearer.Downed && (wearer.InAggroMentalState || wearer.Drafted || (wearer.Faction.HostileTo(Faction.OfPlayer) && !wearer.IsPrisoner) || Find.TickManager.TicksGame < this.lastKeepDisplayTick + this.KeepDisplayingTicks);
			}
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x0022B7E0 File Offset: 0x002299E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.energy, "energy", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksToReset, "ticksToReset", -1, false);
			Scribe_Values.Look<int>(ref this.lastKeepDisplayTick, "lastKeepDisplayTick", 0, false);
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x0022B82D File Offset: 0x00229A2D
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

		// Token: 0x060066CB RID: 26315 RVA: 0x0022B83D File Offset: 0x00229A3D
		public override float GetSpecialApparelScoreOffset()
		{
			return this.EnergyMax * this.ApparelScorePerEnergyMax;
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x0022B84C File Offset: 0x00229A4C
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

		// Token: 0x060066CD RID: 26317 RVA: 0x0022B8D0 File Offset: 0x00229AD0
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

		// Token: 0x060066CE RID: 26318 RVA: 0x0022B95E File Offset: 0x00229B5E
		public void KeepDisplaying()
		{
			this.lastKeepDisplayTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x0022B970 File Offset: 0x00229B70
		private void AbsorbedDamage(DamageInfo dinfo)
		{
			SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			this.impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
			Vector3 loc = base.Wearer.TrueCenter() + this.impactAngleVect.RotatedBy(180f) * 0.5f;
			float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
			FleckMaker.Static(loc, base.Wearer.Map, FleckDefOf.ExplosionFlash, num);
			int num2 = (int)num;
			for (int i = 0; i < num2; i++)
			{
				FleckMaker.ThrowDustPuff(loc, base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.lastAbsorbDamageTick = Find.TickManager.TicksGame;
			this.KeepDisplaying();
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x0022BA60 File Offset: 0x00229C60
		private void Break()
		{
			SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
			FleckMaker.Static(base.Wearer.TrueCenter(), base.Wearer.Map, FleckDefOf.ExplosionFlash, 12f);
			for (int i = 0; i < 6; i++)
			{
				FleckMaker.ThrowDustPuff(base.Wearer.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), base.Wearer.Map, Rand.Range(0.8f, 1.2f));
			}
			this.energy = 0f;
			this.ticksToReset = this.StartingTicksToReset;
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x0022BB34 File Offset: 0x00229D34
		private void Reset()
		{
			if (base.Wearer.Spawned)
			{
				SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(base.Wearer.Position, base.Wearer.Map, false));
				FleckMaker.ThrowLightningGlow(base.Wearer.TrueCenter(), base.Wearer.Map, 3f);
			}
			this.ticksToReset = -1;
			this.energy = this.EnergyOnReset;
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x0022BBAC File Offset: 0x00229DAC
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

		// Token: 0x060066D3 RID: 26323 RVA: 0x0022BC8F File Offset: 0x00229E8F
		public override bool AllowVerbCast(Verb verb)
		{
			return !(verb is Verb_LaunchProjectile);
		}

		// Token: 0x04003A04 RID: 14852
		private float energy;

		// Token: 0x04003A05 RID: 14853
		private int ticksToReset = -1;

		// Token: 0x04003A06 RID: 14854
		private int lastKeepDisplayTick = -9999;

		// Token: 0x04003A07 RID: 14855
		private Vector3 impactAngleVect;

		// Token: 0x04003A08 RID: 14856
		private int lastAbsorbDamageTick = -9999;

		// Token: 0x04003A09 RID: 14857
		private const float MinDrawSize = 1.2f;

		// Token: 0x04003A0A RID: 14858
		private const float MaxDrawSize = 1.55f;

		// Token: 0x04003A0B RID: 14859
		private const float MaxDamagedJitterDist = 0.05f;

		// Token: 0x04003A0C RID: 14860
		private const int JitterDurationTicks = 8;

		// Token: 0x04003A0D RID: 14861
		private int StartingTicksToReset = 3200;

		// Token: 0x04003A0E RID: 14862
		private float EnergyOnReset = 0.2f;

		// Token: 0x04003A0F RID: 14863
		private float EnergyLossPerDamage = 0.033f;

		// Token: 0x04003A10 RID: 14864
		private int KeepDisplayingTicks = 1000;

		// Token: 0x04003A11 RID: 14865
		private float ApparelScorePerEnergyMax = 0.25f;

		// Token: 0x04003A12 RID: 14866
		private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001173 RID: 4467
	[StaticConstructorOnStartup]
	public class CompProjectileInterceptor : ThingComp
	{
		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06006B3E RID: 27454 RVA: 0x0023FED6 File Offset: 0x0023E0D6
		public CompProperties_ProjectileInterceptor Props
		{
			get
			{
				return (CompProperties_ProjectileInterceptor)this.props;
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06006B3F RID: 27455 RVA: 0x0023FEE3 File Offset: 0x0023E0E3
		public bool Active
		{
			get
			{
				return !this.OnCooldown && !this.stunner.Stunned && !this.shutDown && !this.Charging;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06006B40 RID: 27456 RVA: 0x0023FF0D File Offset: 0x0023E10D
		public bool OnCooldown
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastInterceptTicks + this.Props.cooldownTicks;
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06006B41 RID: 27457 RVA: 0x0023FF2D File Offset: 0x0023E12D
		public bool Charging
		{
			get
			{
				return this.nextChargeTick >= 0 && Find.TickManager.TicksGame > this.nextChargeTick;
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06006B42 RID: 27458 RVA: 0x0023FF4C File Offset: 0x0023E14C
		public int ChargeCycleStartTick
		{
			get
			{
				if (this.nextChargeTick < 0)
				{
					return 0;
				}
				return this.nextChargeTick;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06006B43 RID: 27459 RVA: 0x0023FF5F File Offset: 0x0023E15F
		public int ChargingTicksLeft
		{
			get
			{
				if (this.nextChargeTick < 0)
				{
					return 0;
				}
				return this.nextChargeTick + this.Props.chargeDurationTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700127A RID: 4730
		// (get) Token: 0x06006B44 RID: 27460 RVA: 0x0023FF89 File Offset: 0x0023E189
		public int CooldownTicksLeft
		{
			get
			{
				if (!this.OnCooldown)
				{
					return 0;
				}
				return this.Props.cooldownTicks - (Find.TickManager.TicksGame - this.lastInterceptTicks);
			}
		}

		// Token: 0x1700127B RID: 4731
		// (get) Token: 0x06006B45 RID: 27461 RVA: 0x0023FFB2 File Offset: 0x0023E1B2
		public bool ReactivatedThisTick
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastInterceptTicks == this.Props.cooldownTicks;
			}
		}

		// Token: 0x06006B46 RID: 27462 RVA: 0x0023FFD4 File Offset: 0x0023E1D4
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.chargeIntervalTicks > 0)
			{
				this.nextChargeTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
			}
			this.stunner = new StunHandler(this.parent);
		}

		// Token: 0x06006B47 RID: 27463 RVA: 0x00240028 File Offset: 0x0023E228
		public override void PostDeSpawn(Map map)
		{
			if (this.sustainer != null)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x00240040 File Offset: 0x0023E240
		public bool CheckIntercept(Projectile projectile, Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (!ModLister.CheckRoyalty("Projectile interception"))
			{
				return false;
			}
			Vector3 vector = this.parent.Position.ToVector3Shifted();
			float num = this.Props.radius + projectile.def.projectile.SpeedTilesPerTick + 0.1f;
			if ((newExactPos.x - vector.x) * (newExactPos.x - vector.x) + (newExactPos.z - vector.z) * (newExactPos.z - vector.z) > num * num)
			{
				return false;
			}
			if (!this.Active)
			{
				return false;
			}
			if (!CompProjectileInterceptor.InterceptsProjectile(this.Props, projectile))
			{
				return false;
			}
			if ((projectile.Launcher == null || !projectile.Launcher.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			if (!this.Props.interceptOutgoingProjectiles && (new Vector2(vector.x, vector.z) - new Vector2(lastExactPos.x, lastExactPos.z)).sqrMagnitude <= this.Props.radius * this.Props.radius)
			{
				return false;
			}
			if (!GenGeo.IntersectLineCircleOutline(new Vector2(vector.x, vector.z), this.Props.radius, new Vector2(lastExactPos.x, lastExactPos.z), new Vector2(newExactPos.x, newExactPos.z)))
			{
				return false;
			}
			this.lastInterceptAngle = lastExactPos.AngleToFlat(this.parent.TrueCenter());
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = true;
			if (projectile.def.projectile.damageDef == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShield(new DamageInfo(projectile.def.projectile.damageDef, (float)projectile.def.projectile.damageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
			this.TriggerEffecter(newExactPos.ToIntVec3());
			return true;
		}

		// Token: 0x06006B49 RID: 27465 RVA: 0x00240264 File Offset: 0x0023E464
		public bool CheckBombardmentIntercept(Bombardment bombardment, Bombardment.BombardmentProjectile projectile)
		{
			if (!this.Active || !this.Props.interceptAirProjectiles)
			{
				return false;
			}
			if (!projectile.targetCell.InHorDistOf(this.parent.Position, this.Props.radius))
			{
				return false;
			}
			if ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = false;
			this.TriggerEffecter(projectile.targetCell);
			return true;
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x00240304 File Offset: 0x0023E504
		public bool BombardmentCanStartFireAt(Bombardment bombardment, IntVec3 cell)
		{
			return !this.Active || !this.Props.interceptAirProjectiles || ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles) || !cell.InHorDistOf(this.parent.Position, this.Props.radius);
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x0024037A File Offset: 0x0023E57A
		private void TriggerEffecter(IntVec3 pos)
		{
			Effecter effecter = new Effecter(this.Props.interceptEffect ?? EffecterDefOf.Interceptor_BlockedProjectile);
			effecter.Trigger(new TargetInfo(pos, this.parent.Map, false), TargetInfo.Invalid);
			effecter.Cleanup();
		}

		// Token: 0x06006B4C RID: 27468 RVA: 0x002403B8 File Offset: 0x0023E5B8
		public static bool InterceptsProjectile(CompProperties_ProjectileInterceptor props, Projectile projectile)
		{
			bool result;
			if (props.interceptGroundProjectiles)
			{
				result = !projectile.def.projectile.flyOverhead;
			}
			else
			{
				result = (props.interceptAirProjectiles && projectile.def.projectile.flyOverhead);
			}
			return result;
		}

		// Token: 0x06006B4D RID: 27469 RVA: 0x00240404 File Offset: 0x0023E604
		public override void CompTick()
		{
			if (this.ReactivatedThisTick && this.Props.reactivateEffect != null)
			{
				Effecter effecter = new Effecter(this.Props.reactivateEffect);
				effecter.Trigger(this.parent, TargetInfo.Invalid);
				effecter.Cleanup();
			}
			if (Find.TickManager.TicksGame >= this.nextChargeTick + this.Props.chargeDurationTicks)
			{
				this.nextChargeTick += this.Props.chargeIntervalTicks;
			}
			this.stunner.StunHandlerTick();
			if (!this.Props.activeSound.NullOrUndefined())
			{
				if (this.Active)
				{
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainer.Maintain();
					return;
				}
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
			}
		}

		// Token: 0x06006B4E RID: 27470 RVA: 0x00240514 File Offset: 0x0023E714
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			this.shutDown = true;
		}

		// Token: 0x06006B4F RID: 27471 RVA: 0x00240524 File Offset: 0x0023E724
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 pos = this.parent.Position.ToVector3Shifted();
			pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float currentAlpha = this.GetCurrentAlpha();
			if (currentAlpha > 0f)
			{
				Color value;
				if (this.Active || !Find.Selector.IsSelected(this.parent))
				{
					value = this.Props.color;
				}
				else
				{
					value = CompProjectileInterceptor.InactiveColor;
				}
				value.a *= currentAlpha;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(pos, Quaternion.identity, new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompProjectileInterceptor.ForceFieldMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
			float currentConeAlpha_RecentlyIntercepted = this.GetCurrentConeAlpha_RecentlyIntercepted();
			if (currentConeAlpha_RecentlyIntercepted > 0f)
			{
				Color color = this.Props.color;
				color.a *= currentConeAlpha_RecentlyIntercepted;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(pos, Quaternion.Euler(0f, this.lastInterceptAngle - 90f, 0f), new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompProjectileInterceptor.ForceFieldConeMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
		}

		// Token: 0x06006B50 RID: 27472 RVA: 0x002406D6 File Offset: 0x0023E8D6
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), this.Props.minAlpha);
		}

		// Token: 0x06006B51 RID: 27473 RVA: 0x00240710 File Offset: 0x0023E910
		private float GetCurrentAlpha_Idle()
		{
			float idlePulseSpeed = this.Props.idlePulseSpeed;
			float minIdleAlpha = this.Props.minIdleAlpha;
			if (!this.Active)
			{
				return 0f;
			}
			if (this.parent.Faction == Faction.OfPlayer && !this.debugInterceptNonHostileProjectiles)
			{
				return 0f;
			}
			if (Find.Selector.IsSelected(this.parent))
			{
				return 0f;
			}
			return Mathf.Lerp(minIdleAlpha, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * idlePulseSpeed) + 1f) / 2f);
		}

		// Token: 0x06006B52 RID: 27474 RVA: 0x002407B8 File Offset: 0x0023E9B8
		private float GetCurrentAlpha_Selected()
		{
			float num = Mathf.Max(2f, this.Props.idlePulseSpeed);
			if (!Find.Selector.IsSelected(this.parent) || this.stunner.Stunned || this.shutDown)
			{
				return 0f;
			}
			if (!this.Active)
			{
				return 0.41f;
			}
			return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * num) + 1f) / 2f);
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x00240858 File Offset: 0x0023EA58
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		// Token: 0x06006B54 RID: 27476 RVA: 0x00240890 File Offset: 0x0023EA90
		private float GetCurrentAlpha_RecentlyActivated()
		{
			if (!this.Active)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - (this.lastInterceptTicks + this.Props.cooldownTicks);
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		// Token: 0x06006B55 RID: 27477 RVA: 0x002408E4 File Offset: 0x0023EAE4
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			if (!this.drawInterceptCone)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x0024092A File Offset: 0x0023EB2A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				if (this.OnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "Dev: Reset cooldown",
						action = delegate()
						{
							this.lastInterceptTicks = Find.TickManager.TicksGame - this.Props.cooldownTicks;
						}
					};
				}
				yield return new Command_Toggle
				{
					defaultLabel = "Dev: Intercept non-hostile",
					isActive = (() => this.debugInterceptNonHostileProjectiles),
					toggleAction = delegate()
					{
						this.debugInterceptNonHostileProjectiles = !this.debugInterceptNonHostileProjectiles;
					}
				};
			}
			yield break;
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x0024093C File Offset: 0x0023EB3C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.interceptGroundProjectiles || this.Props.interceptAirProjectiles)
			{
				string value;
				if (this.Props.interceptGroundProjectiles)
				{
					value = "InterceptsProjectiles_GroundProjectiles".Translate();
				}
				else
				{
					value = "InterceptsProjectiles_AerialProjectiles".Translate();
				}
				if (this.Props.cooldownTicks > 0)
				{
					stringBuilder.Append("InterceptsProjectilesEvery".Translate(value, this.Props.cooldownTicks.ToStringTicksToPeriod(true, false, true, true)));
				}
				else
				{
					stringBuilder.Append("InterceptsProjectiles".Translate(value));
				}
			}
			if (this.OnCooldown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("CooldownTime".Translate() + ": " + this.CooldownTicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			if (this.stunner.Stunned)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("DisarmedTime".Translate() + ": " + this.stunner.StunTicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			if (this.shutDown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ShutDown".Translate());
			}
			else if (this.Props.chargeIntervalTicks > 0)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.Charging)
				{
					stringBuilder.Append("ChargingTime".Translate() + ": " + this.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true));
				}
				else
				{
					stringBuilder.Append("ChargingNext".Translate((this.ChargeCycleStartTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true), this.Props.chargeDurationTicks.ToStringTicksToPeriod(true, false, true, true), this.Props.chargeIntervalTicks.ToStringTicksToPeriod(true, false, true, true)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x00240B99 File Offset: 0x0023ED99
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(dinfo, out absorbed);
			if (dinfo.Def == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShield(dinfo);
			}
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x00240BC8 File Offset: 0x0023EDC8
		private void BreakShield(DamageInfo dinfo)
		{
			if (this.Active)
			{
				SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(this.parent));
				int num = Mathf.CeilToInt(this.Props.radius * 2f);
				CompProjectileInterceptor.<>c__DisplayClass48_0 CS$<>8__locals1;
				CS$<>8__locals1.fTheta = 6.2831855f / (float)num;
				CS$<>8__locals1.center = this.parent.TrueCenter();
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ConnectingLine(this.<BreakShield>g__PosAtIndex|48_0(i, ref CS$<>8__locals1), this.<BreakShield>g__PosAtIndex|48_0((i + 1) % num, ref CS$<>8__locals1), FleckDefOf.LineEMP, this.parent.Map, 1.5f);
				}
			}
			dinfo.SetAmount((float)this.Props.disarmedByEmpForTicks / 30f);
			this.stunner.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x00240C94 File Offset: 0x0023EE94
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInterceptTicks, "lastInterceptTicks", -999999, false);
			Scribe_Values.Look<bool>(ref this.shutDown, "shutDown", false, false);
			Scribe_Values.Look<int>(ref this.nextChargeTick, "nextChargeTick", -1, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.parent
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.Props.chargeIntervalTicks > 0 && this.nextChargeTick <= 0)
				{
					this.nextChargeTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
				}
				if (this.stunner == null)
				{
					this.stunner = new StunHandler(this.parent);
				}
			}
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x00240E04 File Offset: 0x0023F004
		[CompilerGenerated]
		private Vector3 <BreakShield>g__PosAtIndex|48_0(int index, ref CompProjectileInterceptor.<>c__DisplayClass48_0 A_2)
		{
			return new Vector3(this.Props.radius * Mathf.Cos(A_2.fTheta * (float)index) + A_2.center.x, 0f, this.Props.radius * Mathf.Sin(A_2.fTheta * (float)index) + A_2.center.z);
		}

		// Token: 0x04003BAF RID: 15279
		private int lastInterceptTicks = -999999;

		// Token: 0x04003BB0 RID: 15280
		private int nextChargeTick = -1;

		// Token: 0x04003BB1 RID: 15281
		private bool shutDown;

		// Token: 0x04003BB2 RID: 15282
		private StunHandler stunner;

		// Token: 0x04003BB3 RID: 15283
		private Sustainer sustainer;

		// Token: 0x04003BB4 RID: 15284
		private float lastInterceptAngle;

		// Token: 0x04003BB5 RID: 15285
		private bool drawInterceptCone;

		// Token: 0x04003BB6 RID: 15286
		private bool debugInterceptNonHostileProjectiles;

		// Token: 0x04003BB7 RID: 15287
		private static readonly Material ForceFieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x04003BB8 RID: 15288
		private static readonly Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

		// Token: 0x04003BB9 RID: 15289
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04003BBA RID: 15290
		private const float TextureActualRingSizeFactor = 1.1601562f;

		// Token: 0x04003BBB RID: 15291
		private static readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f);
	}
}

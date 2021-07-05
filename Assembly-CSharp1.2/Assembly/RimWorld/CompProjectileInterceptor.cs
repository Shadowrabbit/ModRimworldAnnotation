using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200180A RID: 6154
	[StaticConstructorOnStartup]
	public class CompProjectileInterceptor : ThingComp
	{
		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x0600881D RID: 34845 RVA: 0x0005B603 File Offset: 0x00059803
		public CompProperties_ProjectileInterceptor Props
		{
			get
			{
				return (CompProperties_ProjectileInterceptor)this.props;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600881E RID: 34846 RVA: 0x0005B610 File Offset: 0x00059810
		public bool Active
		{
			get
			{
				return !this.OnCooldown && !this.stunner.Stunned && !this.shutDown && !this.Charging;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600881F RID: 34847 RVA: 0x0005B63A File Offset: 0x0005983A
		public bool OnCooldown
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastInterceptTicks + this.Props.cooldownTicks;
			}
		}

		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06008820 RID: 34848 RVA: 0x0005B65A File Offset: 0x0005985A
		public bool Charging
		{
			get
			{
				return this.nextChargeTick >= 0 && Find.TickManager.TicksGame > this.nextChargeTick;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06008821 RID: 34849 RVA: 0x0005B679 File Offset: 0x00059879
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

		// Token: 0x1700153B RID: 5435
		// (get) Token: 0x06008822 RID: 34850 RVA: 0x0005B68C File Offset: 0x0005988C
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

		// Token: 0x1700153C RID: 5436
		// (get) Token: 0x06008823 RID: 34851 RVA: 0x0005B6B6 File Offset: 0x000598B6
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

		// Token: 0x1700153D RID: 5437
		// (get) Token: 0x06008824 RID: 34852 RVA: 0x0005B6DF File Offset: 0x000598DF
		public bool ReactivatedThisTick
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastInterceptTicks == this.Props.cooldownTicks;
			}
		}

		// Token: 0x06008825 RID: 34853 RVA: 0x0027D7CC File Offset: 0x0027B9CC
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.chargeIntervalTicks > 0)
			{
				this.nextChargeTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
			}
			this.stunner = new StunHandler(this.parent);
		}

		// Token: 0x06008826 RID: 34854 RVA: 0x0005B6FF File Offset: 0x000598FF
		public override void PostDeSpawn(Map map)
		{
			if (this.sustainer != null)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x06008827 RID: 34855 RVA: 0x0027D820 File Offset: 0x0027BA20
		public bool CheckIntercept(Projectile projectile, Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shields are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it.", 657212, false);
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
			if (projectile.def.projectile.damageDef == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShield(new DamageInfo(projectile.def.projectile.damageDef, (float)projectile.def.projectile.damageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
			Effecter effecter = new Effecter(this.Props.interceptEffect ?? EffecterDefOf.Interceptor_BlockedProjectile);
			effecter.Trigger(new TargetInfo(newExactPos.ToIntVec3(), this.parent.Map, false), TargetInfo.Invalid);
			effecter.Cleanup();
			return true;
		}

		// Token: 0x06008828 RID: 34856 RVA: 0x0027DA78 File Offset: 0x0027BC78
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

		// Token: 0x06008829 RID: 34857 RVA: 0x0027DAC4 File Offset: 0x0027BCC4
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

		// Token: 0x0600882A RID: 34858 RVA: 0x0005B714 File Offset: 0x00059914
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			this.shutDown = true;
		}

		// Token: 0x0600882B RID: 34859 RVA: 0x0027DBD4 File Offset: 0x0027BDD4
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

		// Token: 0x0600882C RID: 34860 RVA: 0x0005B723 File Offset: 0x00059923
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), this.Props.minAlpha);
		}

		// Token: 0x0600882D RID: 34861 RVA: 0x0027DD88 File Offset: 0x0027BF88
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

		// Token: 0x0600882E RID: 34862 RVA: 0x0027DE30 File Offset: 0x0027C030
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

		// Token: 0x0600882F RID: 34863 RVA: 0x0027DED0 File Offset: 0x0027C0D0
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		// Token: 0x06008830 RID: 34864 RVA: 0x0027DF08 File Offset: 0x0027C108
		private float GetCurrentAlpha_RecentlyActivated()
		{
			if (!this.Active)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - (this.lastInterceptTicks + this.Props.cooldownTicks);
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		// Token: 0x06008831 RID: 34865 RVA: 0x0027DF5C File Offset: 0x0027C15C
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		// Token: 0x06008832 RID: 34866 RVA: 0x0005B75C File Offset: 0x0005995C
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

		// Token: 0x06008833 RID: 34867 RVA: 0x0027DF94 File Offset: 0x0027C194
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

		// Token: 0x06008834 RID: 34868 RVA: 0x0005B76C File Offset: 0x0005996C
		public override void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(dinfo, out absorbed);
			if (dinfo.Def == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShield(dinfo);
			}
		}

		// Token: 0x06008835 RID: 34869 RVA: 0x0027E1F4 File Offset: 0x0027C3F4
		private void BreakShield(DamageInfo dinfo)
		{
			if (this.Active)
			{
				SoundDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(this.parent));
				int num = Mathf.CeilToInt(this.Props.radius * 2f);
				CompProjectileInterceptor.<>c__DisplayClass44_0 CS$<>8__locals1;
				CS$<>8__locals1.fTheta = 6.2831855f / (float)num;
				CS$<>8__locals1.center = this.parent.TrueCenter();
				for (int i = 0; i < num; i++)
				{
					MoteMaker.MakeConnectingLine(this.<BreakShield>g__PosAtIndex|44_0(i, ref CS$<>8__locals1), this.<BreakShield>g__PosAtIndex|44_0((i + 1) % num, ref CS$<>8__locals1), ThingDefOf.Mote_LineEMP, this.parent.Map, 1.5f);
				}
			}
			dinfo.SetAmount((float)this.Props.disarmedByEmpForTicks / 30f);
			this.stunner.Notify_DamageApplied(dinfo, true);
		}

		// Token: 0x06008836 RID: 34870 RVA: 0x0027E2C4 File Offset: 0x0027C4C4
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

		// Token: 0x0600883C RID: 34876 RVA: 0x0027E3E4 File Offset: 0x0027C5E4
		[CompilerGenerated]
		private Vector3 <BreakShield>g__PosAtIndex|44_0(int index, ref CompProjectileInterceptor.<>c__DisplayClass44_0 A_2)
		{
			return new Vector3(this.Props.radius * Mathf.Cos(A_2.fTheta * (float)index) + A_2.center.x, 0f, this.Props.radius * Mathf.Sin(A_2.fTheta * (float)index) + A_2.center.z);
		}

		// Token: 0x04005760 RID: 22368
		private int lastInterceptTicks = -999999;

		// Token: 0x04005761 RID: 22369
		private int nextChargeTick = -1;

		// Token: 0x04005762 RID: 22370
		private bool shutDown;

		// Token: 0x04005763 RID: 22371
		private StunHandler stunner;

		// Token: 0x04005764 RID: 22372
		private Sustainer sustainer;

		// Token: 0x04005765 RID: 22373
		private float lastInterceptAngle;

		// Token: 0x04005766 RID: 22374
		private bool debugInterceptNonHostileProjectiles;

		// Token: 0x04005767 RID: 22375
		private static readonly Material ForceFieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x04005768 RID: 22376
		private static readonly Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

		// Token: 0x04005769 RID: 22377
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400576A RID: 22378
		private const float TextureActualRingSizeFactor = 1.1601562f;

		// Token: 0x0400576B RID: 22379
		private static readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f);
	}
}

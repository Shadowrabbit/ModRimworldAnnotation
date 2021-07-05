using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001054 RID: 4180
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x060062EA RID: 25322 RVA: 0x00217F34 File Offset: 0x00216134
		public bool Active
		{
			get
			{
				return (this.powerComp == null || this.powerComp.PowerOn) && (this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated);
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x060062EB RID: 25323 RVA: 0x00217F82 File Offset: 0x00216182
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x060062EC RID: 25324 RVA: 0x00217F8F File Offset: 0x0021618F
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x060062ED RID: 25325 RVA: 0x00217F97 File Offset: 0x00216197
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x060062EE RID: 25326 RVA: 0x00217FA2 File Offset: 0x002161A2
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x060062EF RID: 25327 RVA: 0x00217FAF File Offset: 0x002161AF
		public bool IsMannable
		{
			get
			{
				return this.mannableComp != null;
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x060062F0 RID: 25328 RVA: 0x00217FBA File Offset: 0x002161BA
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060062F1 RID: 25329 RVA: 0x00217FDC File Offset: 0x002161DC
		private bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060062F2 RID: 25330 RVA: 0x00217FEE File Offset: 0x002161EE
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060062F3 RID: 25331 RVA: 0x00217FF6 File Offset: 0x002161F6
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060062F4 RID: 25332 RVA: 0x00218008 File Offset: 0x00216208
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.AttackVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x060062F5 RID: 25333 RVA: 0x00218020 File Offset: 0x00216220
		private bool CanExtractShell
		{
			get
			{
				if (!this.PlayerControlled)
				{
					return false;
				}
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				return compChangeableProjectile != null && compChangeableProjectile.Loaded;
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x060062F6 RID: 25334 RVA: 0x0021804E File Offset: 0x0021624E
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x060062F7 RID: 25335 RVA: 0x0021807E File Offset: 0x0021627E
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x060062F8 RID: 25336 RVA: 0x002180B1 File Offset: 0x002162B1
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x060062F9 RID: 25337 RVA: 0x002180D0 File Offset: 0x002162D0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.dormantComp = base.GetComp<CompCanBeDormant>();
			this.initiatableComp = base.GetComp<CompInitiatable>();
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
			if (!respawningAfterLoad)
			{
				this.top.SetRotationFromOrientation();
				this.burstCooldownTicksLeft = this.def.building.turretInitialCooldownTime.SecondsToTicks();
			}
		}

		// Token: 0x060062FA RID: 25338 RVA: 0x0021813E File Offset: 0x0021633E
		public override void PostMake()
		{
			base.PostMake();
			this.MakeGun();
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x0021814C File Offset: 0x0021634C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.ResetCurrentTarget();
			Effecter effecter = this.progressBarEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x0021816C File Offset: 0x0021636C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x002181F0 File Offset: 0x002163F0
		public override bool ClaimableBy(Faction by)
		{
			return base.ClaimableBy(by) && (this.mannableComp == null || this.mannableComp.ManningPawn == null) && (!this.Active || this.mannableComp != null) && (((this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated)) || (this.powerComp != null && !this.powerComp.PowerOn));
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x00218274 File Offset: 0x00216474
		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (this.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal < this.AttackVerb.verbProps.EffectiveMinRange(targ, this))
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal > this.AttackVerb.verbProps.range)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.forcedTarget != targ)
			{
				this.forcedTarget = targ;
				if (this.burstCooldownTicksLeft <= 0)
				{
					this.TryStartShootSomething(false);
				}
			}
			if (this.holdFire)
			{
				Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(this.def.label), this, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x00218398 File Offset: 0x00216598
		public override void Tick()
		{
			base.Tick();
			if (this.CanExtractShell && this.MannedByColonist)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
				{
					this.ExtractShell();
				}
			}
			if (this.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (this.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			if (this.Active && (this.mannableComp == null || this.mannableComp.MannedNow) && !this.stunner.Stunned && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (this.AttackVerb.state != VerbState.Bursting)
				{
					if (this.WarmingUp)
					{
						this.burstWarmupTicksLeft--;
						if (this.burstWarmupTicksLeft == 0)
						{
							this.BeginBurst();
						}
					}
					else
					{
						if (this.burstCooldownTicksLeft > 0)
						{
							this.burstCooldownTicksLeft--;
							if (this.IsMortar)
							{
								if (this.progressBarEffecter == null)
								{
									this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
								}
								this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
								MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
								mote.progress = 1f - (float)Math.Max(this.burstCooldownTicksLeft, 0) / (float)this.BurstCooldownTime().SecondsToTicks();
								mote.offsetZ = -0.8f;
							}
						}
						if (this.burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(10))
						{
							this.TryStartShootSomething(true);
						}
					}
					this.top.TurretTopTick();
					return;
				}
			}
			else
			{
				this.ResetCurrentTarget();
			}
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x00218570 File Offset: 0x00216770
		protected void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (this.progressBarEffecter != null)
			{
				this.progressBarEffecter.Cleanup();
				this.progressBarEffecter = null;
			}
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !this.AttackVerb.Available())
			{
				this.ResetCurrentTarget();
				return;
			}
			bool isValid = this.currentTargetInt.IsValid;
			if (this.forcedTarget.IsValid)
			{
				this.currentTargetInt = this.forcedTarget;
			}
			else
			{
				this.currentTargetInt = this.TryFindNewTarget();
			}
			if (!isValid && this.currentTargetInt.IsValid)
			{
				SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (!this.currentTargetInt.IsValid)
			{
				this.ResetCurrentTarget();
				return;
			}
			if (this.def.building.turretBurstWarmupTime > 0f)
			{
				this.burstWarmupTicksLeft = this.def.building.turretBurstWarmupTime.SecondsToTicks();
				return;
			}
			if (canBeginBurstImmediately)
			{
				this.BeginBurst();
				return;
			}
			this.burstWarmupTicksLeft = 1;
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x002186A4 File Offset: 0x002168A4
		protected LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.AttackVerb.verbProps.range;
			Building t;
			if (Rand.Value < 0.5f && this.AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
			{
				float num = this.AttackVerb.verbProps.EffectiveMinRange(x, this);
				float num2 = (float)x.Position.DistanceToSquared(this.Position);
				return num2 > num * num && num2 < range * range;
			}).TryRandomElement(out t))
			{
				return t;
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (!this.AttackVerb.ProjectileFliesOverhead())
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToAll;
				targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
			}
			if (this.AttackVerb.IsIncendiary())
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			if (this.IsMortar)
			{
				targetScanFlags |= TargetScanFlags.NeedNotUnderThickRoof;
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, targetScanFlags, new Predicate<Thing>(this.IsValidTarget), 0f, 9999f);
		}

		// Token: 0x06006302 RID: 25346 RVA: 0x002187A7 File Offset: 0x002169A7
		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x002187CC File Offset: 0x002169CC
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (base.Faction == Faction.OfPlayer && pawn.GuestStatus != null)
				{
					GuestStatus? guestStatus = pawn.GuestStatus;
					GuestStatus guestStatus2 = GuestStatus.Prisoner;
					if (guestStatus.GetValueOrDefault() == guestStatus2 & guestStatus != null)
					{
						return false;
					}
				}
				if (this.AttackVerb.ProjectileFliesOverhead())
				{
					RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
					if (roofDef != null && roofDef.isThickRoof)
					{
						return false;
					}
				}
				if (this.mannableComp == null)
				{
					return !GenAI.MachinesLike(base.Faction, pawn);
				}
				if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x00218888 File Offset: 0x00216A88
		protected void BeginBurst()
		{
			this.AttackVerb.TryStartCastOn(this.CurrentTarget, false, true, false);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x06006305 RID: 25349 RVA: 0x002188AB File Offset: 0x00216AAB
		protected void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x06006306 RID: 25350 RVA: 0x002188BE File Offset: 0x00216ABE
		protected float BurstCooldownTime()
		{
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				return this.def.building.turretBurstCooldownTime;
			}
			return this.AttackVerb.verbProps.defaultCooldownTime;
		}

		// Token: 0x06006307 RID: 25351 RVA: 0x002188F8 File Offset: 0x00216AF8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.AttackVerb.verbProps.minRange > 0f)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.AttackVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
			{
				stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
			}
			else if (base.Spawned && this.burstCooldownTicksLeft > 0 && this.BurstCooldownTime() > 5f)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.ToStringSecondsFromTicks());
			}
			CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile != null)
			{
				if (compChangeableProjectile.Loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate(compChangeableProjectile.LoadedShell.LabelCap, compChangeableProjectile.LoadedShell));
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06006308 RID: 25352 RVA: 0x00218A90 File Offset: 0x00216C90
		public override void Draw()
		{
			Vector3 zero = Vector3.zero;
			float recoilAngleOffset = 0f;
			if (this.IsMortar)
			{
				EquipmentUtility.Recoil(this.def.building.turretGunDef, (Verb_LaunchProjectile)this.AttackVerb, out zero, out recoilAngleOffset, this.top.CurRotation);
			}
			this.top.DrawTurret(zero, recoilAngleOffset);
			base.Draw();
		}

		// Token: 0x06006309 RID: 25353 RVA: 0x00218AF4 File Offset: 0x00216CF4
		public override void DrawExtraSelectionOverlays()
		{
			float range = this.AttackVerb.verbProps.range;
			if (range < 90f)
			{
				GenDraw.DrawRadiusRing(base.Position, range);
			}
			float num = this.AttackVerb.verbProps.EffectiveMinRange(true);
			if (num < 90f && num > 0.1f)
			{
				GenDraw.DrawRadiusRing(base.Position, num);
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5f);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)this.def.size.x * 0.5f);
			}
			if (this.forcedTarget.IsValid && (!this.forcedTarget.HasThing || this.forcedTarget.Thing.Spawned))
			{
				Vector3 vector;
				if (this.forcedTarget.HasThing)
				{
					vector = this.forcedTarget.Thing.TrueCenter();
				}
				else
				{
					vector = this.forcedTarget.Cell.ToVector3Shifted();
				}
				Vector3 a = this.TrueCenter();
				vector.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				a.y = vector.y;
				GenDraw.DrawLineBetween(a, vector, Building_TurretGun.ForcedTargetLineMat, 0.2f);
			}
		}

		// Token: 0x0600630A RID: 25354 RVA: 0x00218C28 File Offset: 0x00216E28
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanExtractShell)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				yield return new Command_Action
				{
					defaultLabel = "CommandExtractShell".Translate(),
					defaultDesc = "CommandExtractShellDesc".Translate(),
					icon = compChangeableProjectile.LoadedShell.uiIcon,
					iconAngle = compChangeableProjectile.LoadedShell.uiIconAngle,
					iconOffset = compChangeableProjectile.LoadedShell.uiIconOffset,
					iconDrawScale = GenUI.IconDrawScale(compChangeableProjectile.LoadedShell),
					action = delegate()
					{
						this.ExtractShell();
					}
				};
			}
			CompChangeableProjectile compChangeableProjectile2 = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile2 != null)
			{
				StorageSettings storeSettings = compChangeableProjectile2.GetStoreSettings();
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(storeSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (this.CanSetForcedTarget)
			{
				Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
				command_VerbTarget.defaultLabel = "CommandSetForceAttackTarget".Translate();
				command_VerbTarget.defaultDesc = "CommandSetForceAttackTargetDesc".Translate();
				command_VerbTarget.icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);
				command_VerbTarget.verb = this.AttackVerb;
				command_VerbTarget.hotKey = KeyBindingDefOf.Misc4;
				command_VerbTarget.drawRadius = false;
				if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
				{
					command_VerbTarget.Disable("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
				}
				yield return command_VerbTarget;
			}
			if (this.forcedTarget.IsValid)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandStopForceAttack".Translate();
				command_Action.defaultDesc = "CommandStopForceAttackDesc".Translate();
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true);
				command_Action.action = delegate()
				{
					this.ResetForcedTarget();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				if (!this.forcedTarget.IsValid)
				{
					command_Action.Disable("CommandStopAttackFailNotForceAttacking".Translate());
				}
				command_Action.hotKey = KeyBindingDefOf.Misc5;
				yield return command_Action;
			}
			if (this.CanToggleHoldFire)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandHoldFire".Translate(),
					defaultDesc = "CommandHoldFireDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
					hotKey = KeyBindingDefOf.Misc6,
					toggleAction = delegate()
					{
						this.holdFire = !this.holdFire;
						if (this.holdFire)
						{
							this.ResetForcedTarget();
						}
					},
					isActive = (() => this.holdFire)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600630B RID: 25355 RVA: 0x00218C38 File Offset: 0x00216E38
		private void ExtractShell()
		{
			GenPlace.TryPlaceThing(this.gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
		}

		// Token: 0x0600630C RID: 25356 RVA: 0x00218C73 File Offset: 0x00216E73
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x0600630D RID: 25357 RVA: 0x00218C97 File Offset: 0x00216E97
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x0600630E RID: 25358 RVA: 0x00218CAB File Offset: 0x00216EAB
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x0600630F RID: 25359 RVA: 0x00218CD0 File Offset: 0x00216ED0
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this;
				verb.castCompleteCallback = new Action(this.BurstComplete);
			}
		}

		// Token: 0x04003829 RID: 14377
		protected int burstCooldownTicksLeft;

		// Token: 0x0400382A RID: 14378
		protected int burstWarmupTicksLeft;

		// Token: 0x0400382B RID: 14379
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x0400382C RID: 14380
		private bool holdFire;

		// Token: 0x0400382D RID: 14381
		public Thing gun;

		// Token: 0x0400382E RID: 14382
		protected TurretTop top;

		// Token: 0x0400382F RID: 14383
		protected CompPowerTrader powerComp;

		// Token: 0x04003830 RID: 14384
		protected CompCanBeDormant dormantComp;

		// Token: 0x04003831 RID: 14385
		protected CompInitiatable initiatableComp;

		// Token: 0x04003832 RID: 14386
		protected CompMannable mannableComp;

		// Token: 0x04003833 RID: 14387
		protected Effecter progressBarEffecter;

		// Token: 0x04003834 RID: 14388
		private const int TryStartShootSomethingIntervalTicks = 10;

		// Token: 0x04003835 RID: 14389
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));
	}
}

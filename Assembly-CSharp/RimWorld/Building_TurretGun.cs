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
	// Token: 0x02001680 RID: 5760
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06007DBA RID: 32186 RVA: 0x00257D38 File Offset: 0x00255F38
		public bool Active
		{
			get
			{
				return (this.powerComp == null || this.powerComp.PowerOn) && (this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated);
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06007DBB RID: 32187 RVA: 0x00054805 File Offset: 0x00052A05
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06007DBC RID: 32188 RVA: 0x00054812 File Offset: 0x00052A12
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06007DBD RID: 32189 RVA: 0x0005481A File Offset: 0x00052A1A
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06007DBE RID: 32190 RVA: 0x00054825 File Offset: 0x00052A25
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06007DBF RID: 32191 RVA: 0x00054832 File Offset: 0x00052A32
		public bool IsMannable
		{
			get
			{
				return this.mannableComp != null;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06007DC0 RID: 32192 RVA: 0x0005483D File Offset: 0x00052A3D
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06007DC1 RID: 32193 RVA: 0x0005485F File Offset: 0x00052A5F
		private bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06007DC2 RID: 32194 RVA: 0x00054871 File Offset: 0x00052A71
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06007DC3 RID: 32195 RVA: 0x00054879 File Offset: 0x00052A79
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06007DC4 RID: 32196 RVA: 0x0005488B File Offset: 0x00052A8B
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.AttackVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06007DC5 RID: 32197 RVA: 0x00257D88 File Offset: 0x00255F88
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

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06007DC6 RID: 32198 RVA: 0x000548A2 File Offset: 0x00052AA2
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06007DC7 RID: 32199 RVA: 0x000548D2 File Offset: 0x00052AD2
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x00054905 File Offset: 0x00052B05
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x00257DB8 File Offset: 0x00255FB8
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

		// Token: 0x06007DCA RID: 32202 RVA: 0x00054924 File Offset: 0x00052B24
		public override void PostMake()
		{
			base.PostMake();
			this.MakeGun();
		}

		// Token: 0x06007DCB RID: 32203 RVA: 0x00054932 File Offset: 0x00052B32
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

		// Token: 0x06007DCC RID: 32204 RVA: 0x00257E28 File Offset: 0x00256028
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

		// Token: 0x06007DCD RID: 32205 RVA: 0x00257EAC File Offset: 0x002560AC
		public override bool ClaimableBy(Faction by)
		{
			return base.ClaimableBy(by) && (this.mannableComp == null || this.mannableComp.ManningPawn == null) && (!this.Active || this.mannableComp != null) && (((this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated)) || (this.powerComp != null && !this.powerComp.PowerOn));
		}

		// Token: 0x06007DCE RID: 32206 RVA: 0x00257F30 File Offset: 0x00256130
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

		// Token: 0x06007DCF RID: 32207 RVA: 0x00258054 File Offset: 0x00256254
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

		// Token: 0x06007DD0 RID: 32208 RVA: 0x0025822C File Offset: 0x0025642C
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

		// Token: 0x06007DD1 RID: 32209 RVA: 0x00258360 File Offset: 0x00256560
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

		// Token: 0x06007DD2 RID: 32210 RVA: 0x00054951 File Offset: 0x00052B51
		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		// Token: 0x06007DD3 RID: 32211 RVA: 0x00258464 File Offset: 0x00256664
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
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

		// Token: 0x06007DD4 RID: 32212 RVA: 0x00054975 File Offset: 0x00052B75
		protected void BeginBurst()
		{
			this.AttackVerb.TryStartCastOn(this.CurrentTarget, false, true);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x06007DD5 RID: 32213 RVA: 0x00054997 File Offset: 0x00052B97
		protected void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x06007DD6 RID: 32214 RVA: 0x000549AA File Offset: 0x00052BAA
		protected float BurstCooldownTime()
		{
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				return this.def.building.turretBurstCooldownTime;
			}
			return this.AttackVerb.verbProps.defaultCooldownTime;
		}

		// Token: 0x06007DD7 RID: 32215 RVA: 0x002584E4 File Offset: 0x002566E4
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

		// Token: 0x06007DD8 RID: 32216 RVA: 0x000549E4 File Offset: 0x00052BE4
		public override void Draw()
		{
			this.top.DrawTurret();
			base.Draw();
		}

		// Token: 0x06007DD9 RID: 32217 RVA: 0x0025867C File Offset: 0x0025687C
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
				GenDraw.DrawLineBetween(a, vector, Building_TurretGun.ForcedTargetLineMat);
			}
		}

		// Token: 0x06007DDA RID: 32218 RVA: 0x000549F7 File Offset: 0x00052BF7
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

		// Token: 0x06007DDB RID: 32219 RVA: 0x002587AC File Offset: 0x002569AC
		private void ExtractShell()
		{
			GenPlace.TryPlaceThing(this.gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, default(Rot4));
		}

		// Token: 0x06007DDC RID: 32220 RVA: 0x00054A07 File Offset: 0x00052C07
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x06007DDD RID: 32221 RVA: 0x00054A2B File Offset: 0x00052C2B
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x06007DDE RID: 32222 RVA: 0x00054A3F File Offset: 0x00052C3F
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x06007DDF RID: 32223 RVA: 0x002587E8 File Offset: 0x002569E8
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

		// Token: 0x040051F6 RID: 20982
		protected int burstCooldownTicksLeft;

		// Token: 0x040051F7 RID: 20983
		protected int burstWarmupTicksLeft;

		// Token: 0x040051F8 RID: 20984
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x040051F9 RID: 20985
		private bool holdFire;

		// Token: 0x040051FA RID: 20986
		public Thing gun;

		// Token: 0x040051FB RID: 20987
		protected TurretTop top;

		// Token: 0x040051FC RID: 20988
		protected CompPowerTrader powerComp;

		// Token: 0x040051FD RID: 20989
		protected CompCanBeDormant dormantComp;

		// Token: 0x040051FE RID: 20990
		protected CompInitiatable initiatableComp;

		// Token: 0x040051FF RID: 20991
		protected CompMannable mannableComp;

		// Token: 0x04005200 RID: 20992
		protected Effecter progressBarEffecter;

		// Token: 0x04005201 RID: 20993
		private const int TryStartShootSomethingIntervalTicks = 10;

		// Token: 0x04005202 RID: 20994
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020008A8 RID: 2216
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060036ED RID: 14061 RVA: 0x0002A8E1 File Offset: 0x00028AE1
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060036EE RID: 14062 RVA: 0x0002A8EE File Offset: 0x00028AEE
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060036EF RID: 14063 RVA: 0x0002A900 File Offset: 0x00028B00
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060036F0 RID: 14064 RVA: 0x0002A90D File Offset: 0x00028B0D
		public CompReloadable ReloadableCompSource
		{
			get
			{
				return this.DirectOwner as CompReloadable;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060036F1 RID: 14065 RVA: 0x0002A91A File Offset: 0x00028B1A
		public ThingWithComps EquipmentSource
		{
			get
			{
				if (this.EquipmentCompSource != null)
				{
					return this.EquipmentCompSource.parent;
				}
				if (this.ReloadableCompSource != null)
				{
					return this.ReloadableCompSource.parent;
				}
				return null;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060036F2 RID: 14066 RVA: 0x0002A945 File Offset: 0x00028B45
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060036F3 RID: 14067 RVA: 0x0002A952 File Offset: 0x00028B52
		public Hediff HediffSource
		{
			get
			{
				if (this.HediffCompSource == null)
				{
					return null;
				}
				return this.HediffCompSource.parent;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060036F4 RID: 14068 RVA: 0x0002A969 File Offset: 0x00028B69
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060036F5 RID: 14069 RVA: 0x0002A976 File Offset: 0x00028B76
		public TerrainDef TerrainDefSource
		{
			get
			{
				if (this.TerrainSource == null)
				{
					return null;
				}
				return this.TerrainSource.def;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060036F6 RID: 14070 RVA: 0x0002A98D File Offset: 0x00028B8D
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060036F7 RID: 14071 RVA: 0x0002A995 File Offset: 0x00028B95
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060036F8 RID: 14072 RVA: 0x000187F7 File Offset: 0x000169F7
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060036F9 RID: 14073 RVA: 0x0002A9A2 File Offset: 0x00028BA2
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x0002A9B2 File Offset: 0x00028BB2
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060036FB RID: 14075 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x0002A9BF File Offset: 0x00028BBF
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x0002A9C7 File Offset: 0x00028BC7
		public LocalTargetInfo CurrentDestination
		{
			get
			{
				return this.currentDestination;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060036FE RID: 14078 RVA: 0x0002A9CF File Offset: 0x00028BCF
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060036FF RID: 14079 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06003701 RID: 14081 RVA: 0x0015EB78 File Offset: 0x0015CD78
		public virtual Texture2D UIIcon
		{
			get
			{
				if (this.verbProps.commandIcon != null)
				{
					if (this.commandIconCached == null)
					{
						this.commandIconCached = ContentFinder<Texture2D>.Get(this.verbProps.commandIcon, true);
					}
					return this.commandIconCached;
				}
				if (this.EquipmentSource != null)
				{
					return this.EquipmentSource.def.uiIcon;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x0002A9DC File Offset: 0x00028BDC
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x06003703 RID: 14083 RVA: 0x0002A9E7 File Offset: 0x00028BE7
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x0002A9F4 File Offset: 0x00028BF4
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x06003705 RID: 14085 RVA: 0x0002A9FF File Offset: 0x00028BFF
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x06003706 RID: 14086 RVA: 0x0015EBDC File Offset: 0x0015CDDC
		public Stance_Warmup WarmupStance
		{
			get
			{
				if (this.CasterPawn == null || !this.CasterPawn.Spawned)
				{
					return null;
				}
				Stance_Warmup stance_Warmup;
				if ((stance_Warmup = (this.CasterPawn.stances.curStance as Stance_Warmup)) == null || stance_Warmup.verb != this)
				{
					return null;
				}
				return stance_Warmup;
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x0002AA0A File Offset: 0x00028C0A
		public int WarmupTicksLeft
		{
			get
			{
				if (this.WarmupStance == null)
				{
					return 0;
				}
				return this.WarmupStance.ticksLeft;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x06003708 RID: 14088 RVA: 0x0002AA21 File Offset: 0x00028C21
		public float WarmupProgress
		{
			get
			{
				return 1f - this.WarmupTicksLeft.TicksToSeconds() / this.verbProps.warmupTime;
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x06003709 RID: 14089 RVA: 0x0002AA40 File Offset: 0x00028C40
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x0002AA4D File Offset: 0x00028C4D
		protected virtual float EffectiveRange
		{
			get
			{
				return this.verbProps.range;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0002AA5A File Offset: 0x00028C5A
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x0015EC28 File Offset: 0x0015CE28
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0002AA8D File Offset: 0x00028C8D
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0002AA9F File Offset: 0x00028C9F
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x0002AAD1 File Offset: 0x00028CD1
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x0002AAE9 File Offset: 0x00028CE9
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns);
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x0015ECC4 File Offset: 0x0015CEC4
		public virtual bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).", false);
				return false;
			}
			if (!this.caster.Spawned)
			{
				return false;
			}
			if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				return false;
			}
			if (this.CausesTimeSlowdown(castTarg))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			this.surpriseAttack = surpriseAttack;
			this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
			this.currentTarget = castTarg;
			this.currentDestination = destTarg;
			if (this.CasterIsPawn && this.verbProps.warmupTime > 0f)
			{
				ShootLine newShootLine;
				if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine))
				{
					return false;
				}
				this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
				float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true);
				int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
				this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
			}
			else
			{
				this.WarmupComplete();
			}
			return true;
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x0015EDE8 File Offset: 0x0015CFE8
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
			if (this.CasterIsPawn && this.currentTarget.HasThing)
			{
				Pawn pawn = this.currentTarget.Thing as Pawn;
				if (pawn != null && pawn.IsColonistPlayerControlled)
				{
					this.CasterPawn.records.AccumulateStoryEvent(StoryEventDefOf.AttackedPlayer);
				}
			}
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0002AAF9 File Offset: 0x00028CF9
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				if (!this.caster.Spawned)
				{
					this.Reset();
					return;
				}
				this.ticksToNextBurstShot--;
				if (this.ticksToNextBurstShot <= 0)
				{
					this.TryCastNextBurstShot();
				}
			}
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x0015EE54 File Offset: 0x0015D054
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			ThingWithComps equipmentSource = this.EquipmentSource;
			CompReloadable compReloadable = (equipmentSource != null) ? equipmentSource.GetComp<CompReloadable>() : null;
			return compReloadable == null || compReloadable.CanBeUsed;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x0015EEB8 File Offset: 0x0015D0B8
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					MoteMaker.MakeStaticMote(this.caster.Position, this.caster.Map, ThingDefOf.Mote_ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.Map, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					if (this.CasterPawn.thinker != null)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.TerrainDefSource != null)
					{
						this.CasterPawn.meleeVerbs.Notify_UsedTerrainBasedVerb();
					}
					if (this.CasterPawn.health != null)
					{
						this.CasterPawn.health.Notify_UsedVerb(this, localTargetInfo);
					}
					if (this.EquipmentSource != null)
					{
						this.EquipmentSource.Notify_UsedWeapon(this.CasterPawn);
					}
					if (!this.CasterPawn.Spawned)
					{
						this.Reset();
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.verbProps.ticksBetweenBurstShots;
				if (this.CasterIsPawn && !this.verbProps.nonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.ticksBetweenBurstShots + 1, this.currentTarget, this));
					return;
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn && !this.verbProps.nonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0015F154 File Offset: 0x0015D354
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.verbProps.IsMeleeAttack)
			{
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
				job.playerForced = true;
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					job.killIncappedTarget = pawn.Downed;
				}
				this.CasterPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				return;
			}
			float num = this.verbProps.EffectiveMinRange(target, this.CasterPawn);
			if ((float)this.CasterPawn.Position.DistanceToSquared(target.Cell) < num * num && this.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
			{
				Messages.Message("MessageCantShootInMelee".Translate(), this.CasterPawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job2 = JobMaker.MakeJob(this.verbProps.ai_IsWeapon ? JobDefOf.AttackStatic : JobDefOf.UseVerbOnThing);
			job2.verbToUse = this;
			job2.targetA = target;
			job2.endIfCantShootInMelee = true;
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
		}

		// Token: 0x06003718 RID: 14104
		protected abstract bool TryCastShot();

		// Token: 0x06003719 RID: 14105 RVA: 0x0002AB35 File Offset: 0x00028D35
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0002AB3D File Offset: 0x00028D3D
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x0015F268 File Offset: 0x0015D468
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			}
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0002AB7A File Offset: 0x00028D7A
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0015F2DC File Offset: 0x0015D4DC
		private bool CausesTimeSlowdown(LocalTargetInfo castTarg)
		{
			if (!this.verbProps.CausesTimeSlowdown)
			{
				return false;
			}
			if (!castTarg.HasThing)
			{
				return false;
			}
			Thing thing = castTarg.Thing;
			if (thing.def.category != ThingCategory.Pawn && (thing.def.building == null || !thing.def.building.IsTurret))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			bool flag = pawn != null && pawn.Downed;
			return (thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer)) || (this.caster.Faction == Faction.OfPlayer && thing.HostileTo(Faction.OfPlayer) && !flag);
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0002AB84 File Offset: 0x00028D84
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0015F394 File Offset: 0x0015D594
		public virtual bool ValidateTarget(LocalTargetInfo target)
		{
			Pawn p;
			return !this.CasterIsPawn || (p = (target.Thing as Pawn)) == null || (!p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.HomeFaction, null) && !p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.MiniFaction, null));
		}

		// Token: 0x06003720 RID: 14112 RVA: 0x0002ABC4 File Offset: 0x00028DC4
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				this.DrawHighlightFieldRadiusAroundTarget(target);
			}
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x0015F3E8 File Offset: 0x0015D5E8
		protected void DrawHighlightFieldRadiusAroundTarget(LocalTargetInfo target)
		{
			bool flag;
			float num = this.HighlightFieldRadiusAroundTarget(out flag);
			ShootLine shootLine;
			if (num > 0.2f && this.TryFindShootLineFromTo(this.caster.Position, target, out shootLine))
			{
				if (flag)
				{
					GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num);
					return;
				}
				GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
				where x.InBounds(Find.CurrentMap)
				select x).ToList<IntVec3>());
			}
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x0015F46C File Offset: 0x0015D66C
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0015F4B4 File Offset: 0x0015D6B4
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return !this.ApparelPreventsShooting(root, targ) && this.TryFindShootLineFromTo(root, targ, out shootLine);
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x0015F4FC File Offset: 0x0015D6FC
		public bool ApparelPreventsShooting(IntVec3 root, LocalTargetInfo targ)
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(root, this.caster.Map, targ, this))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0015F560 File Offset: 0x0015D760
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine)
		{
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				return false;
			}
			if (this.verbProps.IsMeleeAttack || this.EffectiveRange <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			CellRect cellRect = targ.HasThing ? targ.Thing.OccupiedRect() : CellRect.SingleCell(targ.Cell);
			float num = this.verbProps.EffectiveMinRange(targ, this.caster);
			float num2 = cellRect.ClosestDistSquaredTo(root);
			if (num2 > this.EffectiveRange * this.EffectiveRange || num2 < num * num)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return false;
			}
			if (!this.verbProps.requireLineOfSight)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return true;
			}
			if (this.CasterIsPawn)
			{
				IntVec3 dest;
				if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
				{
					resultingLine = new ShootLine(root, dest);
					return true;
				}
				ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
				for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
				{
					IntVec3 intVec = Verb.tempLeanShootSources[i];
					if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
					{
						resultingLine = new ShootLine(intVec, dest);
						return true;
					}
				}
			}
			else
			{
				foreach (IntVec3 intVec2 in this.caster.OccupiedRect())
				{
					IntVec3 dest;
					if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
					{
						resultingLine = new ShootLine(intVec2, dest);
						return true;
					}
				}
			}
			resultingLine = new ShootLine(root, targ.Cell);
			return false;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0015F770 File Offset: 0x0015D970
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0015F840 File Offset: 0x0015DA40
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				return false;
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x0015F8CC File Offset: 0x0015DACC
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.HediffCompSource != null)
			{
				text = this.HediffCompSource.Def.label;
			}
			else if (this.EquipmentSource != null)
			{
				text = this.EquipmentSource.def.label;
			}
			else if (this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool) != null)
			{
				text = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool).defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.loadID;
			}
			return base.GetType().ToString() + "(" + text + ")";
		}

		// Token: 0x04002661 RID: 9825
		public VerbProperties verbProps;

		// Token: 0x04002662 RID: 9826
		public VerbTracker verbTracker;

		// Token: 0x04002663 RID: 9827
		public ManeuverDef maneuver;

		// Token: 0x04002664 RID: 9828
		public Tool tool;

		// Token: 0x04002665 RID: 9829
		public Thing caster;

		// Token: 0x04002666 RID: 9830
		public string loadID;

		// Token: 0x04002667 RID: 9831
		public VerbState state;

		// Token: 0x04002668 RID: 9832
		protected LocalTargetInfo currentTarget;

		// Token: 0x04002669 RID: 9833
		protected LocalTargetInfo currentDestination;

		// Token: 0x0400266A RID: 9834
		protected int burstShotsLeft;

		// Token: 0x0400266B RID: 9835
		protected int ticksToNextBurstShot;

		// Token: 0x0400266C RID: 9836
		protected bool surpriseAttack;

		// Token: 0x0400266D RID: 9837
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x0400266E RID: 9838
		public Action castCompleteCallback;

		// Token: 0x0400266F RID: 9839
		private Texture2D commandIconCached;

		// Token: 0x04002670 RID: 9840
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04002671 RID: 9841
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}

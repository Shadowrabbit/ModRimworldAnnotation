using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004F5 RID: 1269
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002642 RID: 9794 RVA: 0x000EDC06 File Offset: 0x000EBE06
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x000EDC13 File Offset: 0x000EBE13
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002644 RID: 9796 RVA: 0x000EDC25 File Offset: 0x000EBE25
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x000EDC32 File Offset: 0x000EBE32
		public CompReloadable ReloadableCompSource
		{
			get
			{
				return this.DirectOwner as CompReloadable;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x000EDC3F File Offset: 0x000EBE3F
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

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000EDC6A File Offset: 0x000EBE6A
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x000EDC77 File Offset: 0x000EBE77
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

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x000EDC8E File Offset: 0x000EBE8E
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x000EDC9B File Offset: 0x000EBE9B
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

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x000EDCB2 File Offset: 0x000EBEB2
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x000EDCBA File Offset: 0x000EBEBA
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x0600264D RID: 9805 RVA: 0x00072AAA File Offset: 0x00070CAA
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x000EDCC7 File Offset: 0x000EBEC7
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x000EDCD7 File Offset: 0x000EBED7
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002651 RID: 9809 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x000EDCE4 File Offset: 0x000EBEE4
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x000EDCEC File Offset: 0x000EBEEC
		public LocalTargetInfo CurrentDestination
		{
			get
			{
				return this.currentDestination;
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002654 RID: 9812 RVA: 0x000EDCF4 File Offset: 0x000EBEF4
		public int LastShotTick
		{
			get
			{
				return this.lastShotTick;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002655 RID: 9813 RVA: 0x000EDCFC File Offset: 0x000EBEFC
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002656 RID: 9814 RVA: 0x00002688 File Offset: 0x00000888
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002657 RID: 9815 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002658 RID: 9816 RVA: 0x000EDD0C File Offset: 0x000EBF0C
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

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x000EDD70 File Offset: 0x000EBF70
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x000EDD7B File Offset: 0x000EBF7B
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x000EDD88 File Offset: 0x000EBF88
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x000EDD93 File Offset: 0x000EBF93
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x000EDDA0 File Offset: 0x000EBFA0
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

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x0600265E RID: 9822 RVA: 0x000EDDE9 File Offset: 0x000EBFE9
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

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x0600265F RID: 9823 RVA: 0x000EDE00 File Offset: 0x000EC000
		public float WarmupProgress
		{
			get
			{
				return 1f - this.WarmupTicksLeft.TicksToSeconds() / this.verbProps.warmupTime;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x000EDE1F File Offset: 0x000EC01F
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06002661 RID: 9825 RVA: 0x000EDE2C File Offset: 0x000EC02C
		protected virtual float EffectiveRange
		{
			get
			{
				return this.verbProps.range;
			}
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000EDE39 File Offset: 0x000EC039
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000EDE6C File Offset: 0x000EC06C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<int>(ref this.lastShotTick, "lastShotTick", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000EDF29 File Offset: 0x000EC129
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000EDF3B File Offset: 0x000EC13B
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000EDF6D File Offset: 0x000EC16D
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000EDF85 File Offset: 0x000EC185
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000EDF98 File Offset: 0x000EC198
		public virtual bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).");
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
			this.preventFriendlyFire = preventFriendlyFire;
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
				if (this.verbProps.stunTargetOnCastStart && castTarg.Pawn != null)
				{
					castTarg.Pawn.stances.stunner.StunFor(ticks, null, false, true);
				}
			}
			else
			{
				this.WarmupComplete();
			}
			return true;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000EE0F6 File Offset: 0x000EC2F6
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000EE114 File Offset: 0x000EC314
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				Pawn pawn;
				if (!this.caster.Spawned || ((pawn = (this.caster as Pawn)) != null && pawn.stances.stunner.Stunned))
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

		// Token: 0x0600266C RID: 9836 RVA: 0x000EE17C File Offset: 0x000EC37C
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
			string text;
			return (compReloadable == null || compReloadable.CanBeUsed) && (!this.CasterIsPawn || this.EquipmentSource == null || !EquipmentUtility.RolePreventsFromUsing(this.CasterPawn, this.EquipmentSource, out text));
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000EE208 File Offset: 0x000EC408
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					FleckMaker.Static(this.caster.Position, this.caster.Map, FleckDefOf.ShotFlash, this.verbProps.muzzleFlashScale);
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

		// Token: 0x0600266E RID: 9838 RVA: 0x000EE4A4 File Offset: 0x000EC6A4
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
				this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
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
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x0600266F RID: 9839
		protected abstract bool TryCastShot();

		// Token: 0x06002670 RID: 9840 RVA: 0x000EE5C3 File Offset: 0x000EC7C3
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000EE5CC File Offset: 0x000EC7CC
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
			this.preventFriendlyFire = false;
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000EE61C File Offset: 0x000EC81C
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

		// Token: 0x06002673 RID: 9843 RVA: 0x000EE690 File Offset: 0x000EC890
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000EE69C File Offset: 0x000EC89C
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

		// Token: 0x06002675 RID: 9845 RVA: 0x000EE754 File Offset: 0x000EC954
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000EE794 File Offset: 0x000EC994
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn p;
			Pawn victim;
			Pawn pawn;
			return (!this.CasterIsPawn || (p = (target.Thing as Pawn)) == null || (!p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.HomeFaction, null) && !p.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.MiniFaction, null))) && (!this.CasterIsPawn || (victim = (target.Thing as Pawn)) == null || !HistoryEventUtility.IsKillingInnocentAnimal(this.CasterPawn, victim) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo")) && (!this.CasterIsPawn || (pawn = (target.Thing as Pawn)) == null || this.CasterPawn.Ideo == null || !this.CasterPawn.Ideo.IsVeneratedAnimal(pawn) || new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"));
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000EE894 File Offset: 0x000ECA94
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				this.DrawHighlightFieldRadiusAroundTarget(target);
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000EE8C4 File Offset: 0x000ECAC4
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

		// Token: 0x06002679 RID: 9849 RVA: 0x000EE948 File Offset: 0x000ECB48
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

		// Token: 0x0600267A RID: 9850 RVA: 0x000EE990 File Offset: 0x000ECB90
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return !this.ApparelPreventsShooting() && this.TryFindShootLineFromTo(root, targ, out shootLine);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000EE9D5 File Offset: 0x000ECBD5
		public bool ApparelPreventsShooting()
		{
			return this.FirstApparelPreventingShooting() != null;
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000EE9E0 File Offset: 0x000ECBE0
		public Apparel FirstApparelPreventingShooting()
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(this))
					{
						return wornApparel[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000EEA3C File Offset: 0x000ECC3C
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

		// Token: 0x0600267E RID: 9854 RVA: 0x000EEC4C File Offset: 0x000ECE4C
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

		// Token: 0x0600267F RID: 9855 RVA: 0x000EED1C File Offset: 0x000ECF1C
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

		// Token: 0x06002680 RID: 9856 RVA: 0x000EEDA8 File Offset: 0x000ECFA8
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

		// Token: 0x04001813 RID: 6163
		public VerbProperties verbProps;

		// Token: 0x04001814 RID: 6164
		public VerbTracker verbTracker;

		// Token: 0x04001815 RID: 6165
		public ManeuverDef maneuver;

		// Token: 0x04001816 RID: 6166
		public Tool tool;

		// Token: 0x04001817 RID: 6167
		public Thing caster;

		// Token: 0x04001818 RID: 6168
		public string loadID;

		// Token: 0x04001819 RID: 6169
		public VerbState state;

		// Token: 0x0400181A RID: 6170
		protected LocalTargetInfo currentTarget;

		// Token: 0x0400181B RID: 6171
		protected LocalTargetInfo currentDestination;

		// Token: 0x0400181C RID: 6172
		protected int burstShotsLeft;

		// Token: 0x0400181D RID: 6173
		protected int ticksToNextBurstShot;

		// Token: 0x0400181E RID: 6174
		protected int lastShotTick = -999999;

		// Token: 0x0400181F RID: 6175
		protected bool surpriseAttack;

		// Token: 0x04001820 RID: 6176
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x04001821 RID: 6177
		protected bool preventFriendlyFire;

		// Token: 0x04001822 RID: 6178
		public Action castCompleteCallback;

		// Token: 0x04001823 RID: 6179
		private Texture2D commandIconCached;

		// Token: 0x04001824 RID: 6180
		private static List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04001825 RID: 6181
		private static List<IntVec3> tempDestList = new List<IntVec3>();
	}
}

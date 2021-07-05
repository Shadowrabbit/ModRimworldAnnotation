using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000D15 RID: 3349
	public class Ability : IVerbOwner, IExposable, ILoadReferenceable
	{
		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06004E64 RID: 20068 RVA: 0x001A453D File Offset: 0x001A273D
		public Verb verb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x001A454A File Offset: 0x001A274A
		public string UniqueVerbOwnerID()
		{
			return this.GetUniqueLoadID();
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x000126F5 File Offset: 0x000108F5
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06004E67 RID: 20071 RVA: 0x001A4552 File Offset: 0x001A2752
		// (set) Token: 0x06004E68 RID: 20072 RVA: 0x001A455A File Offset: 0x001A275A
		public List<Tool> Tools { get; private set; }

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06004E69 RID: 20073 RVA: 0x001A4563 File Offset: 0x001A2763
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x001A456B File Offset: 0x001A276B
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return new List<VerbProperties>
				{
					this.def.verbProperties
				};
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004E6B RID: 20075 RVA: 0x001A4583 File Offset: 0x001A2783
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004E6C RID: 20076 RVA: 0x001A458A File Offset: 0x001A278A
		public int CooldownTicksRemaining
		{
			get
			{
				return this.cooldownTicks;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06004E6D RID: 20077 RVA: 0x001A4592 File Offset: 0x001A2792
		public int CooldownTicksTotal
		{
			get
			{
				return this.cooldownTicksDuration;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004E6E RID: 20078 RVA: 0x001A459A File Offset: 0x001A279A
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004E6F RID: 20079 RVA: 0x001A45B8 File Offset: 0x001A27B8
		public bool HasCooldown
		{
			get
			{
				return this.def.cooldownTicksRange != default(IntRange) || (this.def.groupDef != null && this.def.groupDef.cooldownTicks > 0);
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06004E70 RID: 20080 RVA: 0x001A4604 File Offset: 0x001A2804
		public virtual bool CanCast
		{
			get
			{
				return this.cooldownTicks <= 0;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06004E71 RID: 20081 RVA: 0x001A4614 File Offset: 0x001A2814
		public bool Casting
		{
			get
			{
				if (!this.verb.WarmingUp)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					return ((jobs != null) ? jobs.curDriver : null) is JobDriver_CastAbilityWorld && this.pawn.CurJob.ability == this;
				}
				return true;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06004E72 RID: 20082 RVA: 0x001A4664 File Offset: 0x001A2864
		public string Tooltip
		{
			get
			{
				string text = this.def.GetTooltip(this.pawn);
				if (this.EffectComps != null)
				{
					foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
					{
						string text2 = compAbilityEffect.ExtraTooltipPart();
						if (!text2.NullOrEmpty())
						{
							text = text + "\n\n" + text2;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06004E73 RID: 20083 RVA: 0x001A46E8 File Offset: 0x001A28E8
		public virtual bool CanQueueCast
		{
			get
			{
				return !this.HasCooldown || ((this.pawn.jobs.curJob == null || !this.<get_CanQueueCast>g__SameForQueueing|44_0(this.pawn.jobs.curJob)) && !(from qj in this.pawn.jobs.jobQueue
				where this.<get_CanQueueCast>g__SameForQueueing|44_0(qj.job)
				select qj).Any<QueuedJob>());
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06004E74 RID: 20084 RVA: 0x001A4754 File Offset: 0x001A2954
		public List<CompAbilityEffect> EffectComps
		{
			get
			{
				if (this.effectComps == null)
				{
					this.effectComps = this.CompsOfType<CompAbilityEffect>().ToList<CompAbilityEffect>();
				}
				return this.effectComps;
			}
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x001A4775 File Offset: 0x001A2975
		public Ability()
		{
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x001A479A File Offset: 0x001A299A
		public Ability(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x001A47C6 File Offset: 0x001A29C6
		public Ability(Pawn pawn, Precept sourcePrecept)
		{
			this.pawn = pawn;
			this.sourcePrecept = sourcePrecept;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x001A47F9 File Offset: 0x001A29F9
		public Ability(Pawn pawn, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.Initialize();
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x001A4832 File Offset: 0x001A2A32
		public Ability(Pawn pawn, Precept sourcePrecept, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.sourcePrecept = sourcePrecept;
			this.Initialize();
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x001A4874 File Offset: 0x001A2A74
		public virtual bool CanApplyOn(LocalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.effectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target, null))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x001A48DC File Offset: 0x001A2ADC
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.effectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanApplyOn(target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001A4940 File Offset: 0x001A2B40
		public Window ConfirmationDialog(LocalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null);
			}
			return null;
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x001A49E0 File Offset: 0x001A2BE0
		public Window ConfirmationDialog(GlobalTargetInfo target, Action action)
		{
			if (this.EffectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
				{
					Window window = compAbilityEffect.ConfirmationDialog(target, action);
					if (window != null)
					{
						return window;
					}
				}
			}
			if (!this.def.confirmationDialogText.NullOrEmpty())
			{
				return Dialog_MessageBox.CreateConfirmation(this.def.confirmationDialogText.Formatted(this.pawn.Named("PAWN")), action, false, null);
			}
			return null;
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x001A4A80 File Offset: 0x001A2C80
		public virtual bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!this.EffectComps.Any<CompAbilityEffect>())
			{
				return false;
			}
			this.ApplyEffects(this.EffectComps, this.GetAffectedTargets(target), dest);
			if (this.def.writeCombatLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_AbilityUsed(this.pawn, target.Thing, this.def, RulePackDefOf.Event_AbilityUsed));
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x001A4AF0 File Offset: 0x001A2CF0
		public virtual bool Activate(GlobalTargetInfo target)
		{
			if (!this.EffectComps.Any<CompAbilityEffect>())
			{
				return false;
			}
			this.ApplyEffects(this.EffectComps, target);
			if (this.def.writeCombatLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_AbilityUsed(this.pawn, null, this.def, RulePackDefOf.Event_AbilityUsed));
			}
			this.preCastActions.Clear();
			return true;
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001A4B53 File Offset: 0x001A2D53
		public IEnumerable<LocalTargetInfo> GetAffectedTargets(LocalTargetInfo target)
		{
			if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
			{
				foreach (LocalTargetInfo localTargetInfo in from t in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, this.def.EffectRadius, true)
				where this.verb.targetParams.CanTarget(t, null) && !t.Fogged()
				select new LocalTargetInfo(t))
				{
					yield return localTargetInfo;
				}
				IEnumerator<LocalTargetInfo> enumerator = null;
			}
			else
			{
				yield return target;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001A4B6C File Offset: 0x001A2D6C
		public virtual void QueueCastingJob(LocalTargetInfo target, LocalTargetInfo destination)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnThing);
				job.verbToUse = this.verb;
				job.targetA = target;
				job.targetB = destination;
				job.ability = this;
				this.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			});
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001A4BC4 File Offset: 0x001A2DC4
		public virtual void QueueCastingJob(GlobalTargetInfo target)
		{
			if (!this.CanQueueCast || !this.CanApplyOn(target))
			{
				return;
			}
			this.ShowCastingConfirmationIfNeeded(target, delegate()
			{
				if (!this.pawn.IsCaravanMember())
				{
					Job job = JobMaker.MakeJob(this.def.jobDef ?? JobDefOf.CastAbilityOnWorldTile);
					job.verbToUse = this.verb;
					job.globalTarget = target;
					job.ability = this;
					this.pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					return;
				}
				this.Activate(target);
			});
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x001A4C14 File Offset: 0x001A2E14
		private void ShowCastingConfirmationIfNeeded(LocalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x06004E84 RID: 20100 RVA: 0x001A4C40 File Offset: 0x001A2E40
		private void ShowCastingConfirmationIfNeeded(GlobalTargetInfo target, Action cast)
		{
			Window window = this.ConfirmationDialog(target, cast);
			if (window == null)
			{
				cast();
				return;
			}
			Find.WindowStack.Add(window);
		}

		// Token: 0x06004E85 RID: 20101 RVA: 0x001A4C6C File Offset: 0x001A2E6C
		public bool ValidateGlobalTarget(GlobalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				if (!this.EffectComps[i].Valid(target, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004E86 RID: 20102 RVA: 0x001A4CA8 File Offset: 0x001A2EA8
		public virtual bool GizmoDisabled(out string reason)
		{
			if (!this.CanCast)
			{
				reason = "AbilityOnCooldown".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, true, true));
				return true;
			}
			if (!this.CanQueueCast)
			{
				reason = "AbilityAlreadyQueued".Translate();
				return true;
			}
			if (!this.pawn.Drafted && this.def.disableGizmoWhileUndrafted && this.pawn.GetCaravan() == null)
			{
				reason = "AbilityDisabledUndrafted".Translate();
				return true;
			}
			if (this.pawn.Downed)
			{
				reason = "CommandDisabledUnconscious".TranslateWithBackup("CommandCallRoyalAidUnconscious").Formatted(this.pawn);
				return true;
			}
			if (!this.comps.NullOrEmpty<AbilityComp>())
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (this.comps[i].GizmoDisabled(out reason))
					{
						return true;
					}
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06004E87 RID: 20103 RVA: 0x001A4DB0 File Offset: 0x001A2FB0
		public virtual void AbilityTick()
		{
			this.VerbTracker.VerbsTick();
			if (this.def.emittedFleck != null && this.Casting && this.pawn.IsHashIntervalTick(this.def.emissionInterval))
			{
				FleckMaker.ThrowMetaIcon(this.verb.CurrentTarget.Cell, this.pawn.Map, this.def.emittedFleck, 0.75f);
			}
			if ((this.def.warmupMote != null || this.def.WarmupMoteSocialSymbol != null) && this.Casting)
			{
				Vector3 vector = this.pawn.DrawPos + this.def.moteDrawOffset;
				vector += (this.verb.CurrentTarget.CenterVector3 - vector) * this.def.moteOffsetAmountTowardsTarget;
				if (this.warmupMote == null || this.warmupMote.Destroyed)
				{
					if (this.def.WarmupMoteSocialSymbol == null)
					{
						this.warmupMote = MoteMaker.MakeStaticMote(vector, this.pawn.Map, this.def.warmupMote, 1f);
					}
					else
					{
						this.warmupMote = MoteMaker.MakeInteractionBubble(this.pawn, this.verb.CurrentTarget.Pawn, ThingDefOf.Mote_Speech, this.def.WarmupMoteSocialSymbol, null);
					}
				}
				else
				{
					this.warmupMote.exactPosition = vector;
					this.warmupMote.Maintain();
				}
			}
			if (this.verb.WarmingUp)
			{
				if (!(this.def.targetWorldCell ? this.CanApplyOn(this.pawn.CurJob.globalTarget) : this.CanApplyOn(this.verb.CurrentTarget)))
				{
					if (this.def.targetWorldCell)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
					Stance_Warmup warmupStance = this.verb.WarmupStance;
					if (warmupStance != null)
					{
						warmupStance.Interrupt();
					}
					this.verb.Reset();
					this.preCastActions.Clear();
				}
				else
				{
					for (int i = this.preCastActions.Count - 1; i >= 0; i--)
					{
						if (this.preCastActions[i].ticksAwayFromCast >= this.verb.WarmupTicksLeft)
						{
							this.preCastActions[i].action(this.verb.CurrentTarget, this.verb.CurrentDestination);
							this.preCastActions.RemoveAt(i);
						}
					}
				}
			}
			if (this.pawn.Spawned && this.Casting)
			{
				if (this.def.warmupSound != null)
				{
					if (this.soundCast == null || this.soundCast.Ended)
					{
						this.soundCast = this.def.warmupSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
					}
					else
					{
						this.soundCast.Maintain();
					}
				}
				if (!this.wasCastingOnPrevTick && this.def.warmupStartSound != null)
				{
					this.def.warmupStartSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
				if (this.def.warmupPreEndSound != null && this.verb.WarmupTicksLeft == this.def.warmupPreEndSoundSeconds.SecondsToTicks())
				{
					this.def.warmupPreEndSound.PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
				}
			}
			if (this.cooldownTicks > 0)
			{
				this.cooldownTicks--;
				if (this.cooldownTicks == 0 && this.def.sendLetterOnCooldownComplete)
				{
					Find.LetterStack.ReceiveLetter("AbilityReadyLabel".Translate(this.def.LabelCap), "AbilityReadyText".Translate(this.pawn, this.def.label), LetterDefOf.NeutralEvent, new LookTargets(this.pawn), null, null, null, null);
				}
			}
			for (int j = this.maintainedEffecters.Count - 1; j >= 0; j--)
			{
				Effecter first = this.maintainedEffecters[j].First;
				if (first.ticksLeft > 0)
				{
					TargetInfo second = this.maintainedEffecters[j].Second;
					first.EffectTick(second, second);
					first.ticksLeft--;
				}
				else
				{
					first.Cleanup();
					this.maintainedEffecters.RemoveAt(j);
				}
			}
			this.wasCastingOnPrevTick = this.Casting;
		}

		// Token: 0x06004E88 RID: 20104 RVA: 0x001A5290 File Offset: 0x001A3490
		public void Notify_StartedCasting()
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.preCastActions.AddRange(this.EffectComps[i].GetPreCastActions());
			}
		}

		// Token: 0x06004E89 RID: 20105 RVA: 0x001A52D0 File Offset: 0x001A34D0
		public void DrawEffectPreviews(LocalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.EffectComps[i].DrawEffectPreview(target);
			}
		}

		// Token: 0x06004E8A RID: 20106 RVA: 0x001A5308 File Offset: 0x001A3508
		public bool GizmosVisible()
		{
			if (!this.EffectComps.NullOrEmpty<CompAbilityEffect>())
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ShouldHideGizmo)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004E8B RID: 20107 RVA: 0x001A5370 File Offset: 0x001A3570
		public virtual IEnumerable<Command> GetGizmos()
		{
			if (this.gizmo == null)
			{
				this.gizmo = (Command)Activator.CreateInstance(this.def.gizmoClass, new object[]
				{
					this
				});
				this.gizmo.order = this.def.uiOrder;
			}
			yield return this.gizmo;
			if (Prefs.DevMode && this.cooldownTicks > 0)
			{
				yield return new Command_Action
				{
					defaultLabel = "Reset cooldown",
					action = delegate()
					{
						this.cooldownTicks = 0;
					}
				};
			}
			yield break;
		}

		// Token: 0x06004E8C RID: 20108 RVA: 0x001A5380 File Offset: 0x001A3580
		private void ApplyEffects(IEnumerable<CompAbilityEffect> effects, IEnumerable<LocalTargetInfo> targets, LocalTargetInfo dest)
		{
			foreach (LocalTargetInfo target in targets)
			{
				this.ApplyEffects(effects, target, dest);
			}
			if (this.HasCooldown)
			{
				if (this.def.groupDef != null)
				{
					using (List<Ability>.Enumerator enumerator2 = this.pawn.abilities.AllAbilitiesForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Ability ability = enumerator2.Current;
							int ticks = this.def.overrideGroupCooldown ? this.def.cooldownTicksRange.RandomInRange : this.def.groupDef.cooldownTicks;
							ability.Notify_GroupStartedCooldown(this.def.groupDef, ticks);
						}
						return;
					}
				}
				this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
			}
		}

		// Token: 0x06004E8D RID: 20109 RVA: 0x001A5480 File Offset: 0x001A3680
		public void StartCooldown(int ticks)
		{
			this.cooldownTicksDuration = ticks;
			this.cooldownTicks = this.cooldownTicksDuration;
		}

		// Token: 0x06004E8E RID: 20110 RVA: 0x001A5495 File Offset: 0x001A3695
		public void Notify_GroupStartedCooldown(AbilityGroupDef group, int ticks)
		{
			if (group == this.def.groupDef)
			{
				this.StartCooldown(ticks);
			}
		}

		// Token: 0x06004E8F RID: 20111 RVA: 0x001A54AC File Offset: 0x001A36AC
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target, dest);
			}
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x001A54F4 File Offset: 0x001A36F4
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, GlobalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target);
			}
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x001A553C File Offset: 0x001A373C
		public virtual void OnGizmoUpdate()
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				compAbilityEffect.OnGizmoUpdate();
			}
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x001A558C File Offset: 0x001A378C
		public IEnumerable<T> CompsOfType<T>() where T : AbilityComp
		{
			if (this.comps == null)
			{
				return null;
			}
			return (from c in this.comps
			where c is T
			select c).Cast<T>();
		}

		// Token: 0x06004E93 RID: 20115 RVA: 0x001A55C8 File Offset: 0x001A37C8
		public T CompOfType<T>() where T : AbilityComp
		{
			if (this.comps == null)
			{
				return default(T);
			}
			return this.comps.FirstOrDefault((AbilityComp c) => c is T) as T;
		}

		// Token: 0x06004E94 RID: 20116 RVA: 0x001A561C File Offset: 0x001A381C
		public void Initialize()
		{
			if (this.def.comps.Any<AbilityCompProperties>())
			{
				this.comps = new List<AbilityComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					AbilityComp abilityComp = null;
					try
					{
						abilityComp = (AbilityComp)Activator.CreateInstance(this.def.comps[i].compClass);
						abilityComp.parent = this;
						this.comps.Add(abilityComp);
						abilityComp.Initialize(this.def.comps[i]);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize an AbilityComp: " + arg);
						this.comps.Remove(abilityComp);
					}
				}
			}
			if (this.Id == -1)
			{
				this.Id = Find.UniqueIDsManager.GetNextAbilityID();
			}
			Verb_CastAbility verb_CastAbility = this.VerbTracker.PrimaryVerb as Verb_CastAbility;
			if (verb_CastAbility != null)
			{
				verb_CastAbility.ability = this;
			}
		}

		// Token: 0x06004E95 RID: 20117 RVA: 0x001A571C File Offset: 0x001A391C
		public float FinalPsyfocusCost(LocalTargetInfo target)
		{
			if (this.def.AnyCompOverridesPsyfocusCost)
			{
				foreach (AbilityComp abilityComp in this.comps)
				{
					if (abilityComp.props.OverridesPsyfocusCost)
					{
						return abilityComp.PsyfocusCostForTarget(target);
					}
				}
			}
			return this.def.PsyfocusCost;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x001A579C File Offset: 0x001A399C
		public string WorldMapExtraLabel(GlobalTargetInfo t)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.EffectComps)
			{
				string text = compAbilityEffect.WorldMapExtraLabel(t);
				if (text != null)
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x001A57F8 File Offset: 0x001A39F8
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new Pair<Effecter, TargetInfo>(eff, new TargetInfo(pos, map ?? this.pawn.Map, false)));
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x001A582C File Offset: 0x001A3A2C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
			if (this.def == null)
			{
				return;
			}
			Scribe_Values.Look<int>(ref this.Id, "Id", -1, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.Id == -1)
			{
				this.Id = Find.UniqueIDsManager.GetNextAbilityID();
			}
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Values.Look<int>(ref this.cooldownTicks, "cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.cooldownTicksDuration, "cooldownTicksDuration", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.Initialize();
			}
		}

		// Token: 0x06004E99 RID: 20121 RVA: 0x001A58E2 File Offset: 0x001A3AE2
		public string GetUniqueLoadID()
		{
			return "Ability_" + this.Id;
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x001A58FC File Offset: 0x001A3AFC
		[CompilerGenerated]
		private bool <get_CanQueueCast>g__SameForQueueing|44_0(Job j)
		{
			return j.verbToUse == this.verb || (this.def.groupDef != null && j.ability != null && j.ability.def.groupDef == this.def.groupDef);
		}

		// Token: 0x04002F47 RID: 12103
		public int Id = -1;

		// Token: 0x04002F48 RID: 12104
		public Pawn pawn;

		// Token: 0x04002F49 RID: 12105
		public AbilityDef def;

		// Token: 0x04002F4A RID: 12106
		public List<AbilityComp> comps;

		// Token: 0x04002F4B RID: 12107
		protected Command gizmo;

		// Token: 0x04002F4C RID: 12108
		private VerbTracker verbTracker;

		// Token: 0x04002F4D RID: 12109
		private int cooldownTicks;

		// Token: 0x04002F4E RID: 12110
		private int cooldownTicksDuration;

		// Token: 0x04002F4F RID: 12111
		private Mote warmupMote;

		// Token: 0x04002F50 RID: 12112
		private Sustainer soundCast;

		// Token: 0x04002F51 RID: 12113
		private bool wasCastingOnPrevTick;

		// Token: 0x04002F52 RID: 12114
		public Precept sourcePrecept;

		// Token: 0x04002F53 RID: 12115
		private List<PreCastAction> preCastActions = new List<PreCastAction>();

		// Token: 0x04002F54 RID: 12116
		private List<Pair<Effecter, TargetInfo>> maintainedEffecters = new List<Pair<Effecter, TargetInfo>>();

		// Token: 0x04002F55 RID: 12117
		private List<CompAbilityEffect> effectComps;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001356 RID: 4950
	public class Ability : IVerbOwner, IExposable, ILoadReferenceable
	{
		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06006B86 RID: 27526 RVA: 0x000492FB File Offset: 0x000474FB
		public Verb verb
		{
			get
			{
				return this.verbTracker.PrimaryVerb;
			}
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x00049308 File Offset: 0x00047508
		public string UniqueVerbOwnerID()
		{
			return "Ability_" + this.def.label + this.pawn.ThingID;
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public bool VerbsStillUsableBy(Pawn p)
		{
			return true;
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06006B89 RID: 27529 RVA: 0x0004932A File Offset: 0x0004752A
		// (set) Token: 0x06006B8A RID: 27530 RVA: 0x00049332 File Offset: 0x00047532
		public List<Tool> Tools { get; private set; }

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06006B8B RID: 27531 RVA: 0x0004933B File Offset: 0x0004753B
		public Thing ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06006B8C RID: 27532 RVA: 0x00049343 File Offset: 0x00047543
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

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06006B8D RID: 27533 RVA: 0x0004935B File Offset: 0x0004755B
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06006B8E RID: 27534 RVA: 0x00049362 File Offset: 0x00047562
		public int CooldownTicksRemaining
		{
			get
			{
				return this.cooldownTicks;
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06006B8F RID: 27535 RVA: 0x0004936A File Offset: 0x0004756A
		public int CooldownTicksTotal
		{
			get
			{
				return this.cooldownTicksDuration;
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06006B90 RID: 27536 RVA: 0x00049372 File Offset: 0x00047572
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

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06006B91 RID: 27537 RVA: 0x00211E04 File Offset: 0x00210004
		public bool HasCooldown
		{
			get
			{
				return this.def.cooldownTicksRange != default(IntRange);
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06006B92 RID: 27538 RVA: 0x0004938E File Offset: 0x0004758E
		public virtual bool CanCast
		{
			get
			{
				return this.cooldownTicks <= 0;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06006B93 RID: 27539 RVA: 0x00211E2C File Offset: 0x0021002C
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

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06006B94 RID: 27540 RVA: 0x00211E7C File Offset: 0x0021007C
		public virtual bool CanQueueCast
		{
			get
			{
				return !this.HasCooldown || ((this.pawn.jobs.curJob == null || this.pawn.jobs.curJob.verbToUse != this.verb) && !(from qj in this.pawn.jobs.jobQueue
				where qj.job.verbToUse == this.verb
				select qj).Any<QueuedJob>());
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06006B95 RID: 27541 RVA: 0x0004939C File Offset: 0x0004759C
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

		// Token: 0x06006B96 RID: 27542 RVA: 0x000493BD File Offset: 0x000475BD
		public Ability(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x000493E2 File Offset: 0x000475E2
		public Ability(Pawn pawn, AbilityDef def)
		{
			this.pawn = pawn;
			this.def = def;
			this.Initialize();
		}

		// Token: 0x06006B98 RID: 27544 RVA: 0x00211EF0 File Offset: 0x002100F0
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

		// Token: 0x06006B99 RID: 27545 RVA: 0x00211F58 File Offset: 0x00210158
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

		// Token: 0x06006B9A RID: 27546 RVA: 0x00211FBC File Offset: 0x002101BC
		public string ConfirmationDialogText(LocalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					string text = compAbilityEffect.ConfirmationDialogText(target);
					if (!text.NullOrEmpty())
					{
						return text;
					}
				}
			}
			return this.def.confirmationDialogText;
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x00212030 File Offset: 0x00210230
		public string ConfirmationDialogText(GlobalTargetInfo target)
		{
			if (this.effectComps != null)
			{
				foreach (CompAbilityEffect compAbilityEffect in this.effectComps)
				{
					string text = compAbilityEffect.ConfirmationDialogText(target);
					if (!text.NullOrEmpty())
					{
						return text;
					}
				}
			}
			return this.def.confirmationDialogText;
		}

		// Token: 0x06006B9C RID: 27548 RVA: 0x002120A4 File Offset: 0x002102A4
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

		// Token: 0x06006B9D RID: 27549 RVA: 0x00212114 File Offset: 0x00210314
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

		// Token: 0x06006B9E RID: 27550 RVA: 0x00049414 File Offset: 0x00047614
		public IEnumerable<LocalTargetInfo> GetAffectedTargets(LocalTargetInfo target)
		{
			if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
			{
				foreach (LocalTargetInfo localTargetInfo in from t in GenRadial.RadialDistinctThingsAround(target.Cell, this.pawn.Map, this.def.EffectRadius, true)
				where this.verb.targetParams.CanTarget(t) && !t.Fogged()
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

		// Token: 0x06006B9F RID: 27551 RVA: 0x00212178 File Offset: 0x00210378
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
				this.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			});
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x002121D0 File Offset: 0x002103D0
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
					this.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					return;
				}
				this.Activate(target);
			});
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x00212220 File Offset: 0x00210420
		private void ShowCastingConfirmationIfNeeded(LocalTargetInfo target, Action cast)
		{
			string str = this.ConfirmationDialogText(target);
			if (str.NullOrEmpty())
			{
				cast();
				return;
			}
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(str.Formatted(this.pawn.Named("PAWN")), cast, false, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x00212270 File Offset: 0x00210470
		private void ShowCastingConfirmationIfNeeded(GlobalTargetInfo target, Action cast)
		{
			string str = this.ConfirmationDialogText(target);
			if (str.NullOrEmpty())
			{
				cast();
				return;
			}
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(str.Formatted(this.pawn.Named("PAWN")), cast, false, null);
			Find.WindowStack.Add(window);
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x002122C0 File Offset: 0x002104C0
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

		// Token: 0x06006BA4 RID: 27556 RVA: 0x002122FC File Offset: 0x002104FC
		public virtual bool GizmoDisabled(out string reason)
		{
			if (!this.CanCast)
			{
				reason = "AbilityOnCooldown".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, false, true));
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

		// Token: 0x06006BA5 RID: 27557 RVA: 0x00212404 File Offset: 0x00210604
		public virtual void AbilityTick()
		{
			this.VerbTracker.VerbsTick();
			if (this.def.warmupMote != null && this.Casting)
			{
				Vector3 vector = this.pawn.DrawPos + this.def.moteDrawOffset;
				vector += (this.verb.CurrentTarget.CenterVector3 - vector) * this.def.moteOffsetAmountTowardsTarget;
				if (this.warmupMote == null || this.warmupMote.Destroyed)
				{
					this.warmupMote = MoteMaker.MakeStaticMote(vector, this.pawn.Map, this.def.warmupMote, 1f);
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

		// Token: 0x06006BA6 RID: 27558 RVA: 0x00212818 File Offset: 0x00210A18
		public void Notify_StartedCasting()
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.preCastActions.AddRange(this.EffectComps[i].GetPreCastActions());
			}
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x00212858 File Offset: 0x00210A58
		public void DrawEffectPreviews(LocalTargetInfo target)
		{
			for (int i = 0; i < this.EffectComps.Count; i++)
			{
				this.EffectComps[i].DrawEffectPreview(target);
			}
		}

		// Token: 0x06006BA8 RID: 27560 RVA: 0x0004942B File Offset: 0x0004762B
		public virtual IEnumerable<Command> GetGizmos()
		{
			if (this.gizmo == null)
			{
				this.gizmo = (Command)Activator.CreateInstance(this.def.gizmoClass, new object[]
				{
					this
				});
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

		// Token: 0x06006BA9 RID: 27561 RVA: 0x00212890 File Offset: 0x00210A90
		private void ApplyEffects(IEnumerable<CompAbilityEffect> effects, IEnumerable<LocalTargetInfo> targets, LocalTargetInfo dest)
		{
			foreach (LocalTargetInfo target in targets)
			{
				this.ApplyEffects(effects, target, dest);
			}
			if (this.HasCooldown)
			{
				this.StartCooldown(this.def.cooldownTicksRange.RandomInRange);
			}
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x0004943B File Offset: 0x0004763B
		public void StartCooldown(int ticks)
		{
			this.cooldownTicksDuration = ticks;
			this.cooldownTicks = this.cooldownTicksDuration;
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x002128F8 File Offset: 0x00210AF8
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target, dest);
			}
		}

		// Token: 0x06006BAC RID: 27564 RVA: 0x00212940 File Offset: 0x00210B40
		protected virtual void ApplyEffects(IEnumerable<CompAbilityEffect> effects, GlobalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in effects)
			{
				compAbilityEffect.Apply(target);
			}
		}

		// Token: 0x06006BAD RID: 27565 RVA: 0x00049450 File Offset: 0x00047650
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

		// Token: 0x06006BAE RID: 27566 RVA: 0x00212988 File Offset: 0x00210B88
		public T CompOfType<T>() where T : AbilityComp
		{
			if (this.comps == null)
			{
				return default(T);
			}
			return this.comps.FirstOrDefault((AbilityComp c) => c is T) as T;
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x002129DC File Offset: 0x00210BDC
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
						Log.Error("Could not instantiate or initialize an AbilityComp: " + arg, false);
						this.comps.Remove(abilityComp);
					}
				}
			}
			Verb_CastAbility verb_CastAbility = this.VerbTracker.PrimaryVerb as Verb_CastAbility;
			if (verb_CastAbility != null)
			{
				verb_CastAbility.ability = this;
			}
		}

		// Token: 0x06006BB0 RID: 27568 RVA: 0x00212AC4 File Offset: 0x00210CC4
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

		// Token: 0x06006BB1 RID: 27569 RVA: 0x00212B44 File Offset: 0x00210D44
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

		// Token: 0x06006BB2 RID: 27570 RVA: 0x0004948B File Offset: 0x0004768B
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new Pair<Effecter, TargetInfo>(eff, new TargetInfo(pos, this.pawn.Map, false)));
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x00212BA0 File Offset: 0x00210DA0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<AbilityDef>(ref this.def, "def");
			if (this.def == null)
			{
				return;
			}
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

		// Token: 0x06006BB4 RID: 27572 RVA: 0x000494B7 File Offset: 0x000476B7
		public string GetUniqueLoadID()
		{
			return this.pawn.ThingID + "_Ability_" + this.def.defName;
		}

		// Token: 0x04004780 RID: 18304
		public Pawn pawn;

		// Token: 0x04004781 RID: 18305
		public AbilityDef def;

		// Token: 0x04004782 RID: 18306
		public List<AbilityComp> comps;

		// Token: 0x04004783 RID: 18307
		protected Command gizmo;

		// Token: 0x04004784 RID: 18308
		private VerbTracker verbTracker;

		// Token: 0x04004785 RID: 18309
		private int cooldownTicks;

		// Token: 0x04004786 RID: 18310
		private int cooldownTicksDuration;

		// Token: 0x04004787 RID: 18311
		private Mote warmupMote;

		// Token: 0x04004788 RID: 18312
		private Sustainer soundCast;

		// Token: 0x04004789 RID: 18313
		private bool wasCastingOnPrevTick;

		// Token: 0x0400478A RID: 18314
		private List<PreCastAction> preCastActions = new List<PreCastAction>();

		// Token: 0x0400478B RID: 18315
		private List<Pair<Effecter, TargetInfo>> maintainedEffecters = new List<Pair<Effecter, TargetInfo>>();

		// Token: 0x0400478C RID: 18316
		private List<CompAbilityEffect> effectComps;
	}
}

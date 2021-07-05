using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005DC RID: 1500
	public class MentalState : IExposable
	{
		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002B64 RID: 11108 RVA: 0x001033C0 File Offset: 0x001015C0
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x001033C8 File Offset: 0x001015C8
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x001033D8 File Offset: 0x001015D8
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<bool>(ref this.causedByDamage, "causedByDamage", false, false);
			Scribe_Values.Look<bool>(ref this.causedByPsycast, "causedByPsycast", false, false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreStart()
		{
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x00103450 File Offset: 0x00101650
		public virtual void PostEnd()
		{
			if (!this.def.recoveryMessage.NullOrEmpty() && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = this.def.recoveryMessage.Formatted(this.pawn.LabelShort, this.pawn.Named("PAWN"));
				if (!taggedString.NullOrEmpty())
				{
					Messages.Message(taggedString.AdjustedFor(this.pawn, "PAWN", true).CapitalizeFirst(), this.pawn, MessageTypeDefOf.SituationResolved, true);
				}
			}
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x001034EC File Offset: 0x001016EC
		public virtual void MentalStateTick()
		{
			if (this.pawn.IsHashIntervalTick(30))
			{
				this.age += 30;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 30f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
				{
					this.RecoverFromState();
					return;
				}
				if (this.def.recoverFromSleep && !this.pawn.Awake())
				{
					this.RecoverFromState();
					return;
				}
			}
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x001035A4 File Offset: 0x001017A4
		public void RecoverFromState()
		{
			if (this.pawn.MentalState != this)
			{
				Log.Error(string.Concat(new object[]
				{
					"Recovered from ",
					this.def,
					" but pawn's mental state is not this, it is ",
					this.pawn.MentalState
				}));
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null, null);
				}
				this.pawn.mindState.mentalBreaker.Notify_RecoveredFromMentalState();
				if (this.pawn.story != null && this.pawn.story.traits != null)
				{
					foreach (Trait trait in this.pawn.story.traits.allTraits)
					{
						trait.Notify_MentalStateEndedOn(this.pawn, this.causedByMood);
					}
				}
				if (this.def.IsAggro)
				{
					this.pawn.mindState.enemyTarget = null;
				}
			}
			if (this.pawn.Spawned)
			{
				this.pawn.jobs.StopAll(true, true);
			}
			this.PostEnd();
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x00103744 File Offset: 0x00101944
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x00034716 File Offset: 0x00032916
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x00103754 File Offset: 0x00101954
		public virtual TaggedString GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_SlaughteredAnimal()
		{
		}

		// Token: 0x04001A7C RID: 6780
		public Pawn pawn;

		// Token: 0x04001A7D RID: 6781
		public MentalStateDef def;

		// Token: 0x04001A7E RID: 6782
		private int age;

		// Token: 0x04001A7F RID: 6783
		public bool causedByMood;

		// Token: 0x04001A80 RID: 6784
		public bool causedByDamage;

		// Token: 0x04001A81 RID: 6785
		public bool causedByPsycast;

		// Token: 0x04001A82 RID: 6786
		public int forceRecoverAfterTicks = -1;

		// Token: 0x04001A83 RID: 6787
		private const int TickInterval = 30;
	}
}

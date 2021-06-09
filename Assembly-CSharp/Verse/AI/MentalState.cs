using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A25 RID: 2597
	public class MentalState : IExposable
	{
		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x0002EB22 File Offset: 0x0002CD22
		public int Age
		{
			get
			{
				return this.age;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x0002EB2A File Offset: 0x0002CD2A
		public virtual string InspectLine
		{
			get
			{
				return this.def.baseInspectLine;
			}
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x06003E09 RID: 15881 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x00177870 File Offset: 0x00175A70
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<MentalStateDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
			Scribe_Values.Look<bool>(ref this.causedByMood, "causedByMood", false, false);
			Scribe_Values.Look<int>(ref this.forceRecoverAfterTicks, "forceRecoverAfterTicks", 0, false);
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostStart(string reason)
		{
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreStart()
		{
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x001778C4 File Offset: 0x00175AC4
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

		// Token: 0x06003E0E RID: 15886 RVA: 0x00177960 File Offset: 0x00175B60
		public virtual void MentalStateTick()
		{
			if (this.pawn.IsHashIntervalTick(150))
			{
				this.age += 150;
				if (this.age >= this.def.maxTicksBeforeRecovery || (this.age >= this.def.minTicksBeforeRecovery && this.CanEndBeforeMaxDurationNow && Rand.MTBEventOccurs(this.def.recoveryMtbDays, 60000f, 150f)) || (this.forceRecoverAfterTicks != -1 && this.age >= this.forceRecoverAfterTicks))
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

		// Token: 0x06003E0F RID: 15887 RVA: 0x00177A20 File Offset: 0x00175C20
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
				}), false);
			}
			if (!this.pawn.Dead)
			{
				this.pawn.mindState.mentalStateHandler.ClearMentalStateDirect();
				if (this.causedByMood && this.def.moodRecoveryThought != null && this.pawn.needs.mood != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def.moodRecoveryThought, null);
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
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.PostEnd();
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ForceHostileTo(Thing t)
		{
			return false;
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool ForceHostileTo(Faction f)
		{
			return false;
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x0002EB37 File Offset: 0x0002CD37
		public EffecterDef CurrentStateEffecter()
		{
			return this.def.stateEffecter;
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public virtual RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.SuperActive;
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00177BC0 File Offset: 0x00175DC0
		public virtual string GetBeginLetterText()
		{
			if (this.def.beginLetter.NullOrEmpty())
			{
				return null;
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.pawn.Named("PAWN")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_AttackedTarget(LocalTargetInfo hitTarget)
		{
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_SlaughteredAnimal()
		{
		}

		// Token: 0x04002AE4 RID: 10980
		public Pawn pawn;

		// Token: 0x04002AE5 RID: 10981
		public MentalStateDef def;

		// Token: 0x04002AE6 RID: 10982
		private int age;

		// Token: 0x04002AE7 RID: 10983
		public bool causedByMood;

		// Token: 0x04002AE8 RID: 10984
		public int forceRecoverAfterTicks = -1;

		// Token: 0x04002AE9 RID: 10985
		private const int TickInterval = 150;
	}
}

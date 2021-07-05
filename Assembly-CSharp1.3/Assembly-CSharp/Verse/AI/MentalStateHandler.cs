using System;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x020005C7 RID: 1479
	public class MentalStateHandler : IExposable
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002B19 RID: 11033 RVA: 0x001023D8 File Offset: 0x001005D8
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x001023E3 File Offset: 0x001005E3
		public MentalStateDef CurStateDef
		{
			get
			{
				if (this.curStateInt == null)
				{
					return null;
				}
				return this.curStateInt.def;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x001023FA File Offset: 0x001005FA
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000033AC File Offset: 0x000015AC
		public MentalStateHandler()
		{
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x00102402 File Offset: 0x00100602
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x00102414 File Offset: 0x00100614
		public void ExposeData()
		{
			Scribe_Deep.Look<MentalState>(ref this.curStateInt, "curState", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.neverFleeIndividual, "neverFleeIndividual", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curStateInt != null)
				{
					this.curStateInt.pawn = this.pawn;
				}
				if (Current.ProgramState != ProgramState.Entry && this.pawn.Spawned)
				{
					this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
				}
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x00102498 File Offset: 0x00100698
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x001024A0 File Offset: 0x001006A0
		public void MentalStateHandlerTick()
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed && this.curStateInt.def.recoverFromDowned)
				{
					Log.Error("In mental state while downed, but not allowed: " + this.pawn);
					this.CurState.RecoverFromState();
					return;
				}
				this.curStateInt.MentalStateTick();
			}
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x00102500 File Offset: 0x00100700
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false, bool causedByDamage = false, bool causedByPsycast = false)
		{
			if ((!this.pawn.Spawned && !this.pawn.IsCaravanMember()) || this.CurStateDef == stateDef || this.pawn.Downed || (!forceWake && !this.pawn.Awake()))
			{
				return false;
			}
			if (TutorSystem.TutorialMode && this.pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (!stateDef.Worker.StateCanOccur(this.pawn))
			{
				return false;
			}
			if (this.curStateInt != null && !transitionSilently)
			{
				this.curStateInt.RecoverFromState();
			}
			MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
			mentalState.pawn = this.pawn;
			mentalState.def = stateDef;
			mentalState.causedByMood = causedByMood;
			mentalState.causedByDamage = causedByDamage;
			mentalState.causedByPsycast = causedByPsycast;
			if (otherPawn != null)
			{
				((MentalState_SocialFighting)mentalState).otherPawn = otherPawn;
			}
			mentalState.PreStart();
			if (!transitionSilently)
			{
				if ((this.pawn.IsColonist || this.pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
				{
					TaleRecorder.RecordTale(stateDef.tale, new object[]
					{
						this.pawn
					});
				}
				if (stateDef.IsExtreme && this.pawn.IsPlayerControlledCaravanMember())
				{
					Messages.Message("MessageCaravanMemberHasExtremeMentalBreak".Translate(), this.pawn.GetCaravan(), MessageTypeDefOf.ThreatSmall, true);
				}
				this.pawn.records.Increment(RecordDefOf.TimesInMentalState);
			}
			if (this.pawn.Drafted)
			{
				this.pawn.drafter.Drafted = false;
			}
			this.curStateInt = mentalState;
			if (this.pawn.needs.mood != null)
			{
				this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (stateDef != null && stateDef.IsAggro && this.pawn.caller != null)
			{
				this.pawn.caller.Notify_InAggroMentalState();
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_InMentalState(this.pawn, stateDef);
			}
			if (this.curStateInt != null)
			{
				this.curStateInt.PostStart(reason);
			}
			if (this.pawn.CurJob != null)
			{
				this.pawn.jobs.StopAll(false, true);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
			if (this.pawn.Spawned && forceWake && !this.pawn.Awake())
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			if (!transitionSilently && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				TaggedString taggedString = mentalState.GetBeginLetterText();
				if (!taggedString.NullOrEmpty())
				{
					string str = (stateDef.beginLetterLabel ?? stateDef.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
					if (!reason.NullOrEmpty())
					{
						taggedString += "\n\n" + reason;
					}
					Find.LetterStack.ReceiveLetter(str, taggedString, stateDef.beginLetterDef, this.pawn, null, null, null, null);
				}
			}
			return true;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00102844 File Offset: 0x00100A44
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false, true, false);
				}
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x00102944 File Offset: 0x00100B44
		internal void ClearMentalStateDirect()
		{
			if (this.curStateInt == null)
			{
				return;
			}
			this.curStateInt = null;
			QuestUtility.SendQuestTargetSignals(this.pawn.questTags, "ExitMentalState", this.pawn.Named("SUBJECT"));
			if (this.pawn.Spawned)
			{
				this.pawn.Map.attackTargetsCache.UpdateTarget(this.pawn);
			}
		}

		// Token: 0x04001A69 RID: 6761
		private Pawn pawn;

		// Token: 0x04001A6A RID: 6762
		private MentalState curStateInt;

		// Token: 0x04001A6B RID: 6763
		public bool neverFleeIndividual;
	}
}

using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000A21 RID: 2593
	public class MentalStateHandler : IExposable
	{
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x0002E9D0 File Offset: 0x0002CBD0
		public bool InMentalState
		{
			get
			{
				return this.curStateInt != null;
			}
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0002E9DB File Offset: 0x0002CBDB
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

		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x06003DE7 RID: 15847 RVA: 0x0002E9F2 File Offset: 0x0002CBF2
		public MentalState CurState
		{
			get
			{
				return this.curStateInt;
			}
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00006B8B File Offset: 0x00004D8B
		public MentalStateHandler()
		{
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x0002E9FA File Offset: 0x0002CBFA
		public MentalStateHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00176F38 File Offset: 0x00175138
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

		// Token: 0x06003DEB RID: 15851 RVA: 0x0002EA09 File Offset: 0x0002CC09
		public void Reset()
		{
			this.ClearMentalStateDirect();
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x00176FBC File Offset: 0x001751BC
		public void MentalStateHandlerTick()
		{
			if (this.curStateInt != null)
			{
				if (this.pawn.Downed && this.curStateInt.def.recoverFromDowned)
				{
					Log.Error("In mental state while downed, but not allowed: " + this.pawn, false);
					this.CurState.RecoverFromState();
					return;
				}
				this.curStateInt.MentalStateTick();
			}
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x00177020 File Offset: 0x00175220
		public bool TryStartMentalState(MentalStateDef stateDef, string reason = null, bool forceWake = false, bool causedByMood = false, Pawn otherPawn = null, bool transitionSilently = false)
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
				string text = mentalState.GetBeginLetterText();
				if (!text.NullOrEmpty())
				{
					string str = (stateDef.beginLetterLabel ?? stateDef.LabelCap).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
					if (!reason.NullOrEmpty())
					{
						text = text + "\n\n" + reason;
					}
					Find.LetterStack.ReceiveLetter(str, text, stateDef.beginLetterDef, this.pawn, null, null, null, null);
				}
			}
			return true;
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00177334 File Offset: 0x00175534
		public void Notify_DamageTaken(DamageInfo dinfo)
		{
			if (!this.neverFleeIndividual && this.pawn.Spawned && this.pawn.MentalStateDef == null && !this.pawn.Downed && dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.RaceProps.Humanlike && this.pawn.mindState.canFleeIndividual)
			{
				float lerpPct = (float)(this.pawn.HashOffset() % 100) / 100f;
				float num = this.pawn.kindDef.fleeHealthThresholdRange.LerpThroughRange(lerpPct);
				if (this.pawn.health.summaryHealth.SummaryHealthPercent < num && this.pawn.Faction != Faction.OfPlayer && this.pawn.HostFaction == null)
				{
					this.TryStartMentalState(MentalStateDefOf.PanicFlee, null, false, false, null, false);
				}
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x00177430 File Offset: 0x00175630
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

		// Token: 0x04002AD9 RID: 10969
		private Pawn pawn;

		// Token: 0x04002ADA RID: 10970
		private MentalState curStateInt;

		// Token: 0x04002ADB RID: 10971
		public bool neverFleeIndividual;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD8 RID: 3544
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x060050C8 RID: 20680 RVA: 0x00038AC1 File Offset: 0x00036CC1
		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x00038A40 File Offset: 0x00036C40
		public LordJob_Joinable_MarriageCeremony()
		{
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00038AD6 File Offset: 0x00036CD6
		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001B9228 File Offset: 0x001B7428
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Party lordToil_Party = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party);
			LordToil_MarriageCeremony lordToil_MarriageCeremony = new LordToil_MarriageCeremony(this.firstPawn, this.secondPawn, this.spot);
			stateGraph.AddToil(lordToil_MarriageCeremony);
			LordToil_Party lordToil_Party2 = new LordToil_Party(this.spot, GatheringDefOf.Party, 3.5E-05f);
			stateGraph.AddToil(lordToil_Party2);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_Party, lordToil_MarriageCeremony, false, true);
			transition.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 5000 && this.AreFiancesInPartyArea(), 1));
			transition.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyStarts".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_MarriageCeremony, lordToil_Party2, false, true);
			transition2.AddTrigger(new Trigger_TickCondition(() => this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Spouse, this.secondPawn), 1));
			transition2.AddPreAction(new TransitionAction_Message("MessageNewlyMarried".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.AddAttendedWeddingThoughts();
			}));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition3.AddTrigger(new Trigger_TickCondition(() => this.ShouldAfterPartyBeCalledOff(), 1));
			transition3.AddTrigger(new Trigger_PawnKilled());
			stateGraph.AddTransition(transition3, false);
			this.afterPartyTimeoutTrigger = new Trigger_TicksPassed(7500);
			Transition transition4 = new Transition(lordToil_Party2, lordToil_End, false, true);
			transition4.AddTrigger(this.afterPartyTimeoutTrigger);
			transition4.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyAfterPartyFinished".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.PositiveEvent, this.firstPawn, null, 1f));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition5.AddSource(lordToil_Party);
			transition5.AddTrigger(new Trigger_TickCondition(() => this.lord.ticksInToil >= 120000 && (this.firstPawn.Drafted || this.secondPawn.Drafted || !this.firstPawn.Position.InHorDistOf(this.spot, 4f) || !this.secondPawn.Position.InHorDistOf(this.spot, 4f)), 1));
			transition5.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_MarriageCeremony, lordToil_End, false, true);
			transition6.AddSource(lordToil_Party);
			transition6.AddTrigger(new Trigger_TickCondition(() => this.ShouldCeremonyBeCalledOff(), 1));
			transition6.AddTrigger(new Trigger_PawnKilled());
			transition6.AddPreAction(new TransitionAction_Message("MessageMarriageCeremonyCalledOff".Translate(this.firstPawn.LabelShort, this.secondPawn.LabelShort, this.firstPawn.Named("PAWN1"), this.secondPawn.Named("PAWN2")), MessageTypeDefOf.NegativeEvent, new TargetInfo(this.spot, base.Map, false), null, 1f));
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x001B963C File Offset: 0x001B783C
		private bool AreFiancesInPartyArea()
		{
			return this.lord.ownedPawns.Contains(this.firstPawn) && this.lord.ownedPawns.Contains(this.secondPawn) && this.firstPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.firstPawn.Position, this.spot, base.Map) && this.secondPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.secondPawn.Position, this.spot, base.Map);
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x001B96E4 File Offset: 0x001B78E4
		private bool ShouldCeremonyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || !this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn, this.secondPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn, this.firstPawn));
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x001B9798 File Offset: 0x001B7998
		private bool ShouldAfterPartyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || (this.firstPawn.Downed || this.secondPawn.Downed) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x001B9824 File Offset: 0x001B7A24
		public override float VoluntaryJoinPriorityFor(Pawn p)
		{
			if (this.IsFiance(p))
			{
				if (!MarriageCeremonyUtility.FianceCanContinueCeremony(p, (p == this.firstPawn) ? this.secondPawn : this.firstPawn))
				{
					return 0f;
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyFiance;
			}
			else
			{
				if (!this.IsGuest(p))
				{
					return 0f;
				}
				if (!MarriageCeremonyUtility.ShouldGuestKeepAttendingCeremony(p))
				{
					return 0f;
				}
				if (!this.lord.ownedPawns.Contains(p))
				{
					if (this.IsCeremonyAboutToEnd())
					{
						return 0f;
					}
					LordToil_MarriageCeremony lordToil_MarriageCeremony = this.lord.CurLordToil as LordToil_MarriageCeremony;
					IntVec3 intVec;
					if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, null))
					{
						return 0f;
					}
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
			}
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x001B98EC File Offset: 0x001B7AEC
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x00038AF3 File Offset: 0x00036CF3
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		// Token: 0x060050D3 RID: 20691 RVA: 0x00038B04 File Offset: 0x00036D04
		private bool IsCeremonyAboutToEnd()
		{
			return this.afterPartyTimeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x00038B1B File Offset: 0x00036D1B
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x060050D5 RID: 20693 RVA: 0x001B9938 File Offset: 0x001B7B38
		private bool IsGuest(Pawn p)
		{
			return p.RaceProps.Humanlike && p != this.firstPawn && p != this.secondPawn && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		// Token: 0x060050D6 RID: 20694 RVA: 0x001B9990 File Offset: 0x001B7B90
		private void AddAttendedWeddingThoughts()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && ownedPawns[i].needs.mood != null && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null);
				}
			}
		}

		// Token: 0x04003411 RID: 13329
		public Pawn firstPawn;

		// Token: 0x04003412 RID: 13330
		public Pawn secondPawn;

		// Token: 0x04003413 RID: 13331
		private IntVec3 spot;

		// Token: 0x04003414 RID: 13332
		private Trigger_TicksPassed afterPartyTimeoutTrigger;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000894 RID: 2196
	public class LordJob_Joinable_MarriageCeremony : LordJob_VoluntarilyJoinable
	{
		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x00145D52 File Offset: 0x00143F52
		public override bool LostImportantReferenceDuringLoading
		{
			get
			{
				return this.firstPawn == null || this.secondPawn == null;
			}
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00145BCB File Offset: 0x00143DCB
		public LordJob_Joinable_MarriageCeremony()
		{
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x00145D67 File Offset: 0x00143F67
		public LordJob_Joinable_MarriageCeremony(Pawn firstPawn, Pawn secondPawn, IntVec3 spot)
		{
			this.firstPawn = firstPawn;
			this.secondPawn = secondPawn;
			this.spot = spot;
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00145D84 File Offset: 0x00143F84
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

		// Token: 0x06003A2C RID: 14892 RVA: 0x00146198 File Offset: 0x00144398
		private bool AreFiancesInPartyArea()
		{
			return this.lord.ownedPawns.Contains(this.firstPawn) && this.lord.ownedPawns.Contains(this.secondPawn) && this.firstPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.firstPawn.Position, this.spot, base.Map) && this.secondPawn.Map == base.Map && GatheringsUtility.InGatheringArea(this.secondPawn.Position, this.spot, base.Map);
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x00146240 File Offset: 0x00144440
		private bool ShouldCeremonyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || !this.firstPawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.secondPawn) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.firstPawn, this.secondPawn) || !MarriageCeremonyUtility.FianceCanContinueCeremony(this.secondPawn, this.firstPawn));
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x001462F4 File Offset: 0x001444F4
		private bool ShouldAfterPartyBeCalledOff()
		{
			return this.firstPawn.Destroyed || this.secondPawn.Destroyed || (this.firstPawn.Downed || this.secondPawn.Downed) || (this.spot.GetDangerFor(this.firstPawn, base.Map) != Danger.None || this.spot.GetDangerFor(this.secondPawn, base.Map) != Danger.None) || !GatheringsUtility.AcceptableGameConditionsToContinueGathering(base.Map);
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x00146380 File Offset: 0x00144580
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
					if (lordToil_MarriageCeremony != null && !SpectatorCellFinder.TryFindSpectatorCellFor(p, lordToil_MarriageCeremony.Data.spectateRect, base.Map, out intVec, lordToil_MarriageCeremony.Data.spectateRectAllowedSides, 1, null, null, null))
					{
						return 0f;
					}
				}
				return VoluntarilyJoinableLordJobJoinPriorities.MarriageCeremonyGuest;
			}
		}

		// Token: 0x06003A30 RID: 14896 RVA: 0x00146448 File Offset: 0x00144648
		public override void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.firstPawn, "firstPawn", false);
			Scribe_References.Look<Pawn>(ref this.secondPawn, "secondPawn", false);
			Scribe_Values.Look<IntVec3>(ref this.spot, "spot", default(IntVec3), false);
		}

		// Token: 0x06003A31 RID: 14897 RVA: 0x00146491 File Offset: 0x00144691
		public override string GetReport(Pawn pawn)
		{
			return "LordReportAttendingMarriageCeremony".Translate();
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x001464A2 File Offset: 0x001446A2
		private bool IsCeremonyAboutToEnd()
		{
			return this.afterPartyTimeoutTrigger.TicksLeft < 1200;
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x001464B9 File Offset: 0x001446B9
		private bool IsFiance(Pawn p)
		{
			return p == this.firstPawn || p == this.secondPawn;
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x001464D0 File Offset: 0x001446D0
		private bool IsGuest(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return false;
			}
			if (p == this.firstPawn || p == this.secondPawn)
			{
				return false;
			}
			HistoryEvent ev = new HistoryEvent(this.firstPawn.GetHistoryEventForSpouseCountPlusOne(), p.Named(HistoryEventArgsNames.Doer));
			HistoryEvent ev2 = new HistoryEvent(this.secondPawn.GetHistoryEventForSpouseCountPlusOne(), p.Named(HistoryEventArgsNames.Doer));
			return ev.DoerWillingToDo() && ev2.DoerWillingToDo() && (p.Faction == this.firstPawn.Faction || p.Faction == this.secondPawn.Faction);
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x00146574 File Offset: 0x00144774
		private void AddAttendedWeddingThoughts()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (ownedPawns[i] != this.firstPawn && ownedPawns[i] != this.secondPawn && ownedPawns[i].needs.mood != null && (this.firstPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f) || this.secondPawn.Position.InHorDistOf(ownedPawns[i].Position, 18f)))
				{
					ownedPawns[i].needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.AttendedWedding, null, null);
				}
			}
		}

		// Token: 0x04001FF0 RID: 8176
		public Pawn firstPawn;

		// Token: 0x04001FF1 RID: 8177
		public Pawn secondPawn;

		// Token: 0x04001FF2 RID: 8178
		private IntVec3 spot;

		// Token: 0x04001FF3 RID: 8179
		private Trigger_TicksPassed afterPartyTimeoutTrigger;
	}
}

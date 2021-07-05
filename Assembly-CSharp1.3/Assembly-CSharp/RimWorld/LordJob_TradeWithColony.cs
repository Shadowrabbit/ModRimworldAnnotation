using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000888 RID: 2184
	public class LordJob_TradeWithColony : LordJob
	{
		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060039AA RID: 14762 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_TradeWithColony()
		{
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x00142987 File Offset: 0x00140B87
		public override bool CanOpenAnyDoor(Pawn p)
		{
			return p.RaceProps.FenceBlocked;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x00142999 File Offset: 0x00140B99
		public LordJob_TradeWithColony(Faction faction, IntVec3 chillSpot)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001429B0 File Offset: 0x00140BB0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.chillSpot);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan();
			stateGraph.AddToil(lordToil_DefendTraderCaravan);
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan2 = new LordToil_DefendTraderCaravan(this.chillSpot);
			stateGraph.AddToil(lordToil_DefendTraderCaravan2);
			LordToil_ExitMapAndEscortCarriers lordToil_ExitMapAndEscortCarriers = new LordToil_ExitMapAndEscortCarriers();
			stateGraph.AddToil(lordToil_ExitMapAndEscortCarriers);
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Walk, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			LordToil_ExitMapTraderFighting lordToil_ExitMapTraderFighting = new LordToil_ExitMapTraderFighting();
			stateGraph.AddToil(lordToil_ExitMapTraderFighting);
			Transition transition = new Transition(lordToil_Travel, lordToil_ExitMapAndEscortCarriers, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2
			});
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_Travel, lordToil_ExitMap2, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2,
				lordToil_ExitMapAndEscortCarriers,
				lordToil_ExitMap,
				lordToil_ExitMapTraderFighting
			});
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			transition2.AddPostAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition2.AddPostAction(new TransitionAction_WakeAll());
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ExitMap2, lordToil_ExitMapTraderFighting, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_Travel, lordToil_ExitMapTraderFighting, false, true);
			transition4.AddSources(new LordToil[]
			{
				lordToil_DefendTraderCaravan,
				lordToil_DefendTraderCaravan2,
				lordToil_ExitMapAndEscortCarriers,
				lordToil_ExitMap
			});
			transition4.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition4.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_Travel, lordToil_DefendTraderCaravan, false, true);
			transition5.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			transition5.AddPreAction(new TransitionAction_SetDefendTrader());
			transition5.AddPostAction(new TransitionAction_WakeAll());
			transition5.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_DefendTraderCaravan, lordToil_Travel, false, true);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_Travel, lordToil_DefendTraderCaravan2, false, true);
			transition7.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition7, false);
			Transition transition8 = new Transition(lordToil_DefendTraderCaravan2, lordToil_ExitMapAndEscortCarriers, false, true);
			transition8.AddTrigger(new Trigger_TicksPassed(DebugSettings.instantVisitorsGift ? 0 : Rand.Range(27000, 45000)));
			transition8.AddPreAction(new TransitionAction_CheckGiveGift());
			transition8.AddPreAction(new TransitionAction_Message("MessageTraderCaravanLeaving".Translate(this.faction.Name), null, 1f));
			transition8.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition8, false);
			Transition transition9 = new Transition(lordToil_ExitMapAndEscortCarriers, lordToil_ExitMapAndEscortCarriers, true, true);
			transition9.canMoveToSameState = true;
			transition9.AddTrigger(new Trigger_PawnLost(PawnLostCondition.Undefined, null));
			transition9.AddTrigger(new Trigger_TickCondition(() => LordToil_ExitMapAndEscortCarriers.IsAnyDefendingPosition(this.lord.ownedPawns) && !GenHostility.AnyHostileActiveThreatTo(base.Map, this.faction, true, true), 60));
			stateGraph.AddTransition(transition9, false);
			Transition transition10 = new Transition(lordToil_ExitMapAndEscortCarriers, lordToil_ExitMap, false, true);
			transition10.AddTrigger(new Trigger_TicksPassed(60000));
			transition10.AddPostAction(new TransitionAction_WakeAll());
			stateGraph.AddTransition(transition10, false);
			Transition transition11 = new Transition(lordToil_DefendTraderCaravan2, lordToil_ExitMapAndEscortCarriers, false, true);
			transition11.AddSources(new LordToil[]
			{
				lordToil_Travel,
				lordToil_DefendTraderCaravan
			});
			transition11.AddTrigger(new Trigger_ImportantTraderCaravanPeopleLost());
			transition11.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition11.AddPostAction(new TransitionAction_WakeAll());
			transition11.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition11, false);
			return stateGraph;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00142DD8 File Offset: 0x00140FD8
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
		}

		// Token: 0x04001FA7 RID: 8103
		private Faction faction;

		// Token: 0x04001FA8 RID: 8104
		private IntVec3 chillSpot;
	}
}

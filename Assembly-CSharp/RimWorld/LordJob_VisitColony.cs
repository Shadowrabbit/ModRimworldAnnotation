﻿using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD4 RID: 3540
	public class LordJob_VisitColony : LordJob
	{
		// Token: 0x060050AC RID: 20652 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_VisitColony()
		{
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x00038948 File Offset: 0x00036B48
		public LordJob_VisitColony(Faction faction, IntVec3 chillSpot)
		{
			this.faction = faction;
			this.chillSpot = chillSpot;
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x001B8D34 File Offset: 0x001B6F34
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.chillSpot).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(this.chillSpot, 28f, null);
			stateGraph.AddToil(lordToil_DefendPoint);
			LordToil_TakeWoundedGuest lordToil_TakeWoundedGuest = new LordToil_TakeWoundedGuest();
			stateGraph.AddToil(lordToil_TakeWoundedGuest);
			StateGraph stateGraph2 = new LordJob_TravelAndExit(IntVec3.Invalid).CreateGraph();
			LordToil startingToil2 = stateGraph.AttachSubgraph(stateGraph2).StartingToil;
			LordToil target = stateGraph2.lordToils[1];
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Walk, true, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(startingToil, startingToil2, false, true);
			transition.AddSources(new LordToil[]
			{
				lordToil_DefendPoint
			});
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition2.AddSources(new LordToil[]
			{
				lordToil_DefendPoint,
				lordToil_TakeWoundedGuest
			});
			transition2.AddSources(stateGraph2.lordToils);
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ExitMap, startingToil2, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(startingToil, lordToil_DefendPoint, false, true);
			transition4.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendPoint, lordToil_TakeWoundedGuest, false, true);
			transition5.AddTrigger(new Trigger_WoundedGuestPresent());
			transition5.AddPreAction(new TransitionAction_Message("MessageVisitorsTakingWounded".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_DefendPoint, target, false, true);
			transition6.AddSources(new LordToil[]
			{
				lordToil_TakeWoundedGuest,
				startingToil
			});
			transition6.AddTrigger(new Trigger_BecamePlayerEnemy());
			transition6.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition6.AddPostAction(new TransitionAction_WakeAll());
			transition6.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition6, false);
			Transition transition7 = new Transition(lordToil_DefendPoint, startingToil2, false, true);
			transition7.AddTrigger(new Trigger_TicksPassed(DebugSettings.instantVisitorsGift ? 0 : Rand.Range(8000, 22000)));
			transition7.AddPreAction(new TransitionAction_Message("VisitorsLeaving".Translate(this.faction.Name), null, 1f));
			transition7.AddPreAction(new TransitionAction_CheckGiveGift());
			transition7.AddPostAction(new TransitionAction_WakeAll());
			transition7.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition7, false);
			return stateGraph;
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x001B90B4 File Offset: 0x001B72B4
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.chillSpot, "chillSpot", default(IntVec3), false);
		}

		// Token: 0x04003409 RID: 13321
		private Faction faction;

		// Token: 0x0400340A RID: 13322
		private IntVec3 chillSpot;
	}
}

using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200086E RID: 2158
	public class LordJob_AssistColony : LordJob
	{
		// Token: 0x060038F3 RID: 14579 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_AssistColony()
		{
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0013EE20 File Offset: 0x0013D020
		public LordJob_AssistColony(Faction faction, IntVec3 fallbackLocation)
		{
			this.faction = faction;
			this.fallbackLocation = fallbackLocation;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0013EE38 File Offset: 0x0013D038
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_HuntEnemies lordToil_HuntEnemies = new LordToil_HuntEnemies(this.fallbackLocation);
			stateGraph.AddToil(lordToil_HuntEnemies);
			StateGraph stateGraph2 = new LordJob_Travel(IntVec3.Invalid).CreateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(stateGraph2).StartingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			LordToil_ExitMap lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog, true, false);
			stateGraph.AddToil(lordToil_ExitMap2);
			Transition transition = new Transition(lordToil_HuntEnemies, startingToil, false, true);
			transition.AddPreAction(new TransitionAction_Message("MessageVisitorsDangerousTemperature".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			transition.AddTrigger(new Trigger_PawnExperiencingDangerousTemperatures());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_HuntEnemies, lordToil_ExitMap2, false, true);
			transition2.AddSource(lordToil_ExitMap);
			transition2.AddSources(stateGraph2.lordToils);
			transition2.AddPreAction(new TransitionAction_Message("MessageVisitorsTrappedLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_ExitMap2, startingToil, false, true);
			transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
			transition3.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_HuntEnemies, startingToil, false, true);
			transition4.AddPreAction(new TransitionAction_Message("MessageFriendlyFightersLeaving".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
			transition4.AddTrigger(new Trigger_TicksPassed(25000));
			transition4.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(startingToil, lordToil_ExitMap, false, true);
			transition5.AddTrigger(new Trigger_Memo("TravelArrived"));
			stateGraph.AddTransition(transition5, false);
			return stateGraph;
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x0013F070 File Offset: 0x0013D270
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.fallbackLocation, "fallbackLocation", default(IntVec3), false);
		}

		// Token: 0x04001F47 RID: 8007
		private Faction faction;

		// Token: 0x04001F48 RID: 8008
		private IntVec3 fallbackLocation;
	}
}

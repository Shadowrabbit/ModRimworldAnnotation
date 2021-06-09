using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DBE RID: 3518
	public class LordJob_DefendBase : LordJob
	{
		// Token: 0x0600502D RID: 20525 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_DefendBase()
		{
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x0003846A File Offset: 0x0003666A
		public LordJob_DefendBase(Faction faction, IntVec3 baseCenter)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x001B7030 File Offset: 0x001B5230
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_DefendBase lordToil_DefendBase2 = new LordToil_DefendBase(this.baseCenter);
			stateGraph.AddToil(lordToil_DefendBase2);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(true);
			lordToil_AssaultColony.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2, false, true);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendBase2, lordToil_DefendBase, false, true);
			transition2.AddTrigger(new Trigger_BecamePlayerEnemy());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendBase, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_FractionPawnsLost(0.2f));
			transition3.AddTrigger(new Trigger_PawnHarmed(0.4f, false, null));
			transition3.AddTrigger(new Trigger_ChanceOnTickInteval(2500, 0.03f));
			transition3.AddTrigger(new Trigger_TicksPassed(251999));
			transition3.AddTrigger(new Trigger_UrgentlyHungry());
			transition3.AddTrigger(new Trigger_ChanceOnPlayerHarmNPCBuilding(0.4f));
			transition3.AddTrigger(new Trigger_OnClamor(ClamorDefOf.Ability));
			transition3.AddPostAction(new TransitionAction_WakeAll());
			TaggedString taggedString = "MessageDefendersAttacking".Translate(this.faction.def.pawnsPlural, this.faction.Name, Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst();
			transition3.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x001B71C8 File Offset: 0x001B53C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
		}

		// Token: 0x040033CE RID: 13262
		private Faction faction;

		// Token: 0x040033CF RID: 13263
		private IntVec3 baseCenter;
	}
}

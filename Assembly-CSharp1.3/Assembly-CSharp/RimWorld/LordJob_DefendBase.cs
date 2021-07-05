using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000877 RID: 2167
	public class LordJob_DefendBase : LordJob
	{
		// Token: 0x06003933 RID: 14643 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_DefendBase()
		{
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x001405EB File Offset: 0x0013E7EB
		public LordJob_DefendBase(Faction faction, IntVec3 baseCenter, bool attackWhenPlayerBecameEnemy = false)
		{
			this.faction = faction;
			this.baseCenter = baseCenter;
			this.attackWhenPlayerBecameEnemy = attackWhenPlayerBecameEnemy;
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00140608 File Offset: 0x0013E808
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendBase lordToil_DefendBase = new LordToil_DefendBase(this.baseCenter);
			stateGraph.StartingToil = lordToil_DefendBase;
			LordToil_DefendBase lordToil_DefendBase2 = new LordToil_DefendBase(this.baseCenter);
			stateGraph.AddToil(lordToil_DefendBase2);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(true, false);
			lordToil_AssaultColony.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendBase, lordToil_DefendBase2, false, true);
			transition.AddSource(lordToil_AssaultColony);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendBase2, this.attackWhenPlayerBecameEnemy ? lordToil_AssaultColony : lordToil_DefendBase, false, true);
			if (this.attackWhenPlayerBecameEnemy)
			{
				transition2.AddSource(lordToil_DefendBase);
			}
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
			transition3.AddPreAction(new TransitionAction_Message(taggedString, MessageTypeDefOf.ThreatBig, null, 1f, null));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x001407BC File Offset: 0x0013E9BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<IntVec3>(ref this.baseCenter, "baseCenter", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.attackWhenPlayerBecameEnemy, "attackWhenPlayerBecameEnemy", false, false);
		}

		// Token: 0x04001F6D RID: 8045
		private Faction faction;

		// Token: 0x04001F6E RID: 8046
		private IntVec3 baseCenter;

		// Token: 0x04001F6F RID: 8047
		private bool attackWhenPlayerBecameEnemy;

		// Token: 0x04001F70 RID: 8048
		private List<Pawn> tmpPawns;

		// Token: 0x04001F71 RID: 8049
		private List<IntVec3> tmpCells;
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000875 RID: 2165
	public class LordJob_DefendAndExpandHive : LordJob
	{
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003929 RID: 14633 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanBlockHostileVisitors
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x0600392A RID: 14634 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_DefendAndExpandHive()
		{
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x0014031F File Offset: 0x0013E51F
		public LordJob_DefendAndExpandHive(SpawnedPawnParams parms)
		{
			this.aggressive = parms.aggressive;
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x00140334 File Offset: 0x0013E534
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendAndExpandHive lordToil_DefendAndExpandHive = new LordToil_DefendAndExpandHive();
			lordToil_DefendAndExpandHive.distToHiveToAttack = 10f;
			stateGraph.StartingToil = lordToil_DefendAndExpandHive;
			LordToil_DefendHiveAggressively lordToil_DefendHiveAggressively = new LordToil_DefendHiveAggressively();
			lordToil_DefendHiveAggressively.distToHiveToAttack = 40f;
			stateGraph.AddToil(lordToil_DefendHiveAggressively);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition = new Transition(lordToil_DefendAndExpandHive, this.aggressive ? lordToil_AssaultColony : lordToil_DefendHiveAggressively, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(0.5f, true, null));
			transition.AddTrigger(new Trigger_PawnLostViolently(false));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoAttackedByEnemy));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoBurnedBadly));
			transition.AddTrigger(new Trigger_Memo(Hive.MemoDestroyedNonRoofCollapse));
			transition.AddTrigger(new Trigger_Memo(HediffGiver_Heat.MemoPawnBurnedByAir));
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendAndExpandHive, lordToil_AssaultColony, false, true);
			transition2.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition2.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition2, false);
			Transition transition3 = new Transition(lordToil_DefendHiveAggressively, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_PawnHarmed(0.5f, false, base.Map.ParentFaction));
			transition3.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition3, false);
			Transition transition4 = new Transition(lordToil_DefendAndExpandHive, lordToil_DefendAndExpandHive, true, true);
			transition4.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition4, false);
			Transition transition5 = new Transition(lordToil_DefendHiveAggressively, lordToil_DefendHiveAggressively, true, true);
			transition5.AddTrigger(new Trigger_Memo(Hive.MemoDeSpawned));
			stateGraph.AddTransition(transition5, false);
			Transition transition6 = new Transition(lordToil_AssaultColony, lordToil_DefendAndExpandHive, false, true);
			transition6.AddSource(lordToil_DefendHiveAggressively);
			transition6.AddTrigger(new Trigger_TicksPassedWithoutHarmOrMemos(1200, new string[]
			{
				Hive.MemoAttackedByEnemy,
				Hive.MemoBurnedBadly,
				Hive.MemoDestroyedNonRoofCollapse,
				Hive.MemoDeSpawned,
				HediffGiver_Heat.MemoPawnBurnedByAir
			}));
			transition6.AddPostAction(new TransitionAction_EndAttackBuildingJobs());
			stateGraph.AddTransition(transition6, false);
			return stateGraph;
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x00140541 File Offset: 0x0013E741
		public override void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
		}

		// Token: 0x04001F6B RID: 8043
		private bool aggressive;
	}
}

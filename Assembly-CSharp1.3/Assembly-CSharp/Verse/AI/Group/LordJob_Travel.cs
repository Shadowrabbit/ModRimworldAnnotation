using System;

namespace Verse.AI.Group
{
	// Token: 0x02000665 RID: 1637
	public class LordJob_Travel : LordJob
	{
		// Token: 0x06002E7C RID: 11900 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_Travel()
		{
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x0011633B File Offset: 0x0011453B
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x0011634C File Offset: 0x0011454C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_Travel lordToil_Travel = new LordToil_Travel(this.travelDest);
			stateGraph.StartingToil = lordToil_Travel;
			LordToil_DefendPoint lordToil_DefendPoint = new LordToil_DefendPoint(false);
			stateGraph.AddToil(lordToil_DefendPoint);
			Transition transition = new Transition(lordToil_Travel, lordToil_DefendPoint, false, true);
			transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
			transition.AddPreAction(new TransitionAction_SetDefendLocalGroup());
			transition.AddPostAction(new TransitionAction_EndAllJobs());
			stateGraph.AddTransition(transition, false);
			Transition transition2 = new Transition(lordToil_DefendPoint, lordToil_Travel, false, true);
			transition2.AddTrigger(new Trigger_TicksPassedWithoutHarm(1200));
			transition2.AddPreAction(new TransitionAction_EnsureHaveExitDestination());
			stateGraph.AddTransition(transition2, false);
			return stateGraph;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x001163E8 File Offset: 0x001145E8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04001C91 RID: 7313
		private IntVec3 travelDest;
	}
}

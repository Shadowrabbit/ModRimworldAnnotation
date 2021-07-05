using System;

namespace Verse.AI.Group
{
	// Token: 0x02000ACA RID: 2762
	public class LordJob_Travel : LordJob
	{
		// Token: 0x0600415F RID: 16735 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_Travel()
		{
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00030C49 File Offset: 0x0002EE49
		public LordJob_Travel(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06004161 RID: 16737 RVA: 0x0018768C File Offset: 0x0018588C
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

		// Token: 0x06004162 RID: 16738 RVA: 0x00187728 File Offset: 0x00185928
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002D21 RID: 11553
		private IntVec3 travelDest;
	}
}

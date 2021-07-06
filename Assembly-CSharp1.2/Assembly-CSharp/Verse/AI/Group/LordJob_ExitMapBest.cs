using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AC8 RID: 2760
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06004157 RID: 16727 RVA: 0x00030B9B File Offset: 0x0002ED9B
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x00030BAA File Offset: 0x0002EDAA
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00187544 File Offset: 0x00185744
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(this.locomotion, this.canDig, false);
			lordToil_ExitMap.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_ExitMap);
			if (this.canDefendSelf)
			{
				LordToil_ExitMapFighting lordToil_ExitMapFighting = new LordToil_ExitMapFighting(LocomotionUrgency.Jog, this.canDig, false);
				stateGraph.AddToil(lordToil_ExitMapFighting);
				Transition transition = new Transition(lordToil_ExitMap, lordToil_ExitMapFighting, false, true);
				transition.AddTrigger(new Trigger_PawnHarmed(1f, false, null));
				transition.AddPostAction(new TransitionAction_WakeAll());
				transition.AddPostAction(new TransitionAction_EndAllJobs());
				stateGraph.AddTransition(transition, false);
			}
			return stateGraph;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00030BCE File Offset: 0x0002EDCE
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		// Token: 0x04002D18 RID: 11544
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002D19 RID: 11545
		private bool canDig;

		// Token: 0x04002D1A RID: 11546
		private bool canDefendSelf;
	}
}

using System;

namespace Verse.AI.Group
{
	// Token: 0x02000663 RID: 1635
	public class LordJob_ExitMapBest : LordJob
	{
		// Token: 0x06002E74 RID: 11892 RVA: 0x00116145 File Offset: 0x00114345
		public LordJob_ExitMapBest()
		{
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x00116154 File Offset: 0x00114354
		public LordJob_ExitMapBest(LocomotionUrgency locomotion, bool canDig = false, bool canDefendSelf = false)
		{
			this.locomotion = locomotion;
			this.canDig = canDig;
			this.canDefendSelf = canDefendSelf;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x00116178 File Offset: 0x00114378
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

		// Token: 0x06002E77 RID: 11895 RVA: 0x00116204 File Offset: 0x00114404
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.canDefendSelf, "canDefendSelf", false, false);
		}

		// Token: 0x04001C88 RID: 7304
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04001C89 RID: 7305
		private bool canDig;

		// Token: 0x04001C8A RID: 7306
		private bool canDefendSelf;
	}
}

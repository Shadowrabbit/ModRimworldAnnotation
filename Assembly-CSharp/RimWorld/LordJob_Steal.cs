using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD2 RID: 3538
	public class LordJob_Steal : LordJob
	{
		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060050A4 RID: 20644 RVA: 0x001B8870 File Offset: 0x001B6A70
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_StealCover lordToil_StealCover = new LordToil_StealCover();
			lordToil_StealCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover);
			LordToil_StealCover lordToil_StealCover2 = new LordToil_StealCover();
			lordToil_StealCover2.cover = false;
			lordToil_StealCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_StealCover2);
			Transition transition = new Transition(lordToil_StealCover, lordToil_StealCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassedAndNoRecentHarm(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}
	}
}

using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200087B RID: 2171
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x00141380 File Offset: 0x0013F580
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_KidnapCover lordToil_KidnapCover = new LordToil_KidnapCover();
			lordToil_KidnapCover.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover);
			LordToil_KidnapCover lordToil_KidnapCover2 = new LordToil_KidnapCover();
			lordToil_KidnapCover2.cover = false;
			lordToil_KidnapCover2.useAvoidGrid = true;
			stateGraph.AddToil(lordToil_KidnapCover2);
			Transition transition = new Transition(lordToil_KidnapCover, lordToil_KidnapCover2, false, true);
			transition.AddTrigger(new Trigger_TicksPassed(1200));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0000313F File Offset: 0x0000133F
		public override void ExposeData()
		{
		}
	}
}

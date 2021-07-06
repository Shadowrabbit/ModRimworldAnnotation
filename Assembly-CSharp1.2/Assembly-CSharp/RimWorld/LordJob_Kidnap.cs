using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC3 RID: 3523
	public class LordJob_Kidnap : LordJob
	{
		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06005058 RID: 20568 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool GuiltyOnDowned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x001B79C0 File Offset: 0x001B5BC0
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

		// Token: 0x0600505B RID: 20571 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void ExposeData()
		{
		}
	}
}

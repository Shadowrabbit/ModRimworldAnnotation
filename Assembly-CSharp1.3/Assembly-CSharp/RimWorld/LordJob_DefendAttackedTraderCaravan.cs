using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000876 RID: 2166
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x0600392F RID: 14639 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x00140555 File Offset: 0x0013E755
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x00140564 File Offset: 0x0013E764
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_DefendTraderCaravan lordToil_DefendTraderCaravan = new LordToil_DefendTraderCaravan(this.defendSpot);
			stateGraph.StartingToil = lordToil_DefendTraderCaravan;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			Transition transition = new Transition(lordToil_DefendTraderCaravan, lordToil_ExitMap, false, true);
			transition.AddTrigger(new Trigger_BecameNonHostileToPlayer());
			transition.AddTrigger(new Trigger_TraderAndAllTraderCaravanGuardsLost());
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x001405C4 File Offset: 0x0013E7C4
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x04001F6C RID: 8044
		private IntVec3 defendSpot;
	}
}

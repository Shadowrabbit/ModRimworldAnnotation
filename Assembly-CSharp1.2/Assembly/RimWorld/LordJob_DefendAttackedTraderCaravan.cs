using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DBD RID: 3517
	public class LordJob_DefendAttackedTraderCaravan : LordJob
	{
		// Token: 0x06005029 RID: 20521 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_DefendAttackedTraderCaravan()
		{
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x0003845B File Offset: 0x0003665B
		public LordJob_DefendAttackedTraderCaravan(IntVec3 defendSpot)
		{
			this.defendSpot = defendSpot;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x001B6FA8 File Offset: 0x001B51A8
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

		// Token: 0x0600502C RID: 20524 RVA: 0x001B7008 File Offset: 0x001B5208
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.defendSpot, "defendSpot", default(IntVec3), false);
		}

		// Token: 0x040033CD RID: 13261
		private IntVec3 defendSpot;
	}
}

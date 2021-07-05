using System;

namespace Verse.AI.Group
{
	// Token: 0x02000666 RID: 1638
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x06002E80 RID: 11904 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0011640F File Offset: 0x0011460F
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x00116420 File Offset: 0x00114620
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Travel(this.travelDest).CreateGraph()).StartingToil;
			stateGraph.StartingToil = startingToil;
			LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.None, false, false);
			stateGraph.AddToil(lordToil_ExitMap);
			stateGraph.AddTransition(new Transition(startingToil, lordToil_ExitMap, false, true)
			{
				triggers = 
				{
					new Trigger_Memo("TravelArrived")
				}
			}, false);
			return stateGraph;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x0011648C File Offset: 0x0011468C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04001C92 RID: 7314
		private IntVec3 travelDest;
	}
}

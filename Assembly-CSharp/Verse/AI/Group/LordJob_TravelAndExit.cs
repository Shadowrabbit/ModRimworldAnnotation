using System;

namespace Verse.AI.Group
{
	// Token: 0x02000ACB RID: 2763
	public class LordJob_TravelAndExit : LordJob
	{
		// Token: 0x06004163 RID: 16739 RVA: 0x00030B4B File Offset: 0x0002ED4B
		public LordJob_TravelAndExit()
		{
		}

		// Token: 0x06004164 RID: 16740 RVA: 0x00030C58 File Offset: 0x0002EE58
		public LordJob_TravelAndExit(IntVec3 travelDest)
		{
			this.travelDest = travelDest;
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x00187750 File Offset: 0x00185950
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

		// Token: 0x06004166 RID: 16742 RVA: 0x001877BC File Offset: 0x001859BC
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.travelDest, "travelDest", default(IntVec3), false);
		}

		// Token: 0x04002D22 RID: 11554
		private IntVec3 travelDest;
	}
}

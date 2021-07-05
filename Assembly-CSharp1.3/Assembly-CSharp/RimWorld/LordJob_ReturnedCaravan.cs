using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000881 RID: 2177
	public class LordJob_ReturnedCaravan : LordJob
	{
		// Token: 0x06003980 RID: 14720 RVA: 0x00141CF6 File Offset: 0x0013FEF6
		public LordJob_ReturnedCaravan(IntVec3 entryPoint)
		{
			this.entryPoint = entryPoint;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_ReturnedCaravan()
		{
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x00141D08 File Offset: 0x0013FF08
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ReturnedCaravan_PenAnimals lordToil_ReturnedCaravan_PenAnimals = new LordToil_ReturnedCaravan_PenAnimals(this.entryPoint);
			stateGraph.AddToil(lordToil_ReturnedCaravan_PenAnimals);
			LordToil_End lordToil_End = new LordToil_End();
			stateGraph.AddToil(lordToil_End);
			Transition transition = new Transition(lordToil_ReturnedCaravan_PenAnimals, lordToil_End, false, true);
			transition.AddTrigger(new Trigger_Memo("RepenningFinished"));
			stateGraph.AddTransition(transition, false);
			return stateGraph;
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x00141D5C File Offset: 0x0013FF5C
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.entryPoint, "entryPoint", default(IntVec3), false);
		}

		// Token: 0x04001F97 RID: 8087
		private IntVec3 entryPoint;
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200088B RID: 2187
	public class LordJob_WaitForEscort : LordJob
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x00143508 File Offset: 0x00141708
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00143510 File Offset: 0x00141710
		public LordJob_WaitForEscort()
		{
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x0014351F File Offset: 0x0014171F
		public LordJob_WaitForEscort(IntVec3 point, bool addFleeToil = true)
		{
			this.point = point;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x0014353C File Offset: 0x0014173C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.CheckRoyalty("Shuttle crash rescue"))
			{
				return stateGraph;
			}
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			return stateGraph;
		}

		// Token: 0x060039BE RID: 14782 RVA: 0x00143578 File Offset: 0x00141778
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04001FB2 RID: 8114
		public IntVec3 point;

		// Token: 0x04001FB3 RID: 8115
		private bool addFleeToil = true;
	}
}

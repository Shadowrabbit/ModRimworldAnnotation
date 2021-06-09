using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DD5 RID: 3541
	public class LordJob_WaitForEscort : LordJob
	{
		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x060050B0 RID: 20656 RVA: 0x0003895E File Offset: 0x00036B5E
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x00038966 File Offset: 0x00036B66
		public LordJob_WaitForEscort()
		{
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x00038975 File Offset: 0x00036B75
		public LordJob_WaitForEscort(IntVec3 point, bool addFleeToil = true)
		{
			this.point = point;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001B90EC File Offset: 0x001B72EC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Shuttle crash rescue is a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 3454535, false);
				return stateGraph;
			}
			LordToil_WanderClose lordToil_WanderClose = new LordToil_WanderClose(this.point);
			stateGraph.AddToil(lordToil_WanderClose);
			stateGraph.StartingToil = lordToil_WanderClose;
			return stateGraph;
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x001B9134 File Offset: 0x001B7334
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x0400340B RID: 13323
		public IntVec3 point;

		// Token: 0x0400340C RID: 13324
		private bool addFleeToil = true;
	}
}

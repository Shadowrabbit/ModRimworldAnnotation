using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000DC4 RID: 3524
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x0600505C RID: 20572 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x0600505D RID: 20573 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x000386AC File Offset: 0x000368AC
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x000386BB File Offset: 0x000368BB
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x000386D1 File Offset: 0x000368D1
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x001B7A24 File Offset: 0x001B5C24
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x040033E6 RID: 13286
		public int transportersGroup = -1;
	}
}

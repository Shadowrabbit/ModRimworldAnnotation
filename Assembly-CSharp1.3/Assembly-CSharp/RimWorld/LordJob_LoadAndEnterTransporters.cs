using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200087C RID: 2172
	public class LordJob_LoadAndEnterTransporters : LordJob
	{
		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x001413E3 File Offset: 0x0013F5E3
		public LordJob_LoadAndEnterTransporters()
		{
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x001413F2 File Offset: 0x0013F5F2
		public LordJob_LoadAndEnterTransporters(int transportersGroup)
		{
			this.transportersGroup = transportersGroup;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x00141408 File Offset: 0x0013F608
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", 0, false);
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0014141C File Offset: 0x0013F61C
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterTransporters startingToil = new LordToil_LoadAndEnterTransporters(this.transportersGroup);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x04001F89 RID: 8073
		public int transportersGroup = -1;
	}
}

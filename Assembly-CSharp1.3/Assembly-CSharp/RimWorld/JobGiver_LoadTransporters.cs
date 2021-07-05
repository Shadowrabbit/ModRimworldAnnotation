using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020007B1 RID: 1969
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		// Token: 0x06003571 RID: 13681 RVA: 0x0012E2F8 File Offset: 0x0012C4F8
		protected override Job TryGiveJob(Pawn pawn)
		{
			TransporterUtility.GetTransportersInGroup(pawn.mindState.duty.transportersGroup, pawn.Map, JobGiver_LoadTransporters.tmpTransporters);
			for (int i = 0; i < JobGiver_LoadTransporters.tmpTransporters.Count; i++)
			{
				CompTransporter transporter = JobGiver_LoadTransporters.tmpTransporters[i];
				if (LoadTransportersJobUtility.HasJobOnTransporter(pawn, transporter))
				{
					return LoadTransportersJobUtility.JobOnTransporter(pawn, transporter);
				}
			}
			return null;
		}

		// Token: 0x04001E98 RID: 7832
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000CC1 RID: 3265
	public class JobGiver_LoadTransporters : ThinkNode_JobGiver
	{
		// Token: 0x06004B99 RID: 19353 RVA: 0x001A62EC File Offset: 0x001A44EC
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

		// Token: 0x040031E6 RID: 12774
		private static List<CompTransporter> tmpTransporters = new List<CompTransporter>();
	}
}

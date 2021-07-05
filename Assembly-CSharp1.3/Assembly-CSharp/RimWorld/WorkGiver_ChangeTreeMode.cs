using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000855 RID: 2133
	public class WorkGiver_ChangeTreeMode : WorkGiver_Scanner
	{
		// Token: 0x0600385F RID: 14431 RVA: 0x0013D00E File Offset: 0x0013B20E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x0013D028 File Offset: 0x0013B228
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.Mode != compTreeConnection.desiredMode;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0013D062 File Offset: 0x0013B262
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ChangeTreeMode, t);
			job.playerForced = forced;
			return job;
		}
	}
}

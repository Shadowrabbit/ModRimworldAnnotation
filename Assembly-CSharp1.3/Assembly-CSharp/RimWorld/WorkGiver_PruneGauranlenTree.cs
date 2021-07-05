using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000856 RID: 2134
	public class WorkGiver_PruneGauranlenTree : WorkGiver_Scanner
	{
		// Token: 0x06003863 RID: 14435 RVA: 0x0013D00E File Offset: 0x0013B20E
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner));
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x0013D07C File Offset: 0x0013B27C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
			return compTreeConnection != null && compTreeConnection.ConnectedPawn == pawn && compTreeConnection.ShouldBePrunedNow(forced) && !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0013D0D2 File Offset: 0x0013B2D2
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.PruneGauranlenTree, t);
			job.playerForced = forced;
			return job;
		}

		// Token: 0x04001F32 RID: 7986
		public const float MaxConnectionStrengthForAutoPruning = 0.99f;
	}
}

using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000869 RID: 2153
	public class WorkGiver_UnloadCarriers : WorkGiver_Scanner
	{
		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060038C9 RID: 14537 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x0013DE30 File Offset: 0x0013C030
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].inventory.UnloadEverything)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x0013DE75 File Offset: 0x0013C075
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveInventoryUnloaded;
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x0013DE87 File Offset: 0x0013C087
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x0013DE91 File Offset: 0x0013C091
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.UnloadInventory, t);
		}
	}
}

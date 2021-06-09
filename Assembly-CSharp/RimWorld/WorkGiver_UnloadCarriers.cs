using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DAC RID: 3500
	public class WorkGiver_UnloadCarriers : WorkGiver_Scanner
	{
		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004FC8 RID: 20424 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x001B537C File Offset: 0x001B357C
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

		// Token: 0x06004FCB RID: 20427 RVA: 0x000380A3 File Offset: 0x000362A3
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWhoShouldHaveInventoryUnloaded;
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x000380B5 File Offset: 0x000362B5
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return UnloadCarriersJobGiverUtility.HasJobOnThing(pawn, t, forced);
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x000380BF File Offset: 0x000362BF
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.UnloadInventory, t);
		}
	}
}

using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D5E RID: 3422
	public abstract class WorkGiver_Warden : WorkGiver_Scanner
	{
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x0003738E File Offset: 0x0003558E
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06004E34 RID: 20020 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x00037397 File Offset: 0x00035597
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.PrisonersOfColonySpawned;
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x000373A9 File Offset: 0x000355A9
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.mapPawns.PrisonersOfColonySpawnedCount == 0;
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x000373BE File Offset: 0x000355BE
		[Obsolete("Will be removed in the future")]
		protected bool ShouldTakeCareOfPrisoner(Pawn warden, Thing prisoner)
		{
			return this.ShouldTakeCareOfPrisoner_NewTemp(warden, prisoner, false);
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x001B07C0 File Offset: 0x001AE9C0
		protected bool ShouldTakeCareOfPrisoner_NewTemp(Pawn warden, Thing prisoner, bool forced = false)
		{
			Pawn pawn = prisoner as Pawn;
			return pawn != null && pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure && pawn.Spawned && !pawn.InAggroMentalState && !prisoner.IsForbidden(warden) && !pawn.IsFormingCaravan() && warden.CanReserveAndReach(pawn, PathEndMode.OnCell, warden.NormalMaxDanger(), 1, -1, null, forced);
		}
	}
}

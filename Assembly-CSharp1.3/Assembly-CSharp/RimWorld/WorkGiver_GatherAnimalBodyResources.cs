using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000805 RID: 2053
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x060036CE RID: 14030
		protected abstract JobDef JobDef { get; }

		// Token: 0x060036CF RID: 14031
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x060036D0 RID: 14032 RVA: 0x00136C3C File Offset: 0x00134E3C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x00136C54 File Offset: 0x00134E54
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].RaceProps.Animal)
				{
					CompHasGatherableBodyResource comp = this.GetComp(list[i]);
					if (comp != null && comp.ActiveAndFull)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x00136CB8 File Offset: 0x00134EB8
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			CompHasGatherableBodyResource comp = this.GetComp(pawn2);
			return comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false, false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x00136D18 File Offset: 0x00134F18
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.JobDef, t);
		}
	}
}

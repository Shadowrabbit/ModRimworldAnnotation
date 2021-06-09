using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D40 RID: 3392
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x06004D8A RID: 19850
		protected abstract JobDef JobDef { get; }

		// Token: 0x06004D8B RID: 19851
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06004D8C RID: 19852 RVA: 0x00036D8B File Offset: 0x00034F8B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x001AEF08 File Offset: 0x001AD108
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

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004D8E RID: 19854 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x001AEF6C File Offset: 0x001AD16C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			CompHasGatherableBodyResource comp = this.GetComp(pawn2);
			return comp != null && comp.ActiveAndFull && !pawn2.Downed && pawn2.CanCasuallyInteractNow(false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x00036DA3 File Offset: 0x00034FA3
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.JobDef, t);
		}
	}
}

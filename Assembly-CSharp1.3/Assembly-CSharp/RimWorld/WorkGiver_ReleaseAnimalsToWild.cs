using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000808 RID: 2056
	public class WorkGiver_ReleaseAnimalsToWild : WorkGiver_Scanner
	{
		// Token: 0x060036E0 RID: 14048 RVA: 0x00136FB1 File Offset: 0x001351B1
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.ReleaseAnimalToWild))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060036E1 RID: 14049 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060036E2 RID: 14050 RVA: 0x00136FC1 File Offset: 0x001351C1
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ReleaseAnimalToWild);
		}

		// Token: 0x060036E3 RID: 14051 RVA: 0x00136FDC File Offset: 0x001351DC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) == null)
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState || pawn2.Dead)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			IntVec3 intVec;
			if (!JobDriver_ReleaseAnimalToWild.TryFindClosestOutsideCell(t.Position, t.Map, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), out intVec))
			{
				JobFailReason.Is("NoReachableOutsideCell".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x0013708A File Offset: 0x0013528A
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ReleaseAnimalToWild, t);
			job.count = 1;
			return job;
		}
	}
}

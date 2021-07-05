using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200080E RID: 2062
	public class WorkGiver_Tame : WorkGiver_InteractAnimal
	{
		// Token: 0x060036F6 RID: 14070 RVA: 0x001373DB File Offset: 0x001355DB
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Tame))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060036F7 RID: 14071 RVA: 0x001373EB File Offset: 0x001355EB
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Tame);
		}

		// Token: 0x060036F8 RID: 14072 RVA: 0x00137408 File Offset: 0x00135608
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !TameUtility.CanTame(pawn2))
			{
				return null;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Tame) == null)
			{
				return null;
			}
			if (TameUtility.TriedToTameTooRecently(pawn2))
			{
				JobFailReason.Is(WorkGiver_InteractAnimal.AnimalInteractedTooRecentlyTrans, null);
				return null;
			}
			if (!this.CanInteractWithAnimal(pawn, pawn2, forced))
			{
				return null;
			}
			if (pawn2.RaceProps.EatsFood && !base.HasFoodToInteractAnimal(pawn, pawn2))
			{
				Job job = base.TakeFoodForAnimalInteractJob(pawn, pawn2);
				if (job == null)
				{
					JobFailReason.Is(WorkGiver_InteractAnimal.NoUsableFoodTrans, null);
				}
				return job;
			}
			return JobMaker.MakeJob(JobDefOf.Tame, t);
		}
	}
}

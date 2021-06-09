using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D93 RID: 3475
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		// Token: 0x06004F3C RID: 20284 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x00037C1B File Offset: 0x00035E1B
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			int num;
			for (int i = 0; i < desList.Count; i = num + 1)
			{
				Designation designation = desList[i];
				if (designation.def == DesignationDefOf.CutPlant || designation.def == DesignationDefOf.HarvestPlant)
				{
					yield return designation.target.Thing;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x00037C2B File Offset: 0x00035E2B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004F3F RID: 20287 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x001B451C File Offset: 0x001B271C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (t.def.category != ThingCategory.Plant)
			{
				return null;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return null;
			}
			if (t.IsForbidden(pawn))
			{
				return null;
			}
			if (t.IsBurning())
			{
				return null;
			}
			foreach (Designation designation in pawn.Map.designationManager.AllDesignationsOn(t))
			{
				if (designation.def == DesignationDefOf.HarvestPlant)
				{
					if (!((Plant)t).HarvestableNow)
					{
						return null;
					}
					return JobMaker.MakeJob(JobDefOf.HarvestDesignated, t);
				}
				else if (designation.def == DesignationDefOf.CutPlant)
				{
					return JobMaker.MakeJob(JobDefOf.CutPlantDesignated, t);
				}
			}
			return null;
		}
	}
}

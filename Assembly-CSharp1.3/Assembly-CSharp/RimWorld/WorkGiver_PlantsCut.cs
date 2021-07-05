using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000854 RID: 2132
	public class WorkGiver_PlantsCut : WorkGiver_Scanner
	{
		// Token: 0x06003858 RID: 14424 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0013CE8B File Offset: 0x0013B08B
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

		// Token: 0x0600385A RID: 14426 RVA: 0x0013CE9B File Offset: 0x0013B09B
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.CutPlant) && !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.HarvestPlant);
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600385B RID: 14427 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x0013CED0 File Offset: 0x0013B0D0
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
			if (!PlantUtility.PawnWillingToCutPlant_Job(t, pawn))
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

		// Token: 0x0600385D RID: 14429 RVA: 0x0013CFBC File Offset: 0x0013B1BC
		public override string PostProcessedGerund(Job job)
		{
			if (job.def == JobDefOf.HarvestDesignated)
			{
				return "HarvestGerund".Translate();
			}
			if (job.def == JobDefOf.CutPlantDesignated)
			{
				return "CutGerund".Translate();
			}
			return this.def.gerund;
		}
	}
}

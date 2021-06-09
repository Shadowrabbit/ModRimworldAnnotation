using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DAA RID: 3498
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x0001B6B4 File Offset: 0x000198B4
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x0002EB44 File Offset: 0x0002CD44
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x0003800C File Offset: 0x0003620C
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			List<Designation> desList = pawn.Map.designationManager.allDesignations;
			int num;
			for (int i = 0; i < desList.Count; i = num + 1)
			{
				if (desList[i].def == DesignationDefOf.Flick)
				{
					yield return desList[i].target.Thing;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x0003801C File Offset: 0x0003621C
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x00038036 File Offset: 0x00036236
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x00038067 File Offset: 0x00036267
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Flick, t);
		}
	}
}

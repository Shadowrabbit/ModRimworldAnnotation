using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000868 RID: 2152
	public class WorkGiver_Flick : WorkGiver_Scanner
	{
		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x0009007E File Offset: 0x0008E27E
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0013DDC1 File Offset: 0x0013BFC1
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

		// Token: 0x060038C5 RID: 14533 RVA: 0x0013DDD1 File Offset: 0x0013BFD1
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Flick);
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x0013DDEB File Offset: 0x0013BFEB
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick) != null && pawn.CanReserve(t, 1, -1, null, forced);
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0013DE1C File Offset: 0x0013C01C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Flick, t);
		}
	}
}

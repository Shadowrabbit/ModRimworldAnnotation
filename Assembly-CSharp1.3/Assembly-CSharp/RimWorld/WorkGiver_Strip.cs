using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200085F RID: 2143
	public class WorkGiver_Strip : WorkGiver_Scanner
	{
		// Token: 0x06003898 RID: 14488 RVA: 0x0013D926 File Offset: 0x0013BB26
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Strip))
			{
				if (!designation.target.HasThing)
				{
					Log.ErrorOnce("Strip designation has no target.", 63126);
				}
				else
				{
					yield return designation.target.Thing;
				}
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003899 RID: 14489 RVA: 0x00034716 File Offset: 0x00032916
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0013D936 File Offset: 0x0013BB36
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Strip);
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x0013D950 File Offset: 0x0013BB50
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return t.Map.designationManager.DesignationOn(t, DesignationDefOf.Strip) != null && pawn.CanReserve(t, 1, -1, null, forced) && StrippableUtility.CanBeStrippedByColony(t);
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0013D98B File Offset: 0x0013BB8B
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Strip, t);
		}
	}
}

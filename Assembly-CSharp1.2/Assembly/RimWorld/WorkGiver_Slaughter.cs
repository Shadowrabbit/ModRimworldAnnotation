using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D44 RID: 3396
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x06004D9E RID: 19870 RVA: 0x00036DD4 File Offset: 0x00034FD4
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x06004D9F RID: 19871 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06004DA0 RID: 19872 RVA: 0x00036DE4 File Offset: 0x00034FE4
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter);
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x001AF220 File Offset: 0x001AD420
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Slaughter) == null)
			{
				return false;
			}
			if (pawn.Faction != t.Faction)
			{
				return false;
			}
			if (pawn2.InAggroMentalState)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				JobFailReason.Is("IsIncapableOfViolenceShort".Translate(pawn), null);
				return false;
			}
			return true;
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x00036DFE File Offset: 0x00034FFE
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Slaughter, t);
		}
	}
}

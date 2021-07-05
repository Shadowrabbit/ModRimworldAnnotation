using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200083A RID: 2106
	public class WorkGiver_ExtractSkull : WorkGiver_Scanner
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060037CE RID: 14286 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0013AD3D File Offset: 0x00138F3D
		public static bool CanExtractSkull(Ideo ideo)
		{
			return ideo.HasPrecept(PreceptDefOf.Skullspike_Desired);
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0013AD4C File Offset: 0x00138F4C
		public static bool CanPlayerExtractSkull()
		{
			if (WorkGiver_ExtractSkull.CanExtractSkull(Faction.OfPlayer.ideos.PrimaryIdeo))
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Faction.OfPlayer.ideos.IdeosMinorListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (WorkGiver_ExtractSkull.CanExtractSkull(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0013ADC8 File Offset: 0x00138FC8
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.ExtractSkull))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x0013ADD8 File Offset: 0x00138FD8
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.ExtractSkull);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0013ADF4 File Offset: 0x00138FF4
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (!ModLister.CheckIdeology("Skull extraction"))
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			if (corpse == null || corpse.Destroyed)
			{
				return false;
			}
			if (corpse.Map.designationManager.DesignationOn(t, DesignationDefOf.ExtractSkull) == null)
			{
				return false;
			}
			if (!pawn.CanReserve(t, 1, -1, null, forced))
			{
				return false;
			}
			if (pawn.Ideo == null || !WorkGiver_ExtractSkull.CanExtractSkull(pawn.Ideo))
			{
				JobFailReason.Is("CannotExtractSkull".Translate(), null);
				return false;
			}
			return true;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0013AE7E File Offset: 0x0013907E
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job job = JobMaker.MakeJob(JobDefOf.ExtractSkull, t);
			job.count = 1;
			return job;
		}
	}
}

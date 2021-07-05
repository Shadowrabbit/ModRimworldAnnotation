using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class WorkGiver_Slaughter : WorkGiver_Scanner
	{
		// Token: 0x060036E9 RID: 14057 RVA: 0x001370AA File Offset: 0x001352AA
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			foreach (Designation designation in pawn.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.Slaughter))
			{
				yield return designation.target.Thing;
			}
			IEnumerator<Designation> enumerator = null;
			foreach (Pawn pawn2 in pawn.Map.autoSlaughterManager.AnimalsToSlaughter)
			{
				yield return pawn2;
			}
			List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x170009C9 RID: 2505
		// (get) Token: 0x060036EA RID: 14058 RVA: 0x000126F5 File Offset: 0x000108F5
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x060036EB RID: 14059 RVA: 0x001370BA File Offset: 0x001352BA
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !pawn.Map.designationManager.AnySpawnedDesignationOfDef(DesignationDefOf.Slaughter) && pawn.Map.autoSlaughterManager.AnimalsToSlaughter.Count == 0;
		}

		// Token: 0x060036EC RID: 14060 RVA: 0x001370F0 File Offset: 0x001352F0
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.RaceProps.Animal)
			{
				return false;
			}
			if (!pawn2.ShouldBeSlaughtered())
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
			return (!ModsConfig.IdeologyActive || new HistoryEvent(HistoryEventDefOf.SlaughteredAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (!HistoryEventUtility.IsKillingInnocentAnimal(pawn, pawn2) || new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job()) && (pawn.Ideo == null || !pawn.Ideo.IsVeneratedAnimal(pawn2) || new HistoryEvent(HistoryEventDefOf.SlaughteredVeneratedAnimal, pawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo_Job());
		}

		// Token: 0x060036ED RID: 14061 RVA: 0x001371F5 File Offset: 0x001353F5
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.Slaughter, t);
		}
	}
}

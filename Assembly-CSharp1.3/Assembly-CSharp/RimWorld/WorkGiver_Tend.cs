using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000863 RID: 2147
	public class WorkGiver_Tend : WorkGiver_Scanner
	{
		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x060038AF RID: 14511 RVA: 0x001398A1 File Offset: 0x00137AA1
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x00034716 File Offset: 0x00032916
		public override Danger MaxPathDanger(Pawn pawn)
		{
			return Danger.Deadly;
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x00138ACD File Offset: 0x00136CCD
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x0013DBBA File Offset: 0x0013BDBA
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x0013DBCC File Offset: 0x0013BDCC
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			return pawn2 != null && !pawn.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && (!this.def.tendToHumanlikesOnly || pawn2.RaceProps.Humanlike) && (!this.def.tendToAnimalsOnly || pawn2.RaceProps.Animal) && WorkGiver_Tend.GoodLayingStatusForTend(pawn2, pawn) && HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x0013DC4A File Offset: 0x0013BE4A
		public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
		{
			if (patient == doctor)
			{
				return true;
			}
			if (patient.RaceProps.Humanlike)
			{
				return patient.InBed();
			}
			return patient.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0013DC70 File Offset: 0x0013BE70
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			Thing thing = HealthAIUtility.FindBestMedicine(pawn, pawn2, false);
			if (thing != null)
			{
				return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2, thing);
			}
			return JobMaker.MakeJob(JobDefOf.TendPatient, pawn2);
		}
	}
}

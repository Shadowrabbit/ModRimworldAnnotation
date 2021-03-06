using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A6B RID: 2667
	public class GatheringWorker_MarriageCeremony : GatheringWorker
	{
		// Token: 0x0600400C RID: 16396 RVA: 0x0015B158 File Offset: 0x00159358
		private static void FindFiancees(Pawn organizer, out Pawn firstFiance, out Pawn secondFiance)
		{
			firstFiance = organizer;
			secondFiance = organizer.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Fiance, null);
		}

		// Token: 0x0600400D RID: 16397 RVA: 0x0015B170 File Offset: 0x00159370
		protected override LordJob CreateLordJob(IntVec3 spot, Pawn organizer)
		{
			Pawn firstPawn;
			Pawn secondPawn;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstPawn, out secondPawn);
			return new LordJob_Joinable_MarriageCeremony(firstPawn, secondPawn, spot);
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0015B190 File Offset: 0x00159390
		protected override bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot)
		{
			Pawn firstFiance;
			Pawn secondFiance;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out firstFiance, out secondFiance);
			return RCellFinder.TryFindMarriageSite(firstFiance, secondFiance, out spot);
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0015B1B0 File Offset: 0x001593B0
		protected override void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Pawn pawn;
			Pawn pawn2;
			GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
			Messages.Message("MessageNewMarriageCeremony".Translate(pawn.LabelShort, pawn2.LabelShort, pawn.Named("PAWN1"), pawn2.Named("PAWN2")), new TargetInfo(spot, pawn.Map, false), MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0015B220 File Offset: 0x00159420
		public override bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer != null)
			{
				Pawn pawn;
				Pawn pawn2;
				GatheringWorker_MarriageCeremony.FindFiancees(organizer, out pawn, out pawn2);
				if (!GatheringsUtility.PawnCanStartOrContinueGathering(pawn) || !GatheringsUtility.PawnCanStartOrContinueGathering(pawn2))
				{
					return false;
				}
			}
			return base.CanExecute(map, organizer);
		}
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F96 RID: 3990
	public abstract class GatheringWorker
	{
		// Token: 0x0600577F RID: 22399 RVA: 0x001CD7CC File Offset: 0x001CB9CC
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		// Token: 0x06005780 RID: 22400 RVA: 0x001CD804 File Offset: 0x001CBA04
		public virtual bool TryExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			if (organizer == null)
			{
				return false;
			}
			IntVec3 spot;
			if (!this.TryFindGatherSpot(organizer, out spot))
			{
				return false;
			}
			LordJob lordJob = this.CreateLordJob(spot, organizer);
			Faction faction = organizer.Faction;
			LordJob lordJob2 = lordJob;
			Map map2 = organizer.Map;
			object startingPawns;
			if (!lordJob.OrganizerIsStartingPawn)
			{
				startingPawns = null;
			}
			else
			{
				(startingPawns = new Pawn[1])[0] = organizer;
			}
			LordMaker.MakeNewLord(faction, lordJob2, map2, startingPawns);
			this.SendLetter(spot, organizer);
			return true;
		}

		// Token: 0x06005781 RID: 22401 RVA: 0x001CD86C File Offset: 0x001CBA6C
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		// Token: 0x06005782 RID: 22402 RVA: 0x0003CB20 File Offset: 0x0003AD20
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		// Token: 0x06005783 RID: 22403
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		// Token: 0x06005784 RID: 22404
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		// Token: 0x04003940 RID: 14656
		public GatheringDef def;
	}
}

using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A6A RID: 2666
	public abstract class GatheringWorker
	{
		// Token: 0x06004005 RID: 16389 RVA: 0x0015B048 File Offset: 0x00159248
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		// Token: 0x06004006 RID: 16390 RVA: 0x0015B080 File Offset: 0x00159280
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

		// Token: 0x06004007 RID: 16391 RVA: 0x0015B0E8 File Offset: 0x001592E8
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null);
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x0015B145 File Offset: 0x00159345
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		// Token: 0x06004009 RID: 16393
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		// Token: 0x0600400A RID: 16394
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		// Token: 0x04002452 RID: 9298
		public GatheringDef def;
	}
}

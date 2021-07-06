using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002120 RID: 8480
	public class Caravan_CarryTracker : IExposable
	{
		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x0600B405 RID: 46085 RVA: 0x00074F2B File Offset: 0x0007312B
		public List<Pawn> CarriedPawnsListForReading
		{
			get
			{
				return this.carriedPawns;
			}
		}

		// Token: 0x0600B406 RID: 46086 RVA: 0x00074F33 File Offset: 0x00073133
		public Caravan_CarryTracker()
		{
		}

		// Token: 0x0600B407 RID: 46087 RVA: 0x00074F46 File Offset: 0x00073146
		public Caravan_CarryTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B408 RID: 46088 RVA: 0x00074F60 File Offset: 0x00073160
		public void CarryTrackerTick()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600B409 RID: 46089 RVA: 0x00074F68 File Offset: 0x00073168
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateCarriedPawns();
			}
		}

		// Token: 0x0600B40A RID: 46090 RVA: 0x00074F78 File Offset: 0x00073178
		public bool IsCarried(Pawn p)
		{
			return this.carriedPawns.Contains(p);
		}

		// Token: 0x0600B40B RID: 46091 RVA: 0x00344474 File Offset: 0x00342674
		private void RecalculateCarriedPawns()
		{
			this.carriedPawns.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			if (this.caravan.pather.MovingNow)
			{
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
				this.CalculatePawnsWhoCanCarry(Caravan_CarryTracker.tmpPawnsWhoCanCarry);
				int num = 0;
				while (num < this.caravan.pawns.Count && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
				{
					Pawn pawn = this.caravan.pawns[num];
					if (this.WantsToBeCarried(pawn) && Caravan_CarryTracker.tmpPawnsWhoCanCarry.Any<Pawn>())
					{
						this.carriedPawns.Add(pawn);
						Caravan_CarryTracker.tmpPawnsWhoCanCarry.RemoveLast<Pawn>();
					}
					num++;
				}
				Caravan_CarryTracker.tmpPawnsWhoCanCarry.Clear();
			}
		}

		// Token: 0x0600B40C RID: 46092 RVA: 0x00074F60 File Offset: 0x00073160
		public void Notify_CaravanSpawned()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600B40D RID: 46093 RVA: 0x00074F60 File Offset: 0x00073160
		public void Notify_PawnRemoved()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x0600B40E RID: 46094 RVA: 0x00344534 File Offset: 0x00342734
		private void CalculatePawnsWhoCanCarry(List<Pawn> outPawns)
		{
			outPawns.Clear();
			for (int i = 0; i < this.caravan.pawns.Count; i++)
			{
				Pawn pawn = this.caravan.pawns[i];
				if (pawn.RaceProps.Humanlike && !pawn.Downed && !pawn.InMentalState && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && !this.WantsToBeCarried(pawn))
				{
					outPawns.Add(pawn);
				}
			}
		}

		// Token: 0x0600B40F RID: 46095 RVA: 0x00074F86 File Offset: 0x00073186
		private bool WantsToBeCarried(Pawn p)
		{
			return p.health.beCarriedByCaravanIfSick && CaravanCarryUtility.WouldBenefitFromBeingCarried(p);
		}

		// Token: 0x0600B410 RID: 46096 RVA: 0x003445B8 File Offset: 0x003427B8
		public string GetInspectStringLine()
		{
			if (!this.carriedPawns.Any<Pawn>())
			{
				return null;
			}
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			int num = 0;
			for (int i = 0; i < this.carriedPawns.Count; i++)
			{
				Caravan_CarryTracker.tmpPawnLabels.Add(this.carriedPawns[i].LabelShort);
				if (this.caravan.beds.IsInBed(this.carriedPawns[i]))
				{
					num++;
				}
			}
			string str = (Caravan_CarryTracker.tmpPawnLabels.Count > 5) ? (Caravan_CarryTracker.tmpPawnLabels.Take(5).ToCommaList(false) + "...") : Caravan_CarryTracker.tmpPawnLabels.ToCommaList(true);
			string result = CaravanBedUtility.AppendUsingBedsLabel("BeingCarriedDueToIllness".Translate() + ": " + str.CapitalizeFirst(), num);
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			return result;
		}

		// Token: 0x04007BB4 RID: 31668
		public Caravan caravan;

		// Token: 0x04007BB5 RID: 31669
		private List<Pawn> carriedPawns = new List<Pawn>();

		// Token: 0x04007BB6 RID: 31670
		private static List<Pawn> tmpPawnsWhoCanCarry = new List<Pawn>();

		// Token: 0x04007BB7 RID: 31671
		private static List<string> tmpPawnLabels = new List<string>();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C2 RID: 6082
	public class Caravan_CarryTracker : IExposable
	{
		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06008D1F RID: 36127 RVA: 0x0032CC22 File Offset: 0x0032AE22
		public List<Pawn> CarriedPawnsListForReading
		{
			get
			{
				return this.carriedPawns;
			}
		}

		// Token: 0x06008D20 RID: 36128 RVA: 0x0032CC2A File Offset: 0x0032AE2A
		public Caravan_CarryTracker()
		{
		}

		// Token: 0x06008D21 RID: 36129 RVA: 0x0032CC3D File Offset: 0x0032AE3D
		public Caravan_CarryTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008D22 RID: 36130 RVA: 0x0032CC57 File Offset: 0x0032AE57
		public void CarryTrackerTick()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06008D23 RID: 36131 RVA: 0x0032CC5F File Offset: 0x0032AE5F
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateCarriedPawns();
			}
		}

		// Token: 0x06008D24 RID: 36132 RVA: 0x0032CC6F File Offset: 0x0032AE6F
		public bool IsCarried(Pawn p)
		{
			return this.carriedPawns.Contains(p);
		}

		// Token: 0x06008D25 RID: 36133 RVA: 0x0032CC80 File Offset: 0x0032AE80
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

		// Token: 0x06008D26 RID: 36134 RVA: 0x0032CC57 File Offset: 0x0032AE57
		public void Notify_CaravanSpawned()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06008D27 RID: 36135 RVA: 0x0032CC57 File Offset: 0x0032AE57
		public void Notify_PawnRemoved()
		{
			this.RecalculateCarriedPawns();
		}

		// Token: 0x06008D28 RID: 36136 RVA: 0x0032CD40 File Offset: 0x0032AF40
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

		// Token: 0x06008D29 RID: 36137 RVA: 0x0032CDC4 File Offset: 0x0032AFC4
		private bool WantsToBeCarried(Pawn p)
		{
			return p.health.beCarriedByCaravanIfSick && CaravanCarryUtility.WouldBenefitFromBeingCarried(p);
		}

		// Token: 0x06008D2A RID: 36138 RVA: 0x0032CDDC File Offset: 0x0032AFDC
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
			string str = (Caravan_CarryTracker.tmpPawnLabels.Count > 5) ? (Caravan_CarryTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_CarryTracker.tmpPawnLabels.ToCommaList(true, false);
			string result = CaravanBedUtility.AppendUsingBedsLabel("BeingCarriedDueToIllness".Translate() + ": " + str.CapitalizeFirst(), num);
			Caravan_CarryTracker.tmpPawnLabels.Clear();
			return result;
		}

		// Token: 0x0400597A RID: 22906
		public Caravan caravan;

		// Token: 0x0400597B RID: 22907
		private List<Pawn> carriedPawns = new List<Pawn>();

		// Token: 0x0400597C RID: 22908
		private static List<Pawn> tmpPawnsWhoCanCarry = new List<Pawn>();

		// Token: 0x0400597D RID: 22909
		private static List<string> tmpPawnLabels = new List<string>();
	}
}

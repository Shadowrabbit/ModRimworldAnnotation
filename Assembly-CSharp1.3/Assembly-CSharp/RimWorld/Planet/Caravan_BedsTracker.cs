using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C1 RID: 6081
	public class Caravan_BedsTracker : IExposable
	{
		// Token: 0x06008D11 RID: 36113 RVA: 0x0032C797 File Offset: 0x0032A997
		public Caravan_BedsTracker()
		{
		}

		// Token: 0x06008D12 RID: 36114 RVA: 0x0032C7AA File Offset: 0x0032A9AA
		public Caravan_BedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008D13 RID: 36115 RVA: 0x0032C7C4 File Offset: 0x0032A9C4
		public void BedsTrackerTick()
		{
			this.RecalculateUsedBeds();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				PawnUtility.GainComfortFromThingIfPossible(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06008D14 RID: 36116 RVA: 0x0032C82C File Offset: 0x0032AA2C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateUsedBeds();
			}
		}

		// Token: 0x06008D15 RID: 36117 RVA: 0x0032C83C File Offset: 0x0032AA3C
		private void RecalculateUsedBeds()
		{
			this.usedBeds.Clear();
			if (!this.caravan.Spawned)
			{
				return;
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
			this.GetUsableBeds(Caravan_BedsTracker.tmpUsableBeds);
			if (!this.caravan.pather.MovingNow)
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.BedRestEffectiveness, true));
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					Pawn pawn = this.caravan.pawns[i];
					if (pawn.needs != null && pawn.needs.rest != null)
					{
						Building_Bed andRemoveFirstAvailableBedFor = this.GetAndRemoveFirstAvailableBedFor(pawn, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor != null)
						{
							this.usedBeds.Add(pawn, andRemoveFirstAvailableBedFor);
						}
					}
				}
			}
			else
			{
				Caravan_BedsTracker.tmpUsableBeds.SortByDescending((Building_Bed x) => x.GetStatValue(StatDefOf.ImmunityGainSpeedFactor, true));
				for (int j = 0; j < this.caravan.pawns.Count; j++)
				{
					Pawn pawn2 = this.caravan.pawns[j];
					if (pawn2.needs != null && pawn2.needs.rest != null && CaravanBedUtility.WouldBenefitFromRestingInBed(pawn2) && (!this.caravan.pather.MovingNow || pawn2.CarriedByCaravan()))
					{
						Building_Bed andRemoveFirstAvailableBedFor2 = this.GetAndRemoveFirstAvailableBedFor(pawn2, Caravan_BedsTracker.tmpUsableBeds);
						if (andRemoveFirstAvailableBedFor2 != null)
						{
							this.usedBeds.Add(pawn2, andRemoveFirstAvailableBedFor2);
						}
					}
				}
			}
			Caravan_BedsTracker.tmpUsableBeds.Clear();
		}

		// Token: 0x06008D16 RID: 36118 RVA: 0x0032C9DA File Offset: 0x0032ABDA
		public void Notify_CaravanSpawned()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x06008D17 RID: 36119 RVA: 0x0032C9DA File Offset: 0x0032ABDA
		public void Notify_PawnRemoved()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x06008D18 RID: 36120 RVA: 0x0032C9E4 File Offset: 0x0032ABE4
		public Building_Bed GetBedUsedBy(Pawn p)
		{
			Building_Bed building_Bed;
			if (this.usedBeds.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				return building_Bed;
			}
			return null;
		}

		// Token: 0x06008D19 RID: 36121 RVA: 0x0032CA0C File Offset: 0x0032AC0C
		public bool IsInBed(Pawn p)
		{
			return this.GetBedUsedBy(p) != null;
		}

		// Token: 0x06008D1A RID: 36122 RVA: 0x0032CA18 File Offset: 0x0032AC18
		public int GetUsedBedCount()
		{
			return this.usedBeds.Count;
		}

		// Token: 0x06008D1B RID: 36123 RVA: 0x0032CA28 File Offset: 0x0032AC28
		private void GetUsableBeds(List<Building_Bed> outBeds)
		{
			outBeds.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Bed building_Bed = list[i].GetInnerIfMinified() as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_caravansCanUse)
				{
					for (int j = 0; j < list[i].stackCount; j++)
					{
						for (int k = 0; k < building_Bed.SleepingSlotsCount; k++)
						{
							outBeds.Add(building_Bed);
						}
					}
				}
			}
		}

		// Token: 0x06008D1C RID: 36124 RVA: 0x0032CAB4 File Offset: 0x0032ACB4
		private Building_Bed GetAndRemoveFirstAvailableBedFor(Pawn p, List<Building_Bed> beds)
		{
			for (int i = 0; i < beds.Count; i++)
			{
				if (RestUtility.CanUseBedEver(p, beds[i].def))
				{
					Building_Bed result = beds[i];
					beds.RemoveAt(i);
					return result;
				}
			}
			return null;
		}

		// Token: 0x06008D1D RID: 36125 RVA: 0x0032CAF8 File Offset: 0x0032ACF8
		public string GetInBedForMedicalReasonsInspectStringLine()
		{
			if (this.usedBeds.Count == 0)
			{
				return null;
			}
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				if (!this.caravan.carryTracker.IsCarried(keyValuePair.Key) && CaravanBedUtility.WouldBenefitFromRestingInBed(keyValuePair.Key))
				{
					Caravan_BedsTracker.tmpPawnLabels.Add(keyValuePair.Key.LabelShort);
				}
			}
			if (!Caravan_BedsTracker.tmpPawnLabels.Any<string>())
			{
				return null;
			}
			string t = (Caravan_BedsTracker.tmpPawnLabels.Count > 5) ? (Caravan_BedsTracker.tmpPawnLabels.Take(5).ToCommaList(false, false) + "...") : Caravan_BedsTracker.tmpPawnLabels.ToCommaList(true, false);
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			return "UsingBedrollsDueToIllness".Translate() + ": " + t;
		}

		// Token: 0x04005976 RID: 22902
		public Caravan caravan;

		// Token: 0x04005977 RID: 22903
		private Dictionary<Pawn, Building_Bed> usedBeds = new Dictionary<Pawn, Building_Bed>();

		// Token: 0x04005978 RID: 22904
		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		// Token: 0x04005979 RID: 22905
		private static List<string> tmpPawnLabels = new List<string>();
	}
}

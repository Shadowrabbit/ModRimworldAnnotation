using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200211E RID: 8478
	public class Caravan_BedsTracker : IExposable
	{
		// Token: 0x0600B3F3 RID: 46067 RVA: 0x00074E8F File Offset: 0x0007308F
		public Caravan_BedsTracker()
		{
		}

		// Token: 0x0600B3F4 RID: 46068 RVA: 0x00074EA2 File Offset: 0x000730A2
		public Caravan_BedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B3F5 RID: 46069 RVA: 0x00344064 File Offset: 0x00342264
		public void BedsTrackerTick()
		{
			this.RecalculateUsedBeds();
			foreach (KeyValuePair<Pawn, Building_Bed> keyValuePair in this.usedBeds)
			{
				PawnUtility.GainComfortFromThingIfPossible(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x0600B3F6 RID: 46070 RVA: 0x00074EBC File Offset: 0x000730BC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecalculateUsedBeds();
			}
		}

		// Token: 0x0600B3F7 RID: 46071 RVA: 0x003440CC File Offset: 0x003422CC
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

		// Token: 0x0600B3F8 RID: 46072 RVA: 0x00074ECC File Offset: 0x000730CC
		public void Notify_CaravanSpawned()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x0600B3F9 RID: 46073 RVA: 0x00074ECC File Offset: 0x000730CC
		public void Notify_PawnRemoved()
		{
			this.RecalculateUsedBeds();
		}

		// Token: 0x0600B3FA RID: 46074 RVA: 0x0034426C File Offset: 0x0034246C
		public Building_Bed GetBedUsedBy(Pawn p)
		{
			Building_Bed building_Bed;
			if (this.usedBeds.TryGetValue(p, out building_Bed) && !building_Bed.DestroyedOrNull())
			{
				return building_Bed;
			}
			return null;
		}

		// Token: 0x0600B3FB RID: 46075 RVA: 0x00074ED4 File Offset: 0x000730D4
		public bool IsInBed(Pawn p)
		{
			return this.GetBedUsedBy(p) != null;
		}

		// Token: 0x0600B3FC RID: 46076 RVA: 0x00074EE0 File Offset: 0x000730E0
		public int GetUsedBedCount()
		{
			return this.usedBeds.Count;
		}

		// Token: 0x0600B3FD RID: 46077 RVA: 0x00344294 File Offset: 0x00342494
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

		// Token: 0x0600B3FE RID: 46078 RVA: 0x00344320 File Offset: 0x00342520
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

		// Token: 0x0600B3FF RID: 46079 RVA: 0x00344364 File Offset: 0x00342564
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
			string t = (Caravan_BedsTracker.tmpPawnLabels.Count > 5) ? (Caravan_BedsTracker.tmpPawnLabels.Take(5).ToCommaList(false) + "...") : Caravan_BedsTracker.tmpPawnLabels.ToCommaList(true);
			Caravan_BedsTracker.tmpPawnLabels.Clear();
			return "UsingBedrollsDueToIllness".Translate() + ": " + t;
		}

		// Token: 0x04007BAD RID: 31661
		public Caravan caravan;

		// Token: 0x04007BAE RID: 31662
		private Dictionary<Pawn, Building_Bed> usedBeds = new Dictionary<Pawn, Building_Bed>();

		// Token: 0x04007BAF RID: 31663
		private static List<Building_Bed> tmpUsableBeds = new List<Building_Bed>();

		// Token: 0x04007BB0 RID: 31664
		private static List<string> tmpPawnLabels = new List<string>();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002123 RID: 8483
	public class Caravan_NeedsTracker : IExposable
	{
		// Token: 0x0600B422 RID: 46114 RVA: 0x00006B8B File Offset: 0x00004D8B
		public Caravan_NeedsTracker()
		{
		}

		// Token: 0x0600B423 RID: 46115 RVA: 0x00075039 File Offset: 0x00073239
		public Caravan_NeedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x0600B424 RID: 46116 RVA: 0x00006A05 File Offset: 0x00004C05
		public void ExposeData()
		{
		}

		// Token: 0x0600B425 RID: 46117 RVA: 0x00075048 File Offset: 0x00073248
		public void NeedsTrackerTick()
		{
			this.TrySatisfyPawnsNeeds();
		}

		// Token: 0x0600B426 RID: 46118 RVA: 0x00344864 File Offset: 0x00342A64
		public void TrySatisfyPawnsNeeds()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				this.TrySatisfyPawnNeeds(pawnsListForReading[i]);
			}
		}

		// Token: 0x0600B427 RID: 46119 RVA: 0x003448A0 File Offset: 0x00342AA0
		private void TrySatisfyPawnNeeds(Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				Need need = allNeeds[i];
				Need_Rest need_Rest = need as Need_Rest;
				Need_Food need_Food = need as Need_Food;
				Need_Chemical need_Chemical = need as Need_Chemical;
				Need_Joy need_Joy = need as Need_Joy;
				if (need_Rest != null)
				{
					this.TrySatisfyRestNeed(pawn, need_Rest);
				}
				else if (need_Food != null)
				{
					this.TrySatisfyFoodNeed(pawn, need_Food);
				}
				else if (need_Chemical != null)
				{
					this.TrySatisfyChemicalNeed(pawn, need_Chemical);
				}
				else if (need_Joy != null)
				{
					this.TrySatisfyJoyNeed(pawn, need_Joy);
				}
			}
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy.Psylink != null)
			{
				this.TryGainPsyfocus(psychicEntropy);
			}
		}

		// Token: 0x0600B428 RID: 46120 RVA: 0x00344948 File Offset: 0x00342B48
		private void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest)
		{
			if (!this.caravan.pather.MovingNow || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				Building_Bed building_Bed = pawn.CurrentCaravanBed();
				float restEffectiveness = (building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true) : StatDefOf.BedRestEffectiveness.valueIfMissing;
				rest.TickResting(restEffectiveness);
			}
		}

		// Token: 0x0600B429 RID: 46121 RVA: 0x003449A4 File Offset: 0x00342BA4
		private void TrySatisfyFoodNeed(Pawn pawn, Need_Food food)
		{
			if (food.CurCategory < HungerCategory.Hungry)
			{
				return;
			}
			if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
			{
				VirtualPlantsUtility.EatVirtualPlants(pawn);
				return;
			}
			Thing thing;
			Pawn pawn2;
			if (CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
			{
				food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
				if (thing.Destroyed)
				{
					if (pawn2 != null)
					{
						pawn2.inventory.innerContainer.Remove(thing);
						this.caravan.RecacheImmobilizedNow();
						this.caravan.RecacheDaysWorthOfFood();
					}
					if (!this.caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(this.caravan, pawn, out thing, out pawn2))
					{
						Messages.Message("MessageCaravanRanOutOfFood".Translate(this.caravan.LabelCap, pawn.Label, pawn.Named("PAWN")), this.caravan, MessageTypeDefOf.ThreatBig, true);
						this.caravan.notifiedOutOfFood = true;
					}
				}
			}
		}

		// Token: 0x0600B42A RID: 46122 RVA: 0x00344AA8 File Offset: 0x00342CA8
		private void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical)
		{
			if (chemical.CurCategory >= DrugDesireCategory.Satisfied)
			{
				return;
			}
			Thing drug;
			Pawn drugOwner;
			if (CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(this.caravan, pawn, chemical, out drug, out drugOwner))
			{
				this.IngestDrug(pawn, drug, drugOwner);
			}
		}

		// Token: 0x0600B42B RID: 46123 RVA: 0x00344ADC File Offset: 0x00342CDC
		public void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner)
		{
			float num = drug.Ingested(pawn, 0f);
			Need_Food food = pawn.needs.food;
			if (food != null)
			{
				food.CurLevel += num;
			}
			if (drug.Destroyed && drugOwner != null)
			{
				drugOwner.inventory.innerContainer.Remove(drug);
				this.caravan.RecacheImmobilizedNow();
				this.caravan.RecacheDaysWorthOfFood();
			}
		}

		// Token: 0x0600B42C RID: 46124 RVA: 0x00344B48 File Offset: 0x00342D48
		private void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy)
		{
			if (pawn.IsHashIntervalTick(1250))
			{
				float num = this.GetCurrentJoyGainPerTick(pawn);
				if (num <= 0f)
				{
					return;
				}
				num *= 1250f;
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
				this.GetAvailableJoyKindsFor(pawn, Caravan_NeedsTracker.tmpAvailableJoyKinds);
				JoyKindDef joyKind;
				if (!Caravan_NeedsTracker.tmpAvailableJoyKinds.TryRandomElementByWeight((JoyKindDef x) => 1f - Mathf.Clamp01(pawn.needs.joy.tolerances[x]), out joyKind))
				{
					return;
				}
				joy.GainJoy(num, joyKind);
				Caravan_NeedsTracker.tmpAvailableJoyKinds.Clear();
			}
		}

		// Token: 0x0600B42D RID: 46125 RVA: 0x00075050 File Offset: 0x00073250
		public float GetCurrentJoyGainPerTick(Pawn pawn)
		{
			if (this.caravan.pather.MovingNow)
			{
				return 0f;
			}
			return 4E-05f;
		}

		// Token: 0x0600B42E RID: 46126 RVA: 0x0007506F File Offset: 0x0007326F
		public void TryGainPsyfocus(Pawn_PsychicEntropyTracker tracker)
		{
			if (!this.caravan.pather.MovingNow && !this.caravan.NightResting)
			{
				tracker.GainPsyfocus(null);
			}
		}

		// Token: 0x0600B42F RID: 46127 RVA: 0x00344BDC File Offset: 0x00342DDC
		public bool AnyPawnOutOfFood(out string malnutritionHediff)
		{
			Caravan_NeedsTracker.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this.caravan);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					Caravan_NeedsTracker.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					bool flag = false;
					for (int k = 0; k < Caravan_NeedsTracker.tmpInvFood.Count; k++)
					{
						if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(Caravan_NeedsTracker.tmpInvFood[k].def, pawn))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num = -1;
						string text = null;
						for (int l = 0; l < pawnsListForReading.Count; l++)
						{
							Hediff firstHediffOfDef = pawnsListForReading[l].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
							if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num))
							{
								num = firstHediffOfDef.CurStageIndex;
								text = firstHediffOfDef.LabelCap;
							}
						}
						malnutritionHediff = text;
						Caravan_NeedsTracker.tmpInvFood.Clear();
						return true;
					}
				}
			}
			malnutritionHediff = null;
			Caravan_NeedsTracker.tmpInvFood.Clear();
			return false;
		}

		// Token: 0x0600B430 RID: 46128 RVA: 0x00344D3C File Offset: 0x00342F3C
		private void GetAvailableJoyKindsFor(Pawn p, List<JoyKindDef> outJoyKinds)
		{
			outJoyKinds.Clear();
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Meditative))
			{
				outJoyKinds.Add(JoyKindDefOf.Meditative);
			}
			if (!p.needs.joy.tolerances.BoredOf(JoyKindDefOf.Social))
			{
				int num = 0;
				for (int i = 0; i < this.caravan.pawns.Count; i++)
				{
					if (this.caravan.pawns[i].RaceProps.Humanlike && !this.caravan.pawns[i].Downed && !this.caravan.pawns[i].InMentalState)
					{
						num++;
					}
				}
				if (num >= 2)
				{
					outJoyKinds.Add(JoyKindDefOf.Social);
				}
			}
		}

		// Token: 0x04007BBF RID: 31679
		public Caravan caravan;

		// Token: 0x04007BC0 RID: 31680
		private static List<JoyKindDef> tmpAvailableJoyKinds = new List<JoyKindDef>();

		// Token: 0x04007BC1 RID: 31681
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}

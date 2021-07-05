using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C4 RID: 6084
	public class Caravan_NeedsTracker : IExposable
	{
		// Token: 0x06008D34 RID: 36148 RVA: 0x000033AC File Offset: 0x000015AC
		public Caravan_NeedsTracker()
		{
		}

		// Token: 0x06008D35 RID: 36149 RVA: 0x0032D043 File Offset: 0x0032B243
		public Caravan_NeedsTracker(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x06008D36 RID: 36150 RVA: 0x0000313F File Offset: 0x0000133F
		public void ExposeData()
		{
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x0032D052 File Offset: 0x0032B252
		public void NeedsTrackerTick()
		{
			this.TrySatisfyPawnsNeeds();
		}

		// Token: 0x06008D38 RID: 36152 RVA: 0x0032D05C File Offset: 0x0032B25C
		public void TrySatisfyPawnsNeeds()
		{
			List<Pawn> pawnsListForReading = this.caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				this.TrySatisfyPawnNeeds(pawnsListForReading[i]);
			}
		}

		// Token: 0x06008D39 RID: 36153 RVA: 0x0032D098 File Offset: 0x0032B298
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

		// Token: 0x06008D3A RID: 36154 RVA: 0x0032D140 File Offset: 0x0032B340
		private void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest)
		{
			if (!this.caravan.pather.MovingNow || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				Building_Bed building_Bed = pawn.CurrentCaravanBed();
				float restEffectiveness = (building_Bed != null) ? building_Bed.GetStatValue(StatDefOf.BedRestEffectiveness, true) : StatDefOf.BedRestEffectiveness.valueIfMissing;
				rest.TickResting(restEffectiveness);
			}
		}

		// Token: 0x06008D3B RID: 36155 RVA: 0x0032D19C File Offset: 0x0032B39C
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
						Messages.Message("MessageCaravanRanOutOfFood".Translate(this.caravan.LabelCap), this.caravan, MessageTypeDefOf.ThreatBig, true);
						this.caravan.notifiedOutOfFood = true;
					}
				}
			}
		}

		// Token: 0x06008D3C RID: 36156 RVA: 0x0032D288 File Offset: 0x0032B488
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

		// Token: 0x06008D3D RID: 36157 RVA: 0x0032D2BC File Offset: 0x0032B4BC
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

		// Token: 0x06008D3E RID: 36158 RVA: 0x0032D328 File Offset: 0x0032B528
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

		// Token: 0x06008D3F RID: 36159 RVA: 0x0032D3BA File Offset: 0x0032B5BA
		public float GetCurrentJoyGainPerTick(Pawn pawn)
		{
			if (this.caravan.pather.MovingNow)
			{
				return 0f;
			}
			return 4E-05f;
		}

		// Token: 0x06008D40 RID: 36160 RVA: 0x0032D3D9 File Offset: 0x0032B5D9
		public void TryGainPsyfocus(Pawn_PsychicEntropyTracker tracker)
		{
			if (!this.caravan.pather.MovingNow && !this.caravan.NightResting)
			{
				tracker.GainPsyfocus(null);
			}
		}

		// Token: 0x06008D41 RID: 36161 RVA: 0x0032D404 File Offset: 0x0032B604
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

		// Token: 0x06008D42 RID: 36162 RVA: 0x0032D564 File Offset: 0x0032B764
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

		// Token: 0x04005981 RID: 22913
		public Caravan caravan;

		// Token: 0x04005982 RID: 22914
		private static List<JoyKindDef> tmpAvailableJoyKinds = new List<JoyKindDef>();

		// Token: 0x04005983 RID: 22915
		private static List<Thing> tmpInvFood = new List<Thing>();
	}
}

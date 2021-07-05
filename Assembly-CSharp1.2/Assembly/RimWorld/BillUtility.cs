using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B1C RID: 2844
	public static class BillUtility
	{
		// Token: 0x06004294 RID: 17044 RVA: 0x0003185A File Offset: 0x0002FA5A
		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00031875 File Offset: 0x0002FA75
		public static Bill MakeNewBill(this RecipeDef recipe)
		{
			if (recipe.UsesUnfinishedThing)
			{
				return new Bill_ProductionWithUft(recipe);
			}
			return new Bill_Production(recipe);
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x0003188C File Offset: 0x0002FA8C
		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)))
				{
					IBillGiver billGiver = thing as IBillGiver;
					if (billGiver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774, false);
					}
					else
					{
						yield return billGiver;
					}
				}
				List<Thing>.Enumerator enumerator2 = default(List<Thing>.Enumerator);
				foreach (Thing outerThing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)))
				{
					IBillGiver billGiver2 = outerThing.GetInnerIfMinified() as IBillGiver;
					if (billGiver2 != null)
					{
						yield return billGiver2;
					}
				}
				enumerator2 = default(List<Thing>.Enumerator);
				map = null;
			}
			List<Map>.Enumerator enumerator = default(List<Map>.Enumerator);
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				foreach (Thing outerThing2 in caravan.AllThings)
				{
					IBillGiver billGiver3 = outerThing2.GetInnerIfMinified() as IBillGiver;
					if (billGiver3 != null)
					{
						yield return billGiver3;
					}
				}
				IEnumerator<Thing> enumerator4 = null;
			}
			List<Caravan>.Enumerator enumerator3 = default(List<Caravan>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x00031895 File Offset: 0x0002FA95
		public static IEnumerable<Bill> GlobalBills()
		{
			foreach (IBillGiver billGiver in BillUtility.GlobalBillGivers())
			{
				foreach (Bill bill in billGiver.BillStack)
				{
					yield return bill;
				}
				IEnumerator<Bill> enumerator2 = null;
			}
			IEnumerator<IBillGiver> enumerator = null;
			if (BillUtility.Clipboard != null)
			{
				yield return BillUtility.Clipboard;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x0018A94C File Offset: 0x00188B4C
		public static void Notify_ZoneStockpileRemoved(Zone_Stockpile stockpile)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x0018A998 File Offset: 0x00188B98
		public static void Notify_ColonistUnavailable(Pawn pawn)
		{
			try
			{
				foreach (Bill bill in BillUtility.GlobalBills())
				{
					bill.ValidateSettings();
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not notify bills: " + arg, false);
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x0018AA04 File Offset: 0x00188C04
		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282, false);
				return null;
			}
			List<WorkGiverDef> allDefsListForReading = DefDatabase<WorkGiverDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkGiverDef workGiverDef = allDefsListForReading[i];
				WorkGiver_DoBill workGiver_DoBill = workGiverDef.Worker as WorkGiver_DoBill;
				if (workGiver_DoBill != null && workGiver_DoBill.ThingIsUsableBillGiver(thing))
				{
					return workGiverDef;
				}
			}
			Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705, false);
			return null;
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x0018AA90 File Offset: 0x00188C90
		public static bool IsSurgeryViolationOnExtraFactionMember(this Bill_Medical bill, Pawn billDoer)
		{
			if (bill.recipe.IsSurgery && bill.recipe.Worker != null)
			{
				RecipeWorker worker = bill.recipe.Worker;
				Faction sharedExtraFaction = billDoer.GetSharedExtraFaction(bill.GiverPawn, ExtraFactionType.HomeFaction, null);
				if (sharedExtraFaction != null && worker.IsViolationOnPawn(bill.GiverPawn, bill.Part, sharedExtraFaction))
				{
					return true;
				}
				Faction sharedExtraFaction2 = billDoer.GetSharedExtraFaction(bill.GiverPawn, ExtraFactionType.MiniFaction, null);
				if (sharedExtraFaction2 != null && worker.IsViolationOnPawn(bill.GiverPawn, bill.Part, sharedExtraFaction2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002DA4 RID: 11684
		public static Bill Clipboard;
	}
}

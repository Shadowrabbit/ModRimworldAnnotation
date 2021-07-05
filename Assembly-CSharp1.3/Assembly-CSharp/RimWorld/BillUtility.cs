using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B7 RID: 1719
	public static class BillUtility
	{
		// Token: 0x06002FBD RID: 12221 RVA: 0x0011A593 File Offset: 0x00118793
		public static void TryDrawIngredientSearchRadiusOnMap(this Bill bill, IntVec3 center)
		{
			if (bill.ingredientSearchRadius < GenRadial.MaxRadialPatternRadius)
			{
				GenDraw.DrawRadiusRing(center, bill.ingredientSearchRadius);
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0011A5AE File Offset: 0x001187AE
		public static Bill MakeNewBill(this RecipeDef recipe, Precept_ThingStyle precept = null)
		{
			if (recipe.UsesUnfinishedThing)
			{
				return new Bill_ProductionWithUft(recipe, precept);
			}
			return new Bill_Production(recipe, precept);
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0011A5C7 File Offset: 0x001187C7
		public static IEnumerable<IBillGiver> GlobalBillGivers()
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.PotentialBillGiver)))
				{
					IBillGiver billGiver = thing as IBillGiver;
					if (billGiver == null)
					{
						Log.ErrorOnce("Found non-bill-giver tagged as PotentialBillGiver", 13389774);
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

		// Token: 0x06002FC0 RID: 12224 RVA: 0x0011A5D0 File Offset: 0x001187D0
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

		// Token: 0x06002FC1 RID: 12225 RVA: 0x0011A5DC File Offset: 0x001187DC
		public static void Notify_ZoneStockpileRemoved(Zone_Stockpile stockpile)
		{
			foreach (Bill bill in BillUtility.GlobalBills())
			{
				bill.ValidateSettings();
			}
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x0011A628 File Offset: 0x00118828
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
				Log.Error("Could not notify bills: " + arg);
			}
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x0011A694 File Offset: 0x00118894
		public static WorkGiverDef GetWorkgiver(this IBillGiver billGiver)
		{
			Thing thing = billGiver as Thing;
			if (thing == null)
			{
				Log.ErrorOnce(string.Format("Attempting to get the workgiver for a non-Thing IBillGiver {0}", billGiver.ToString()), 96810282);
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
			Log.ErrorOnce(string.Format("Can't find a WorkGiver for a BillGiver {0}", thing.ToString()), 57348705);
			return null;
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x0011A720 File Offset: 0x00118920
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

		// Token: 0x04001D17 RID: 7447
		public static Bill Clipboard;
	}
}

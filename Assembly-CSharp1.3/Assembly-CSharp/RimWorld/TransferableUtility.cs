using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200131B RID: 4891
	public static class TransferableUtility
	{
		// Token: 0x06007605 RID: 30213 RVA: 0x0028B950 File Offset: 0x00289B50
		public static void Transfer(List<Thing> things, int count, Action<Thing, IThingHolder> transferred)
		{
			if (count <= 0)
			{
				return;
			}
			TransferableUtility.tmpThings.Clear();
			TransferableUtility.tmpThings.AddRange(things);
			int num = count;
			for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
			{
				Thing thing = TransferableUtility.tmpThings[i];
				int num2 = Mathf.Min(num, thing.stackCount);
				if (num2 > 0)
				{
					IThingHolder parentHolder = thing.ParentHolder;
					Thing thing2 = thing.SplitOff(num2);
					num -= num2;
					if (thing2 == thing)
					{
						things.Remove(thing);
					}
					transferred(thing2, parentHolder);
					if (num <= 0)
					{
						break;
					}
				}
			}
			TransferableUtility.tmpThings.Clear();
			if (num > 0)
			{
				Log.Error("Can't transfer things because there is nothing left.");
			}
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x0028B9F4 File Offset: 0x00289BF4
		public static void TransferNoSplit(List<Thing> things, int count, Action<Thing, int> transfer, bool removeIfTakingEntireThing = true, bool errorIfNotEnoughThings = true)
		{
			if (count <= 0)
			{
				return;
			}
			TransferableUtility.tmpThings.Clear();
			TransferableUtility.tmpThings.AddRange(things);
			int num = count;
			for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
			{
				Thing thing = TransferableUtility.tmpThings[i];
				int num2 = Mathf.Min(num, thing.stackCount);
				if (num2 > 0)
				{
					num -= num2;
					if (removeIfTakingEntireThing && num2 >= thing.stackCount)
					{
						things.Remove(thing);
					}
					transfer(thing, num2);
					if (num <= 0)
					{
						break;
					}
				}
			}
			TransferableUtility.tmpThings.Clear();
			if (num > 0 && errorIfNotEnoughThings)
			{
				Log.Error("Can't transfer things because there is nothing left.");
			}
		}

		// Token: 0x06007607 RID: 30215 RVA: 0x0028BA90 File Offset: 0x00289C90
		public static bool TransferAsOne(Thing a, Thing b, TransferAsOneMode mode)
		{
			if (a == b)
			{
				return true;
			}
			if (a.def != b.def)
			{
				return false;
			}
			a = a.GetInnerIfMinified();
			b = b.GetInnerIfMinified();
			if (a.def.tradeNeverStack || b.def.tradeNeverStack)
			{
				return false;
			}
			if (!TransferableUtility.CanStack(a) || !TransferableUtility.CanStack(b))
			{
				return false;
			}
			if (a.def != b.def || a.Stuff != b.Stuff)
			{
				return false;
			}
			if (mode == TransferAsOneMode.PodsOrCaravanPacking)
			{
				float num = -1f;
				CompRottable compRottable = a.TryGetComp<CompRottable>();
				if (compRottable != null)
				{
					num = compRottable.RotProgressPct;
				}
				float num2 = -1f;
				CompRottable compRottable2 = b.TryGetComp<CompRottable>();
				if (compRottable2 != null)
				{
					num2 = compRottable2.RotProgressPct;
				}
				if (Mathf.Abs(num - num2) > 0.1f)
				{
					return false;
				}
			}
			if (a is Corpse && b is Corpse)
			{
				Pawn innerPawn = ((Corpse)a).InnerPawn;
				Pawn innerPawn2 = ((Corpse)b).InnerPawn;
				return innerPawn.def == innerPawn2.def && innerPawn.kindDef == innerPawn2.kindDef && !innerPawn.RaceProps.Humanlike && !innerPawn2.RaceProps.Humanlike && (innerPawn.Name == null || innerPawn.Name.Numerical) && (innerPawn2.Name == null || innerPawn2.Name.Numerical);
			}
			if (a.def.category == ThingCategory.Pawn)
			{
				if (b.def != a.def)
				{
					return false;
				}
				Pawn pawn = (Pawn)a;
				Pawn pawn2 = (Pawn)b;
				return pawn.kindDef == pawn2.kindDef && pawn.gender == pawn2.gender && pawn.ageTracker.CurLifeStageIndex == pawn2.ageTracker.CurLifeStageIndex && Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - pawn2.ageTracker.AgeBiologicalYearsFloat) <= 1f;
			}
			else
			{
				Apparel apparel = a as Apparel;
				Apparel apparel2 = b as Apparel;
				if (apparel != null && apparel2 != null && apparel.WornByCorpse != apparel2.WornByCorpse)
				{
					return false;
				}
				if (mode != TransferAsOneMode.InactiveTradeable && a.def.useHitPoints && a.def.healthAffectsPrice && Mathf.Abs(a.HitPoints - b.HitPoints) >= 10)
				{
					return false;
				}
				QualityCategory qualityCategory;
				QualityCategory qualityCategory2;
				if (a.TryGetQuality(out qualityCategory) && b.TryGetQuality(out qualityCategory2) && qualityCategory != qualityCategory2)
				{
					return false;
				}
				if (a.def.category == ThingCategory.Item)
				{
					return a.CanStackWith(b);
				}
				if (a.def.category == ThingCategory.Building)
				{
					return true;
				}
				Log.Error(string.Concat(new object[]
				{
					"Unknown TransferAsOne pair: ",
					a,
					", ",
					b
				}));
				return false;
			}
		}

		// Token: 0x06007608 RID: 30216 RVA: 0x0028BD60 File Offset: 0x00289F60
		public static bool CanStack(Thing thing)
		{
			if (thing.def.category == ThingCategory.Pawn)
			{
				if (thing.def.race.Humanlike)
				{
					return false;
				}
				Pawn pawn = (Pawn)thing;
				if (pawn.health.summaryHealth.SummaryHealthPercent < 0.9999f)
				{
					return false;
				}
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					return false;
				}
				if (pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) != null)
				{
					return false;
				}
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06007609 RID: 30217 RVA: 0x0028BDF8 File Offset: 0x00289FF8
		public static T TransferableMatching<T>(Thing thing, List<T> transferables, TransferAsOneMode mode) where T : Transferable
		{
			if (thing == null || transferables == null)
			{
				return default(T);
			}
			for (int i = 0; i < transferables.Count; i++)
			{
				T t = transferables[i];
				if (t.HasAnyThing && TransferableUtility.TransferAsOne(thing, t.AnyThing, mode))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600760A RID: 30218 RVA: 0x0028BE5C File Offset: 0x0028A05C
		public static Tradeable TradeableMatching(Thing thing, List<Tradeable> tradeables)
		{
			if (thing == null || tradeables == null)
			{
				return null;
			}
			for (int i = 0; i < tradeables.Count; i++)
			{
				Tradeable tradeable = tradeables[i];
				if (tradeable.HasAnyThing)
				{
					TransferAsOneMode mode = tradeable.TraderWillTrade ? TransferAsOneMode.Normal : TransferAsOneMode.InactiveTradeable;
					if (TransferableUtility.TransferAsOne(thing, tradeable.AnyThing, mode))
					{
						return tradeable;
					}
				}
			}
			return null;
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x0028BEB4 File Offset: 0x0028A0B4
		public static TransferableOneWay TransferableMatchingDesperate(Thing thing, List<TransferableOneWay> transferables, TransferAsOneMode mode)
		{
			if (thing == null || transferables == null)
			{
				return null;
			}
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.things.Contains(thing))
				{
					return transferableOneWay;
				}
			}
			for (int j = 0; j < transferables.Count; j++)
			{
				TransferableOneWay transferableOneWay2 = transferables[j];
				if (transferableOneWay2.HasAnyThing && TransferableUtility.TransferAsOne(thing, transferableOneWay2.AnyThing, mode))
				{
					return transferableOneWay2;
				}
			}
			if (!(thing is Pawn))
			{
				for (int k = 0; k < transferables.Count; k++)
				{
					TransferableOneWay transferableOneWay3 = transferables[k];
					if (transferableOneWay3.HasAnyThing && transferableOneWay3.ThingDef == thing.def)
					{
						return transferableOneWay3;
					}
				}
			}
			return null;
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x0028BF70 File Offset: 0x0028A170
		public static List<Pawn> GetPawnsFromTransferables(List<TransferableOneWay> transferables)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].CountToTransfer > 0 && transferables[i].AnyThing is Pawn)
				{
					for (int j = 0; j < transferables[i].CountToTransfer; j++)
					{
						Pawn item = (Pawn)transferables[i].things[j];
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x0028BFF0 File Offset: 0x0028A1F0
		public static void SimulateTradeableTransfer(List<Thing> all, List<Tradeable> tradeables, List<ThingCount> outThingsAfterTransfer)
		{
			outThingsAfterTransfer.Clear();
			for (int i = 0; i < all.Count; i++)
			{
				outThingsAfterTransfer.Add(new ThingCount(all[i], all[i].stackCount));
			}
			Action<Thing, int> <>9__0;
			Action<Thing, int> <>9__1;
			for (int j = 0; j < tradeables.Count; j++)
			{
				int countToTransferToSource = tradeables[j].CountToTransferToSource;
				int countToTransferToDestination = tradeables[j].CountToTransferToDestination;
				if (countToTransferToSource > 0)
				{
					List<Thing> thingsTrader = tradeables[j].thingsTrader;
					int count = countToTransferToSource;
					Action<Thing, int> transfer;
					if ((transfer = <>9__0) == null)
					{
						transfer = (<>9__0 = delegate(Thing originalThing, int toTake)
						{
							outThingsAfterTransfer.Add(new ThingCount(originalThing, toTake));
						});
					}
					TransferableUtility.TransferNoSplit(thingsTrader, count, transfer, false, false);
				}
				else if (countToTransferToDestination > 0)
				{
					List<Thing> thingsColony = tradeables[j].thingsColony;
					int count2 = countToTransferToDestination;
					Action<Thing, int> transfer2;
					if ((transfer2 = <>9__1) == null)
					{
						transfer2 = (<>9__1 = delegate(Thing originalThing, int toTake)
						{
							for (int k = 0; k < outThingsAfterTransfer.Count; k++)
							{
								ThingCount thingCount = outThingsAfterTransfer[k];
								if (thingCount.Thing == originalThing)
								{
									outThingsAfterTransfer[k] = thingCount.WithCount(thingCount.Count - toTake);
									return;
								}
							}
						});
					}
					TransferableUtility.TransferNoSplit(thingsColony, count2, transfer2, false, false);
				}
			}
		}

		// Token: 0x04004171 RID: 16753
		private static List<Thing> tmpThings = new List<Thing>();
	}
}

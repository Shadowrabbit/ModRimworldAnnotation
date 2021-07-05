using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2C RID: 7212
	public static class CollectionsMassCalculator
	{
		// Token: 0x06009E93 RID: 40595 RVA: 0x002E8488 File Offset: 0x002E6688
		public static float Capacity(List<ThingCount> thingCounts, StringBuilder explanation = null)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				if (thingCounts[i].Count > 0)
				{
					Pawn pawn = thingCounts[i].Thing as Pawn;
					if (pawn != null)
					{
						num += MassUtility.Capacity(pawn, explanation) * (float)thingCounts[i].Count;
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06009E94 RID: 40596 RVA: 0x002E84FC File Offset: 0x002E66FC
		public static float MassUsage(List<ThingCount> thingCounts, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			float num = 0f;
			for (int i = 0; i < thingCounts.Count; i++)
			{
				int count = thingCounts[i].Count;
				if (count > 0)
				{
					Thing thing = thingCounts[i].Thing;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						if (includePawnsMass)
						{
							num += pawn.GetStatValue(StatDefOf.Mass, true) * (float)count;
						}
						else
						{
							num += MassUtility.GearAndInventoryMass(pawn) * (float)count;
						}
						if (InventoryCalculatorsUtility.ShouldIgnoreInventoryOf(pawn, ignoreInventory))
						{
							num -= MassUtility.InventoryMass(pawn) * (float)count;
						}
					}
					else
					{
						num += thing.GetStatValue(StatDefOf.Mass, true) * (float)count;
						if (ignoreSpawnedCorpsesGearAndInventory)
						{
							Corpse corpse = thing as Corpse;
							if (corpse != null && corpse.Spawned)
							{
								num -= MassUtility.GearAndInventoryMass(corpse.InnerPawn) * (float)count;
							}
						}
					}
				}
			}
			return Mathf.Max(num, 0f);
		}

		// Token: 0x06009E95 RID: 40597 RVA: 0x002E85E0 File Offset: 0x002E67E0
		public static float CapacityTransferables(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
					}, false, false);
				}
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E96 RID: 40598 RVA: 0x002E867C File Offset: 0x002E687C
		public static float CapacityLeftAfterTransfer(List<TransferableOneWay> transferables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].HasAnyThing && transferables[i].AnyThing is Pawn)
				{
					CollectionsMassCalculator.thingsInReverse.Clear();
					CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
					CollectionsMassCalculator.thingsInReverse.Reverse();
					TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
					{
						CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
					}, false, false);
				}
			}
			CollectionsMassCalculator.thingsInReverse.Clear();
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E97 RID: 40599 RVA: 0x00069894 File Offset: 0x00067A94
		public static float CapacityLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, StringBuilder explanation = null)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E98 RID: 40600 RVA: 0x002E875C File Offset: 0x002E695C
		public static float Capacity<T>(List<T> things, StringBuilder explanation = null) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.Capacity(CollectionsMassCalculator.tmpThingCounts, explanation);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E99 RID: 40601 RVA: 0x002E87C8 File Offset: 0x002E69C8
		public static float MassUsageTransferables(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableUtility.TransferNoSplit(transferables[i].things, transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E9A RID: 40602 RVA: 0x002E8848 File Offset: 0x002E6A48
		public static float MassUsageLeftAfterTransfer(List<TransferableOneWay> transferables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < transferables.Count; i++)
			{
				CollectionsMassCalculator.thingsInReverse.Clear();
				CollectionsMassCalculator.thingsInReverse.AddRange(transferables[i].things);
				CollectionsMassCalculator.thingsInReverse.Reverse();
				TransferableUtility.TransferNoSplit(CollectionsMassCalculator.thingsInReverse, transferables[i].MaxCount - transferables[i].CountToTransfer, delegate(Thing originalThing, int toTake)
				{
					CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(originalThing, toTake));
				}, false, false);
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E9B RID: 40603 RVA: 0x002E88F8 File Offset: 0x002E6AF8
		public static float MassUsage<T>(List<T> things, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false) where T : Thing
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			for (int i = 0; i < things.Count; i++)
			{
				CollectionsMassCalculator.tmpThingCounts.Add(new ThingCount(things[i], things[i].stackCount));
			}
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x06009E9C RID: 40604 RVA: 0x000698C1 File Offset: 0x00067AC1
		public static float MassUsageLeftAfterTradeableTransfer(List<Thing> allCurrentThings, List<Tradeable> tradeables, IgnorePawnsInventoryMode ignoreInventory, bool includePawnsMass = false, bool ignoreSpawnedCorpsesGearAndInventory = false)
		{
			CollectionsMassCalculator.tmpThingCounts.Clear();
			TransferableUtility.SimulateTradeableTransfer(allCurrentThings, tradeables, CollectionsMassCalculator.tmpThingCounts);
			float result = CollectionsMassCalculator.MassUsage(CollectionsMassCalculator.tmpThingCounts, ignoreInventory, includePawnsMass, ignoreSpawnedCorpsesGearAndInventory);
			CollectionsMassCalculator.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x04006527 RID: 25895
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04006528 RID: 25896
		private static List<Thing> thingsInReverse = new List<Thing>();
	}
}

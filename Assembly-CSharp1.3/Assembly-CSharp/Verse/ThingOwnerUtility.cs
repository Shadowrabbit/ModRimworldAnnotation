using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200037A RID: 890
	public static class ThingOwnerUtility
	{
		// Token: 0x060019D1 RID: 6609 RVA: 0x00097270 File Offset: 0x00095470
		public static bool ThisOrAnyCompIsThingHolder(this ThingDef thingDef)
		{
			if (typeof(IThingHolder).IsAssignableFrom(thingDef.thingClass))
			{
				return true;
			}
			for (int i = 0; i < thingDef.comps.Count; i++)
			{
				if (typeof(IThingHolder).IsAssignableFrom(thingDef.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000972D4 File Offset: 0x000954D4
		public static ThingOwner TryGetInnerInteractableThingOwner(this Thing thing)
		{
			IThingHolder thingHolder = thing as IThingHolder;
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingHolder != null)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				if (directlyHeldThings != null)
				{
					return directlyHeldThings;
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					IThingHolder thingHolder2 = allComps[i] as IThingHolder;
					if (thingHolder2 != null)
					{
						ThingOwner directlyHeldThings2 = thingHolder2.GetDirectlyHeldThings();
						if (directlyHeldThings2 != null)
						{
							return directlyHeldThings2;
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			if (thingHolder != null)
			{
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
				{
					ThingOwner directlyHeldThings3 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
					if (directlyHeldThings3 != null)
					{
						ThingOwnerUtility.tmpHolders.Clear();
						return directlyHeldThings3;
					}
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps2 = thingWithComps.AllComps;
				for (int j = 0; j < allComps2.Count; j++)
				{
					IThingHolder thingHolder3 = allComps2[j] as IThingHolder;
					if (thingHolder3 != null)
					{
						thingHolder3.GetChildHolders(ThingOwnerUtility.tmpHolders);
						if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
						{
							ThingOwner directlyHeldThings4 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
							if (directlyHeldThings4 != null)
							{
								ThingOwnerUtility.tmpHolders.Clear();
								return directlyHeldThings4;
							}
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			return null;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00097402 File Offset: 0x00095602
		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			return ThingOwnerUtility.SpawnedParentOrMe(holder) != null;
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00097410 File Offset: 0x00095610
		public static Thing SpawnedParentOrMe(IThingHolder holder)
		{
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Spawned)
				{
					return thing;
				}
				ThingComp thingComp = holder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00097460 File Offset: 0x00095660
		public static IntVec3 GetRootPosition(IThingHolder holder)
		{
			IntVec3 result = IntVec3.Invalid;
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Position.IsValid)
				{
					result = thing.Position;
				}
				else
				{
					ThingComp thingComp = holder as ThingComp;
					if (thingComp != null && thingComp.parent.Position.IsValid)
					{
						result = thingComp.parent.Position;
					}
				}
				holder = holder.ParentHolder;
			}
			return result;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x000974D0 File Offset: 0x000956D0
		public static Map GetRootMap(IThingHolder holder)
		{
			while (holder != null)
			{
				Map map = holder as Map;
				if (map != null)
				{
					return map;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x000974F8 File Offset: 0x000956F8
		public static int GetRootTile(IThingHolder holder)
		{
			while (holder != null)
			{
				WorldObject worldObject = holder as WorldObject;
				if (worldObject != null && worldObject.Tile >= 0)
				{
					return worldObject.Tile;
				}
				holder = holder.ParentHolder;
			}
			return -1;
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00097530 File Offset: 0x00095730
		public static bool ContentsSuspended(IThingHolder holder)
		{
			while (holder != null)
			{
				ISuspendableThingHolder suspendableThingHolder;
				if (holder is Building_CryptosleepCasket || ((suspendableThingHolder = (holder as ISuspendableThingHolder)) != null && suspendableThingHolder.IsContentsSuspended))
				{
					return true;
				}
				holder = holder.ParentHolder;
			}
			return false;
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00097567 File Offset: 0x00095767
		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000975A2 File Offset: 0x000957A2
		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x000975BA File Offset: 0x000957BA
		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000975C8 File Offset: 0x000957C8
		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x000975D0 File Offset: 0x000957D0
		public static void AppendThingHoldersFromThings(List<IThingHolder> outThingsHolders, IList<Thing> container)
		{
			if (container == null)
			{
				return;
			}
			int i = 0;
			int count = container.Count;
			while (i < count)
			{
				IThingHolder thingHolder = container[i] as IThingHolder;
				if (thingHolder != null)
				{
					outThingsHolders.Add(thingHolder);
				}
				ThingWithComps thingWithComps = container[i] as ThingWithComps;
				if (thingWithComps != null)
				{
					List<ThingComp> allComps = thingWithComps.AllComps;
					for (int j = 0; j < allComps.Count; j++)
					{
						IThingHolder thingHolder2 = allComps[j] as IThingHolder;
						if (thingHolder2 != null)
						{
							outThingsHolders.Add(thingHolder2);
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00097655 File Offset: 0x00095855
		public static bool AnyParentIs<T>(Thing thing) where T : class, IThingHolder
		{
			return ThingOwnerUtility.GetAnyParent<T>(thing) != null;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00097668 File Offset: 0x00095868
		public static T GetAnyParent<T>(Thing thing) where T : class, IThingHolder
		{
			T t = thing as T;
			if (t != null)
			{
				return t;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				T t2 = parentHolder as T;
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x000976C0 File Offset: 0x000958C0
		public static Thing GetFirstSpawnedParentThing(Thing thing)
		{
			if (thing.Spawned)
			{
				return thing;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				Thing thing2 = parentHolder as Thing;
				if (thing2 != null && thing2.Spawned)
				{
					return thing2;
				}
				ThingComp thingComp = parentHolder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
			}
			return null;
		}

		// Token: 0x060019E1 RID: 6625 RVA: 0x00097720 File Offset: 0x00095920
		public static void GetAllThingsRecursively(IThingHolder holder, List<Thing> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null)
		{
			outThings.Clear();
			if (passCheck != null && !passCheck(holder))
			{
				return;
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpStack.Push(holder);
			while (ThingOwnerUtility.tmpStack.Count != 0)
			{
				IThingHolder thingHolder = ThingOwnerUtility.tmpStack.Pop();
				if (allowUnreal || ThingOwnerUtility.AreImmediateContentsReal(thingHolder))
				{
					ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						outThings.AddRange(directlyHeldThings);
					}
				}
				ThingOwnerUtility.tmpHolders.Clear();
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				for (int i = 0; i < ThingOwnerUtility.tmpHolders.Count; i++)
				{
					if (passCheck == null || passCheck(ThingOwnerUtility.tmpHolders[i]))
					{
						ThingOwnerUtility.tmpStack.Push(ThingOwnerUtility.tmpHolders[i]);
					}
				}
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpHolders.Clear();
		}

		// Token: 0x060019E2 RID: 6626 RVA: 0x000977F8 File Offset: 0x000959F8
		public static void GetAllThingsRecursively<T>(Map map, ThingRequest request, List<T> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null, bool alsoGetSpawnedThings = true) where T : Thing
		{
			outThings.Clear();
			if (alsoGetSpawnedThings)
			{
				List<Thing> list = map.listerThings.ThingsMatching(request);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i] as T;
					if (t != null)
					{
						outThings.Add(t);
					}
				}
			}
			ThingOwnerUtility.tmpMapChildHolders.Clear();
			map.GetChildHolders(ThingOwnerUtility.tmpMapChildHolders);
			for (int j = 0; j < ThingOwnerUtility.tmpMapChildHolders.Count; j++)
			{
				ThingOwnerUtility.tmpThings.Clear();
				ThingOwnerUtility.GetAllThingsRecursively(ThingOwnerUtility.tmpMapChildHolders[j], ThingOwnerUtility.tmpThings, allowUnreal, passCheck);
				for (int k = 0; k < ThingOwnerUtility.tmpThings.Count; k++)
				{
					T t2 = ThingOwnerUtility.tmpThings[k] as T;
					if (t2 != null && request.Accepts(t2))
					{
						outThings.Add(t2);
					}
				}
			}
			ThingOwnerUtility.tmpThings.Clear();
			ThingOwnerUtility.tmpMapChildHolders.Clear();
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00097904 File Offset: 0x00095B04
		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal, null);
			return list;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00097921 File Offset: 0x00095B21
		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x0009793C File Offset: 0x00095B3C
		public static bool TryGetFixedTemperature(IThingHolder holder, Thing forThing, out float temperature)
		{
			if (holder is Pawn_InventoryTracker && forThing.TryGetComp<CompHatcher>() != null)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompLaunchable || holder is ActiveDropPodInfo || holder is TravelingTransportPods)
			{
				temperature = 14f;
				return true;
			}
			if (holder is Settlement_TraderTracker || holder is TradeShip)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompTransporter)
			{
				temperature = 14f;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x04001118 RID: 4376
		private static Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		// Token: 0x04001119 RID: 4377
		private static List<IThingHolder> tmpHolders = new List<IThingHolder>();

		// Token: 0x0400111A RID: 4378
		private static List<Thing> tmpThings = new List<Thing>();

		// Token: 0x0400111B RID: 4379
		private static List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();
	}
}

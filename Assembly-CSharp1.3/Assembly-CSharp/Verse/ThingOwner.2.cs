using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000379 RID: 889
	public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001999 RID: 6553 RVA: 0x0009653F File Offset: 0x0009473F
		public IThingHolder Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x0600199A RID: 6554
		public abstract int Count { get; }

		// Token: 0x17000553 RID: 1363
		public Thing this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x0600199C RID: 6556 RVA: 0x00096550 File Offset: 0x00094750
		public bool Any
		{
			get
			{
				return this.Count > 0;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x0600199D RID: 6557 RVA: 0x0009655C File Offset: 0x0009475C
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					num += this.GetAt(i).stackCount;
				}
				return num;
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x0600199E RID: 6558 RVA: 0x0009658E File Offset: 0x0009478E
		public string ContentsString
		{
			get
			{
				if (this.Any)
				{
					return GenThing.ThingsToCommaList(this, false, true, -1);
				}
				return "NothingLower".Translate();
			}
		}

		// Token: 0x17000557 RID: 1367
		Thing IList<Thing>.this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x060019A1 RID: 6561 RVA: 0x000126F5 File Offset: 0x000108F5
		bool ICollection<Thing>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x000965B1 File Offset: 0x000947B1
		public ThingOwner()
		{
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000965CB File Offset: 0x000947CB
		public ThingOwner(IThingHolder owner)
		{
			this.owner = owner;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x000965EC File Offset: 0x000947EC
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : this(owner)
		{
			this.maxStacks = (oneStackOnly ? 1 : 999999);
			this.contentsLookMode = contentsLookMode;
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0009660D File Offset: 0x0009480D
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
			Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x00096638 File Offset: 0x00094838
		public void ThingOwnerTick(bool removeIfDestroyed = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing at = this.GetAt(i);
				if (at.def.tickerType == TickerType.Normal)
				{
					at.Tick();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x00096688 File Offset: 0x00094888
		public void ThingOwnerTickRare(bool removeIfDestroyed = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing at = this.GetAt(i);
				if (at.def.tickerType == TickerType.Rare)
				{
					at.TickRare();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x000966D8 File Offset: 0x000948D8
		public void ThingOwnerTickLong(bool removeIfDestroyed = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing at = this.GetAt(i);
				if (at.def.tickerType == TickerType.Long)
				{
					at.TickRare();
					if (at.Destroyed && removeIfDestroyed)
					{
						this.Remove(at);
					}
				}
			}
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00096728 File Offset: 0x00094928
		public void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.Remove(this.GetAt(i));
			}
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00096758 File Offset: 0x00094958
		public void ClearAndDestroyContents(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.Destroy(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0009679C File Offset: 0x0009499C
		public void ClearAndDestroyContentsOrPassToWorld(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.DestroyOrPassToWorld(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x000967DD File Offset: 0x000949DD
		public bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			return this.GetCountCanAccept(item, canMergeWithExistingStacks) > 0;
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x000967EC File Offset: 0x000949EC
		public virtual int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null || item.stackCount <= 0)
			{
				return 0;
			}
			if (this.maxStacks == 999999)
			{
				return item.stackCount;
			}
			int num = 0;
			if (this.Count < this.maxStacks)
			{
				num += (this.maxStacks - this.Count) * item.def.stackLimit;
			}
			if (num >= item.stackCount)
			{
				return Mathf.Min(num, item.stackCount);
			}
			if (canMergeWithExistingStacks)
			{
				int i = 0;
				int count = this.Count;
				while (i < count)
				{
					Thing at = this.GetAt(i);
					if (at.stackCount < at.def.stackLimit && at.CanStackWith(item))
					{
						num += at.def.stackLimit - at.stackCount;
						if (num >= item.stackCount)
						{
							return Mathf.Min(num, item.stackCount);
						}
					}
					i++;
				}
			}
			return Mathf.Min(num, item.stackCount);
		}

		// Token: 0x060019AE RID: 6574
		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		// Token: 0x060019AF RID: 6575
		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		// Token: 0x060019B0 RID: 6576
		public abstract int IndexOf(Thing item);

		// Token: 0x060019B1 RID: 6577
		public abstract bool Remove(Thing item);

		// Token: 0x060019B2 RID: 6578
		protected abstract Thing GetAt(int index);

		// Token: 0x060019B3 RID: 6579 RVA: 0x000968CE File Offset: 0x00094ACE
		public bool Contains(Thing item)
		{
			return item != null && item.holdingOwner == this;
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x000968DE File Offset: 0x00094ADE
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.GetAt(index));
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00096906 File Offset: 0x00094B06
		public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, count, canMergeWithExistingStacks);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x00096938 File Offset: 0x00094B38
		public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, canMergeWithExistingStacks);
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x00096968 File Offset: 0x00094B68
		public void TryAddRangeOrTransfer(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things == this)
			{
				return;
			}
			ThingOwner thingOwner = things as ThingOwner;
			if (thingOwner != null)
			{
				thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				if (destroyLeftover)
				{
					thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				IList<Thing> list = things as IList<Thing>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!this.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (Thing thing in things)
				{
					if (!this.TryAddOrTransfer(thing, canMergeWithExistingStacks) && destroyLeftover)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x00096A28 File Offset: 0x00094C28
		public int RemoveAll(Predicate<Thing> predicate)
		{
			int num = 0;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				if (predicate(this.GetAt(i)))
				{
					this.Remove(this.GetAt(i));
					num++;
				}
			}
			return num;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00096A6C File Offset: 0x00094C6C
		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
		{
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00096A88 File Offset: 0x00094C88
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x00096AA4 File Offset: 0x00094CA4
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			if (!this.Contains(item))
			{
				Log.Error(string.Concat(new object[]
				{
					"Can't transfer item ",
					item,
					" because it's not here. owner=",
					this.owner.ToStringSafe<IThingHolder>()
				}));
				resultingTransferredItem = null;
				return 0;
			}
			if (otherContainer == this && count > 0)
			{
				resultingTransferredItem = item;
				return item.stackCount;
			}
			if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (count <= 0)
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (this.owner is Map || otherContainer.owner is Map)
			{
				Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))");
				resultingTransferredItem = null;
				return 0;
			}
			int num = Mathf.Min(item.stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.Contains(thing))
			{
				this.Remove(thing);
			}
			if (otherContainer.TryAdd(thing, canMergeWithExistingStacks))
			{
				resultingTransferredItem = thing;
				return thing.stackCount;
			}
			resultingTransferredItem = null;
			if (otherContainer.Contains(thing) || thing.stackCount <= 0 || thing.Destroyed)
			{
				return thing.stackCount;
			}
			int result = num - thing.stackCount;
			if (item != thing)
			{
				item.TryAbsorbStack(thing, false);
				return result;
			}
			this.TryAdd(thing, false);
			return result;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00096BCC File Offset: 0x00094DCC
		public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.TryTransferToContainer(this.GetAt(i), other, canMergeWithExistingStacks);
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00096BFC File Offset: 0x00094DFC
		public Thing Take(Thing thing, int count)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe<Thing>() + " but it's not here.");
				return null;
			}
			if (count > thing.stackCount)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get ",
					count,
					" of ",
					thing.ToStringSafe<Thing>(),
					" while only having ",
					thing.stackCount
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				this.Remove(thing);
				return thing;
			}
			Thing thing2 = thing.SplitOff(count);
			thing2.holdingOwner = null;
			return thing2;
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00096CA8 File Offset: 0x00094EA8
		public Thing Take(Thing thing)
		{
			return this.Take(thing, thing.stackCount);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00096CB8 File Offset: 0x00094EB8
		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction, nearPlaceValidator);
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00096D18 File Offset: 0x00094F18
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe<Thing>() + " but it's not here.");
				resultingThing = null;
				return false;
			}
			if (thing.stackCount < count)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to drop ",
					count,
					" of ",
					thing,
					" while only having ",
					thing.stackCount
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					this.Remove(thing);
					return true;
				}
				return false;
			}
			else
			{
				Thing thing2 = thing.SplitOff(count);
				if (GenDrop.TryDropSpawn(thing2, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					return true;
				}
				thing.TryAbsorbStack(thing2, false);
				return false;
			}
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x00096DF8 File Offset: 0x00094FF8
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x00096E54 File Offset: 0x00095054
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe<IThingHolder>() + " container tried to drop  " + thing.ToStringSafe<Thing>() + " which it didn't contain.");
				lastResultingThing = null;
				return false;
			}
			if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, playDropSound))
			{
				this.Remove(thing);
				return true;
			}
			return false;
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x00096EB4 File Offset: 0x000950B4
		public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode, Action<Thing, int> placeAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			bool result = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing thing;
				if (!this.TryDrop(this.GetAt(i), dropLoc, map, mode, out thing, placeAction, nearPlaceValidator, playDropSound))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x00096EF3 File Offset: 0x000950F3
		public bool Contains(ThingDef def)
		{
			return this.Contains(def, 1);
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00096F00 File Offset: 0x00095100
		public bool Contains(ThingDef def, int minCount)
		{
			if (minCount <= 0)
			{
				return true;
			}
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
				if (num >= minCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00096F50 File Offset: 0x00095150
		public int TotalStackCountOfDef(ThingDef def)
		{
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
			}
			return num;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x00096F91 File Offset: 0x00095191
		public void Notify_ContainedItemDestroyed(Thing t)
		{
			if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
			{
				this.Remove(t);
			}
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00096FA8 File Offset: 0x000951A8
		protected void NotifyAdded(Thing item)
		{
			if (ThingOwnerUtility.ShouldAutoExtinguishInnerThings(this.owner) && item.HasAttachment(ThingDefOf.Fire))
			{
				item.GetAttachment(ThingDefOf.Fire).Destroy(DestroyMode.Vanish);
			}
			if (ThingOwnerUtility.ShouldRemoveDesignationsOnAddedThings(this.owner))
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].designationManager.RemoveAllDesignationsOn(item, false);
				}
			}
			Thing thing;
			WorldObject worldObject;
			Pawn_InventoryTracker pawn_InventoryTracker;
			if (((thing = (this.owner as Thing)) != null && thing.Faction.IsPlayerSafe()) || ((worldObject = (this.owner as WorldObject)) != null && worldObject.Faction.IsPlayerSafe()) || ((pawn_InventoryTracker = (this.owner as Pawn_InventoryTracker)) != null && pawn_InventoryTracker.pawn.Faction.IsPlayerSafe()))
			{
				item.EverSeenByPlayer = true;
			}
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAdded(item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnAdded((Pawn)item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelAdded((Apparel)item);
				if (pawn_ApparelTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
				if (pawn_EquipmentTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			this.NotifyColonistBarIfColonistCorpse(item);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x0009712C File Offset: 0x0009532C
		protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
		{
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
			}
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00097150 File Offset: 0x00095350
		protected void NotifyRemoved(Thing item)
		{
			Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				pawn_InventoryTracker.Notify_ItemRemoved(item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelRemoved((Apparel)item);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentRemoved((ThingWithComps)item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnRemoved((Pawn)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000971CC File Offset: 0x000953CC
		private void NotifyColonistBarIfColonistCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000964E6 File Offset: 0x000946E6
		void IList<Thing>.Insert(int index, Thing item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0009721C File Offset: 0x0009541C
		void ICollection<Thing>.Add(Thing item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00097228 File Offset: 0x00095428
		void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this.GetAt(i);
			}
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00097252 File Offset: 0x00095452
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.Count; i = num + 1)
			{
				yield return this.GetAt(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00097261 File Offset: 0x00095461
		IEnumerator IEnumerable.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.Count; i = num + 1)
			{
				yield return this.GetAt(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x04001114 RID: 4372
		protected IThingHolder owner;

		// Token: 0x04001115 RID: 4373
		protected int maxStacks = 999999;

		// Token: 0x04001116 RID: 4374
		public LookMode contentsLookMode = LookMode.Deep;

		// Token: 0x04001117 RID: 4375
		private const int InfMaxStacks = 999999;
	}
}

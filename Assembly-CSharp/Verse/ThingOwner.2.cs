using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000513 RID: 1299
	public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x0001CB9D File Offset: 0x0001AD9D
		public IThingHolder Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060020C6 RID: 8390
		public abstract int Count { get; }

		// Token: 0x17000639 RID: 1593
		public Thing this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x0001CBAE File Offset: 0x0001ADAE
		public bool Any
		{
			get
			{
				return this.Count > 0;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x00104658 File Offset: 0x00102858
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

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x0001CBB9 File Offset: 0x0001ADB9
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

		// Token: 0x1700063D RID: 1597
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

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x0000A2A7 File Offset: 0x000084A7
		bool ICollection<Thing>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x0001CBDC File Offset: 0x0001ADDC
		public ThingOwner()
		{
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x0001CBF6 File Offset: 0x0001ADF6
		public ThingOwner(IThingHolder owner)
		{
			this.owner = owner;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x0001CC17 File Offset: 0x0001AE17
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : this(owner)
		{
			this.maxStacks = (oneStackOnly ? 1 : 999999);
			this.contentsLookMode = contentsLookMode;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x0001CC38 File Offset: 0x0001AE38
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
			Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x0010468C File Offset: 0x0010288C
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

		// Token: 0x060020D3 RID: 8403 RVA: 0x001046DC File Offset: 0x001028DC
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

		// Token: 0x060020D4 RID: 8404 RVA: 0x0010472C File Offset: 0x0010292C
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

		// Token: 0x060020D5 RID: 8405 RVA: 0x0010477C File Offset: 0x0010297C
		public void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.Remove(this.GetAt(i));
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x001047AC File Offset: 0x001029AC
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

		// Token: 0x060020D7 RID: 8407 RVA: 0x001047F0 File Offset: 0x001029F0
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

		// Token: 0x060020D8 RID: 8408 RVA: 0x0001CC62 File Offset: 0x0001AE62
		public bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			return this.GetCountCanAccept(item, canMergeWithExistingStacks) > 0;
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x00104834 File Offset: 0x00102A34
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

		// Token: 0x060020DA RID: 8410
		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		// Token: 0x060020DB RID: 8411
		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		// Token: 0x060020DC RID: 8412
		public abstract int IndexOf(Thing item);

		// Token: 0x060020DD RID: 8413
		public abstract bool Remove(Thing item);

		// Token: 0x060020DE RID: 8414
		protected abstract Thing GetAt(int index);

		// Token: 0x060020DF RID: 8415 RVA: 0x0001CC6F File Offset: 0x0001AE6F
		public bool Contains(Thing item)
		{
			return item != null && item.holdingOwner == this;
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x0001CC7F File Offset: 0x0001AE7F
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.GetAt(index));
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0001CCA7 File Offset: 0x0001AEA7
		public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.", false);
				return 0;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, count, canMergeWithExistingStacks);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x0001CCDA File Offset: 0x0001AEDA
		public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.", false);
				return false;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, canMergeWithExistingStacks);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00104918 File Offset: 0x00102B18
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

		// Token: 0x060020E4 RID: 8420 RVA: 0x001049D8 File Offset: 0x00102BD8
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

		// Token: 0x060020E5 RID: 8421 RVA: 0x0001CD0B File Offset: 0x0001AF0B
		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
		{
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00104A1C File Offset: 0x00102C1C
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00104A38 File Offset: 0x00102C38
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
				}), false);
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
				Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))", false);
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

		// Token: 0x060020E8 RID: 8424 RVA: 0x00104B60 File Offset: 0x00102D60
		public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.TryTransferToContainer(this.GetAt(i), other, canMergeWithExistingStacks);
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00104B90 File Offset: 0x00102D90
		public Thing Take(Thing thing, int count)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe<Thing>() + " but it's not here.", false);
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
				}), false);
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

		// Token: 0x060020EA RID: 8426 RVA: 0x0001CD24 File Offset: 0x0001AF24
		public Thing Take(Thing thing)
		{
			return this.Take(thing, thing.stackCount);
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00104C40 File Offset: 0x00102E40
		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.", false);
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction, nearPlaceValidator);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00104CA0 File Offset: 0x00102EA0
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe<Thing>() + " but it's not here.", false);
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
				}), false);
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				if (GenDrop.TryDropSpawn_NewTmp(thing, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					this.Remove(thing);
					return true;
				}
				return false;
			}
			else
			{
				Thing thing2 = thing.SplitOff(count);
				if (GenDrop.TryDropSpawn_NewTmp(thing2, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					return true;
				}
				thing.TryAbsorbStack(thing2, false);
				return false;
			}
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00104D80 File Offset: 0x00102F80
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + thing + " without a dropLoc and with an owner whose map is null.", false);
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop_NewTmp(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00104DDC File Offset: 0x00102FDC
		[Obsolete("Only used for mod compatibility")]
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			return this.TryDrop_NewTmp(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00104DFC File Offset: 0x00102FFC
		public bool TryDrop_NewTmp(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe<IThingHolder>() + " container tried to drop  " + thing.ToStringSafe<Thing>() + " which it didn't contain.", false);
				lastResultingThing = null;
				return false;
			}
			if (GenDrop.TryDropSpawn_NewTmp(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, playDropSound))
			{
				this.Remove(thing);
				return true;
			}
			return false;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00104E60 File Offset: 0x00103060
		public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode, Action<Thing, int> placeAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			bool result = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing thing;
				if (!this.TryDrop_NewTmp(this.GetAt(i), dropLoc, map, mode, out thing, placeAction, nearPlaceValidator, true))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x0001CD33 File Offset: 0x0001AF33
		public bool Contains(ThingDef def)
		{
			return this.Contains(def, 1);
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x00104EA0 File Offset: 0x001030A0
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

		// Token: 0x060020F3 RID: 8435 RVA: 0x00104EF0 File Offset: 0x001030F0
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

		// Token: 0x060020F4 RID: 8436 RVA: 0x0001CD3D File Offset: 0x0001AF3D
		public void Notify_ContainedItemDestroyed(Thing t)
		{
			if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
			{
				this.Remove(t);
			}
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x00104F34 File Offset: 0x00103134
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
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x0010501C File Offset: 0x0010321C
		protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
		{
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00105040 File Offset: 0x00103240
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

		// Token: 0x060020F8 RID: 8440 RVA: 0x001050BC File Offset: 0x001032BC
		private void NotifyColonistBarIfColonistCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x0001CAF1 File Offset: 0x0001ACF1
		void IList<Thing>.Insert(int index, Thing item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x0001CD54 File Offset: 0x0001AF54
		void ICollection<Thing>.Add(Thing item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x0010510C File Offset: 0x0010330C
		void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this.GetAt(i);
			}
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x0001CD5F File Offset: 0x0001AF5F
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

		// Token: 0x060020FD RID: 8445 RVA: 0x0001CD6E File Offset: 0x0001AF6E
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

		// Token: 0x040016B1 RID: 5809
		protected IThingHolder owner;

		// Token: 0x040016B2 RID: 5810
		protected int maxStacks = 999999;

		// Token: 0x040016B3 RID: 5811
		public LookMode contentsLookMode = LookMode.Deep;

		// Token: 0x040016B4 RID: 5812
		private const int InfMaxStacks = 999999;
	}
}

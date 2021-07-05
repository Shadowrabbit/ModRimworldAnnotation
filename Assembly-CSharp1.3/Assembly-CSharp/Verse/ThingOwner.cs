using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000375 RID: 885
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable where T : Thing
	{
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x00095DC8 File Offset: 0x00093FC8
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x17000548 RID: 1352
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x00095DDE File Offset: 0x00093FDE
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x1700054A RID: 1354
		T IList<!0>.this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x000126F5 File Offset: 0x000108F5
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00095DF7 File Offset: 0x00093FF7
		public ThingOwner()
		{
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00095E0A File Offset: 0x0009400A
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00095E1E File Offset: 0x0009401E
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00095E34 File Offset: 0x00094034
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<T>(ref this.innerList, true, "innerList", this.contentsLookMode, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.innerList.RemoveAll((T x) => x == null);
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					if (this.innerList[i] != null)
					{
						this.innerList[i].holdingOwner = this;
					}
				}
			}
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00095EE6 File Offset: 0x000940E6
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00095EF3 File Offset: 0x000940F3
		public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (!(item is T))
			{
				return 0;
			}
			return base.GetCountCanAccept(item, canMergeWithExistingStacks);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00095F08 File Offset: 0x00094108
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (count <= 0)
			{
				return 0;
			}
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return 0;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to add ",
					count,
					" of ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return 0;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return 0;
			}
			int stackCount = item.stackCount;
			int num = Mathf.Min(stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.TryAdd((T)((object)thing), canMergeWithExistingStacks))
			{
				return num;
			}
			if (thing != item)
			{
				int result = stackCount - item.stackCount - thing.stackCount;
				item.TryAbsorbStack(thing, false);
				return result;
			}
			return stackCount - item.stackCount;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00096020 File Offset: 0x00094220
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return false;
			}
			T t = item as T;
			if (t == null)
			{
				return false;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return false;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return false;
			}
			if (canMergeWithExistingStacks)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					T t2 = this.innerList[i];
					if (t2.CanStackWith(item))
					{
						int num = Mathf.Min(item.stackCount, t2.def.stackLimit - t2.stackCount);
						if (num > 0)
						{
							Thing other = item.SplitOff(num);
							int stackCount = t2.stackCount;
							t2.TryAbsorbStack(other, true);
							if (t2.stackCount > stackCount)
							{
								base.NotifyAddedAndMergedWith(t2, t2.stackCount - stackCount);
							}
							if (item.Destroyed || item.stackCount == 0)
							{
								return true;
							}
						}
					}
				}
			}
			if (this.Count >= this.maxStacks)
			{
				return false;
			}
			item.holdingOwner = this;
			this.innerList.Add(t);
			base.NotifyAdded(t);
			return true;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x000961E4 File Offset: 0x000943E4
		public void TryAddRangeOrTransfer(IEnumerable<T> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
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
				IList<T> list = things as IList<!0>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!base.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (T t in things)
				{
					if (!base.TryAddOrTransfer(t, canMergeWithExistingStacks) && destroyLeftover)
					{
						t.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x000962B8 File Offset: 0x000944B8
		public override int IndexOf(Thing item)
		{
			T t = item as T;
			if (t == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(t);
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x000962E8 File Offset: 0x000944E8
		public override bool Remove(Thing item)
		{
			if (!base.Contains(item))
			{
				return false;
			}
			if (item.holdingOwner == this)
			{
				item.holdingOwner = null;
			}
			int index = this.innerList.LastIndexOf((T)((object)item));
			this.innerList.RemoveAt(index);
			base.NotifyRemoved(item);
			return true;
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00096338 File Offset: 0x00094538
		public int RemoveAll(Predicate<T> predicate)
		{
			int num = 0;
			for (int i = this.innerList.Count - 1; i >= 0; i--)
			{
				if (predicate(this.innerList[i]))
				{
					this.Remove(this.innerList[i]);
					num++;
				}
			}
			return num;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00096390 File Offset: 0x00094590
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x000963A4 File Offset: 0x000945A4
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x000963CB File Offset: 0x000945CB
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x000963DA File Offset: 0x000945DA
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x000963E8 File Offset: 0x000945E8
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2, nearPlaceValidator);
			resultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0009643C File Offset: 0x0009463C
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, mode, out thing2, placedAction2, nearPlaceValidator);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x00096488 File Offset: 0x00094688
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator, true);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x000964D8 File Offset: 0x000946D8
		int IList<!0>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x000964E6 File Offset: 0x000946E6
		void IList<!0>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x000964F2 File Offset: 0x000946F2
		void ICollection<!0>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00096502 File Offset: 0x00094702
		void ICollection<!0>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00096511 File Offset: 0x00094711
		bool ICollection<!0>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0009651F File Offset: 0x0009471F
		bool ICollection<!0>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0009652D File Offset: 0x0009472D
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0009652D File Offset: 0x0009472D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x04001113 RID: 4371
		private List<T> innerList = new List<T>();
	}
}

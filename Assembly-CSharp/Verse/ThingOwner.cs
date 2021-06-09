using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200050D RID: 1293
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable where T : Thing
	{
		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x0001CA26 File Offset: 0x0001AC26
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x17000632 RID: 1586
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x0001CA3C File Offset: 0x0001AC3C
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000634 RID: 1588
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

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x0600209D RID: 8349 RVA: 0x0000A2A7 File Offset: 0x000084A7
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0001CA55 File Offset: 0x0001AC55
		public ThingOwner()
		{
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x0001CA68 File Offset: 0x0001AC68
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x0001CA7C File Offset: 0x0001AC7C
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep) : base(owner, oneStackOnly, contentsLookMode)
		{
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x00103FFC File Offset: 0x001021FC
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

		// Token: 0x060020A2 RID: 8354 RVA: 0x0001CA92 File Offset: 0x0001AC92
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x0001CA9F File Offset: 0x0001AC9F
		public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (!(item is T))
			{
				return 0;
			}
			return base.GetCountCanAccept(item, canMergeWithExistingStacks);
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x001040B0 File Offset: 0x001022B0
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (count <= 0)
			{
				return 0;
			}
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.", false);
				return 0;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item + " to ThingOwner but this item is already here.", false);
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
				}), false);
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

		// Token: 0x060020A5 RID: 8357 RVA: 0x001041CC File Offset: 0x001023CC
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.", false);
				return false;
			}
			T t = item as T;
			if (t == null)
			{
				return false;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.", false);
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
				}), false);
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

		// Token: 0x060020A6 RID: 8358 RVA: 0x00104394 File Offset: 0x00102594
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

		// Token: 0x060020A7 RID: 8359 RVA: 0x00104468 File Offset: 0x00102668
		public override int IndexOf(Thing item)
		{
			T t = item as T;
			if (t == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(t);
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x00104498 File Offset: 0x00102698
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

		// Token: 0x060020A9 RID: 8361 RVA: 0x001044E8 File Offset: 0x001026E8
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

		// Token: 0x060020AA RID: 8362 RVA: 0x0001CAB3 File Offset: 0x0001ACB3
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x00104540 File Offset: 0x00102740
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x0001CAC6 File Offset: 0x0001ACC6
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x0001CAD5 File Offset: 0x0001ACD5
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00104568 File Offset: 0x00102768
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

		// Token: 0x060020AF RID: 8367 RVA: 0x001045BC File Offset: 0x001027BC
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

		// Token: 0x060020B0 RID: 8368 RVA: 0x00104608 File Offset: 0x00102808
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
			bool result = base.TryDrop_NewTmp(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator, true);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x0001CAE3 File Offset: 0x0001ACE3
		int IList<!0>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x0001CAF1 File Offset: 0x0001ACF1
		void IList<!0>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x0001CAFD File Offset: 0x0001ACFD
		void ICollection<!0>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x0001CB0D File Offset: 0x0001AD0D
		void ICollection<!0>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x0001CB1C File Offset: 0x0001AD1C
		bool ICollection<!0>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x0001CB2A File Offset: 0x0001AD2A
		bool ICollection<!0>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x0001CB38 File Offset: 0x0001AD38
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x0001CB38 File Offset: 0x0001AD38
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x040016AB RID: 5803
		private List<T> innerList = new List<T>();
	}
}

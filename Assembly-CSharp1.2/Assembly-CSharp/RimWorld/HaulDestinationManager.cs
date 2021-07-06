using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200168D RID: 5773
	public sealed class HaulDestinationManager
	{
		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06007E40 RID: 32320 RVA: 0x00054E34 File Offset: 0x00053034
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06007E41 RID: 32321 RVA: 0x00054E34 File Offset: 0x00053034
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06007E42 RID: 32322 RVA: 0x00054E34 File Offset: 0x00053034
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06007E43 RID: 32323 RVA: 0x00054E3C File Offset: 0x0005303C
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06007E44 RID: 32324 RVA: 0x00054E3C File Offset: 0x0005303C
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x06007E45 RID: 32325 RVA: 0x00054E3C File Offset: 0x0005303C
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06007E46 RID: 32326 RVA: 0x00054E44 File Offset: 0x00053044
		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				int num;
				for (int i = 0; i < this.allGroupsInOrder.Count; i = num + 1)
				{
					List<IntVec3> cellsList = this.allGroupsInOrder[i].CellsList;
					int j = 0;
					while (j < this.allGroupsInOrder.Count)
					{
						yield return cellsList[j];
						num = i;
						i = num + 1;
					}
					cellsList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06007E47 RID: 32327 RVA: 0x0025987C File Offset: 0x00257A7C
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x06007E48 RID: 32328 RVA: 0x002598D8 File Offset: 0x00257AD8
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>(), false);
				return;
			}
			this.allHaulDestinationsInOrder.Add(haulDestination);
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					return;
				}
				this.allGroupsInOrder.Add(slotGroup);
				this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.SetCellFor(cellsList[i], slotGroup);
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x06007E49 RID: 32329 RVA: 0x002599C8 File Offset: 0x00257BC8
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>(), false);
				return;
			}
			this.allHaulDestinationsInOrder.Remove(haulDestination);
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					return;
				}
				this.allGroupsInOrder.Remove(slotGroup);
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					IntVec3 intVec = cellsList[i];
					this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x06007E4A RID: 32330 RVA: 0x00054E54 File Offset: 0x00053054
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
		}

		// Token: 0x06007E4B RID: 32331 RVA: 0x00259AA8 File Offset: 0x00257CA8
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x06007E4C RID: 32332 RVA: 0x00259AD4 File Offset: 0x00257CD4
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x06007E4D RID: 32333 RVA: 0x00054E84 File Offset: 0x00053084
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x06007E4E RID: 32334 RVA: 0x00259B00 File Offset: 0x00257D00
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		// Token: 0x06007E4F RID: 32335 RVA: 0x00259B20 File Offset: 0x00257D20
		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" overwriting slot group square ",
					c,
					" of ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x06007E50 RID: 32336 RVA: 0x00259B8C File Offset: 0x00257D8C
		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" clearing group grid square ",
					c,
					" containing ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x04005232 RID: 21042
		private Map map;

		// Token: 0x04005233 RID: 21043
		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x04005234 RID: 21044
		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x04005235 RID: 21045
		private SlotGroup[,,] groupGrid;
	}
}

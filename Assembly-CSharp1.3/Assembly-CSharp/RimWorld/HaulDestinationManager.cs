using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105A RID: 4186
	public sealed class HaulDestinationManager
	{
		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600633B RID: 25403 RVA: 0x002192DB File Offset: 0x002174DB
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x0600633C RID: 25404 RVA: 0x002192DB File Offset: 0x002174DB
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600633D RID: 25405 RVA: 0x002192DB File Offset: 0x002174DB
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x0600633E RID: 25406 RVA: 0x002192E3 File Offset: 0x002174E3
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x0600633F RID: 25407 RVA: 0x002192E3 File Offset: 0x002174E3
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06006340 RID: 25408 RVA: 0x002192E3 File Offset: 0x002174E3
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06006341 RID: 25409 RVA: 0x002192EB File Offset: 0x002174EB
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

		// Token: 0x06006342 RID: 25410 RVA: 0x002192FC File Offset: 0x002174FC
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x00219358 File Offset: 0x00217558
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>());
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
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
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

		// Token: 0x06006344 RID: 25412 RVA: 0x00219444 File Offset: 0x00217644
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>());
				return;
			}
			this.allHaulDestinationsInOrder.Remove(haulDestination);
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>());
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

		// Token: 0x06006345 RID: 25413 RVA: 0x00219520 File Offset: 0x00217720
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x00219550 File Offset: 0x00217750
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x0021957C File Offset: 0x0021777C
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x002195A7 File Offset: 0x002177A7
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x06006349 RID: 25417 RVA: 0x002195C8 File Offset: 0x002177C8
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x002195E8 File Offset: 0x002177E8
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
				}));
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x00219654 File Offset: 0x00217854
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
				}));
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x04003844 RID: 14404
		private Map map;

		// Token: 0x04003845 RID: 14405
		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x04003846 RID: 14406
		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x04003847 RID: 14407
		private SlotGroup[,,] groupGrid;
	}
}

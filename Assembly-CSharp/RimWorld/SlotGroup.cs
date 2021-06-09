using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001692 RID: 5778
	public class SlotGroup
	{
		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06007E66 RID: 32358 RVA: 0x00054EDA File Offset: 0x000530DA
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06007E67 RID: 32359 RVA: 0x00054EE7 File Offset: 0x000530E7
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06007E68 RID: 32360 RVA: 0x00054EF4 File Offset: 0x000530F4
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				List<IntVec3> cellsList = this.CellsList;
				Map map = this.Map;
				int num;
				for (int i = 0; i < cellsList.Count; i = num + 1)
				{
					List<Thing> thingList = map.thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						if (thingList[j].def.EverStorable(false))
						{
							yield return thingList[j];
						}
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06007E69 RID: 32361 RVA: 0x00054F04 File Offset: 0x00053104
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x06007E6A RID: 32362 RVA: 0x00054F11 File Offset: 0x00053111
		public IEnumerator<IntVec3> GetEnumerator()
		{
			List<IntVec3> cellsList = this.CellsList;
			int num;
			for (int i = 0; i < cellsList.Count; i = num + 1)
			{
				yield return cellsList[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06007E6B RID: 32363 RVA: 0x00054F20 File Offset: 0x00053120
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x06007E6C RID: 32364 RVA: 0x00054F2F File Offset: 0x0005312F
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06007E6D RID: 32365 RVA: 0x00054F65 File Offset: 0x00053165
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06007E6E RID: 32366 RVA: 0x00054F9B File Offset: 0x0005319B
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		// Token: 0x0400523D RID: 21053
		public ISlotGroupParent parent;
	}
}

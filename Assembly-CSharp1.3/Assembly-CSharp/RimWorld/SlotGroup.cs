using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105E RID: 4190
	public class SlotGroup
	{
		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x06006359 RID: 25433 RVA: 0x002196C0 File Offset: 0x002178C0
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x0600635A RID: 25434 RVA: 0x002196CD File Offset: 0x002178CD
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x0600635B RID: 25435 RVA: 0x002196DA File Offset: 0x002178DA
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

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x0600635C RID: 25436 RVA: 0x002196EA File Offset: 0x002178EA
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x0600635D RID: 25437 RVA: 0x002196F7 File Offset: 0x002178F7
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

		// Token: 0x0600635E RID: 25438 RVA: 0x00219706 File Offset: 0x00217906
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x0600635F RID: 25439 RVA: 0x00219715 File Offset: 0x00217915
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06006360 RID: 25440 RVA: 0x0021974B File Offset: 0x0021794B
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06006361 RID: 25441 RVA: 0x00219784 File Offset: 0x00217984
		public void RemoveHaulDesignationOnStoredThings()
		{
			if (this.parent.Map == null)
			{
				return;
			}
			foreach (Thing t in this.HeldThings)
			{
				if (this.Settings.AllowedToAccept(t))
				{
					Designation designation = this.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul);
					if (designation != null)
					{
						this.Map.designationManager.RemoveDesignation(designation);
					}
				}
			}
		}

		// Token: 0x06006362 RID: 25442 RVA: 0x00219814 File Offset: 0x00217A14
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		// Token: 0x04003848 RID: 14408
		public ISlotGroupParent parent;
	}
}

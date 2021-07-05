using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C81 RID: 3201
	public class ListerHaulables
	{
		// Token: 0x06004AA0 RID: 19104 RVA: 0x0018AB9C File Offset: 0x00188D9C
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0018ABCC File Offset: 0x00188DCC
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x0018ABD4 File Offset: 0x00188DD4
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0018ABDD File Offset: 0x00188DDD
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0018ABD4 File Offset: 0x00188DD4
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0018ABDD File Offset: 0x00188DDD
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0018ABD4 File Offset: 0x00188DD4
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x0018ABDD File Offset: 0x00188DDD
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x0018ABE8 File Offset: 0x00188DE8
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				sg.RemoveHaulDesignationOnStoredThings();
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0018AC24 File Offset: 0x00188E24
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			if (allGroupsListForReading.Count == 0)
			{
				return;
			}
			int num = ListerHaulables.groupCycleIndex % allGroupsListForReading.Count;
			SlotGroup slotGroup = allGroupsListForReading[ListerHaulables.groupCycleIndex % allGroupsListForReading.Count];
			if (slotGroup.CellsList.Count != 0)
			{
				while (this.cellCycleIndices.Count <= num)
				{
					this.cellCycleIndices.Add(0);
				}
				if (this.cellCycleIndices[num] >= 2147473647)
				{
					this.cellCycleIndices[num] = 0;
				}
				for (int i = 0; i < 4; i++)
				{
					List<int> list = this.cellCycleIndices;
					int index = num;
					int num2 = list[index];
					list[index] = num2 + 1;
					List<Thing> thingList = slotGroup.CellsList[this.cellCycleIndices[num] % slotGroup.CellsList.Count].GetThingList(this.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].def.EverHaulable)
						{
							this.Check(thingList[j]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x0018AD74 File Offset: 0x00188F74
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0018ADAC File Offset: 0x00188FAC
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0018ADF4 File Offset: 0x00188FF4
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				if (!this.haulables.Contains(t))
				{
					this.haulables.Add(t);
					return;
				}
			}
			else if (this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0018AE40 File Offset: 0x00189040
		private bool ShouldBeHaulable(Thing t)
		{
			if (t.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (!t.def.alwaysHaulable)
			{
				if (!t.def.EverHaulable)
				{
					return false;
				}
				if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
				{
					return false;
				}
			}
			return !t.IsInValidBestStorage();
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0018AEA5 File Offset: 0x001890A5
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0018AECA File Offset: 0x001890CA
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0018AEF8 File Offset: 0x001890F8
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}

		// Token: 0x04002D4D RID: 11597
		private Map map;

		// Token: 0x04002D4E RID: 11598
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x04002D4F RID: 11599
		private const int CellsPerTick = 4;

		// Token: 0x04002D50 RID: 11600
		private static int groupCycleIndex;

		// Token: 0x04002D51 RID: 11601
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x04002D52 RID: 11602
		private string debugOutput = "uninitialized";
	}
}

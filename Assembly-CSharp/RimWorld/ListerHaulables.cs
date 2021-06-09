using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126F RID: 4719
	public class ListerHaulables
	{
		// Token: 0x060066D7 RID: 26327 RVA: 0x00046459 File Offset: 0x00044659
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x00046489 File Offset: 0x00044689
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x00046491 File Offset: 0x00044691
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x0004649A File Offset: 0x0004469A
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x00046491 File Offset: 0x00044691
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x0004649A File Offset: 0x0004469A
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x00046491 File Offset: 0x00044691
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x0004649A File Offset: 0x0004469A
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x001FA530 File Offset: 0x001F8730
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x001FA568 File Offset: 0x001F8768
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

		// Token: 0x060066E1 RID: 26337 RVA: 0x001FA6B8 File Offset: 0x001F88B8
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x001FA6F0 File Offset: 0x001F88F0
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x001FA738 File Offset: 0x001F8938
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

		// Token: 0x060066E4 RID: 26340 RVA: 0x001FA784 File Offset: 0x001F8984
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

		// Token: 0x060066E5 RID: 26341 RVA: 0x000464A3 File Offset: 0x000446A3
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x000464C8 File Offset: 0x000446C8
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x001FA7EC File Offset: 0x001F89EC
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

		// Token: 0x0400446C RID: 17516
		private Map map;

		// Token: 0x0400446D RID: 17517
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x0400446E RID: 17518
		private const int CellsPerTick = 4;

		// Token: 0x0400446F RID: 17519
		private static int groupCycleIndex;

		// Token: 0x04004470 RID: 17520
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x04004471 RID: 17521
		private string debugOutput = "uninitialized";
	}
}

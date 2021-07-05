using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001270 RID: 4720
	public class ListerMergeables
	{
		// Token: 0x060066E9 RID: 26345 RVA: 0x000464F3 File Offset: 0x000446F3
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x00046518 File Offset: 0x00044718
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x00046520 File Offset: 0x00044720
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x00046529 File Offset: 0x00044729
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x00046520 File Offset: 0x00044720
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x060066EE RID: 26350 RVA: 0x00046529 File Offset: 0x00044729
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x060066EF RID: 26351 RVA: 0x001FA8A0 File Offset: 0x001F8AA0
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			if (sg.CellsList != null)
			{
				for (int i = 0; i < sg.CellsList.Count; i++)
				{
					this.RecalcAllInCell(sg.CellsList[i]);
				}
			}
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x00046532 File Offset: 0x00044732
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x001FA8E0 File Offset: 0x001F8AE0
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x0004653B File Offset: 0x0004473B
		private void Check(Thing t)
		{
			if (this.ShouldBeMergeable(t))
			{
				if (!this.mergeables.Contains(t))
				{
					this.mergeables.Add(t);
					return;
				}
			}
			else
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x0004656E File Offset: 0x0004476E
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x0004659F File Offset: 0x0004479F
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x060066F5 RID: 26357 RVA: 0x000465C4 File Offset: 0x000447C4
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x060066F6 RID: 26358 RVA: 0x001FA918 File Offset: 0x001F8B18
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All mergeables (Count " + this.mergeables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.mergeables)
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

		// Token: 0x04004472 RID: 17522
		private Map map;

		// Token: 0x04004473 RID: 17523
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x04004474 RID: 17524
		private string debugOutput = "uninitialized";
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C82 RID: 3202
	public class ListerMergeables
	{
		// Token: 0x06004AB2 RID: 19122 RVA: 0x0018AFAC File Offset: 0x001891AC
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0018AFD1 File Offset: 0x001891D1
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0018AFD9 File Offset: 0x001891D9
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0018AFE2 File Offset: 0x001891E2
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0018AFD9 File Offset: 0x001891D9
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0018AFE2 File Offset: 0x001891E2
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0018AFEC File Offset: 0x001891EC
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

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0018B029 File Offset: 0x00189229
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0018B034 File Offset: 0x00189234
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0018B06C File Offset: 0x0018926C
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

		// Token: 0x06004ABC RID: 19132 RVA: 0x0018B09F File Offset: 0x0018929F
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06004ABD RID: 19133 RVA: 0x0018B0D0 File Offset: 0x001892D0
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x0018B0F5 File Offset: 0x001892F5
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x0018B114 File Offset: 0x00189314
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

		// Token: 0x04002D53 RID: 11603
		private Map map;

		// Token: 0x04002D54 RID: 11604
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x04002D55 RID: 11605
		private string debugOutput = "uninitialized";
	}
}

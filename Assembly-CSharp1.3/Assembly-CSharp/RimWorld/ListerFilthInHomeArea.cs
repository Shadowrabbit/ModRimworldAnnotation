using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C80 RID: 3200
	public class ListerFilthInHomeArea
	{
		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004A99 RID: 19097 RVA: 0x0018A984 File Offset: 0x00188B84
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0018A98C File Offset: 0x00188B8C
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0018A9A8 File Offset: 0x00188BA8
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0018AA08 File Offset: 0x00188C08
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0018AA34 File Offset: 0x00188C34
		public void Notify_FilthDespawned(Filth f)
		{
			for (int i = 0; i < this.filthInHomeArea.Count; i++)
			{
				if (this.filthInHomeArea[i] == f)
				{
					this.filthInHomeArea.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x0018AA74 File Offset: 0x00188C74
		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (this.map.areaManager.Home[c])
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Filth filth = thingList[i] as Filth;
					if (filth != null)
					{
						this.filthInHomeArea.Add(filth);
					}
				}
				return;
			}
			for (int j = this.filthInHomeArea.Count - 1; j >= 0; j--)
			{
				if (this.filthInHomeArea[j].Position == c)
				{
					this.filthInHomeArea.RemoveAt(j);
				}
			}
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0018AB14 File Offset: 0x00188D14
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing thing in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(thing.ThingID + " " + thing.Position);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002D4B RID: 11595
		private Map map;

		// Token: 0x04002D4C RID: 11596
		private List<Thing> filthInHomeArea = new List<Thing>();
	}
}

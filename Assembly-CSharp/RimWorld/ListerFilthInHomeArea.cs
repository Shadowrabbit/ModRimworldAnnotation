using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200126E RID: 4718
	public class ListerFilthInHomeArea
	{
		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x060066D0 RID: 26320 RVA: 0x0004640C File Offset: 0x0004460C
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x00046414 File Offset: 0x00044614
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x001FA368 File Offset: 0x001F8568
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x0004642E File Offset: 0x0004462E
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x001FA3C8 File Offset: 0x001F85C8
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

		// Token: 0x060066D5 RID: 26325 RVA: 0x001FA408 File Offset: 0x001F8608
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

		// Token: 0x060066D6 RID: 26326 RVA: 0x001FA4A8 File Offset: 0x001F86A8
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

		// Token: 0x0400446A RID: 17514
		private Map map;

		// Token: 0x0400446B RID: 17515
		private List<Thing> filthInHomeArea = new List<Thing>();
	}
}

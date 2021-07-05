using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200013C RID: 316
	public class TickList
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00028A88 File Offset: 0x00026C88
		private int TickInterval
		{
			get
			{
				switch (this.tickType)
				{
				case TickerType.Normal:
					return 1;
				case TickerType.Rare:
					return 250;
				case TickerType.Long:
					return 2000;
				default:
					return -1;
				}
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00028AC4 File Offset: 0x00026CC4
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00028B20 File Offset: 0x00026D20
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00028B6C File Offset: 0x00026D6C
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00028BBC File Offset: 0x00026DBC
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00028BCA File Offset: 0x00026DCA
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00028BD8 File Offset: 0x00026DD8
		public void Tick()
		{
			for (int i = 0; i < this.thingsToRegister.Count; i++)
			{
				this.BucketOf(this.thingsToRegister[i]).Add(this.thingsToRegister[i]);
			}
			this.thingsToRegister.Clear();
			for (int j = 0; j < this.thingsToDeregister.Count; j++)
			{
				this.BucketOf(this.thingsToDeregister[j]).Remove(this.thingsToDeregister[j]);
			}
			this.thingsToDeregister.Clear();
			if (DebugSettings.fastEcology)
			{
				Find.World.tileTemperatures.ClearCaches();
				for (int k = 0; k < this.thingLists.Count; k++)
				{
					List<Thing> list = this.thingLists[k];
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].def.category == ThingCategory.Plant)
						{
							list[l].TickLong();
						}
					}
				}
			}
			List<Thing> list2 = this.thingLists[Find.TickManager.TicksGame % this.TickInterval];
			for (int m = 0; m < list2.Count; m++)
			{
				if (!list2[m].Destroyed)
				{
					try
					{
						switch (this.tickType)
						{
						case TickerType.Normal:
							list2[m].Tick();
							break;
						case TickerType.Rare:
							list2[m].TickRare();
							break;
						case TickerType.Long:
							list2[m].TickLong();
							break;
						}
					}
					catch (Exception ex)
					{
						string text = list2[m].Spawned ? (" (at " + list2[m].Position + ")") : "";
						if (Prefs.DevMode)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								": ",
								ex
							}));
						}
						else
						{
							Log.ErrorOnce(string.Concat(new object[]
							{
								"Exception ticking ",
								list2[m].ToStringSafe<Thing>(),
								text,
								". Suppressing further errors. Exception: ",
								ex
							}), list2[m].thingIDNumber ^ 576876901);
						}
					}
				}
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00028E5C File Offset: 0x0002705C
		private List<Thing> BucketOf(Thing t)
		{
			int num = t.GetHashCode();
			if (num < 0)
			{
				num *= -1;
			}
			int index = num % this.TickInterval;
			return this.thingLists[index];
		}

		// Token: 0x0400080B RID: 2059
		private TickerType tickType;

		// Token: 0x0400080C RID: 2060
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x0400080D RID: 2061
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x0400080E RID: 2062
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}

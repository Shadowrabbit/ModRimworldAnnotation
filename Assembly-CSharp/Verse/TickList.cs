using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001DF RID: 479
	public class TickList
	{
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x000A4984 File Offset: 0x000A2B84
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

		// Token: 0x06000C77 RID: 3191 RVA: 0x000A49C0 File Offset: 0x000A2BC0
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x000A4A1C File Offset: 0x000A2C1C
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x000A4A68 File Offset: 0x000A2C68
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0000FA48 File Offset: 0x0000DC48
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0000FA56 File Offset: 0x0000DC56
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x000A4AB8 File Offset: 0x000A2CB8
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
							}), false);
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
							}), list2[m].thingIDNumber ^ 576876901, false);
						}
					}
				}
			}
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x000A4D3C File Offset: 0x000A2F3C
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

		// Token: 0x04000ACE RID: 2766
		private TickerType tickType;

		// Token: 0x04000ACF RID: 2767
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04000AD0 RID: 2768
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04000AD1 RID: 2769
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}

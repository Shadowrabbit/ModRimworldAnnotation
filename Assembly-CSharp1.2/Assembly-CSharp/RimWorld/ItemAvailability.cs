using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D51 RID: 3409
	public class ItemAvailability
	{
		// Token: 0x06004DEC RID: 19948 RVA: 0x00037100 File Offset: 0x00035300
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DED RID: 19949 RVA: 0x0003711A File Offset: 0x0003531A
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x06004DEE RID: 19950 RVA: 0x001B0090 File Offset: 0x001AE290
		public bool ThingsAvailableAnywhere(ThingDefCountClass need, Pawn pawn)
		{
			int key = Gen.HashCombine<Faction>(need.GetHashCode(), pawn.Faction);
			bool flag;
			if (!this.cachedResults.TryGetValue(key, out flag))
			{
				List<Thing> list = this.map.listerThings.ThingsOfDef(need.thingDef);
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (!list[i].IsForbidden(pawn))
					{
						num += list[i].stackCount;
						if (num >= need.count)
						{
							break;
						}
					}
				}
				flag = (num >= need.count);
				this.cachedResults.Add(key, flag);
			}
			return flag;
		}

		// Token: 0x04003307 RID: 13063
		private Map map;

		// Token: 0x04003308 RID: 13064
		private Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();
	}
}

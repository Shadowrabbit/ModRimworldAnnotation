using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000816 RID: 2070
	public class ItemAvailability
	{
		// Token: 0x06003722 RID: 14114 RVA: 0x0013807E File Offset: 0x0013627E
		public ItemAvailability(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x00138098 File Offset: 0x00136298
		public void Tick()
		{
			this.cachedResults.Clear();
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x001380A8 File Offset: 0x001362A8
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

		// Token: 0x04001F0C RID: 7948
		private Map map;

		// Token: 0x04001F0D RID: 7949
		private Dictionary<int, bool> cachedResults = new Dictionary<int, bool>();
	}
}

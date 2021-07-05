using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001243 RID: 4675
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06007039 RID: 28729 RVA: 0x002560FD File Offset: 0x002542FD
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x0600703A RID: 28730 RVA: 0x00256130 File Offset: 0x00254330
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (maps[i].areaManager.Home[thing.Position] && !thing.Position.Fogged(thing.Map))
						{
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x002561C2 File Offset: 0x002543C2
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}

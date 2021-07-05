using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193B RID: 6459
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06008F2F RID: 36655 RVA: 0x0005FD70 File Offset: 0x0005DF70
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x06008F30 RID: 36656 RVA: 0x00293B70 File Offset: 0x00291D70
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

		// Token: 0x06008F31 RID: 36657 RVA: 0x0005FDA2 File Offset: 0x0005DFA2
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF5 RID: 3829
	public class PreceptWorker_Relic : PreceptWorker
	{
		// Token: 0x06005B26 RID: 23334 RVA: 0x001F8220 File Offset: 0x001F6420
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			return -thingChance.chance;
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06005B27 RID: 23335 RVA: 0x001F8229 File Offset: 0x001F6429
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.relicChance != 0f)
					{
						yield return new PreceptThingChance
						{
							chance = thingDef.relicChance,
							def = thingDef
						};
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}
		}
	}
}

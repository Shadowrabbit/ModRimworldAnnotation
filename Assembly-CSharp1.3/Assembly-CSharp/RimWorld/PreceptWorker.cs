using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAA RID: 2730
	public class PreceptWorker
	{
		// Token: 0x060040D9 RID: 16601 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float GetThingOrder(PreceptThingChance thingChance)
		{
			return 0f;
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x060040DB RID: 16603 RVA: 0x0015E307 File Offset: 0x0015C507
		public virtual IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				return from dc in this.def.buildingDefChances
				select dc;
			}
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x0015E338 File Offset: 0x0015C538
		public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			return true;
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x0015E340 File Offset: 0x0015C540
		public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo)
		{
			foreach (PreceptThingChance preceptThingChance in this.ThingDefs)
			{
				if (this.CanUse(preceptThingChance.def, ideo))
				{
					yield return preceptThingChance;
				}
			}
			IEnumerator<PreceptThingChance> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04002619 RID: 9753
		public PreceptDef def;
	}
}

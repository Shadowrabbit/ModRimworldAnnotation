using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF2 RID: 3826
	public class PreceptWorker_Animal : PreceptWorker
	{
		// Token: 0x06005B09 RID: 23305 RVA: 0x001F7B50 File Offset: 0x001F5D50
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Animal precept_Animal;
					if ((precept_Animal = (enumerator.Current as Precept_Animal)) != null && precept_Animal.ThingDef == def)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06005B0A RID: 23306 RVA: 0x001F7BC0 File Offset: 0x001F5DC0
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef def in from x in DefDatabase<ThingDef>.AllDefs
				where x.thingCategories != null && x.thingCategories.Contains(ThingCategoryDefOf.Animals) && !x.race.Insect && !x.race.Dryad
				select x)
				{
					yield return new PreceptThingChance
					{
						def = def,
						chance = 1f
					};
				}
				IEnumerator<ThingDef> enumerator = null;
				yield break;
				yield break;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EF4 RID: 3828
	public class PreceptWorker_Apparel : PreceptWorker
	{
		// Token: 0x06005B1F RID: 23327 RVA: 0x001F80C4 File Offset: 0x001F62C4
		public override bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			using (List<Precept>.Enumerator enumerator = ideo.PreceptsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Precept_Apparel precept_Apparel;
					if ((precept_Apparel = (enumerator.Current as Precept_Apparel)) != null && precept_Apparel.apparelDef == thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005B20 RID: 23328 RVA: 0x001F8128 File Offset: 0x001F6328
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			if (thingChance.def.IsApparel)
			{
				return (float)((thingChance.def.apparel.LastLayer == ApparelLayerDefOf.Overhead) ? 1 : 2);
			}
			return 0f;
		}

		// Token: 0x06005B21 RID: 23329 RVA: 0x001F815C File Offset: 0x001F635C
		private bool IsValidApparel(ThingDef td)
		{
			if (!td.IsApparel || !td.MadeFromStuff)
			{
				return false;
			}
			if (!td.apparel.canBeDesiredForIdeo)
			{
				return false;
			}
			if (td.thingCategories != null && (td.thingCategories.Contains(ThingCategoryDefOf.ArmorHeadgear) || td.thingCategories.Contains(ThingCategoryDefOf.ApparelArmor)))
			{
				return false;
			}
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel;
				if ((preceptComp_Apparel = (this.def.comps[i] as PreceptComp_Apparel)) != null && !preceptComp_Apparel.CanApplyToApparel(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005B22 RID: 23330 RVA: 0x001F81F9 File Offset: 0x001F63F9
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			return this.IsValidApparel(def);
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06005B23 RID: 23331 RVA: 0x001F8207 File Offset: 0x001F6407
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef def in from x in DefDatabase<ThingDef>.AllDefs
				where this.IsValidApparel(x)
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

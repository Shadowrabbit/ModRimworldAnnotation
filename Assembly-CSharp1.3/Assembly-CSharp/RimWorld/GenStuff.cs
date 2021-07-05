using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001487 RID: 5255
	public static class GenStuff
	{
		// Token: 0x06007DAD RID: 32173 RVA: 0x002C7854 File Offset: 0x002C5A54
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			if (!bd.MadeFromStuff)
			{
				return null;
			}
			ThingDef thingDef = bd as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.defaultStuff != null)
				{
					return thingDef.defaultStuff;
				}
				if (thingDef.IsMeleeWeapon)
				{
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
					if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Plasteel;
					}
				}
				if (thingDef.IsApparel)
				{
					if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Cloth;
					}
					if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Leather_Plain;
					}
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
				}
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
			{
				return ThingDefOf.WoodLog;
			}
			if (ThingDefOf.Steel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Steel;
			}
			if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Plasteel;
			}
			if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
			{
				return ThingDefOf.BlocksGranite;
			}
			if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Cloth;
			}
			if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Leather_Plain;
			}
			return GenStuff.AllowedStuffsFor(bd, TechLevel.Undefined).First<ThingDef>();
		}

		// Token: 0x06007DAE RID: 32174 RVA: 0x002C79AB File Offset: 0x002C5BAB
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined).RandomElement<ThingDef>();
		}

		// Token: 0x06007DAF RID: 32175 RVA: 0x002C79C4 File Offset: 0x002C5BC4
		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			ThingDef result;
			if (!GenStuff.TryRandomStuffByCommonalityFor(td, out result, maxTechLevel))
			{
				result = GenStuff.DefaultStuffFor(td);
			}
			return result;
		}

		// Token: 0x06007DB0 RID: 32176 RVA: 0x002C79EE File Offset: 0x002C5BEE
		public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				yield break;
			}
			List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allDefs.Count; i = num + 1)
			{
				ThingDef thingDef = allDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel) && thingDef.stuffProps.CanMake(td))
				{
					yield return thingDef;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007DB1 RID: 32177 RVA: 0x002C7A05 File Offset: 0x002C5C05
		public static IEnumerable<ThingDef> AllowedStuffs(List<StuffCategoryDef> categories, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			List<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allDefs.Count; i = num + 1)
			{
				ThingDef thingDef = allDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel))
				{
					bool flag = false;
					for (int j = 0; j < thingDef.stuffProps.categories.Count; j++)
					{
						for (int k = 0; k < categories.Count; k++)
						{
							if (thingDef.stuffProps.categories[j] == categories[k])
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						yield return thingDef;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007DB2 RID: 32178 RVA: 0x002C7A1C File Offset: 0x002C5C1C
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		// Token: 0x06007DB3 RID: 32179 RVA: 0x002C7A58 File Offset: 0x002C5C58
		public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			if (validator != null)
			{
				GenStuff.allowedStuffTmp.Clear();
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(td, maxTechLevel).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef = enumerator.Current;
						if (validator(thingDef))
						{
							GenStuff.allowedStuffTmp.Add(thingDef);
						}
					}
					goto IL_73;
				}
			}
			GenStuff.allowedStuffTmp.Clear();
			GenStuff.allowedStuffTmp.AddRange(GenStuff.AllowedStuffsFor(td, maxTechLevel));
			IL_73:
			return GenStuff.allowedStuffTmp.TryRandomElement(out stuff);
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x002C7AF4 File Offset: 0x002C5CF4
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x002C7B10 File Offset: 0x002C5D10
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!thingDef.MadeFromStuff)
			{
				return null;
			}
			IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(thingDef, maxTechLevel);
			if (validator != null)
			{
				enumerable = from x in enumerable
				where validator(x)
				select x;
			}
			float cheapestPrice = -1f;
			foreach (ThingDef thingDef2 in enumerable)
			{
				float num = thingDef2.BaseMarketValue / thingDef2.VolumePerUnit;
				if (cheapestPrice == -1f || num < cheapestPrice)
				{
					cheapestPrice = num;
				}
			}
			enumerable = from x in enumerable
			where x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f
			select x;
			ThingDef result;
			if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04004E61 RID: 20065
		private static List<ThingDef> allowedStuffTmp = new List<ThingDef>();
	}
}

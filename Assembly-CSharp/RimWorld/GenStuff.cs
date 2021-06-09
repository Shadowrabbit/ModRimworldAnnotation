using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBC RID: 7356
	public static class GenStuff
	{
		// Token: 0x0600A002 RID: 40962 RVA: 0x002ECA90 File Offset: 0x002EAC90
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			if (!bd.MadeFromStuff)
			{
				return null;
			}
			ThingDef thingDef = bd as ThingDef;
			if (thingDef != null)
			{
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

		// Token: 0x0600A003 RID: 40963 RVA: 0x0006AB07 File Offset: 0x00068D07
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined).RandomElement<ThingDef>();
		}

		// Token: 0x0600A004 RID: 40964 RVA: 0x002ECBD8 File Offset: 0x002EADD8
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

		// Token: 0x0600A005 RID: 40965 RVA: 0x0006AB1F File Offset: 0x00068D1F
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

		// Token: 0x0600A006 RID: 40966 RVA: 0x0006AB36 File Offset: 0x00068D36
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

		// Token: 0x0600A007 RID: 40967 RVA: 0x0006AB4D File Offset: 0x00068D4D
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		// Token: 0x0600A008 RID: 40968 RVA: 0x002ECC04 File Offset: 0x002EAE04
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

		// Token: 0x0600A009 RID: 40969 RVA: 0x0006AB88 File Offset: 0x00068D88
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		// Token: 0x0600A00A RID: 40970 RVA: 0x002ECCA0 File Offset: 0x002EAEA0
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

		// Token: 0x04006CB5 RID: 27829
		private static List<ThingDef> allowedStuffTmp = new List<ThingDef>();
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2E RID: 7214
	public static class CostListCalculator
	{
		// Token: 0x06009EA4 RID: 40612 RVA: 0x00069926 File Offset: 0x00067B26
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x06009EA5 RID: 40613 RVA: 0x00069932 File Offset: 0x00067B32
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x06009EA6 RID: 40614 RVA: 0x002E8964 File Offset: 0x002E6B64
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			CostListCalculator.CostListPair key = new CostListCalculator.CostListPair(entDef, stuff);
			List<ThingDefCountClass> list;
			if (!CostListCalculator.cachedCosts.TryGetValue(key, out list))
			{
				list = new List<ThingDefCountClass>();
				int num = 0;
				if (entDef.MadeFromStuff)
				{
					if (errorOnNullStuff && stuff == null)
					{
						Log.Error("Cannot get AdjustedCostList for " + entDef + " with null Stuff.", false);
						if (GenStuff.DefaultStuffFor(entDef) == null)
						{
							return null;
						}
						return entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					else if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.costStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.costStuffCount;
					}
				}
				else if (stuff != null)
				{
					Log.Error(string.Concat(new object[]
					{
						"Got AdjustedCostList for ",
						entDef,
						" with stuff ",
						stuff,
						" but is not MadeFromStuff."
					}), false);
				}
				bool flag = false;
				if (entDef.costList != null)
				{
					for (int i = 0; i < entDef.costList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.costList[i];
						if (thingDefCountClass.thingDef == stuff)
						{
							list.Add(new ThingDefCountClass(thingDefCountClass.thingDef, thingDefCountClass.count + num));
							flag = true;
						}
						else
						{
							list.Add(thingDefCountClass);
						}
					}
				}
				if (!flag && num > 0)
				{
					list.Add(new ThingDefCountClass(stuff, num));
				}
				CostListCalculator.cachedCosts.Add(key, list);
			}
			return list;
		}

		// Token: 0x0400652E RID: 25902
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x02001C2F RID: 7215
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x06009EA8 RID: 40616 RVA: 0x00069957 File Offset: 0x00067B57
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x06009EA9 RID: 40617 RVA: 0x00069967 File Offset: 0x00067B67
			public override int GetHashCode()
			{
				return Gen.HashCombine<ThingDef>(Gen.HashCombine<BuildableDef>(0, this.buildable), this.stuff);
			}

			// Token: 0x06009EAA RID: 40618 RVA: 0x00069980 File Offset: 0x00067B80
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x06009EAB RID: 40619 RVA: 0x00069998 File Offset: 0x00067B98
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x06009EAC RID: 40620 RVA: 0x000699A6 File Offset: 0x00067BA6
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x06009EAD RID: 40621 RVA: 0x000699C6 File Offset: 0x00067BC6
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400652F RID: 25903
			public BuildableDef buildable;

			// Token: 0x04006530 RID: 25904
			public ThingDef stuff;
		}

		// Token: 0x02001C30 RID: 7216
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x06009EAE RID: 40622 RVA: 0x000699D2 File Offset: 0x00067BD2
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x06009EAF RID: 40623 RVA: 0x000699DB File Offset: 0x00067BDB
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04006531 RID: 25905
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F1 RID: 5105
	public static class CostListCalculator
	{
		// Token: 0x06007C58 RID: 31832 RVA: 0x002C1E2D File Offset: 0x002C002D
		public static void Reset()
		{
			CostListCalculator.cachedCosts.Clear();
		}

		// Token: 0x06007C59 RID: 31833 RVA: 0x002C1E39 File Offset: 0x002C0039
		public static List<ThingDefCountClass> CostListAdjusted(this Thing thing)
		{
			return thing.def.CostListAdjusted(thing.Stuff, true);
		}

		// Token: 0x06007C5A RID: 31834 RVA: 0x002C1E50 File Offset: 0x002C0050
		public static List<ThingDefCountClass> CostListAdjusted(this BuildableDef entDef, ThingDef stuff, bool errorOnNullStuff = true)
		{
			if (CostListCalculator.cachedDifficulty != Find.Storyteller.difficulty)
			{
				CostListCalculator.Reset();
				CostListCalculator.cachedDifficulty = Find.Storyteller.difficulty;
			}
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
						Log.Error("Cannot get AdjustedCostList for " + entDef + " with null Stuff.");
						if (GenStuff.DefaultStuffFor(entDef) == null)
						{
							return null;
						}
						return entDef.CostListAdjusted(GenStuff.DefaultStuffFor(entDef), true);
					}
					else if (stuff != null)
					{
						num = Mathf.RoundToInt((float)entDef.CostStuffCount / stuff.VolumePerUnit);
						if (num < 1)
						{
							num = 1;
						}
					}
					else
					{
						num = entDef.CostStuffCount;
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
					}));
				}
				bool flag = false;
				if (entDef.CostList != null)
				{
					for (int i = 0; i < entDef.CostList.Count; i++)
					{
						ThingDefCountClass thingDefCountClass = entDef.CostList[i];
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

		// Token: 0x040044D6 RID: 17622
		private static Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>> cachedCosts = new Dictionary<CostListCalculator.CostListPair, List<ThingDefCountClass>>(CostListCalculator.FastCostListPairComparer.Instance);

		// Token: 0x040044D7 RID: 17623
		private static Difficulty cachedDifficulty = null;

		// Token: 0x020027F9 RID: 10233
		private struct CostListPair : IEquatable<CostListCalculator.CostListPair>
		{
			// Token: 0x0600DB92 RID: 56210 RVA: 0x00417737 File Offset: 0x00415937
			public CostListPair(BuildableDef buildable, ThingDef stuff)
			{
				this.buildable = buildable;
				this.stuff = stuff;
			}

			// Token: 0x0600DB93 RID: 56211 RVA: 0x00417747 File Offset: 0x00415947
			public override int GetHashCode()
			{
				return Gen.HashCombine<ThingDef>(Gen.HashCombine<BuildableDef>(0, this.buildable), this.stuff);
			}

			// Token: 0x0600DB94 RID: 56212 RVA: 0x00417760 File Offset: 0x00415960
			public override bool Equals(object obj)
			{
				return obj is CostListCalculator.CostListPair && this.Equals((CostListCalculator.CostListPair)obj);
			}

			// Token: 0x0600DB95 RID: 56213 RVA: 0x00417778 File Offset: 0x00415978
			public bool Equals(CostListCalculator.CostListPair other)
			{
				return this == other;
			}

			// Token: 0x0600DB96 RID: 56214 RVA: 0x00417786 File Offset: 0x00415986
			public static bool operator ==(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return lhs.buildable == rhs.buildable && lhs.stuff == rhs.stuff;
			}

			// Token: 0x0600DB97 RID: 56215 RVA: 0x004177A6 File Offset: 0x004159A6
			public static bool operator !=(CostListCalculator.CostListPair lhs, CostListCalculator.CostListPair rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0400971E RID: 38686
			public BuildableDef buildable;

			// Token: 0x0400971F RID: 38687
			public ThingDef stuff;
		}

		// Token: 0x020027FA RID: 10234
		private class FastCostListPairComparer : IEqualityComparer<CostListCalculator.CostListPair>
		{
			// Token: 0x0600DB98 RID: 56216 RVA: 0x004177B2 File Offset: 0x004159B2
			public bool Equals(CostListCalculator.CostListPair x, CostListCalculator.CostListPair y)
			{
				return x == y;
			}

			// Token: 0x0600DB99 RID: 56217 RVA: 0x004177BB File Offset: 0x004159BB
			public int GetHashCode(CostListCalculator.CostListPair obj)
			{
				return obj.GetHashCode();
			}

			// Token: 0x04009720 RID: 38688
			public static readonly CostListCalculator.FastCostListPairComparer Instance = new CostListCalculator.FastCostListPairComparer();
		}
	}
}

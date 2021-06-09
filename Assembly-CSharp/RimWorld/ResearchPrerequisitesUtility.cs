using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B58 RID: 7000
	public static class ResearchPrerequisitesUtility
	{
		// Token: 0x06009A4F RID: 39503 RVA: 0x002D714C File Offset: 0x002D534C
		private static Dictionary<Def, List<ResearchProjectDef>> ComputeResearchPrerequisites()
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = new Dictionary<Def, List<ResearchProjectDef>>();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				foreach (Def key in researchProjectDef.UnlockedDefs)
				{
					List<ResearchProjectDef> list;
					if (!dictionary.TryGetValue(key, out list))
					{
						list = new List<ResearchProjectDef>();
						dictionary.Add(key, list);
					}
					list.Add(researchProjectDef);
				}
			}
			return dictionary;
		}

		// Token: 0x06009A50 RID: 39504 RVA: 0x002D71FC File Offset: 0x002D53FC
		public static List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef rd)
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = ResearchPrerequisitesUtility.ComputeResearchPrerequisites();
			List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>> list = new List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>>();
			foreach (Def def in rd.UnlockedDefs)
			{
				list.Add(new Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>(def, new ResearchPrerequisitesUtility.UnlockedHeader(dictionary[def].Except(rd).ToList<ResearchProjectDef>())));
			}
			return (from pair in list
			orderby pair.Second.unlockedBy.Count
			group pair.First by pair.Second).Select(new Func<IGrouping<ResearchPrerequisitesUtility.UnlockedHeader, Def>, Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>(GenCollection.ConvertIGroupingToPair<ResearchPrerequisitesUtility.UnlockedHeader, Def>)).ToList<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>();
		}

		// Token: 0x02001B59 RID: 7001
		public class UnlockedHeader : IEquatable<ResearchPrerequisitesUtility.UnlockedHeader>
		{
			// Token: 0x06009A51 RID: 39505 RVA: 0x00066BC1 File Offset: 0x00064DC1
			public UnlockedHeader(List<ResearchProjectDef> unlockedBy)
			{
				this.unlockedBy = unlockedBy;
			}

			// Token: 0x06009A52 RID: 39506 RVA: 0x00066BD0 File Offset: 0x00064DD0
			public bool Equals(ResearchPrerequisitesUtility.UnlockedHeader other)
			{
				return other != null && (this == other || this.unlockedBy.SequenceEqual(other.unlockedBy));
			}

			// Token: 0x06009A53 RID: 39507 RVA: 0x00066BEE File Offset: 0x00064DEE
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ResearchPrerequisitesUtility.UnlockedHeader)obj)));
			}

			// Token: 0x06009A54 RID: 39508 RVA: 0x00066C1C File Offset: 0x00064E1C
			public override int GetHashCode()
			{
				if (!this.unlockedBy.Any<ResearchProjectDef>())
				{
					return 23;
				}
				return this.unlockedBy.First<ResearchProjectDef>().GetHashCode();
			}

			// Token: 0x040062B1 RID: 25265
			public List<ResearchProjectDef> unlockedBy;
		}
	}
}

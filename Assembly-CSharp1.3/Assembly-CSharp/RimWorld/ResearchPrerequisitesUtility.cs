using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001370 RID: 4976
	public static class ResearchPrerequisitesUtility
	{
		// Token: 0x06007919 RID: 31001 RVA: 0x002AE9B4 File Offset: 0x002ACBB4
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

		// Token: 0x0600791A RID: 31002 RVA: 0x002AEA64 File Offset: 0x002ACC64
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

		// Token: 0x0200278F RID: 10127
		public class UnlockedHeader : IEquatable<ResearchPrerequisitesUtility.UnlockedHeader>
		{
			// Token: 0x0600DA0D RID: 55821 RVA: 0x00414532 File Offset: 0x00412732
			public UnlockedHeader(List<ResearchProjectDef> unlockedBy)
			{
				this.unlockedBy = unlockedBy;
			}

			// Token: 0x0600DA0E RID: 55822 RVA: 0x00414541 File Offset: 0x00412741
			public bool Equals(ResearchPrerequisitesUtility.UnlockedHeader other)
			{
				return other != null && (this == other || this.unlockedBy.SequenceEqual(other.unlockedBy));
			}

			// Token: 0x0600DA0F RID: 55823 RVA: 0x0041455F File Offset: 0x0041275F
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ResearchPrerequisitesUtility.UnlockedHeader)obj)));
			}

			// Token: 0x0600DA10 RID: 55824 RVA: 0x0041458D File Offset: 0x0041278D
			public override int GetHashCode()
			{
				if (!this.unlockedBy.Any<ResearchProjectDef>())
				{
					return 23;
				}
				return this.unlockedBy.First<ResearchProjectDef>().GetHashCode();
			}

			// Token: 0x040095B9 RID: 38329
			public List<ResearchProjectDef> unlockedBy;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B1 RID: 5297
	public class BackstoryCategoryFilter
	{
		// Token: 0x060071FC RID: 29180 RVA: 0x0022E2AC File Offset: 0x0022C4AC
		public bool Matches(PawnBio bio)
		{
			return (this.exclude == null || !(from e in this.exclude
			where bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)
			select e).Any<string>()) && (this.categories == null || (from c in this.categories
			where bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c)
			select c).Any<string>());
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x0022E314 File Offset: 0x0022C514
		public bool Matches(Backstory backstory)
		{
			return (this.exclude == null || !backstory.spawnCategories.Any((string e) => this.exclude.Contains(e))) && (this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c)));
		}

		// Token: 0x04004AFB RID: 19195
		public List<string> categories;

		// Token: 0x04004AFC RID: 19196
		public List<string> exclude;

		// Token: 0x04004AFD RID: 19197
		public float commonality = 1f;
	}
}

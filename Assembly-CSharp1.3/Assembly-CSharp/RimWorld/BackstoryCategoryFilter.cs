using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E20 RID: 3616
	public class BackstoryCategoryFilter
	{
		// Token: 0x06005382 RID: 21378 RVA: 0x001C47B4 File Offset: 0x001C29B4
		public bool Matches(PawnBio bio)
		{
			if (this.exclude != null && (from e in this.exclude
			where bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)
			select e).Any<string>())
			{
				return false;
			}
			if (this.excludeChildhood != null && bio.childhood.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (this.excludeAdulthood != null && bio.adulthood.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			bool flag = true;
			if (this.categoriesChildhood != null)
			{
				flag &= bio.childhood.spawnCategories.Any((string e) => this.categoriesChildhood.Contains(e));
			}
			if (this.categoriesAdulthood != null)
			{
				flag &= bio.adulthood.spawnCategories.Any((string e) => this.categoriesAdulthood.Contains(e));
			}
			if (this.categoriesChildhood != null || this.categoriesAdulthood != null)
			{
				return flag;
			}
			return this.categories == null || (from c in this.categories
			where bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c)
			select c).Any<string>();
		}

		// Token: 0x06005383 RID: 21379 RVA: 0x001C48EC File Offset: 0x001C2AEC
		public bool Matches(Backstory backstory)
		{
			if (this.exclude != null && backstory.spawnCategories.Any((string e) => this.exclude.Contains(e)))
			{
				return false;
			}
			if (this.excludeChildhood != null && backstory.slot == BackstorySlot.Childhood && backstory.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (this.excludeAdulthood != null && backstory.slot == BackstorySlot.Adulthood && backstory.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			if (this.categoriesChildhood != null && backstory.slot == BackstorySlot.Childhood)
			{
				return backstory.spawnCategories.Any((string c) => this.categoriesChildhood.Contains(c));
			}
			if (this.categoriesAdulthood != null && backstory.slot == BackstorySlot.Adulthood)
			{
				return backstory.spawnCategories.Any((string c) => this.categoriesAdulthood.Contains(c));
			}
			return this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c));
		}

		// Token: 0x04003105 RID: 12549
		public List<string> categories;

		// Token: 0x04003106 RID: 12550
		public List<string> exclude;

		// Token: 0x04003107 RID: 12551
		public List<string> categoriesChildhood;

		// Token: 0x04003108 RID: 12552
		public List<string> excludeChildhood;

		// Token: 0x04003109 RID: 12553
		public List<string> categoriesAdulthood;

		// Token: 0x0400310A RID: 12554
		public List<string> excludeAdulthood;

		// Token: 0x0400310B RID: 12555
		public float commonality = 1f;
	}
}

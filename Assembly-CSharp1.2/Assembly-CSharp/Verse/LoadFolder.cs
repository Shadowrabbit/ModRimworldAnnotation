using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060015BE RID: 5566 RVA: 0x000157A8 File Offset: 0x000139A8
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredPackageIds.NullOrEmpty<string>() || ModLister.AnyFromListActive(this.requiredPackageIds)) && (this.disallowedPackageIds.NullOrEmpty<string>() || !ModLister.AnyFromListActive(this.disallowedPackageIds));
			}
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x000D3AF4 File Offset: 0x000D1CF4
		public LoadFolder(string folderName, List<string> requiredPackageIds, List<string> disallowedPackageIds)
		{
			this.folderName = folderName;
			this.requiredPackageIds = requiredPackageIds;
			this.disallowedPackageIds = disallowedPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredPackageIds != null) ? requiredPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedPackageIds != null) ? disallowedPackageIds.GetHashCode() : 0);
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000157E3 File Offset: 0x000139E3
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000D3B68 File Offset: 0x000D1D68
		public override bool Equals(object obj)
		{
			LoadFolder other;
			return (other = (obj as LoadFolder)) != null && this.Equals(other);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x000157F8 File Offset: 0x000139F8
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x040010C1 RID: 4289
		public string folderName;

		// Token: 0x040010C2 RID: 4290
		public List<string> requiredPackageIds;

		// Token: 0x040010C3 RID: 4291
		public List<string> disallowedPackageIds;

		// Token: 0x040010C4 RID: 4292
		private readonly int hashCodeCached;
	}
}

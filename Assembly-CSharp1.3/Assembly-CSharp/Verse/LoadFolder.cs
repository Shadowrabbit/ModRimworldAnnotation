using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200023E RID: 574
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0005DF8D File Offset: 0x0005C18D
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredPackageIds.NullOrEmpty<string>() || ModLister.AnyFromListActive(this.requiredPackageIds)) && (this.disallowedPackageIds.NullOrEmpty<string>() || !ModLister.AnyFromListActive(this.disallowedPackageIds));
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x0005DFC8 File Offset: 0x0005C1C8
		public LoadFolder(string folderName, List<string> requiredPackageIds, List<string> disallowedPackageIds)
		{
			this.folderName = folderName;
			this.requiredPackageIds = requiredPackageIds;
			this.disallowedPackageIds = disallowedPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredPackageIds != null) ? requiredPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedPackageIds != null) ? disallowedPackageIds.GetHashCode() : 0);
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x0005E03C File Offset: 0x0005C23C
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x0005E054 File Offset: 0x0005C254
		public override bool Equals(object obj)
		{
			LoadFolder other;
			return (other = (obj as LoadFolder)) != null && this.Equals(other);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x0005E074 File Offset: 0x0005C274
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x04000CC0 RID: 3264
		public string folderName;

		// Token: 0x04000CC1 RID: 3265
		public List<string> requiredPackageIds;

		// Token: 0x04000CC2 RID: 3266
		public List<string> disallowedPackageIds;

		// Token: 0x04000CC3 RID: 3267
		private readonly int hashCodeCached;
	}
}

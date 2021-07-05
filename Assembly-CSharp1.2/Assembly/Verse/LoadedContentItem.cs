using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200033E RID: 830
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x000150C4 File Offset: 0x000132C4
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		// Token: 0x0400105B RID: 4187
		public VirtualFile internalFile;

		// Token: 0x0400105C RID: 4188
		public T contentItem;

		// Token: 0x0400105D RID: 4189
		public IDisposable extraDisposable;
	}
}

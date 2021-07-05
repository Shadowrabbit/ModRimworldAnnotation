using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000239 RID: 569
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06001026 RID: 4134 RVA: 0x0005C10B File Offset: 0x0005A30B
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		// Token: 0x04000C8B RID: 3211
		public VirtualFile internalFile;

		// Token: 0x04000C8C RID: 3212
		public T contentItem;

		// Token: 0x04000C8D RID: 3213
		public IDisposable extraDisposable;
	}
}

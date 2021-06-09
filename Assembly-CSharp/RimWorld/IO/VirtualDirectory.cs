using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002202 RID: 8706
	public abstract class VirtualDirectory
	{
		// Token: 0x17001BD6 RID: 7126
		// (get) Token: 0x0600BAC0 RID: 47808
		public abstract string Name { get; }

		// Token: 0x17001BD7 RID: 7127
		// (get) Token: 0x0600BAC1 RID: 47809
		public abstract string FullPath { get; }

		// Token: 0x17001BD8 RID: 7128
		// (get) Token: 0x0600BAC2 RID: 47810
		public abstract bool Exists { get; }

		// Token: 0x0600BAC3 RID: 47811
		public abstract VirtualDirectory GetDirectory(string directoryName);

		// Token: 0x0600BAC4 RID: 47812
		public abstract VirtualFile GetFile(string filename);

		// Token: 0x0600BAC5 RID: 47813
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		// Token: 0x0600BAC6 RID: 47814
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		// Token: 0x0600BAC7 RID: 47815 RVA: 0x00078FBA File Offset: 0x000771BA
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		// Token: 0x0600BAC8 RID: 47816 RVA: 0x00078FC8 File Offset: 0x000771C8
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		// Token: 0x0600BAC9 RID: 47817 RVA: 0x00078FD6 File Offset: 0x000771D6
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}

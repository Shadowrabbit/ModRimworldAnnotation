using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02001823 RID: 6179
	public abstract class VirtualDirectory
	{
		// Token: 0x170017E6 RID: 6118
		// (get) Token: 0x0600910F RID: 37135
		public abstract string Name { get; }

		// Token: 0x170017E7 RID: 6119
		// (get) Token: 0x06009110 RID: 37136
		public abstract string FullPath { get; }

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06009111 RID: 37137
		public abstract bool Exists { get; }

		// Token: 0x06009112 RID: 37138
		public abstract VirtualDirectory GetDirectory(string directoryName);

		// Token: 0x06009113 RID: 37139
		public abstract VirtualFile GetFile(string filename);

		// Token: 0x06009114 RID: 37140
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		// Token: 0x06009115 RID: 37141
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		// Token: 0x06009116 RID: 37142 RVA: 0x00340299 File Offset: 0x0033E499
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		// Token: 0x06009117 RID: 37143 RVA: 0x003402A7 File Offset: 0x0033E4A7
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		// Token: 0x06009118 RID: 37144 RVA: 0x003402B5 File Offset: 0x0033E4B5
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}

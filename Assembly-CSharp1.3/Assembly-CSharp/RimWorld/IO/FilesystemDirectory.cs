using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x0200181F RID: 6175
	internal class FilesystemDirectory : VirtualDirectory
	{
		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x060090DA RID: 37082 RVA: 0x0033F77B File Offset: 0x0033D97B
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x060090DB RID: 37083 RVA: 0x0033F788 File Offset: 0x0033D988
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		// Token: 0x170017DA RID: 6106
		// (get) Token: 0x060090DC RID: 37084 RVA: 0x0033F795 File Offset: 0x0033D995
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		// Token: 0x060090DD RID: 37085 RVA: 0x0033F7A2 File Offset: 0x0033D9A2
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		// Token: 0x060090DE RID: 37086 RVA: 0x0033F7B6 File Offset: 0x0033D9B6
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		// Token: 0x060090DF RID: 37087 RVA: 0x0033F7C5 File Offset: 0x0033D9C5
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			foreach (DirectoryInfo dir in this.dirInfo.GetDirectories(searchPattern, searchOption))
			{
				yield return new FilesystemDirectory(dir);
			}
			DirectoryInfo[] array = null;
			yield break;
		}

		// Token: 0x060090E0 RID: 37088 RVA: 0x0033F7E3 File Offset: 0x0033D9E3
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		// Token: 0x060090E1 RID: 37089 RVA: 0x0033F7F6 File Offset: 0x0033D9F6
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		// Token: 0x060090E2 RID: 37090 RVA: 0x0033F80E File Offset: 0x0033DA0E
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			foreach (FileInfo fileInfo in this.dirInfo.GetFiles(searchPattern, searchOption))
			{
				yield return new FilesystemFile(fileInfo);
			}
			FileInfo[] array = null;
			yield break;
		}

		// Token: 0x04005B03 RID: 23299
		private DirectoryInfo dirInfo;
	}
}

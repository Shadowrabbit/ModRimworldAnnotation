using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020021F6 RID: 8694
	internal class FilesystemDirectory : VirtualDirectory
	{
		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x0600BA4C RID: 47692 RVA: 0x00078AF5 File Offset: 0x00076CF5
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600BA4D RID: 47693 RVA: 0x00078B02 File Offset: 0x00076D02
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600BA4E RID: 47694 RVA: 0x00078B0F File Offset: 0x00076D0F
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		// Token: 0x0600BA4F RID: 47695 RVA: 0x00078B1C File Offset: 0x00076D1C
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		// Token: 0x0600BA50 RID: 47696 RVA: 0x00078B30 File Offset: 0x00076D30
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		// Token: 0x0600BA51 RID: 47697 RVA: 0x00078B3F File Offset: 0x00076D3F
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			foreach (DirectoryInfo dir in this.dirInfo.GetDirectories(searchPattern, searchOption))
			{
				yield return new FilesystemDirectory(dir);
			}
			DirectoryInfo[] array = null;
			yield break;
		}

		// Token: 0x0600BA52 RID: 47698 RVA: 0x00078B5D File Offset: 0x00076D5D
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		// Token: 0x0600BA53 RID: 47699 RVA: 0x00078B70 File Offset: 0x00076D70
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		// Token: 0x0600BA54 RID: 47700 RVA: 0x00078B88 File Offset: 0x00076D88
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			foreach (FileInfo fileInfo in this.dirInfo.GetFiles(searchPattern, searchOption))
			{
				yield return new FilesystemFile(fileInfo);
			}
			FileInfo[] array = null;
			yield break;
		}

		// Token: 0x04007F2E RID: 32558
		private DirectoryInfo dirInfo;
	}
}

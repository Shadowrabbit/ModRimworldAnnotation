using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x020021F9 RID: 8697
	internal class FilesystemFile : VirtualFile
	{
		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x0600BA65 RID: 47717 RVA: 0x00078BFA File Offset: 0x00076DFA
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x0600BA66 RID: 47718 RVA: 0x00078C07 File Offset: 0x00076E07
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600BA67 RID: 47719 RVA: 0x00078C14 File Offset: 0x00076E14
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x0600BA68 RID: 47720 RVA: 0x00078C21 File Offset: 0x00076E21
		public override long Length
		{
			get
			{
				return this.fileInfo.Length;
			}
		}

		// Token: 0x0600BA69 RID: 47721 RVA: 0x00078C2E File Offset: 0x00076E2E
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		// Token: 0x0600BA6A RID: 47722 RVA: 0x00078C3D File Offset: 0x00076E3D
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		// Token: 0x0600BA6B RID: 47723 RVA: 0x00078C4A File Offset: 0x00076E4A
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		// Token: 0x0600BA6C RID: 47724 RVA: 0x00078C5C File Offset: 0x00076E5C
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		// Token: 0x0600BA6D RID: 47725 RVA: 0x00078C6E File Offset: 0x00076E6E
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		// Token: 0x0600BA6E RID: 47726 RVA: 0x00078C80 File Offset: 0x00076E80
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		// Token: 0x0600BA6F RID: 47727 RVA: 0x00359840 File Offset: 0x00357A40
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.fileInfo.Length.ToString());
		}

		// Token: 0x04007F43 RID: 32579
		private FileInfo fileInfo;
	}
}

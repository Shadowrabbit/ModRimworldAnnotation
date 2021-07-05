using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02001820 RID: 6176
	internal class FilesystemFile : VirtualFile
	{
		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x060090E3 RID: 37091 RVA: 0x0033F82C File Offset: 0x0033DA2C
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x060090E4 RID: 37092 RVA: 0x0033F839 File Offset: 0x0033DA39
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x060090E5 RID: 37093 RVA: 0x0033F846 File Offset: 0x0033DA46
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		// Token: 0x170017DE RID: 6110
		// (get) Token: 0x060090E6 RID: 37094 RVA: 0x0033F853 File Offset: 0x0033DA53
		public override long Length
		{
			get
			{
				return this.fileInfo.Length;
			}
		}

		// Token: 0x060090E7 RID: 37095 RVA: 0x0033F860 File Offset: 0x0033DA60
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		// Token: 0x060090E8 RID: 37096 RVA: 0x0033F86F File Offset: 0x0033DA6F
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		// Token: 0x060090E9 RID: 37097 RVA: 0x0033F87C File Offset: 0x0033DA7C
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		// Token: 0x060090EA RID: 37098 RVA: 0x0033F88E File Offset: 0x0033DA8E
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		// Token: 0x060090EB RID: 37099 RVA: 0x0033F8A0 File Offset: 0x0033DAA0
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		// Token: 0x060090EC RID: 37100 RVA: 0x0033F8B2 File Offset: 0x0033DAB2
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		// Token: 0x060090ED RID: 37101 RVA: 0x0033F8BC File Offset: 0x0033DABC
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.fileInfo.Length.ToString());
		}

		// Token: 0x04005B04 RID: 23300
		private FileInfo fileInfo;
	}
}

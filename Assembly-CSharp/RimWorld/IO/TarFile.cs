using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002201 RID: 8705
	internal class TarFile : VirtualFile
	{
		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x0600BAB3 RID: 47795 RVA: 0x00078F40 File Offset: 0x00077140
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17001BD3 RID: 7123
		// (get) Token: 0x0600BAB4 RID: 47796 RVA: 0x00078F48 File Offset: 0x00077148
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600BAB5 RID: 47797 RVA: 0x00078F50 File Offset: 0x00077150
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x17001BD5 RID: 7125
		// (get) Token: 0x0600BAB6 RID: 47798 RVA: 0x00078F5B File Offset: 0x0007715B
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		// Token: 0x0600BAB7 RID: 47799 RVA: 0x00078F66 File Offset: 0x00077166
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		// Token: 0x0600BAB8 RID: 47800 RVA: 0x00078F83 File Offset: 0x00077183
		private TarFile()
		{
		}

		// Token: 0x0600BAB9 RID: 47801 RVA: 0x00078F8B File Offset: 0x0007718B
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		// Token: 0x0600BABA RID: 47802 RVA: 0x00078F9B File Offset: 0x0007719B
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		// Token: 0x0600BABB RID: 47803 RVA: 0x0035A634 File Offset: 0x00358834
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		// Token: 0x0600BABC RID: 47804 RVA: 0x0035A66C File Offset: 0x0035886C
		public override string[] ReadAllLines()
		{
			this.CheckAccess();
			List<string> list = new List<string>();
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					while (!streamReader.EndOfStream)
					{
						list.Add(streamReader.ReadLine());
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600BABD RID: 47805 RVA: 0x0035A6E8 File Offset: 0x003588E8
		public override string ReadAllText()
		{
			this.CheckAccess();
			string result;
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x0600BABE RID: 47806 RVA: 0x0035A74C File Offset: 0x0035894C
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		// Token: 0x04007F77 RID: 32631
		public static readonly TarFile NotFound = new TarFile();

		// Token: 0x04007F78 RID: 32632
		public byte[] data;

		// Token: 0x04007F79 RID: 32633
		public string fullPath;

		// Token: 0x04007F7A RID: 32634
		public string name;
	}
}

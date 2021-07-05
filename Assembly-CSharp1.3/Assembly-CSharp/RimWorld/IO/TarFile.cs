using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02001822 RID: 6178
	internal class TarFile : VirtualFile
	{
		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x06009102 RID: 37122 RVA: 0x003400DA File Offset: 0x0033E2DA
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x06009103 RID: 37123 RVA: 0x003400E2 File Offset: 0x0033E2E2
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x06009104 RID: 37124 RVA: 0x003400EA File Offset: 0x0033E2EA
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x170017E5 RID: 6117
		// (get) Token: 0x06009105 RID: 37125 RVA: 0x003400F5 File Offset: 0x0033E2F5
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		// Token: 0x06009106 RID: 37126 RVA: 0x00340100 File Offset: 0x0033E300
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		// Token: 0x06009107 RID: 37127 RVA: 0x0034011D File Offset: 0x0033E31D
		private TarFile()
		{
		}

		// Token: 0x06009108 RID: 37128 RVA: 0x00340125 File Offset: 0x0033E325
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		// Token: 0x06009109 RID: 37129 RVA: 0x00340135 File Offset: 0x0033E335
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		// Token: 0x0600910A RID: 37130 RVA: 0x00340148 File Offset: 0x0033E348
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		// Token: 0x0600910B RID: 37131 RVA: 0x00340180 File Offset: 0x0033E380
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

		// Token: 0x0600910C RID: 37132 RVA: 0x003401FC File Offset: 0x0033E3FC
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

		// Token: 0x0600910D RID: 37133 RVA: 0x00340260 File Offset: 0x0033E460
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		// Token: 0x04005B0E RID: 23310
		public static readonly TarFile NotFound = new TarFile();

		// Token: 0x04005B0F RID: 23311
		public byte[] data;

		// Token: 0x04005B10 RID: 23312
		public string fullPath;

		// Token: 0x04005B11 RID: 23313
		public string name;
	}
}

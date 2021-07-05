using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02001824 RID: 6180
	public abstract class VirtualFile
	{
		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x0600911A RID: 37146
		public abstract string Name { get; }

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x0600911B RID: 37147
		public abstract string FullPath { get; }

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x0600911C RID: 37148
		public abstract bool Exists { get; }

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x0600911D RID: 37149
		public abstract long Length { get; }

		// Token: 0x0600911E RID: 37150
		public abstract Stream CreateReadStream();

		// Token: 0x0600911F RID: 37151
		public abstract string ReadAllText();

		// Token: 0x06009120 RID: 37152
		public abstract string[] ReadAllLines();

		// Token: 0x06009121 RID: 37153
		public abstract byte[] ReadAllBytes();
	}
}

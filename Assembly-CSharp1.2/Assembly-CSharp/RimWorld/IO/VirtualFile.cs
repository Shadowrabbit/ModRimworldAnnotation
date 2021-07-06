using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02002203 RID: 8707
	public abstract class VirtualFile
	{
		// Token: 0x17001BD9 RID: 7129
		// (get) Token: 0x0600BACB RID: 47819
		public abstract string Name { get; }

		// Token: 0x17001BDA RID: 7130
		// (get) Token: 0x0600BACC RID: 47820
		public abstract string FullPath { get; }

		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x0600BACD RID: 47821
		public abstract bool Exists { get; }

		// Token: 0x17001BDC RID: 7132
		// (get) Token: 0x0600BACE RID: 47822
		public abstract long Length { get; }

		// Token: 0x0600BACF RID: 47823
		public abstract Stream CreateReadStream();

		// Token: 0x0600BAD0 RID: 47824
		public abstract string ReadAllText();

		// Token: 0x0600BAD1 RID: 47825
		public abstract string[] ReadAllLines();

		// Token: 0x0600BAD2 RID: 47826
		public abstract byte[] ReadAllBytes();
	}
}

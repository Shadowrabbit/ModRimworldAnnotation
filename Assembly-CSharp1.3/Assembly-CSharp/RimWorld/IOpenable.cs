using System;

namespace RimWorld
{
	// Token: 0x02001046 RID: 4166
	public interface IOpenable
	{
		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06006289 RID: 25225
		bool CanOpen { get; }

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x0600628A RID: 25226
		int OpenTicks { get; }

		// Token: 0x0600628B RID: 25227
		void Open();
	}
}

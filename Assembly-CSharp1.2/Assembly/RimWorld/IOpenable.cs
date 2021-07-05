using System;

namespace RimWorld
{
	// Token: 0x02001671 RID: 5745
	public interface IOpenable
	{
		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06007D52 RID: 32082
		bool CanOpen { get; }

		// Token: 0x06007D53 RID: 32083
		void Open();
	}
}

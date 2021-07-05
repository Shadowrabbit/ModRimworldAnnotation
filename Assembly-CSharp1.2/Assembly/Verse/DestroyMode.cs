using System;

namespace Verse
{
	// Token: 0x02000502 RID: 1282
	public enum DestroyMode : byte
	{
		// Token: 0x04001669 RID: 5737
		Vanish,
		// Token: 0x0400166A RID: 5738
		WillReplace,
		// Token: 0x0400166B RID: 5739
		KillFinalize,
		// Token: 0x0400166C RID: 5740
		Deconstruct,
		// Token: 0x0400166D RID: 5741
		FailConstruction,
		// Token: 0x0400166E RID: 5742
		Cancel,
		// Token: 0x0400166F RID: 5743
		Refund,
		// Token: 0x04001670 RID: 5744
		QuestLogic
	}
}

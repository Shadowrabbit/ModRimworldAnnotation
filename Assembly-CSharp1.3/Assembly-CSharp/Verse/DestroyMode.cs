using System;

namespace Verse
{
	// Token: 0x02000371 RID: 881
	public enum DestroyMode : byte
	{
		// Token: 0x040010E9 RID: 4329
		Vanish,
		// Token: 0x040010EA RID: 4330
		WillReplace,
		// Token: 0x040010EB RID: 4331
		KillFinalize,
		// Token: 0x040010EC RID: 4332
		Deconstruct,
		// Token: 0x040010ED RID: 4333
		FailConstruction,
		// Token: 0x040010EE RID: 4334
		Cancel,
		// Token: 0x040010EF RID: 4335
		Refund,
		// Token: 0x040010F0 RID: 4336
		QuestLogic
	}
}

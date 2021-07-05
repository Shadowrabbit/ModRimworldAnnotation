using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000ABF RID: 2751
	public class DutyDef : Def
	{
		// Token: 0x04002CD4 RID: 11476
		public ThinkNode thinkNode;

		// Token: 0x04002CD5 RID: 11477
		public ThinkNode constantThinkNode;

		// Token: 0x04002CD6 RID: 11478
		public bool alwaysShowWeapon;

		// Token: 0x04002CD7 RID: 11479
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x04002CD8 RID: 11480
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x04002CD9 RID: 11481
		public bool threatDisabled;
	}
}

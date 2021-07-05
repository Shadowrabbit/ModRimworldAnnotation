using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000658 RID: 1624
	public class DutyDef : Def
	{
		// Token: 0x04001C29 RID: 7209
		public ThinkNode thinkNode;

		// Token: 0x04001C2A RID: 7210
		public ThinkNode constantThinkNode;

		// Token: 0x04001C2B RID: 7211
		public bool alwaysShowWeapon;

		// Token: 0x04001C2C RID: 7212
		public ThinkTreeDutyHook hook = ThinkTreeDutyHook.HighPriority;

		// Token: 0x04001C2D RID: 7213
		public RandomSocialMode socialModeMax = RandomSocialMode.SuperActive;

		// Token: 0x04001C2E RID: 7214
		public bool threatDisabled;

		// Token: 0x04001C2F RID: 7215
		public bool ritualSpectateTarget;
	}
}

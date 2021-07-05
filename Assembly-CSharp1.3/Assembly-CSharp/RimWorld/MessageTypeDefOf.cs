using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200144F RID: 5199
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x06007D42 RID: 32066 RVA: 0x002C4A7E File Offset: 0x002C2C7E
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x04004CE6 RID: 19686
		public static MessageTypeDef ThreatBig;

		// Token: 0x04004CE7 RID: 19687
		public static MessageTypeDef ThreatSmall;

		// Token: 0x04004CE8 RID: 19688
		public static MessageTypeDef PawnDeath;

		// Token: 0x04004CE9 RID: 19689
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x04004CEA RID: 19690
		public static MessageTypeDef NegativeEvent;

		// Token: 0x04004CEB RID: 19691
		public static MessageTypeDef NeutralEvent;

		// Token: 0x04004CEC RID: 19692
		public static MessageTypeDef TaskCompletion;

		// Token: 0x04004CED RID: 19693
		public static MessageTypeDef PositiveEvent;

		// Token: 0x04004CEE RID: 19694
		public static MessageTypeDef SituationResolved;

		// Token: 0x04004CEF RID: 19695
		public static MessageTypeDef RejectInput;

		// Token: 0x04004CF0 RID: 19696
		public static MessageTypeDef CautionInput;

		// Token: 0x04004CF1 RID: 19697
		public static MessageTypeDef SilentInput;
	}
}

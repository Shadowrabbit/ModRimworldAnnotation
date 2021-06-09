using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8E RID: 7310
	[DefOf]
	public static class MessageTypeDefOf
	{
		// Token: 0x06009F91 RID: 40849 RVA: 0x0006A60E File Offset: 0x0006880E
		static MessageTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(MessageTypeDefOf));
		}

		// Token: 0x04006BEB RID: 27627
		public static MessageTypeDef ThreatBig;

		// Token: 0x04006BEC RID: 27628
		public static MessageTypeDef ThreatSmall;

		// Token: 0x04006BED RID: 27629
		public static MessageTypeDef PawnDeath;

		// Token: 0x04006BEE RID: 27630
		public static MessageTypeDef NegativeHealthEvent;

		// Token: 0x04006BEF RID: 27631
		public static MessageTypeDef NegativeEvent;

		// Token: 0x04006BF0 RID: 27632
		public static MessageTypeDef NeutralEvent;

		// Token: 0x04006BF1 RID: 27633
		public static MessageTypeDef TaskCompletion;

		// Token: 0x04006BF2 RID: 27634
		public static MessageTypeDef PositiveEvent;

		// Token: 0x04006BF3 RID: 27635
		public static MessageTypeDef SituationResolved;

		// Token: 0x04006BF4 RID: 27636
		public static MessageTypeDef RejectInput;

		// Token: 0x04006BF5 RID: 27637
		public static MessageTypeDef CautionInput;

		// Token: 0x04006BF6 RID: 27638
		public static MessageTypeDef SilentInput;
	}
}

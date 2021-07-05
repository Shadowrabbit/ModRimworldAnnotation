using System;

namespace RimWorld
{
	// Token: 0x02001434 RID: 5172
	[DefOf]
	public static class InteractionDefOf
	{
		// Token: 0x06007D27 RID: 32039 RVA: 0x002C48B3 File Offset: 0x002C2AB3
		static InteractionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InteractionDefOf));
		}

		// Token: 0x04004BA6 RID: 19366
		public static InteractionDef Chitchat;

		// Token: 0x04004BA7 RID: 19367
		public static InteractionDef DeepTalk;

		// Token: 0x04004BA8 RID: 19368
		public static InteractionDef Insult;

		// Token: 0x04004BA9 RID: 19369
		public static InteractionDef RomanceAttempt;

		// Token: 0x04004BAA RID: 19370
		public static InteractionDef MarriageProposal;

		// Token: 0x04004BAB RID: 19371
		public static InteractionDef BuildRapport;

		// Token: 0x04004BAC RID: 19372
		public static InteractionDef RecruitAttempt;

		// Token: 0x04004BAD RID: 19373
		public static InteractionDef SparkJailbreak;

		// Token: 0x04004BAE RID: 19374
		[MayRequireIdeology]
		public static InteractionDef ReduceWill;

		// Token: 0x04004BAF RID: 19375
		[MayRequireIdeology]
		public static InteractionDef EnslaveAttempt;

		// Token: 0x04004BB0 RID: 19376
		[MayRequireIdeology]
		public static InteractionDef ConvertIdeoAttempt;

		// Token: 0x04004BB1 RID: 19377
		[MayRequireIdeology]
		public static InteractionDef Suppress;

		// Token: 0x04004BB2 RID: 19378
		[MayRequireIdeology]
		public static InteractionDef SparkSlaveRebellion;

		// Token: 0x04004BB3 RID: 19379
		public static InteractionDef AnimalChat;

		// Token: 0x04004BB4 RID: 19380
		public static InteractionDef TrainAttempt;

		// Token: 0x04004BB5 RID: 19381
		public static InteractionDef TameAttempt;

		// Token: 0x04004BB6 RID: 19382
		public static InteractionDef Nuzzle;

		// Token: 0x04004BB7 RID: 19383
		public static InteractionDef ReleaseToWild;

		// Token: 0x04004BB8 RID: 19384
		[MayRequireIdeology]
		public static InteractionDef Counsel_Success;

		// Token: 0x04004BB9 RID: 19385
		[MayRequireIdeology]
		public static InteractionDef Counsel_Failure;

		// Token: 0x04004BBA RID: 19386
		[MayRequireIdeology]
		public static InteractionDef Convert_Success;

		// Token: 0x04004BBB RID: 19387
		[MayRequireIdeology]
		public static InteractionDef Convert_Failure;

		// Token: 0x04004BBC RID: 19388
		[MayRequireIdeology]
		public static InteractionDef Reassure;

		// Token: 0x04004BBD RID: 19389
		[MayRequireIdeology]
		public static InteractionDef Trial_Accuse;

		// Token: 0x04004BBE RID: 19390
		[MayRequireIdeology]
		public static InteractionDef Trial_Defend;
	}
}

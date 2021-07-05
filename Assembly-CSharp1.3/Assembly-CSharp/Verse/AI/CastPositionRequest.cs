using System;

namespace Verse.AI
{
	// Token: 0x02000654 RID: 1620
	public struct CastPositionRequest
	{
		// Token: 0x04001C05 RID: 7173
		public Pawn caster;

		// Token: 0x04001C06 RID: 7174
		public Thing target;

		// Token: 0x04001C07 RID: 7175
		public Verb verb;

		// Token: 0x04001C08 RID: 7176
		public float maxRangeFromCaster;

		// Token: 0x04001C09 RID: 7177
		public float maxRangeFromTarget;

		// Token: 0x04001C0A RID: 7178
		public IntVec3 locus;

		// Token: 0x04001C0B RID: 7179
		public float maxRangeFromLocus;

		// Token: 0x04001C0C RID: 7180
		public bool wantCoverFromTarget;

		// Token: 0x04001C0D RID: 7181
		public IntVec3? preferredCastPosition;

		// Token: 0x04001C0E RID: 7182
		public Func<IntVec3, bool> validator;

		// Token: 0x04001C0F RID: 7183
		public int maxRegions;
	}
}

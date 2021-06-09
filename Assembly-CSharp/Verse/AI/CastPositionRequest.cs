using System;

namespace Verse.AI
{
	// Token: 0x02000AB7 RID: 2743
	public struct CastPositionRequest
	{
		// Token: 0x04002CA9 RID: 11433
		public Pawn caster;

		// Token: 0x04002CAA RID: 11434
		public Thing target;

		// Token: 0x04002CAB RID: 11435
		public Verb verb;

		// Token: 0x04002CAC RID: 11436
		public float maxRangeFromCaster;

		// Token: 0x04002CAD RID: 11437
		public float maxRangeFromTarget;

		// Token: 0x04002CAE RID: 11438
		public IntVec3 locus;

		// Token: 0x04002CAF RID: 11439
		public float maxRangeFromLocus;

		// Token: 0x04002CB0 RID: 11440
		public bool wantCoverFromTarget;

		// Token: 0x04002CB1 RID: 11441
		public int maxRegions;
	}
}

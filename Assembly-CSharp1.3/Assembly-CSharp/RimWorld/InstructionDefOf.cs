using System;

namespace RimWorld
{
	// Token: 0x02001463 RID: 5219
	[DefOf]
	public static class InstructionDefOf
	{
		// Token: 0x06007D56 RID: 32086 RVA: 0x002C4BD2 File Offset: 0x002C2DD2
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		// Token: 0x04004D55 RID: 19797
		public static InstructionDef RandomizeCharacter;

		// Token: 0x04004D56 RID: 19798
		public static InstructionDef ChooseLandingSite;
	}
}

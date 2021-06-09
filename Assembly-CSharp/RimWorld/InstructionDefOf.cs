using System;

namespace RimWorld
{
	// Token: 0x02001CA3 RID: 7331
	[DefOf]
	public static class InstructionDefOf
	{
		// Token: 0x06009FA6 RID: 40870 RVA: 0x0006A773 File Offset: 0x00068973
		static InstructionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(InstructionDefOf));
		}

		// Token: 0x04006C62 RID: 27746
		public static InstructionDef RandomizeCharacter;

		// Token: 0x04006C63 RID: 27747
		public static InstructionDef ChooseLandingSite;
	}
}

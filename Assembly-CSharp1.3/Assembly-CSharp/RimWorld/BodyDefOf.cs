using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200141A RID: 5146
	[DefOf]
	public static class BodyDefOf
	{
		// Token: 0x06007D0D RID: 32013 RVA: 0x002C46F9 File Offset: 0x002C28F9
		static BodyDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyDefOf));
		}

		// Token: 0x04004942 RID: 18754
		public static BodyDef Human;

		// Token: 0x04004943 RID: 18755
		public static BodyDef MechanicalCentipede;
	}
}

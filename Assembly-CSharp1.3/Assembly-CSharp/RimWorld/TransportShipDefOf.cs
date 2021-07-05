using System;

namespace RimWorld
{
	// Token: 0x0200146C RID: 5228
	[DefOf]
	public static class TransportShipDefOf
	{
		// Token: 0x06007D5E RID: 32094 RVA: 0x002C4C5A File Offset: 0x002C2E5A
		static TransportShipDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(TransportShipDefOf));
		}

		// Token: 0x04004E04 RID: 19972
		[MayRequireRoyalty]
		public static TransportShipDef Ship_Shuttle;
	}
}

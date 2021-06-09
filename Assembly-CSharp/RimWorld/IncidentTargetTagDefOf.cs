using System;

namespace RimWorld
{
	// Token: 0x02001C91 RID: 7313
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		// Token: 0x06009F94 RID: 40852 RVA: 0x0006A641 File Offset: 0x00068841
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		// Token: 0x04006BFB RID: 27643
		public static IncidentTargetTagDef World;

		// Token: 0x04006BFC RID: 27644
		public static IncidentTargetTagDef Caravan;

		// Token: 0x04006BFD RID: 27645
		public static IncidentTargetTagDef Map_RaidBeacon;

		// Token: 0x04006BFE RID: 27646
		public static IncidentTargetTagDef Map_PlayerHome;

		// Token: 0x04006BFF RID: 27647
		public static IncidentTargetTagDef Map_Misc;
	}
}

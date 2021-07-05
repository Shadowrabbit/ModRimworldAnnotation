using System;

namespace RimWorld
{
	// Token: 0x02001452 RID: 5202
	[DefOf]
	public static class IncidentTargetTagDefOf
	{
		// Token: 0x06007D45 RID: 32069 RVA: 0x002C4AB1 File Offset: 0x002C2CB1
		static IncidentTargetTagDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(IncidentTargetTagDefOf));
		}

		// Token: 0x04004CF6 RID: 19702
		public static IncidentTargetTagDef World;

		// Token: 0x04004CF7 RID: 19703
		public static IncidentTargetTagDef Caravan;

		// Token: 0x04004CF8 RID: 19704
		public static IncidentTargetTagDef Map_RaidBeacon;

		// Token: 0x04004CF9 RID: 19705
		public static IncidentTargetTagDef Map_PlayerHome;

		// Token: 0x04004CFA RID: 19706
		public static IncidentTargetTagDef Map_Misc;
	}
}

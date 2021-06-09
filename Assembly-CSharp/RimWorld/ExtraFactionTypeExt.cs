using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CA RID: 4298
	public static class ExtraFactionTypeExt
	{
		// Token: 0x06005DBC RID: 23996 RVA: 0x00040FC7 File Offset: 0x0003F1C7
		public static string GetLabel(this ExtraFactionType factionType)
		{
			return ("ExtraFactionType_" + factionType.ToString()).Translate();
		}
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B77 RID: 2935
	public static class ExtraFactionTypeExt
	{
		// Token: 0x060044A0 RID: 17568 RVA: 0x0016C086 File Offset: 0x0016A286
		public static string GetLabel(this ExtraFactionType factionType)
		{
			return ("ExtraFactionType_" + factionType.ToString()).Translate();
		}
	}
}

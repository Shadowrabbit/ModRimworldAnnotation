using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9D RID: 3997
	public class RitualSpectatorFilter_NatureFocus : RitualSpectatorFilter
	{
		// Token: 0x06005E8B RID: 24203 RVA: 0x002069F9 File Offset: 0x00204BF9
		public override bool Allowed(Pawn p)
		{
			return MeditationFocusDefOf.Natural.CanPawnUse(p);
		}
	}
}

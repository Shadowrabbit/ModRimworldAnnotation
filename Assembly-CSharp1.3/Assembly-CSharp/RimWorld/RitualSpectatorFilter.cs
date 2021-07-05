using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9C RID: 3996
	public abstract class RitualSpectatorFilter
	{
		// Token: 0x06005E89 RID: 24201
		public abstract bool Allowed(Pawn p);

		// Token: 0x04003688 RID: 13960
		[MustTranslate]
		public string description;
	}
}

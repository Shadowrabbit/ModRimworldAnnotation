using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011FC RID: 4604
	public abstract class CompPlaysMusic : ThingComp
	{
		// Token: 0x1700133B RID: 4923
		// (get) Token: 0x06006EBE RID: 28350
		public abstract bool Playing { get; }

		// Token: 0x1700133C RID: 4924
		// (get) Token: 0x06006EBF RID: 28351
		public abstract FloatRange SoundRange { get; }
	}
}

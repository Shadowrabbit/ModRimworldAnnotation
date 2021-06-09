using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x020009E2 RID: 2530
	public static class Toils_Effects
	{
		// Token: 0x06003D06 RID: 15622 RVA: 0x001741C8 File Offset: 0x001723C8
		public static Toil MakeSound(SoundDef soundDef)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				soundDef.PlayOneShot(new TargetInfo(actor.Position, actor.Map, false));
			};
			return toil;
		}
	}
}

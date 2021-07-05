using System;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x020005B0 RID: 1456
	public static class Toils_Effects
	{
		// Token: 0x06002A99 RID: 10905 RVA: 0x000FFF40 File Offset: 0x000FE140
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

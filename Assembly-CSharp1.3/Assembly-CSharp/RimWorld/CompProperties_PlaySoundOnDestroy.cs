using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001170 RID: 4464
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		// Token: 0x06006B39 RID: 27449 RVA: 0x0023FE62 File Offset: 0x0023E062
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		// Token: 0x04003B9E RID: 15262
		public SoundDef sound;
	}
}

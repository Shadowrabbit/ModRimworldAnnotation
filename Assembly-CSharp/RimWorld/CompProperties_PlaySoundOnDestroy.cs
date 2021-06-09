using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001807 RID: 6151
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		// Token: 0x06008818 RID: 34840 RVA: 0x0005B58F File Offset: 0x0005978F
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		// Token: 0x0400574F RID: 22351
		public SoundDef sound;
	}
}

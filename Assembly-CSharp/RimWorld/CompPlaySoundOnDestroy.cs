using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001808 RID: 6152
	public class CompPlaySoundOnDestroy : ThingComp
	{
		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06008819 RID: 34841 RVA: 0x0005B5A7 File Offset: 0x000597A7
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		// Token: 0x0600881A RID: 34842 RVA: 0x0005B5B4 File Offset: 0x000597B4
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}

using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001171 RID: 4465
	public class CompPlaySoundOnDestroy : ThingComp
	{
		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06006B3A RID: 27450 RVA: 0x0023FE7A File Offset: 0x0023E07A
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x0023FE87 File Offset: 0x0023E087
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}

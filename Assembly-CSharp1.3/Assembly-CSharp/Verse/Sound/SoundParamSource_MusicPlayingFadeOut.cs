using System;

namespace Verse.Sound
{
	// Token: 0x02000551 RID: 1361
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002898 RID: 10392 RVA: 0x000F6F8C File Offset: 0x000F518C
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000F6F93 File Offset: 0x000F5193
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.MusicManagerPlay == null)
			{
				return 1f;
			}
			return Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
		}
	}
}

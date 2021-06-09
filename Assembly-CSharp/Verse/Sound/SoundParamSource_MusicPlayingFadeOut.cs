using System;

namespace Verse.Sound
{
	// Token: 0x02000926 RID: 2342
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060039D3 RID: 14803 RVA: 0x0002C9E8 File Offset: 0x0002ABE8
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x0002C9EF File Offset: 0x0002ABEF
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

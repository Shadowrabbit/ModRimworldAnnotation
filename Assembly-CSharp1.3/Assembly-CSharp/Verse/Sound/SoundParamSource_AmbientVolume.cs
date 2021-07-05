using System;

namespace Verse.Sound
{
	// Token: 0x02000552 RID: 1362
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x000F6FB4 File Offset: 0x000F51B4
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000F6FBB File Offset: 0x000F51BB
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}

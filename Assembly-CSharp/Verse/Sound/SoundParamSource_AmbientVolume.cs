using System;

namespace Verse.Sound
{
	// Token: 0x02000927 RID: 2343
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x060039D6 RID: 14806 RVA: 0x0002CA10 File Offset: 0x0002AC10
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x0002CA17 File Offset: 0x0002AC17
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}

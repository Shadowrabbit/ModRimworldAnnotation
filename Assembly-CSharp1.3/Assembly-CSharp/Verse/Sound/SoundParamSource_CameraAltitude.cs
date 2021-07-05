using System;

namespace Verse.Sound
{
	// Token: 0x0200054F RID: 1359
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06002892 RID: 10386 RVA: 0x000F6F3C File Offset: 0x000F513C
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000F6F43 File Offset: 0x000F5143
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}

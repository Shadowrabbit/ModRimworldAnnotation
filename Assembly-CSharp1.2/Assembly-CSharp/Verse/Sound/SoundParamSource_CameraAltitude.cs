using System;

namespace Verse.Sound
{
	// Token: 0x02000924 RID: 2340
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x060039CD RID: 14797 RVA: 0x0002C998 File Offset: 0x0002AB98
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x0002C99F File Offset: 0x0002AB9F
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}

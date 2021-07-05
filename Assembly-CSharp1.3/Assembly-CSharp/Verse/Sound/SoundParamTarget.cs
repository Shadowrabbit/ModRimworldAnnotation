using System;

namespace Verse.Sound
{
	// Token: 0x02000558 RID: 1368
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060028A8 RID: 10408
		public abstract string Label { get; }

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x00002688 File Offset: 0x00000888
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060028AA RID: 10410
		public abstract void SetOn(Sample sample, float value);
	}
}

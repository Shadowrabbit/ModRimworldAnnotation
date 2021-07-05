using System;

namespace Verse.Sound
{
	// Token: 0x0200092D RID: 2349
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x060039E3 RID: 14819
		public abstract string Label { get; }

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x060039E4 RID: 14820 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060039E5 RID: 14821
		public abstract void SetOn(Sample sample, float value);
	}
}

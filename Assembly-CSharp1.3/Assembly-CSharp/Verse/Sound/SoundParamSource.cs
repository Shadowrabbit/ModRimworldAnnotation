using System;

namespace Verse.Sound
{
	// Token: 0x0200054C RID: 1356
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06002889 RID: 10377
		public abstract string Label { get; }

		// Token: 0x0600288A RID: 10378
		public abstract float ValueFor(Sample samp);
	}
}

using System;

namespace Verse.Sound
{
	// Token: 0x02000921 RID: 2337
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x060039C4 RID: 14788
		public abstract string Label { get; }

		// Token: 0x060039C5 RID: 14789
		public abstract float ValueFor(Sample samp);
	}
}

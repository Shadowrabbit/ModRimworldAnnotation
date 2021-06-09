using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000813 RID: 2067
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x060033FA RID: 13306 RVA: 0x0000A876 File Offset: 0x00008A76
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00028BD1 File Offset: 0x00026DD1
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}

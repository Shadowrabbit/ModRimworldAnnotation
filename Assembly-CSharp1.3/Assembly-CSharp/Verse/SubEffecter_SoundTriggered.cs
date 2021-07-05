using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004A0 RID: 1184
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x060023FD RID: 9213 RVA: 0x000158D1 File Offset: 0x00013AD1
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x060023FE RID: 9214 RVA: 0x000DFEC8 File Offset: 0x000DE0C8
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}

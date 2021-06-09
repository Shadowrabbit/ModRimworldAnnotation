using System;

namespace Verse
{
	// Token: 0x020000A5 RID: 165
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x0000A880 File Offset: 0x00008A80
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000A8BB File Offset: 0x00008ABB
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}

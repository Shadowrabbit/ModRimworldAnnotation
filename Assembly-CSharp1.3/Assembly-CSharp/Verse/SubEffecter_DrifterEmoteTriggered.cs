using System;

namespace Verse
{
	// Token: 0x0200005D RID: 93
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x00015A41 File Offset: 0x00013C41
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015AA4 File Offset: 0x00013CA4
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A);
		}
	}
}

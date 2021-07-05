using System;

namespace Verse
{
	// Token: 0x020004A4 RID: 1188
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06002405 RID: 9221 RVA: 0x000E0497 File Offset: 0x000DE697
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000E0515 File Offset: 0x000DE715
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}

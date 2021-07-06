using System;

namespace Verse
{
	// Token: 0x02000817 RID: 2071
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x06003402 RID: 13314 RVA: 0x00028BFC File Offset: 0x00026DFC
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00028C06 File Offset: 0x00026E06
		public override void SubTrigger(TargetInfo A, TargetInfo B)
		{
			base.MakeMote(A, B);
		}
	}
}

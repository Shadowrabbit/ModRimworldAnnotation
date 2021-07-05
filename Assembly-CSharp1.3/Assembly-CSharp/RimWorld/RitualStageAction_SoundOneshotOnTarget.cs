using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000FA3 RID: 4003
	public class RitualStageAction_SoundOneshotOnTarget : RitualStageAction
	{
		// Token: 0x06005E9E RID: 24222 RVA: 0x00206CBC File Offset: 0x00204EBC
		public override void Apply(LordJob_Ritual ritual)
		{
			this.sound.PlayOneShot(SoundInfo.InMap(ritual.selectedTarget, MaintenanceType.None));
		}

		// Token: 0x06005E9F RID: 24223 RVA: 0x00206CD5 File Offset: 0x00204ED5
		public override void ExposeData()
		{
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x0400368E RID: 13966
		public SoundDef sound;
	}
}

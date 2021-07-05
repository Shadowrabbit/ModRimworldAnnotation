using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010A2 RID: 4258
	public class SignalAction_SoundOneShot : SignalAction
	{
		// Token: 0x06006578 RID: 25976 RVA: 0x0022432F File Offset: 0x0022252F
		protected override void DoAction(SignalArgs args)
		{
			this.sound.PlayOneShot(this);
		}

		// Token: 0x06006579 RID: 25977 RVA: 0x00224342 File Offset: 0x00222542
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SoundDef>(ref this.sound, "sound");
		}

		// Token: 0x04003928 RID: 14632
		public SoundDef sound;
	}
}

using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F21 RID: 3873
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x0600558A RID: 21898 RVA: 0x0003B5D7 File Offset: 0x000397D7
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x040036B1 RID: 14001
		public float massCapacity = 150f;

		// Token: 0x040036B2 RID: 14002
		public float restEffectiveness;

		// Token: 0x040036B3 RID: 14003
		public bool max1PerGroup;

		// Token: 0x040036B4 RID: 14004
		public bool canChangeAssignedThingsAfterStarting;

		// Token: 0x040036B5 RID: 14005
		public bool showOverallStats = true;

		// Token: 0x040036B6 RID: 14006
		public SoundDef pawnLoadedSound;

		// Token: 0x040036B7 RID: 14007
		public SoundDef pawnExitSound;
	}
}

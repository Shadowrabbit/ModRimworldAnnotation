using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A11 RID: 2577
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x06003F0A RID: 16138 RVA: 0x00157F82 File Offset: 0x00156182
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x04002210 RID: 8720
		public float massCapacity = 150f;

		// Token: 0x04002211 RID: 8721
		public float restEffectiveness;

		// Token: 0x04002212 RID: 8722
		public bool max1PerGroup;

		// Token: 0x04002213 RID: 8723
		public bool canChangeAssignedThingsAfterStarting;

		// Token: 0x04002214 RID: 8724
		public bool showOverallStats = true;

		// Token: 0x04002215 RID: 8725
		public SoundDef pawnLoadedSound;

		// Token: 0x04002216 RID: 8726
		public SoundDef pawnExitSound;
	}
}

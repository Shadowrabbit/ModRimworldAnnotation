using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000112 RID: 274
	public abstract class DeathActionWorker
	{
		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0000C054 File Offset: 0x0000A254
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x0600077D RID: 1917 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600077E RID: 1918
		public abstract void PawnDied(Corpse corpse);
	}
}

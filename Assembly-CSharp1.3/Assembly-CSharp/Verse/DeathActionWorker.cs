using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000AC RID: 172
	public abstract class DeathActionWorker
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600055B RID: 1371 RVA: 0x0001BD16 File Offset: 0x00019F16
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600055D RID: 1373
		public abstract void PawnDied(Corpse corpse);
	}
}

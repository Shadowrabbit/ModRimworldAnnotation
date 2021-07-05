using System;

namespace Verse
{
	// Token: 0x020002C9 RID: 713
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x0006D4D2 File Offset: 0x0006B6D2
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
				return;
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0006D500 File Offset: 0x0006B700
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}

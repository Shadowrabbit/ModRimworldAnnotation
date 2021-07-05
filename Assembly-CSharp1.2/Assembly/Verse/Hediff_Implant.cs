using System;

namespace Verse
{
	// Token: 0x020003B2 RID: 946
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600177A RID: 6010 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00016869 File Offset: 0x00014A69
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.", false);
				return;
			}
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000DC6C8 File Offset: 0x000DA8C8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.", false);
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}

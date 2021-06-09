using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200052A RID: 1322
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x0001D593 File Offset: 0x0001B793
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x0001D5A0 File Offset: 0x0001B7A0
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x0001D5BA File Offset: 0x0001B7BA
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x00107CDC File Offset: 0x00105EDC
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			}
		}
	}
}

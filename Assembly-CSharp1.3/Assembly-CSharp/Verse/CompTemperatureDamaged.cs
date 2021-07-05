using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000389 RID: 905
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001A80 RID: 6784 RVA: 0x00099CC1 File Offset: 0x00097EC1
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00099CCE File Offset: 0x00097ECE
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00099CE8 File Offset: 0x00097EE8
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00099CF0 File Offset: 0x00097EF0
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			}
		}
	}
}

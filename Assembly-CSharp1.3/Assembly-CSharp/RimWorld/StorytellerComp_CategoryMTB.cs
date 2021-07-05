using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C38 RID: 3128
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x0600497C RID: 18812 RVA: 0x0018521D File Offset: 0x0018341D
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x0018522A File Offset: 0x0018342A
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			float num = this.Props.mtbDays;
			if (this.Props.mtbDaysFactorByDaysPassedCurve != null)
			{
				num *= this.Props.mtbDaysFactorByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (Rand.MTBEventOccurs(num, 60000f, 1000f))
			{
				IncidentParms parms = this.GenerateParms(this.Props.category, target);
				IncidentDef def;
				if (base.TrySelectRandomIncident(base.UsableIncidentsInCategory(this.Props.category, parms), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00185241 File Offset: 0x00183441
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}

using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001218 RID: 4632
	public class StorytellerComp_CategoryMTB : StorytellerComp
	{
		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06006557 RID: 25943 RVA: 0x0004556A File Offset: 0x0004376A
		protected StorytellerCompProperties_CategoryMTB Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryMTB)this.props;
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x00045577 File Offset: 0x00043777
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
				if (base.UsableIncidentsInCategory(this.Props.category, parms).TryRandomElementByWeight((IncidentDef incDef) => base.IncidentChanceFinal(incDef), out def))
				{
					yield return new FiringIncident(def, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x0004558E File Offset: 0x0004378E
		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}
	}
}

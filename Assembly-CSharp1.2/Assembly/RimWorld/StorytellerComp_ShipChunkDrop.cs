using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200123A RID: 4666
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x060065E2 RID: 26082 RVA: 0x001F7650 File Offset: 0x001F5850
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x060065E3 RID: 26083 RVA: 0x00045AA8 File Offset: 0x00043CA8
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef shipChunkDrop = IncidentDefOf.ShipChunkDrop;
				IncidentParms parms = this.GenerateParms(shipChunkDrop.category, target);
				if (shipChunkDrop.Worker.CanFireNow(parms, false))
				{
					yield return new FiringIncident(shipChunkDrop, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x040043D6 RID: 17366
		private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(1f, 40f),
				true
			},
			{
				new CurvePoint(2f, 80f),
				true
			},
			{
				new CurvePoint(2.75f, 135f),
				true
			}
		};
	}
}

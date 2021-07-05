using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4D RID: 3149
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x001858A8 File Offset: 0x00183AA8
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x001858D2 File Offset: 0x00183AD2
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef shipChunkDrop = IncidentDefOf.ShipChunkDrop;
				IncidentParms parms = this.GenerateParms(shipChunkDrop.category, target);
				if (shipChunkDrop.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(shipChunkDrop, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x04002CCB RID: 11467
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

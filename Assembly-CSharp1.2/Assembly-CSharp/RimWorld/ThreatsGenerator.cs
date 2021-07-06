using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D79 RID: 7545
	public static class ThreatsGenerator
	{
		// Token: 0x0600A40C RID: 41996 RVA: 0x0006CCA1 File Offset: 0x0006AEA1
		public static IEnumerable<FiringIncident> MakeIntervalIncidents(ThreatsGeneratorParams parms, IIncidentTarget target, int startTick)
		{
			float num = ThreatsGenerator.ThreatScaleToCountFactorCurve.Evaluate(Find.Storyteller.difficultyValues.threatScale);
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, parms.randSeed, (float)startTick / 60000f, parms.onDays, parms.offDays, parms.minSpacingDays, parms.numIncidentsRange.min * num, parms.numIncidentsRange.max * num, 1f);
			int num2;
			for (int i = 0; i < incCount; i = num2 + 1)
			{
				FiringIncident firingIncident = ThreatsGenerator.MakeThreat(parms, target);
				if (firingIncident != null)
				{
					yield return firingIncident;
				}
				num2 = i;
			}
			yield break;
		}

		// Token: 0x0600A40D RID: 41997 RVA: 0x002FC614 File Offset: 0x002FA814
		private static FiringIncident MakeThreat(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			IncidentDef def;
			if (!(from x in ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats)
			where x.Worker.CanFireNow(incParms, false)
			select x).TryRandomElementByWeight((IncidentDef x) => x.Worker.BaseChanceThisGame, out def))
			{
				return null;
			}
			return new FiringIncident
			{
				def = def,
				parms = incParms
			};
		}

		// Token: 0x0600A40E RID: 41998 RVA: 0x002FC694 File Offset: 0x002FA894
		public static bool AnyIncidentPossible(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			return ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats).Any((IncidentDef x) => x.Worker.BaseChanceThisGame > 0f && x.Worker.CanFireNow(incParms, false));
		}

		// Token: 0x0600A40F RID: 41999 RVA: 0x002FC6D0 File Offset: 0x002FA8D0
		private static IncidentParms GetIncidentParms(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = target;
			incidentParms.points = (parms.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(target) * parms.currentThreatPointsFactor));
			if (parms.minThreatPoints != null)
			{
				incidentParms.points = Mathf.Max(incidentParms.points, parms.minThreatPoints.Value);
			}
			incidentParms.faction = parms.faction;
			incidentParms.forced = true;
			return incidentParms;
		}

		// Token: 0x0600A410 RID: 42000 RVA: 0x0006CCBF File Offset: 0x0006AEBF
		private static IEnumerable<IncidentDef> GetPossibleIncidents(AllowedThreatsGeneratorThreats allowedThreats)
		{
			if ((allowedThreats & AllowedThreatsGeneratorThreats.Raids) != AllowedThreatsGeneratorThreats.None)
			{
				yield return IncidentDefOf.RaidEnemy;
			}
			if ((allowedThreats & AllowedThreatsGeneratorThreats.MechClusters) != AllowedThreatsGeneratorThreats.None)
			{
				yield return IncidentDefOf.MechCluster;
			}
			yield break;
		}

		// Token: 0x04006F30 RID: 28464
		private static readonly SimpleCurve ThreatScaleToCountFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.1f),
				true
			},
			{
				new CurvePoint(0.3f, 0.5f),
				true
			},
			{
				new CurvePoint(0.6f, 0.8f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(1.55f, 1.1f),
				true
			},
			{
				new CurvePoint(2.2f, 1.2f),
				true
			},
			{
				new CurvePoint(10f, 2f),
				true
			}
		};
	}
}

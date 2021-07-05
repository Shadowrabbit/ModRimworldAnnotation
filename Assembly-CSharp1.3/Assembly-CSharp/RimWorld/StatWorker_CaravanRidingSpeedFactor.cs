using System;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F3 RID: 5363
	public class StatWorker_CaravanRidingSpeedFactor : StatWorker
	{
		// Token: 0x06007FDB RID: 32731 RVA: 0x002D40C8 File Offset: 0x002D22C8
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.HasThing && req.Thing.def.race != null)
			{
				stringBuilder.Append("StatsReport_CaravanRideableAge".Translate()).Append(": ").Append(CaravanRideableUtility.RideableLifeStagesDesc(req.Thing.def.race)).AppendLine();
			}
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			return stringBuilder.ToString();
		}
	}
}

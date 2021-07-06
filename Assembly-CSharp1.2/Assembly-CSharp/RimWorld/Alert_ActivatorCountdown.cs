using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001954 RID: 6484
	public class Alert_ActivatorCountdown : Alert
	{
		// Token: 0x06008F96 RID: 36758 RVA: 0x00060341 File Offset: 0x0005E541
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06008F97 RID: 36759 RVA: 0x0029570C File Offset: 0x0029390C
		private List<Thing> ActivatorCountdowns
		{
			get
			{
				this.activatorCountdownsResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.ActivatorCountdown)))
						{
							CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
							if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft > 0)
							{
								this.activatorCountdownsResult.Add(thing);
							}
						}
					}
				}
				return this.activatorCountdownsResult;
			}
		}

		// Token: 0x06008F98 RID: 36760 RVA: 0x0006035B File Offset: 0x0005E55B
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		// Token: 0x06008F99 RID: 36761 RVA: 0x002957E4 File Offset: 0x002939E4
		public override string GetLabel()
		{
			int count = this.ActivatorCountdowns.Count;
			if (count > 1)
			{
				return "ActivatorCountdownMultiple".Translate(count);
			}
			if (count == 0)
			{
				return "";
			}
			CompSendSignalOnCountdown compSendSignalOnCountdown = this.ActivatorCountdowns[0].TryGetComp<CompSendSignalOnCountdown>();
			return "ActivatorCountdown".Translate(compSendSignalOnCountdown.ticksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x06008F9A RID: 36762 RVA: 0x00295858 File Offset: 0x00293A58
		public override TaggedString GetExplanation()
		{
			int num = this.ActivatorCountdowns.Count<Thing>();
			if (num > 1)
			{
				return "ActivatorCountdownDescMultiple".Translate(num);
			}
			if (num == 0)
			{
				return "";
			}
			return "ActivatorCountdownDesc".Translate();
		}

		// Token: 0x04005B72 RID: 23410
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}

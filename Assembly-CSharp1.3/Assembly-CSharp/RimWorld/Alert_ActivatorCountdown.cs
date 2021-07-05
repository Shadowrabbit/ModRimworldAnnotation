using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001265 RID: 4709
	public class Alert_ActivatorCountdown : Alert
	{
		// Token: 0x060070C7 RID: 28871 RVA: 0x00259430 File Offset: 0x00257630
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013AA RID: 5034
		// (get) Token: 0x060070C8 RID: 28872 RVA: 0x0025944C File Offset: 0x0025764C
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

		// Token: 0x060070C9 RID: 28873 RVA: 0x00259524 File Offset: 0x00257724
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		// Token: 0x060070CA RID: 28874 RVA: 0x00259540 File Offset: 0x00257740
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

		// Token: 0x060070CB RID: 28875 RVA: 0x002595B4 File Offset: 0x002577B4
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

		// Token: 0x04003E25 RID: 15909
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}

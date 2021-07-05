using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001267 RID: 4711
	public class Alert_SlavesUnattended : Alert
	{
		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x060070D0 RID: 28880 RVA: 0x0025973C File Offset: 0x0025793C
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (SlaveRebellionUtility.IsUnattendedByColonists(maps[i]))
					{
						this.targetsResult.AddRange(maps[i].mapPawns.SlavesOfColonySpawned);
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060070D1 RID: 28881 RVA: 0x0025979B File Offset: 0x0025799B
		public Alert_SlavesUnattended()
		{
			this.defaultLabel = "SlaveUnattendedLabel".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070D2 RID: 28882 RVA: 0x002597CA File Offset: 0x002579CA
		public override string GetLabel()
		{
			if (this.Targets.Count == 1)
			{
				return "SlaveUnattendedLabel".Translate();
			}
			return "SlaveUnattendedMultipleLabel".Translate();
		}

		// Token: 0x060070D3 RID: 28883 RVA: 0x002597F9 File Offset: 0x002579F9
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060070D4 RID: 28884 RVA: 0x00259808 File Offset: 0x00257A08
		public override TaggedString GetExplanation()
		{
			Pawn value = this.Targets[0];
			return "SlavesUnattendedDesc".Translate(value);
		}

		// Token: 0x04003E27 RID: 15911
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001955 RID: 6485
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x170016AF RID: 5807
		// (get) Token: 0x06008F9C RID: 36764 RVA: 0x002958A0 File Offset: 0x00293AA0
		private List<Pawn> IdleColonists
		{
			get
			{
				this.idleColonistsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.mindState.IsIdle)
							{
								if (pawn.royalty != null)
								{
									RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
									if (mostSeniorTitle == null || !mostSeniorTitle.def.suppressIdleAlert)
									{
										this.idleColonistsResult.Add(pawn);
									}
								}
								else
								{
									this.idleColonistsResult.Add(pawn);
								}
							}
						}
					}
				}
				return this.idleColonistsResult;
			}
		}

		// Token: 0x06008F9D RID: 36765 RVA: 0x00060389 File Offset: 0x0005E589
		public override string GetLabel()
		{
			return "ColonistsIdle".Translate(this.IdleColonists.Count.ToStringCached());
		}

		// Token: 0x06008F9E RID: 36766 RVA: 0x00295984 File Offset: 0x00293B84
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F9F RID: 36767 RVA: 0x000603AF File Offset: 0x0005E5AF
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		// Token: 0x04005B73 RID: 23411
		public const int MinDaysPassed = 1;

		// Token: 0x04005B74 RID: 23412
		private List<Pawn> idleColonistsResult = new List<Pawn>();
	}
}

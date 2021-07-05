using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001268 RID: 4712
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x060070D6 RID: 28886 RVA: 0x00259848 File Offset: 0x00257A48
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

		// Token: 0x060070D7 RID: 28887 RVA: 0x0025992C File Offset: 0x00257B2C
		public override string GetLabel()
		{
			return "ColonistsIdle".Translate(this.IdleColonists.Count.ToStringCached());
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x00259954 File Offset: 0x00257B54
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.IdleColonists)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060070D9 RID: 28889 RVA: 0x002599DC File Offset: 0x00257BDC
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		// Token: 0x04003E28 RID: 15912
		public const int MinDaysPassed = 1;

		// Token: 0x04003E29 RID: 15913
		private List<Pawn> idleColonistsResult = new List<Pawn>();
	}
}

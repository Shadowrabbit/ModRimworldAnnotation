using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001945 RID: 6469
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x06008F59 RID: 36697 RVA: 0x0005FFD6 File Offset: 0x0005E1D6
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x06008F5A RID: 36698 RVA: 0x0029419C File Offset: 0x0029239C
		private List<Pawn> Patients
		{
			get
			{
				this.patientsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool flag = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonists)
						{
							if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							foreach (Pawn pawn2 in maps[i].mapPawns.FreeColonists)
							{
								if ((pawn2.Spawned || pawn2.BrieflyDespawned()) && ((pawn2.Downed && pawn2.needs != null && pawn2.needs.food.CurCategory < HungerCategory.Fed && pawn2.InBed()) || HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2)))
								{
									this.patientsResult.Add(pawn2);
								}
							}
						}
					}
				}
				return this.patientsResult;
			}
		}

		// Token: 0x06008F5B RID: 36699 RVA: 0x00294314 File Offset: 0x00292514
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x06008F5C RID: 36700 RVA: 0x00060005 File Offset: 0x0005E205
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		// Token: 0x04005B5F RID: 23391
		private List<Pawn> patientsResult = new List<Pawn>();
	}
}

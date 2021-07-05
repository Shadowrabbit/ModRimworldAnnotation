using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001256 RID: 4694
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x06007089 RID: 28809 RVA: 0x002578D5 File Offset: 0x00255AD5
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x0600708A RID: 28810 RVA: 0x00257904 File Offset: 0x00255B04
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

		// Token: 0x0600708B RID: 28811 RVA: 0x00257A7C File Offset: 0x00255C7C
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x00257B04 File Offset: 0x00255D04
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		// Token: 0x04003E12 RID: 15890
		private List<Pawn> patientsResult = new List<Pawn>();
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001282 RID: 4738
	public class Alert_AnimalPenNeeded : Alert
	{
		// Token: 0x06007137 RID: 28983 RVA: 0x0025BA7E File Offset: 0x00259C7E
		public Alert_AnimalPenNeeded()
		{
			this.defaultLabel = "AlertAnimalPenNeeded".Translate();
		}

		// Token: 0x06007138 RID: 28984 RVA: 0x0025BAB4 File Offset: 0x00259CB4
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.lowSkillHandlers.Clear();
			this.anyHandlers = false;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					Pawn pawn = null;
					int num = -1;
					foreach (Pawn pawn2 in map.mapPawns.FreeColonistsSpawned)
					{
						if (pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Handling))
						{
							int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
							if (level > num)
							{
								pawn = pawn2;
								num = level;
							}
						}
					}
					foreach (Pawn pawn3 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
					{
						if (AnimalPenUtility.NeedsToBeManagedByRope(pawn3) && pawn3.CanReachMapEdge())
						{
							bool flag = !AnimalPenUtility.AnySuitablePens(pawn3, true);
							if (pawn == null)
							{
								flag = true;
							}
							else
							{
								this.anyHandlers = true;
								int num2 = TrainableUtility.MinimumHandlingSkill(pawn3);
								if (num2 > num)
								{
									flag = true;
									this.lowSkillHandlers.Add(new Alert_AnimalPenNeeded.LowSkillInfo
									{
										animal = pawn3,
										bestHandler = pawn,
										bestSkillLevel = num,
										requiredSkillLevel = num2
									});
								}
							}
							if (flag)
							{
								this.targets.Add(pawn3);
							}
						}
					}
				}
			}
		}

		// Token: 0x06007139 RID: 28985 RVA: 0x0025BCAC File Offset: 0x00259EAC
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("AlertAnimalPenNeededDesc".Translate());
			if (!this.anyHandlers)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AlertAnimalPenNeededNoHandlers".Translate());
			}
			else if (this.lowSkillHandlers.Any<Alert_AnimalPenNeeded.LowSkillInfo>())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AlertAnimalPenNeededLowSkillHandlers".Translate());
				stringBuilder.AppendLine();
				foreach (Alert_AnimalPenNeeded.LowSkillInfo lowSkillInfo in this.lowSkillHandlers)
				{
					stringBuilder.Append("  - ").Append(lowSkillInfo.animal.NameShortColored.Resolve()).Append(" (");
					stringBuilder.Append("AlertAnimalPenNeededLowSkillHandlersDetail".Translate(lowSkillInfo.requiredSkillLevel.Named("MINSKILL"), lowSkillInfo.bestSkillLevel.Named("BESTSKILL"), lowSkillInfo.bestHandler.Named("HANDLER")));
					stringBuilder.AppendLine(")");
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("AlertAnimalPenNeededDescExplanation".Translate());
			return stringBuilder.ToString();
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x0025BE2C File Offset: 0x0025A02C
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x04003E46 RID: 15942
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E47 RID: 15943
		private List<Alert_AnimalPenNeeded.LowSkillInfo> lowSkillHandlers = new List<Alert_AnimalPenNeeded.LowSkillInfo>();

		// Token: 0x04003E48 RID: 15944
		private bool anyHandlers;

		// Token: 0x020025FD RID: 9725
		private struct LowSkillInfo
		{
			// Token: 0x040090DE RID: 37086
			public Pawn animal;

			// Token: 0x040090DF RID: 37087
			public int requiredSkillLevel;

			// Token: 0x040090E0 RID: 37088
			public Pawn bestHandler;

			// Token: 0x040090E1 RID: 37089
			public int bestSkillLevel;
		}
	}
}

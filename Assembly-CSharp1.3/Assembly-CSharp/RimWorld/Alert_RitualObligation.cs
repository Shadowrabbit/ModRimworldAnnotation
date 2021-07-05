using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001249 RID: 4681
	public class Alert_RitualObligation : Alert_Precept
	{
		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x0600704D RID: 28749 RVA: 0x00256455 File Offset: 0x00254655
		public override string GetJumpToTargetsText
		{
			get
			{
				return "RitualJumpToTargets".Translate();
			}
		}

		// Token: 0x0600704E RID: 28750 RVA: 0x00256466 File Offset: 0x00254666
		public Alert_RitualObligation()
		{
		}

		// Token: 0x0600704F RID: 28751 RVA: 0x00256490 File Offset: 0x00254690
		public Alert_RitualObligation(RitualObligation obligation)
		{
			this.obligation = obligation;
			string text = obligation.precept.obligationTargetFilter.LabelExtraPart(obligation);
			if (text.NullOrEmpty())
			{
				this.label = "RitualExpected".Translate(obligation.precept.LabelCap);
			}
			else
			{
				this.label = "RitualExpectedFor".Translate(obligation.precept.LabelCap) + " " + text;
			}
			this.sourcePrecept = obligation.precept;
		}

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06007050 RID: 28752 RVA: 0x00256550 File Offset: 0x00254750
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				for (int i = 0; i < Find.Maps.Count; i++)
				{
					Map map = Find.Maps[i];
					foreach (TargetInfo target in this.obligation.precept.obligationTargetFilter.GetTargets(this.obligation, map))
					{
						if (!target.Fogged)
						{
							this.targets.Add(target);
						}
					}
				}
				if (this.targets.Count == 0)
				{
					this.targets.Add(this.obligation.FirstValidTarget);
				}
				if (Find.CurrentMap != null)
				{
					MapPawns mapPawns = Find.CurrentMap.mapPawns;
					List<RitualRole> roles = this.obligation.precept.behavior.def.roles;
					for (int j = 0; j < mapPawns.FreeColonistsAndPrisonersSpawned.Count; j++)
					{
						Pawn pawn = mapPawns.FreeColonistsAndPrisonersSpawned[j];
						for (int k = 0; k < roles.Count; k++)
						{
							string text;
							if (roles[k].AppliesToPawn(pawn, out text, null, null, null))
							{
								this.targets.Add(pawn);
							}
						}
					}
					for (int l = 0; l < mapPawns.SpawnedColonyAnimals.Count; l++)
					{
						Pawn pawn2 = mapPawns.SpawnedColonyAnimals[l];
						for (int m = 0; m < roles.Count; m++)
						{
							string text;
							if (roles[m].AppliesToPawn(pawn2, out text, null, null, null))
							{
								this.targets.Add(pawn2);
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06007051 RID: 28753 RVA: 0x00256728 File Offset: 0x00254928
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			if (!this.obligation.showAlert)
			{
				return false;
			}
			if (!this.obligation.precept.activeObligations.Contains(this.obligation))
			{
				return false;
			}
			for (int i = 0; i < Find.Maps.Count; i++)
			{
				using (List<Lord>.Enumerator enumerator = Find.Maps[i].lordManager.lords.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						LordJob_Ritual lordJob_Ritual;
						if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.Ritual == this.obligation.precept)
						{
							return false;
						}
					}
				}
			}
			return new AlertReport
			{
				active = true,
				culpritsTargets = this.Targets
			};
		}

		// Token: 0x06007052 RID: 28754 RVA: 0x00256828 File Offset: 0x00254A28
		public override TaggedString GetExplanation()
		{
			Precept_Ritual precept = this.obligation.precept;
			this.sbTemp.Clear();
			this.sbTemp.AppendLine(precept.AlertDescription);
			this.sbTemp.AppendLine();
			this.sbTemp.AppendLine("RitualTargetsExplanation".Translate(precept.Named("RITUAL"), precept.obligationTargetFilter.GetTargetInfos(this.obligation).ToLineList("  - ", false)).CapitalizeFirst());
			string text = precept.RolesDescription();
			if (text != null)
			{
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine("RitualRolesExplanation".Translate(precept.Named("RITUAL"), text));
			}
			if (precept.outcomeEffect != null)
			{
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine("RitualCompletionEffects".Translate(precept.Named("RITUAL")) + ":");
				this.sbTemp.AppendLine("  - " + precept.outcomeEffect.def.Description);
				string text2 = precept.outcomeEffect.ExtraAlertParagraph(precept);
				if (!text2.NullOrEmpty())
				{
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLine(text2);
				}
			}
			if (this.obligation.expires || precept.OnGracePeriod)
			{
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine("RitualObligationDelayEffects".Translate(precept.Named("RITUAL")) + ":");
				for (int i = 0; i < RitualObligation.StageDays.Length - 1; i++)
				{
					this.sbTemp.AppendLine("  - " + "RitualObligationDelayEffectDesc".Translate(RitualObligation.StageDays[i], ThoughtDefOf.RitualDelayed.stages[i].baseMoodEffect, precept.ideo.memberName));
				}
				this.sbTemp.Append("  - " + "RitualExpirationTime".Translate(RitualObligation.StageDays[RitualObligation.StageDays.Length - 1]));
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine("RitualExpiresIn".Translate() + " " + (RitualObligation.StageDays[RitualObligation.StageDays.Length - 1] * 60000 - this.obligation.ActiveForTicks).ToStringTicksToPeriodVague(true, true) + ".");
			}
			this.unfilledRolesTemp.Clear();
			foreach (RitualRole ritualRole in precept.behavior.def.RequiredRoles())
			{
				Precept_Role precept_Role = ritualRole.FindInstance(precept.ideo);
				if (!ritualRole.substitutable && precept_Role != null && precept_Role.Active && precept_Role.ChosenPawnSingle() == null)
				{
					this.unfilledRolesTemp.Add(precept_Role.LabelCap);
				}
			}
			if (this.unfilledRolesTemp.Any<string>())
			{
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine();
				this.sbTemp.AppendLine("RitualUnfilledRoles".Translate(precept.LabelCap) + ":");
				this.sbTemp.Append(this.unfilledRolesTemp.ToLineList("  - "));
			}
			return this.sbTemp.ToString();
		}

		// Token: 0x04003E00 RID: 15872
		private RitualObligation obligation;

		// Token: 0x04003E01 RID: 15873
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E02 RID: 15874
		private StringBuilder sbTemp = new StringBuilder();

		// Token: 0x04003E03 RID: 15875
		private List<string> unfilledRolesTemp = new List<string>();
	}
}

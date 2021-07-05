using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCF RID: 3023
	public sealed class ResearchManager : IExposable
	{
		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x060046F2 RID: 18162 RVA: 0x0017747D File Offset: 0x0017567D
		public bool AnyProjectIsAvailable
		{
			get
			{
				return DefDatabase<ResearchProjectDef>.AllDefsListForReading.Find((ResearchProjectDef x) => x.CanStartNow) != null;
			}
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x001774AC File Offset: 0x001756AC
		public void ExposeData()
		{
			Scribe_Defs.Look<ResearchProjectDef>(ref this.currentProj, "currentProj");
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.progress, "progress", LookMode.Def, LookMode.Value);
			Scribe_Collections.Look<ResearchProjectDef, int>(ref this.techprints, "techprints", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.ResearchManagerPostLoadInit();
			}
			if (Scribe.mode != LoadSaveMode.Saving && this.techprints == null)
			{
				this.techprints = new Dictionary<ResearchProjectDef, int>();
			}
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00177518 File Offset: 0x00175718
		public float GetProgress(ResearchProjectDef proj)
		{
			float result;
			if (this.progress.TryGetValue(proj, out result))
			{
				return result;
			}
			this.progress.Add(proj, 0f);
			return 0f;
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x00177550 File Offset: 0x00175750
		public int GetTechprints(ResearchProjectDef proj)
		{
			int result;
			if (!this.techprints.TryGetValue(proj, out result))
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x00177570 File Offset: 0x00175770
		public void ApplyTechprint(ResearchProjectDef proj, Pawn applyingPawn)
		{
			if (!ModLister.CheckRoyalty("Techprint"))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("LetterTechprintAppliedPartIntro".Translate(proj.Named("PROJECT")));
			stringBuilder.AppendLine();
			if (proj.TechprintCount > this.GetTechprints(proj))
			{
				this.AddTechprints(proj, 1);
				if (proj.TechprintCount == this.GetTechprints(proj))
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartJustUnlocked".Translate(proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendLine("LetterTechprintAppliedPartNotUnlockedYet".Translate(this.GetTechprints(proj), proj.TechprintCount.ToString(), proj.Named("PROJECT")));
					stringBuilder.AppendLine();
				}
			}
			else if (proj.IsFinished)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyResearched".Translate(proj.Named("PROJECT")));
				stringBuilder.AppendLine();
			}
			else if (!proj.IsFinished)
			{
				float num = (proj.baseCost - this.GetProgress(proj)) * 0.5f;
				stringBuilder.AppendLine("LetterTechprintAppliedPartAlreadyUnlocked".Translate(num, proj.Named("PROJECT")));
				stringBuilder.AppendLine();
				float num2;
				if (!this.progress.TryGetValue(proj, out num2))
				{
					this.progress.Add(proj, Mathf.Min(num, proj.baseCost));
				}
				else
				{
					this.progress[proj] = Mathf.Min(num2 + num, proj.baseCost);
				}
			}
			if (applyingPawn != null)
			{
				stringBuilder.AppendLine("LetterTechprintAppliedPartExpAwarded".Translate(2000.ToString(), SkillDefOf.Intellectual.label, applyingPawn.Named("PAWN")));
				applyingPawn.skills.Learn(SkillDefOf.Intellectual, 2000f, false);
			}
			if (stringBuilder.Length > 0)
			{
				Find.LetterStack.ReceiveLetter("LetterTechprintAppliedLabel".Translate(proj.Named("PROJECT")), stringBuilder.ToString().TrimEndNewlines(), LetterDefOf.PositiveEvent, null);
			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x001777BC File Offset: 0x001759BC
		public void AddTechprints(ResearchProjectDef proj, int amount)
		{
			int num;
			if (this.techprints.TryGetValue(proj, out num))
			{
				num += amount;
				if (num > proj.TechprintCount)
				{
					num = proj.TechprintCount;
				}
				this.techprints[proj] = num;
				return;
			}
			this.techprints.Add(proj, amount);
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00177808 File Offset: 0x00175A08
		public void ResearchPerformed(float amount, Pawn researcher)
		{
			if (this.currentProj == null)
			{
				Log.Error("Researched without having an active project.");
				return;
			}
			amount *= 0.00825f;
			amount *= Find.Storyteller.difficulty.researchSpeedFactor;
			if (researcher != null && researcher.Faction != null)
			{
				amount /= this.currentProj.CostFactor(researcher.Faction.def.techLevel);
			}
			if (DebugSettings.fastResearch)
			{
				amount *= 500f;
			}
			if (researcher != null)
			{
				researcher.records.AddTo(RecordDefOf.ResearchPointsResearched, amount);
			}
			float num = this.GetProgress(this.currentProj);
			num += amount;
			this.progress[this.currentProj] = num;
			if (this.currentProj.IsFinished)
			{
				this.FinishProject(this.currentProj, true, researcher);
			}
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x001778D0 File Offset: 0x00175AD0
		public void ReapplyAllMods()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				if (researchProjectDef.IsFinished)
				{
					researchProjectDef.ReapplyAllMods();
				}
			}
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x00177924 File Offset: 0x00175B24
		public void FinishProject(ResearchProjectDef proj, bool doCompletionDialog = false, Pawn researcher = null)
		{
			if (proj.prerequisites != null)
			{
				for (int i = 0; i < proj.prerequisites.Count; i++)
				{
					if (!proj.prerequisites[i].IsFinished)
					{
						this.FinishProject(proj.prerequisites[i], false, null);
					}
				}
			}
			int num = this.GetTechprints(proj);
			if (num < proj.TechprintCount)
			{
				this.AddTechprints(proj, proj.TechprintCount - num);
			}
			this.progress[proj] = proj.baseCost;
			if (researcher != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.FinishedResearchProject, new object[]
				{
					researcher,
					this.currentProj
				});
			}
			this.ReapplyAllMods();
			if (doCompletionDialog)
			{
				DiaNode diaNode = new DiaNode("ResearchFinished".Translate(this.currentProj.LabelCap) + "\n\n" + this.currentProj.description);
				diaNode.options.Add(DiaOption.DefaultOK);
				DiaOption diaOption = new DiaOption("ResearchScreen".Translate());
				diaOption.resolveTree = true;
				diaOption.action = delegate()
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Research, true);
				};
				diaNode.options.Add(diaOption);
				Find.WindowStack.Add(new Dialog_NodeTree(diaNode, true, false, null));
			}
			if (!proj.discoveredLetterTitle.NullOrEmpty() && Find.Storyteller.difficulty.AllowedBy(proj.discoveredLetterDisabledWhen))
			{
				Find.LetterStack.ReceiveLetter(proj.discoveredLetterTitle, proj.discoveredLetterText, LetterDefOf.NeutralEvent, null);
			}
			if (this.currentProj == proj)
			{
				this.currentProj = null;
			}
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00177AE3 File Offset: 0x00175CE3
		public void ResetAllProgress()
		{
			this.progress.Clear();
			this.currentProj = null;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00177AF8 File Offset: 0x00175CF8
		public void DebugSetAllProjectsFinished()
		{
			this.progress.Clear();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				this.progress.Add(researchProjectDef, researchProjectDef.baseCost);
			}
			this.ReapplyAllMods();
		}

		// Token: 0x04002B7D RID: 11133
		public ResearchProjectDef currentProj;

		// Token: 0x04002B7E RID: 11134
		private Dictionary<ResearchProjectDef, float> progress = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04002B7F RID: 11135
		private Dictionary<ResearchProjectDef, int> techprints = new Dictionary<ResearchProjectDef, int>();

		// Token: 0x04002B80 RID: 11136
		public const float ResearchPointsPerWorkTick = 0.00825f;

		// Token: 0x04002B81 RID: 11137
		public const int IntellectualExpPerTechprint = 2000;
	}
}

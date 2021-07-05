using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EFD RID: 3837
	[StaticConstructorOnStartup]
	public class Precept_Ritual : Precept
	{
		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06005B75 RID: 23413 RVA: 0x001F9782 File Offset: 0x001F7982
		public float RepeatQualityPenalty
		{
			get
			{
				return Mathf.Lerp(-0.95f, 0f, this.RepeatPenaltyProgress);
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06005B76 RID: 23414 RVA: 0x001F979C File Offset: 0x001F799C
		public override string UIInfoFirstLine
		{
			get
			{
				if (!this.ShortDescOverrideCap.NullOrEmpty())
				{
					return this.ShortDescOverrideCap;
				}
				return this.def.LabelCap.Resolve();
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06005B77 RID: 23415 RVA: 0x001F9183 File Offset: 0x001F7383
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06005B78 RID: 23416 RVA: 0x001F97D0 File Offset: 0x001F79D0
		public bool RepeatPenaltyActive
		{
			get
			{
				return this.isAnytime && this.lastFinishedTick != -1 && this.def.useRepeatPenalty && this.TicksSinceLastPerformed < 1200000;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06005B79 RID: 23417 RVA: 0x001F97FF File Offset: 0x001F79FF
		public int TicksSinceLastPerformed
		{
			get
			{
				return GenTicks.TicksGame - this.lastFinishedTick;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06005B7A RID: 23418 RVA: 0x001F980D File Offset: 0x001F7A0D
		public float RepeatPenaltyProgress
		{
			get
			{
				return (float)this.TicksSinceLastPerformed / 1200000f;
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x001F981C File Offset: 0x001F7A1C
		public string RepeatPenaltyTimeLeft
		{
			get
			{
				return (1200000 - this.TicksSinceLastPerformed).ToStringTicksToPeriod(true, false, true, true);
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06005B7C RID: 23420 RVA: 0x001F9833 File Offset: 0x001F7A33
		public bool SupportsAttachableOutcomeEffect
		{
			get
			{
				RitualOutcomeEffectWorker ritualOutcomeEffectWorker = this.outcomeEffect;
				return ritualOutcomeEffectWorker != null && ritualOutcomeEffectWorker.SupportsAttachableOutcomeEffect;
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06005B7D RID: 23421 RVA: 0x001F9846 File Offset: 0x001F7A46
		public bool OnGracePeriod
		{
			get
			{
				return this.gracePeriodTicksSinceGameStarted > 0 && GenTicks.TicksGame < this.gracePeriodTicksSinceGameStarted;
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06005B7E RID: 23422 RVA: 0x001F9860 File Offset: 0x001F7A60
		public string ShortDescOverrideCap
		{
			get
			{
				if (this.shortDescOverrideCap.NullOrEmpty() && this.shortDescOverride != null)
				{
					this.shortDescOverrideCap = this.shortDescOverride.CapitalizeFirst();
				}
				return this.shortDescOverrideCap;
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06005B7F RID: 23423 RVA: 0x001F9890 File Offset: 0x001F7A90
		public override Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					string text = this.iconPathOverride ?? this.def.iconPath;
					if (text != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(text, true);
					}
					else
					{
						this.icon = this.ideo.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x001F98EC File Offset: 0x001F7AEC
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = ((!this.def.tipLabelOverride.NullOrEmpty()) ? this.def.tipLabelOverride : base.LabelCap);
					if (!this.def.LabelCap.NullOrEmpty() && this.def.LabelCap != this.tipLabelCached)
					{
						this.tipLabelCached += "\n" + this.def.LabelCap;
					}
					if (this.isAnytime)
					{
						this.tipLabelCached += "\n" + "RitualStartAnyTime".Translate();
					}
					else
					{
						RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
						if (ritualObligationTrigger_Date != null)
						{
							this.tipLabelCached = this.tipLabelCached + "\n" + ritualObligationTrigger_Date.DateString;
						}
						else
						{
							this.tipLabelCached += "\n" + "RitualStartEvent".Translate();
						}
					}
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06005B81 RID: 23425 RVA: 0x001F9A24 File Offset: 0x001F7C24
		public string AlertDescription
		{
			get
			{
				int num = 0;
				using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Ideo == this.ideo)
						{
							num++;
						}
					}
				}
				RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
				string value = (num > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, num) : this.ideo.memberName;
				TaggedString taggedString = this.ritualExpectedDesc.Formatted(this.Named("RITUAL"), this.ideo.memberName.Named("MEMBER"), this.ideo.MemberNamePlural.Named("MEMBERS"), this.ideo.name.Named("IDEO"), (ritualObligationTrigger_Date != null) ? ritualObligationTrigger_Date.DateString.Named("DATE") : "");
				TaggedString t = this.ritualExpectedPostfix.Formatted(num, value);
				if (!t.NullOrEmpty())
				{
					if (!taggedString.NullOrEmpty())
					{
						taggedString += " ";
					}
					taggedString += t;
				}
				return taggedString;
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06005B82 RID: 23426 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06005B83 RID: 23427 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005B84 RID: 23428 RVA: 0x001F9B78 File Offset: 0x001F7D78
		public override void ClearTipCache()
		{
			base.ClearTipCache();
			this.tipLabelCached = null;
		}

		// Token: 0x06005B85 RID: 23429 RVA: 0x001F9B87 File Offset: 0x001F7D87
		public override string GetTip()
		{
			return this.TipMainPart() + this.TipExtraPart();
		}

		// Token: 0x06005B86 RID: 23430 RVA: 0x001F9B9C File Offset: 0x001F7D9C
		public string TipMainPart()
		{
			Precept.tmpCompsDesc.Clear();
			if (this.RepeatPenaltyActive)
			{
				float value = (float)Mathf.RoundToInt(this.RepeatPenaltyProgress * 20f * 10f) / 10f;
				float value2 = (float)Mathf.RoundToInt((1f - this.RepeatPenaltyProgress) * 20f * 10f) / 10f;
				Precept.tmpCompsDesc.AppendLine(base.ColorizeWarning("RitualRepeatPenaltyTip".Translate(20, value, this.RepeatQualityPenalty.ToStringPercent(), value2)));
				Precept.tmpCompsDesc.AppendLine();
			}
			if (!base.Description.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Append(base.Description);
			}
			if (this.outcomeEffect != null)
			{
				StringBuilder stringBuilder = new StringBuilder(this.outcomeEffect.def.Description);
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						stringBuilder.Append(" " + str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendInNewLine(this.attachableOutcomeEffect.DescriptionForRitualValidated(this));
				}
				if (stringBuilder.Length > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(stringBuilder.ToString());
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x001F9D7C File Offset: 0x001F7F7C
		public string TipExtraPart()
		{
			this.tmpTipExtraSb.Clear();
			if (this.obligationTargetFilter.GetTargetInfos(null).Any<string>())
			{
				this.tmpTipExtraSb.AppendLine();
				this.tmpTipExtraSb.AppendInNewLine(base.ColorizeDescTitle("RitualFocusObjects".Translate() + ":"));
				this.tmpTipExtraSb.AppendInNewLine(this.obligationTargetFilter.GetTargetInfos(null).ToLineList("  - ", false));
			}
			string text = this.RolesDescription();
			if (text != null)
			{
				this.tmpTipExtraSb.Append(base.ColorizeDescTitle("\n\n" + "RitualParticipatingRoles".Translate() + ":" + "\n"));
				this.tmpTipExtraSb.Append(text);
			}
			return this.tmpTipExtraSb.ToString();
		}

		// Token: 0x06005B88 RID: 23432 RVA: 0x001F9E58 File Offset: 0x001F8058
		public string RolesDescription()
		{
			if (this.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RitualRole ritualRole in this.behavior.def.roles)
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				stringBuilder.AppendInNewLine("  - " + ((precept_Role != null) ? precept_Role.LabelCap : ritualRole.LabelCap) + " (" + ((ritualRole.required && !ritualRole.substitutable) ? "Required".Translate().ToLower() : "Optional".Translate()) + ")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x001F9F4C File Offset: 0x001F814C
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			base.RegenerateName();
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x001F9F5C File Offset: 0x001F815C
		public override void PostMake()
		{
			this.sourcePattern = this.def.ritualPatternBase;
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x001F9F70 File Offset: 0x001F8170
		public override string GenerateNameRaw()
		{
			if (this.nameMaker == null)
			{
				return this.def.label;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (this.ideo.culture.festivalNameMaker != null)
			{
				request.Includes.Add(this.ideo.culture.festivalNameMaker);
			}
			List<string> list = new List<string>();
			string text = GrammarResolver.Resolve("r_ritualName", request, null, false, null, null, list, false);
			this.usesDefiniteArticle = !list.Contains("noArticle");
			if (this.def.capitalizeAsTitle)
			{
				text = GenText.CapitalizeAsTitle(text);
			}
			return text;
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x001FA024 File Offset: 0x001F8224
		public void AddObligation(RitualObligation obligation)
		{
			if (!this.ideo.ObligationsActive)
			{
				return;
			}
			if (!ModLister.CheckIdeology("Ritual obligations"))
			{
				return;
			}
			foreach (RitualRole ritualRole in this.behavior.def.RequiredRoles())
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				if (precept_Role != null && !precept_Role.Active)
				{
					RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
					if (ritualObligationTrigger_Date != null)
					{
						Find.LetterStack.ReceiveLetter("LetterObligationRoleInactive".Translate(base.LabelCap), "LetterObligationRoleInactiveDateDesc".Translate(ritualObligationTrigger_Date.DateString, base.LabelCap, this.ideo.memberName, precept_Role.Named("ROLE")), LetterDefOf.NeutralEvent, null);
					}
					return;
				}
			}
			if (this.activeObligations == null)
			{
				this.activeObligations = new List<RitualObligation>();
			}
			if (obligation.onlyForPawns == null)
			{
				obligation.onlyForPawns = new List<Pawn>();
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.Ideo == this.ideo)
				{
					obligation.onlyForPawns.Add(pawn);
				}
			}
			this.activeObligations.Add(obligation);
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x001FA1B0 File Offset: 0x001F83B0
		public bool ShouldShowGizmo(TargetInfo target)
		{
			if (this.activeObligations != null)
			{
				for (int i = 0; i < this.activeObligations.Count; i++)
				{
					if (this.obligationTargetFilter.CanUseTarget(target, this.activeObligations[i]).ShouldShowGizmo)
					{
						return true;
					}
				}
			}
			return (this.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date) != null || this.isAnytime) && this.obligationTargetFilter.CanUseTarget(target, null).ShouldShowGizmo;
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x001FA24A File Offset: 0x001F844A
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			return this.obligationTargetFilter.CanUseTarget(target, obligation);
		}

		// Token: 0x06005B8F RID: 23439 RVA: 0x001FA25C File Offset: 0x001F845C
		public override void Tick()
		{
			base.Tick();
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				for (int i = this.activeObligations.Count - 1; i >= 0; i--)
				{
					if (!this.activeObligations[i].StillValid || !this.obligationTargetFilter.ObligationTargetsValid(this.activeObligations[i]))
					{
						this.activeObligations.RemoveAt(i);
					}
				}
			}
			for (int j = 0; j < this.obligationTriggers.Count; j++)
			{
				try
				{
					this.obligationTriggers[j].Tick();
				}
				catch (Exception arg)
				{
					Log.Error("Error while ticking a ritual obligation trigger: " + arg);
				}
			}
		}

		// Token: 0x06005B90 RID: 23440 RVA: 0x001FA31C File Offset: 0x001F851C
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x06005B91 RID: 23441 RVA: 0x001FA32C File Offset: 0x001F852C
		public override IEnumerable<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			if (!this.OnGracePeriod && !this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				int num;
				for (int o = 0; o < this.activeObligations.Count; o = num + 1)
				{
					RitualObligation obligation = this.activeObligations[o];
					if (obligation.expires && !activeThoughts.Any(delegate(Thought_Situational t)
					{
						Thought_DelayedRitual thought_DelayedRitual2;
						return (thought_DelayedRitual2 = (t as Thought_DelayedRitual)) != null && thought_DelayedRitual2.obligation == obligation;
					}) && (obligation.onlyForPawns == null || obligation.onlyForPawns.Contains(pawn)))
					{
						Thought_DelayedRitual thought_DelayedRitual = (Thought_DelayedRitual)ThoughtMaker.MakeThought(ThoughtDefOf.RitualDelayed);
						if (thought_DelayedRitual != null)
						{
							thought_DelayedRitual.pawn = pawn;
							thought_DelayedRitual.obligation = obligation;
							thought_DelayedRitual.sourcePrecept = this;
							yield return thought_DelayedRitual;
						}
					}
					num = o;
				}
			}
			yield break;
		}

		// Token: 0x06005B92 RID: 23442 RVA: 0x001FA34C File Offset: 0x001F854C
		public override void Notify_MemberDied(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberDied(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberDied(p);
			}
		}

		// Token: 0x06005B93 RID: 23443 RVA: 0x001FA3B0 File Offset: 0x001F85B0
		public override void Notify_MemberCorpseDestroyed(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberCorpseDestroyed(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberCorpseDestroyed(p);
			}
		}

		// Token: 0x06005B94 RID: 23444 RVA: 0x001FA414 File Offset: 0x001F8614
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
		}

		// Token: 0x06005B95 RID: 23445 RVA: 0x001FA47C File Offset: 0x001F867C
		public override void Notify_MemberSpawned(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberSpawned(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberSpawned(pawn);
			}
		}

		// Token: 0x06005B96 RID: 23446 RVA: 0x001FA4E0 File Offset: 0x001F86E0
		public override void Notify_MemberLost(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberLost(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberLost(pawn);
			}
		}

		// Token: 0x06005B97 RID: 23447 RVA: 0x001FA544 File Offset: 0x001F8744
		public override void Notify_MemberGenerated(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberGenerated(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGenerated(pawn);
			}
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x001FA5A8 File Offset: 0x001F87A8
		public override void Notify_GameStarted()
		{
			base.Notify_GameStarted();
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_GameStarted();
			}
		}

		// Token: 0x06005B99 RID: 23449 RVA: 0x001FA600 File Offset: 0x001F8800
		public IEnumerable<Gizmo> GetGizmoFor(TargetInfo t)
		{
			Precept_Ritual.<>c__DisplayClass87_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.t = t;
			using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(CS$<>8__locals1.t.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Ritual == this)
					{
						yield break;
					}
				}
			}
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				foreach (RitualObligation obligation in this.activeObligations)
				{
					Command_Ritual command_Ritual = this.<GetGizmoFor>g__CommandForObligation|87_0(obligation, ref CS$<>8__locals1);
					if (command_Ritual != null)
					{
						yield return command_Ritual;
						if (this.mergeGizmosForObligations)
						{
							break;
						}
					}
				}
				List<RitualObligation>.Enumerator enumerator2 = default(List<RitualObligation>.Enumerator);
			}
			else if (this.isAnytime)
			{
				RitualTargetUseReport ritualTargetUseReport = this.CanUseTarget(CS$<>8__locals1.t, null);
				Command_Ritual command_Ritual2 = new Command_Ritual(this, CS$<>8__locals1.t, null);
				if (!ritualTargetUseReport.failReason.NullOrEmpty())
				{
					command_Ritual2.disabledReason = ritualTargetUseReport.failReason;
					command_Ritual2.disabled = true;
				}
				yield return command_Ritual2;
			}
			else
			{
				List<RitualObligationTrigger> list = this.obligationTriggers;
				RitualObligationTrigger ritualObligationTrigger;
				if (list == null)
				{
					ritualObligationTrigger = null;
				}
				else
				{
					ritualObligationTrigger = list.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
				}
				RitualObligationTrigger ritualObligationTrigger2 = ritualObligationTrigger;
				if (ritualObligationTrigger2 != null)
				{
					RitualObligationTrigger_Date ritualObligationTrigger_Date = (RitualObligationTrigger_Date)ritualObligationTrigger2;
					int num = ritualObligationTrigger_Date.OccursOnTick();
					int num2 = ritualObligationTrigger_Date.CurrentTickRelative();
					if (num2 > num)
					{
						num += 3600000;
					}
					yield return new Command_Ritual(this, CS$<>8__locals1.t, null)
					{
						disabledReason = "DateRitualNoObligation".Translate(base.LabelCap, (num - num2).ToStringTicksToPeriod(true, false, true, true), ritualObligationTrigger_Date.DateString),
						disabled = true
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06005B9A RID: 23450 RVA: 0x001FA618 File Offset: 0x001F8818
		public void ShowRitualBeginWindow(TargetInfo targetInfo, RitualObligation forObligation = null, Pawn selectedPawn = null)
		{
			RitualObligation ritualObligation = forObligation;
			if (ritualObligation == null)
			{
				ritualObligation = ((this.activeObligations != null) ? this.activeObligations.FirstOrDefault((RitualObligation o) => this.obligationTargetFilter.CanUseTarget(targetInfo, o).canUse) : null);
			}
			Window ritualBeginWindow = this.GetRitualBeginWindow(targetInfo, ritualObligation, null, null, null, selectedPawn);
			if (ritualBeginWindow != null)
			{
				Find.WindowStack.Add(ritualBeginWindow);
			}
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x001FA684 File Offset: 0x001F8884
		public Window GetRitualBeginWindow(TargetInfo targetInfo, RitualObligation obligation = null, Action onConfirm = null, Pawn organizer = null, Dictionary<string, Pawn> forcedForRole = null, Pawn selectedPawn = null)
		{
			string text = this.behavior.CanStartRitualNow(targetInfo, this, selectedPawn, forcedForRole);
			if (!string.IsNullOrEmpty(text))
			{
				Messages.Message(text, targetInfo, MessageTypeDefOf.RejectInput, false);
			}
			List<string> list = new List<string>();
			if (this.outcomeEffect != null)
			{
				if (!this.outcomeEffect.def.extraInfoLines.NullOrEmpty<string>())
				{
					foreach (string item in this.outcomeEffect.def.extraInfoLines)
					{
						list.Add(item);
					}
				}
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						list.Add(str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					list.Add(this.attachableOutcomeEffect.DescriptionForRitualValidated(this, targetInfo.Map));
				}
			}
			string header = "ChooseParticipants".Translate(this.Named("RITUAL"));
			string ritualLabel = this.Label.CapitalizeFirst();
			TargetInfo targetInfo2 = targetInfo;
			Map map = targetInfo.Map;
			Dialog_BeginRitual.ActionCallback action = delegate(RitualRoleAssignments assignments)
			{
				this.behavior.TryExecuteOn(targetInfo, organizer, this, obligation, assignments, true);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 != null)
				{
					onConfirm2();
				}
				return true;
			};
			Pawn organizer2 = organizer;
			RitualObligation obligation2 = obligation;
			Func<Pawn, bool, bool, bool> filter = (Pawn pawn, bool voluntary, bool allowOtherIdeos) => pawn.GetLord() == null && (!pawn.RaceProps.Animal || this.behavior.def.roles.Any(delegate(RitualRole r)
			{
				string text2;
				return r.AppliesToPawn(pawn, out text2, null, null, null);
			})) && (!this.ritualOnlyForIdeoMembers || (pawn.Ideo == this.ideo || !voluntary || allowOtherIdeos) || pawn.IsPrisonerOfColony || pawn.RaceProps.Animal || (!forcedForRole.NullOrEmpty<string, Pawn>() && forcedForRole.ContainsValue(pawn)));
			string confirmText = "Begin".Translate();
			object requiredPawns;
			if (organizer == null)
			{
				requiredPawns = null;
			}
			else
			{
				(requiredPawns = new List<Pawn>()).Add(organizer);
			}
			List<string> extraInfoText = list;
			return new Dialog_BeginRitual(header, ritualLabel, this, targetInfo2, map, action, organizer2, obligation2, filter, confirmText, requiredPawns, forcedForRole, null, null, extraInfoText, selectedPawn);
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x001FA8C0 File Offset: 0x001F8AC0
		public override IEnumerable<Alert> GetAlerts()
		{
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				int num;
				for (int i = 0; i < this.activeObligations.Count; i = num + 1)
				{
					yield return this.activeObligations[i].AlertCached;
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x001FA8D0 File Offset: 0x001F8AD0
		public override void Notify_IdeoNotPrimaryAnymore(Ideo newIdeo)
		{
			base.Notify_IdeoNotPrimaryAnymore(newIdeo);
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				this.activeObligations.Clear();
			}
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x001FA8F1 File Offset: 0x001F8AF1
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x001FA91C File Offset: 0x001F8B1C
		public override void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			base.DrawPreceptBox(preceptBox, editMode, forceHighlight);
			Rect position = new Rect(preceptBox.xMax - (float)this.DateIconSize.x, preceptBox.yMin, (float)this.DateIconSize.x, (float)this.DateIconSize.z);
			if (this.isAnytime)
			{
				GUI.DrawTexture(position, Precept_Ritual.AnytimeRitualTex);
				return;
			}
			if (this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
			{
				GUI.DrawTexture(position, Precept_Ritual.DateRitualTex);
				return;
			}
			GUI.DrawTexture(position, Precept_Ritual.EventRitualTex);
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x001F6637 File Offset: 0x001F4837
		public override void Notify_RecachedPrecepts()
		{
			this.tipCached = null;
		}

		// Token: 0x06005BA1 RID: 23457 RVA: 0x001FA9AC File Offset: 0x001F8BAC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.nameMaker, "nameMaker");
			Scribe_Values.Look<bool>(ref this.ritualOnlyForIdeoMembers, "ritualOnlyForIdeoMembers", false, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedDesc, "ritualExpectedDesc", null, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedPostfix, "ritualExpectedPostfix", null, false);
			Scribe_Values.Look<string>(ref this.shortDescOverride, "shortDescOverride", null, false);
			Scribe_Values.Look<string>(ref this.iconPathOverride, "iconPathOverride", null, false);
			Scribe_Values.Look<string>(ref this.patternGroupTag, "patternGroupTag", null, false);
			Scribe_Values.Look<TechLevel>(ref this.minTechLevel, "minTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<TechLevel>(ref this.maxTechLevel, "maxTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<bool>(ref this.isAnytime, "isAnytime", false, false);
			Scribe_Values.Look<bool>(ref this.canBeAnytime, "canBeAnytime", false, false);
			Scribe_Values.Look<int>(ref this.lastFinishedTick, "lastFinishedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.playsIdeoMusic, "playsIdeoMusic", false, false);
			Scribe_Values.Look<string>(ref this.ritualExplanation, "ritualExplanation", null, false);
			Scribe_Values.Look<bool>(ref this.mergeGizmosForObligations, "mergeGizmosForObligations", false, false);
			Scribe_Values.Look<bool>(ref this.generatedAttachedReward, "generatedAttachedReward", false, false);
			Scribe_Values.Look<int>(ref this.gracePeriodTicksSinceGameStarted, "gracePeriodTicksSinceGameStarted", 0, false);
			Scribe_Defs.Look<RitualPatternDef>(ref this.sourcePattern, "sourcePattern");
			Scribe_Deep.Look<RitualObligationTargetFilter>(ref this.obligationTargetFilter, "obligationTargetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualTargetFilter>(ref this.targetFilter, "targetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualBehaviorWorker>(ref this.behavior, "behavior", Array.Empty<object>());
			Scribe_Collections.Look<RitualObligationTrigger>(ref this.obligationTriggers, "triggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualObligation>(ref this.activeObligations, "activeObligations", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<RitualAttachableOutcomeEffectDef>(ref this.attachableOutcomeEffect, "attachableOutcomeEffect");
			Scribe_Deep.Look<RitualOutcomeEffectWorker>(ref this.outcomeEffect, "outcomeEffect", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.activeObligations.NullOrEmpty<RitualObligation>())
				{
					if (this.activeObligations.RemoveAll((RitualObligation t) => t == null) != 0)
					{
						Log.Warning("Some activeObligations were null.");
					}
					foreach (RitualObligation ritualObligation in this.activeObligations)
					{
						ritualObligation.precept = this;
					}
				}
				if (!this.obligationTriggers.NullOrEmpty<RitualObligationTrigger>())
				{
					if (this.obligationTriggers.RemoveAll((RitualObligationTrigger t) => t == null) != 0)
					{
						Log.Warning("Some obligationTriggers were null.");
					}
					foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
					{
						ritualObligationTrigger.ritual = this;
					}
				}
				if (this.obligationTargetFilter != null)
				{
					this.obligationTargetFilter.parent = this;
				}
				if (this.attachableOutcomeEffect == null && !this.generatedAttachedReward && this.SupportsAttachableOutcomeEffect)
				{
					this.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where d.CanAttachToRitual(this)
					select d).RandomElementWithFallback(null);
					this.generatedAttachedReward = true;
				}
				if (!this.outcomeEffect.SupportsAttachableOutcomeEffect)
				{
					this.attachableOutcomeEffect = null;
				}
				if (this.behavior.def.defName == "DancePartyTech")
				{
					this.behavior = this.behavior.def.GetInstance();
				}
				if (this.sourcePattern != null)
				{
					this.gracePeriodTicksSinceGameStarted = this.sourcePattern.gracePeriodTicksSinceGameStarted;
					this.shortDescOverride = this.sourcePattern.shortDescOverride;
				}
			}
		}

		// Token: 0x06005BA4 RID: 23460 RVA: 0x001FADD4 File Offset: 0x001F8FD4
		[CompilerGenerated]
		private Command_Ritual <GetGizmoFor>g__CommandForObligation|87_0(RitualObligation obligation, ref Precept_Ritual.<>c__DisplayClass87_0 A_2)
		{
			RitualTargetUseReport ritualTargetUseReport = this.CanUseTarget(A_2.t, obligation);
			if (ritualTargetUseReport.canUse)
			{
				return new Command_Ritual(this, A_2.t, obligation);
			}
			if (!ritualTargetUseReport.failReason.NullOrEmpty())
			{
				return new Command_Ritual(this, A_2.t, obligation)
				{
					disabledReason = ritualTargetUseReport.failReason,
					disabled = true
				};
			}
			return null;
		}

		// Token: 0x0400353F RID: 13631
		public List<RitualObligation> activeObligations;

		// Token: 0x04003540 RID: 13632
		public RulePackDef nameMaker;

		// Token: 0x04003541 RID: 13633
		public List<RitualObligationTrigger> obligationTriggers = new List<RitualObligationTrigger>();

		// Token: 0x04003542 RID: 13634
		public RitualOutcomeEffectWorker outcomeEffect;

		// Token: 0x04003543 RID: 13635
		public RitualAttachableOutcomeEffectDef attachableOutcomeEffect;

		// Token: 0x04003544 RID: 13636
		public RitualObligationTargetFilter obligationTargetFilter;

		// Token: 0x04003545 RID: 13637
		public RitualTargetFilter targetFilter;

		// Token: 0x04003546 RID: 13638
		public RitualBehaviorWorker behavior;

		// Token: 0x04003547 RID: 13639
		public bool ritualOnlyForIdeoMembers;

		// Token: 0x04003548 RID: 13640
		public bool isAnytime;

		// Token: 0x04003549 RID: 13641
		public bool canBeAnytime;

		// Token: 0x0400354A RID: 13642
		public bool playsIdeoMusic;

		// Token: 0x0400354B RID: 13643
		public bool mergeGizmosForObligations;

		// Token: 0x0400354C RID: 13644
		public int lastFinishedTick = -1;

		// Token: 0x0400354D RID: 13645
		public int gracePeriodTicksSinceGameStarted;

		// Token: 0x0400354E RID: 13646
		public string ritualExpectedDesc;

		// Token: 0x0400354F RID: 13647
		public string ritualExpectedPostfix;

		// Token: 0x04003550 RID: 13648
		public string shortDescOverride;

		// Token: 0x04003551 RID: 13649
		public string ritualExplanation;

		// Token: 0x04003552 RID: 13650
		public string iconPathOverride;

		// Token: 0x04003553 RID: 13651
		public string patternGroupTag;

		// Token: 0x04003554 RID: 13652
		public TechLevel minTechLevel;

		// Token: 0x04003555 RID: 13653
		public TechLevel maxTechLevel;

		// Token: 0x04003556 RID: 13654
		public bool generatedAttachedReward;

		// Token: 0x04003557 RID: 13655
		public RitualPatternDef sourcePattern;

		// Token: 0x04003558 RID: 13656
		private string shortDescOverrideCap;

		// Token: 0x04003559 RID: 13657
		public const float RepeatQualityPenaltyMax = -0.95f;

		// Token: 0x0400355A RID: 13658
		public const int RepeatPenaltyDurationDays = 20;

		// Token: 0x0400355B RID: 13659
		public const float LowQualityWarningThreshold = 0.25f;

		// Token: 0x0400355C RID: 13660
		private readonly IntVec2 DateIconSize = new IntVec2(28, 28);

		// Token: 0x0400355D RID: 13661
		public static readonly Texture2D DateRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/DateRitual", true);

		// Token: 0x0400355E RID: 13662
		public static readonly Texture2D AnytimeRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/AnytimeRitual", true);

		// Token: 0x0400355F RID: 13663
		public static readonly Texture2D EventRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/EventRitual", true);

		// Token: 0x04003560 RID: 13664
		private Texture2D icon;

		// Token: 0x04003561 RID: 13665
		private string tipLabelCached;

		// Token: 0x04003562 RID: 13666
		private StringBuilder tmpTipExtraSb = new StringBuilder();
	}
}

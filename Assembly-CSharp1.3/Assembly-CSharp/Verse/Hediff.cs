using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200027A RID: 634
	public class Hediff : IExposable, ILoadReferenceable
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x000680EC File Offset: 0x000662EC
		public virtual string LabelBase
		{
			get
			{
				HediffStage curStage = this.CurStage;
				return ((curStage != null) ? curStage.overrideLabel : null) ?? this.def.label;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x0006810F File Offset: 0x0006630F
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00068124 File Offset: 0x00066324
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00068162 File Offset: 0x00066362
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00068175 File Offset: 0x00066375
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00068182 File Offset: 0x00066382
		public virtual string LabelInBrackets
		{
			get
			{
				if (this.CurStage != null && !this.CurStage.label.NullOrEmpty())
				{
					return this.CurStage.label;
				}
				return null;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000681AB File Offset: 0x000663AB
		public virtual string SeverityLabel
		{
			get
			{
				if (this.def.lethalSeverity > 0f)
				{
					return (this.Severity / this.def.lethalSeverity).ToStringPercent();
				}
				return null;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x000681D8 File Offset: 0x000663D8
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x000681E8 File Offset: 0x000663E8
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay)
					{
						stringBuilder.AppendLine(statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x00068268 File Offset: 0x00066468
		public virtual HediffStage CurStage
		{
			get
			{
				if (!this.def.stages.NullOrEmpty<HediffStage>())
				{
					return this.def.stages[this.CurStageIndex];
				}
				return null;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x00068294 File Offset: 0x00066494
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x000682A6 File Offset: 0x000664A6
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x000682CC File Offset: 0x000664CC
		public virtual float BleedRateScaled
		{
			get
			{
				return this.BleedRate / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x000682E0 File Offset: 0x000664E0
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x000682EF File Offset: 0x000664EF
		public virtual float PainOffset
		{
			get
			{
				if (this.CurStage != null && !this.causesNoPain)
				{
					return this.CurStage.painOffset;
				}
				return 0f;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x00068312 File Offset: 0x00066512
		public virtual float PainFactor
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.painFactor;
				}
				return 1f;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x0006832D File Offset: 0x0006652D
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				if (this.CurStage != null)
				{
					return this.CurStage.capMods;
				}
				return null;
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000682C5 File Offset: 0x000664C5
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00068344 File Offset: 0x00066544
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000683B1 File Offset: 0x000665B1
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x000683B8 File Offset: 0x000665B8
		public virtual int CurStageIndex
		{
			get
			{
				if (this.def.stages == null)
				{
					return 0;
				}
				List<HediffStage> stages = this.def.stages;
				float severity = this.Severity;
				for (int i = stages.Count - 1; i >= 0; i--)
				{
					if (severity >= stages[i].minSeverity)
					{
						return i;
					}
				}
				return 0;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0006840C File Offset: 0x0006660C
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x00068414 File Offset: 0x00066614
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.def.lethalSeverity > 0f && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00068524 File Offset: 0x00066724
		// (set) Token: 0x060011E9 RID: 4585 RVA: 0x0006852C File Offset: 0x0006672C
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && this.part != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x060011EA RID: 4586 RVA: 0x00068550 File Offset: 0x00066750
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent())
			{
				return false;
			}
			if (!ignoreTimer)
			{
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060011EB RID: 4587 RVA: 0x000685AC File Offset: 0x000667AC
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (target == null || !Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.source, "source");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x000686DC File Offset: 0x000668DC
		public virtual void Tick()
		{
			this.ageTicks++;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, null, false, false, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					this.TryDoRandomMentalBreak();
				}
				if (curStage.vomitMtbDays > 0f && this.pawn.IsHashIntervalTick(600) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake() && this.pawn.RaceProps.IsFlesh)
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
				}
				Thought_Memory th;
				if (curStage.forgetMemoryThoughtMtbDays > 0f && this.pawn.needs != null && this.pawn.needs.mood != null && this.pawn.IsHashIntervalTick(400) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f) && this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
				{
					this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f && this.pawn.IsHashIntervalTick(200) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
				{
					this.pawn.Kill(null, this);
				}
			}
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00068AE8 File Offset: 0x00066CE8
		private void TryDoRandomMentalBreak()
		{
			HediffStage curStage = this.CurStage;
			if (curStage == null)
			{
				return;
			}
			MentalBreakDef mentalBreakDef;
			if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where x.Worker.BreakCanOccur(this.pawn) && (curStage.allowedMentalBreakIntensities == null || curStage.allowedMentalBreakIntensities.Contains(x.intensity))
			select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn, false), out mentalBreakDef))
			{
				TaggedString t = "MentalStateReason_Hediff".Translate(this.Label);
				if (!curStage.mentalBreakExplanation.NullOrEmpty())
				{
					t += "\n\n" + curStage.mentalBreakExplanation.Formatted(this.pawn.Named("PAWN"));
				}
				mentalBreakDef.Worker.TryStart(this.pawn, t.Resolve(), false);
			}
		}

		// Token: 0x060011EE RID: 4590 RVA: 0x00068BB5 File Offset: 0x00066DB5
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
		}

		// Token: 0x060011EF RID: 4591 RVA: 0x00068BEB File Offset: 0x00066DEB
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			if (!this.def.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x060011F0 RID: 4592 RVA: 0x00068C0F File Offset: 0x00066E0F
		public virtual void PostRemoved()
		{
			if ((this.def.causesNeed != null || !this.def.disablesNeeds.NullOrEmpty<NeedDef>()) && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostTick()
		{
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x00068C4D File Offset: 0x00066E4D
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00068C77 File Offset: 0x00066E77
		public virtual bool TryMergeWith(Hediff other)
		{
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				return false;
			}
			this.Severity += other.Severity;
			this.ageTicks = 0;
			return true;
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00068CB8 File Offset: 0x00066EB8
		public virtual bool CauseDeathNow()
		{
			if (this.def.lethalSeverity >= 0f)
			{
				bool flag = this.Severity >= this.def.lethalSeverity;
				if (flag && DebugViewSettings.logCauseOfDeath)
				{
					Log.Message(string.Concat(new object[]
					{
						"CauseOfDeath: lethal severity exceeded ",
						this.Severity,
						" >= ",
						this.def.lethalSeverity
					}));
				}
				return flag;
			}
			return false;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00068D3C File Offset: 0x00066F3C
		public virtual string GetTooltip(Pawn pawn, bool showHediffsDebugInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (string.IsNullOrWhiteSpace(this.def.overrideTooltip))
			{
				HediffStage curStage = this.CurStage;
				if (string.IsNullOrWhiteSpace((curStage != null) ? curStage.overrideTooltip : null))
				{
					string severityLabel = this.SeverityLabel;
					bool flag = showHediffsDebugInfo && !this.DebugString().NullOrEmpty();
					if (!this.Label.NullOrEmpty() || !severityLabel.NullOrEmpty() || !this.CapMods.NullOrEmpty<PawnCapacityModifier>() || flag)
					{
						stringBuilder.Append(this.LabelCap);
						if (!severityLabel.NullOrEmpty())
						{
							stringBuilder.Append(": " + severityLabel);
						}
						stringBuilder.AppendLine();
						string tipStringExtra = this.TipStringExtra;
						if (!tipStringExtra.NullOrEmpty())
						{
							stringBuilder.AppendLine(tipStringExtra.TrimEndNewlines().Indented("    "));
						}
						if (flag)
						{
							stringBuilder.AppendLine(this.DebugString().TrimEndNewlines());
						}
					}
					string text = this.<GetTooltip>g__Cause|86_0();
					if (text != null)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append("Cause".Translate());
						stringBuilder.AppendLine(":");
						stringBuilder.AppendLine(text);
					}
				}
				else
				{
					stringBuilder.Append(this.CurStage.overrideTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|86_0().Named("CAUSE")));
				}
			}
			else
			{
				stringBuilder.Append(this.def.overrideTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|86_0().Named("CAUSE")));
			}
			if (!string.IsNullOrWhiteSpace(this.def.extraTooltip))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.def.extraTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|86_0().Named("CAUSE")));
			}
			HediffStage curStage2 = this.CurStage;
			if (!string.IsNullOrWhiteSpace((curStage2 != null) ? curStage2.extraTooltip : null))
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(this.CurStage.extraTooltip.Formatted(pawn.Named("PAWN"), this.TipStringExtra.Named("TipStringExtra"), this.<GetTooltip>g__Cause|86_0().Named("CAUSE")));
			}
			return stringBuilder.ToString().TrimEnd(Array.Empty<char>());
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00068FDC File Offset: 0x000671DC
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity >= this.def.maxSeverity) ? " (reached max)" : "");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority;
			}
			return text.Indented("    ");
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x0006906C File Offset: 0x0006726C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.def.defName,
				(this.part != null) ? (" " + this.part.Label) : "",
				" ticksSinceCreation=",
				this.ageTicks,
				")"
			});
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x000690DF File Offset: 0x000672DF
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x0006910C File Offset: 0x0006730C
		[CompilerGenerated]
		private string <GetTooltip>g__Cause|86_0()
		{
			TaggedString taggedString;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(Gen.YieldSingle<Hediff>(this), out taggedString, out logEntry))
			{
				return taggedString.Resolve().Indented("    ");
			}
			return null;
		}

		// Token: 0x04000DB4 RID: 3508
		public HediffDef def;

		// Token: 0x04000DB5 RID: 3509
		public int ageTicks;

		// Token: 0x04000DB6 RID: 3510
		private BodyPartRecord part;

		// Token: 0x04000DB7 RID: 3511
		public ThingDef source;

		// Token: 0x04000DB8 RID: 3512
		public BodyPartGroupDef sourceBodyPartGroup;

		// Token: 0x04000DB9 RID: 3513
		public HediffDef sourceHediffDef;

		// Token: 0x04000DBA RID: 3514
		public int loadID = -1;

		// Token: 0x04000DBB RID: 3515
		protected float severityInt;

		// Token: 0x04000DBC RID: 3516
		private bool recordedTale;

		// Token: 0x04000DBD RID: 3517
		protected bool causesNoPain;

		// Token: 0x04000DBE RID: 3518
		private bool visible;

		// Token: 0x04000DBF RID: 3519
		public WeakReference<LogEntry> combatLogEntry;

		// Token: 0x04000DC0 RID: 3520
		public string combatLogText;

		// Token: 0x04000DC1 RID: 3521
		public int temp_partIndexToSetLater = -1;

		// Token: 0x04000DC2 RID: 3522
		[Unsaved(false)]
		public Pawn pawn;
	}
}

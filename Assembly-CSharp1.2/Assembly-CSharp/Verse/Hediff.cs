using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020003AD RID: 941
	public class Hediff : IExposable, ILoadReferenceable
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600173D RID: 5949 RVA: 0x00016547 File Offset: 0x00014747
		public virtual string LabelBase
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600173E RID: 5950 RVA: 0x00016554 File Offset: 0x00014754
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600173F RID: 5951 RVA: 0x000DBA5C File Offset: 0x000D9C5C
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001740 RID: 5952 RVA: 0x00016567 File Offset: 0x00014767
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001741 RID: 5953 RVA: 0x0001657A File Offset: 0x0001477A
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001742 RID: 5954 RVA: 0x00016587 File Offset: 0x00014787
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

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001743 RID: 5955 RVA: 0x000165B0 File Offset: 0x000147B0
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

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001744 RID: 5956 RVA: 0x000165DD File Offset: 0x000147DD
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001745 RID: 5957 RVA: 0x000DBA9C File Offset: 0x000D9C9C
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

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001746 RID: 5958 RVA: 0x000165EA File Offset: 0x000147EA
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

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001747 RID: 5959 RVA: 0x00016616 File Offset: 0x00014816
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00016628 File Offset: 0x00014828
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001749 RID: 5961 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x0001664E File Offset: 0x0001484E
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x0001665D File Offset: 0x0001485D
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

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x00016680 File Offset: 0x00014880
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

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x0600174D RID: 5965 RVA: 0x0001669B File Offset: 0x0001489B
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

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x00016647 File Offset: 0x00014847
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x000DBB1C File Offset: 0x000D9D1C
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

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001750 RID: 5968 RVA: 0x000166B2 File Offset: 0x000148B2
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001751 RID: 5969 RVA: 0x000DBB8C File Offset: 0x000D9D8C
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

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001752 RID: 5970 RVA: 0x000166B9 File Offset: 0x000148B9
		// (set) Token: 0x06001753 RID: 5971 RVA: 0x000DBBE0 File Offset: 0x000D9DE0
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

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06001754 RID: 5972 RVA: 0x000166C1 File Offset: 0x000148C1
		// (set) Token: 0x06001755 RID: 5973 RVA: 0x000166C9 File Offset: 0x000148C9
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
					Log.Error("Hediff: Cannot set Part without setting pawn first.", false);
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x000DBCF0 File Offset: 0x000D9EF0
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

		// Token: 0x06001757 RID: 5975 RVA: 0x000DBD4C File Offset: 0x000D9F4C
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

		// Token: 0x06001758 RID: 5976 RVA: 0x000DBE7C File Offset: 0x000DA07C
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
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, null, false);
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
					bool flag = PawnUtility.ShouldSendNotificationAbout(this.pawn);
					Caravan caravan = this.pawn.GetCaravan();
					this.pawn.Kill(null, null);
					if (flag)
					{
						this.pawn.health.NotifyPlayerOfKilled(null, this, caravan);
					}
					return;
				}
			}
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x000DC2BC File Offset: 0x000DA4BC
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
				mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_Hediff".Translate(this.Label), false);
			}
		}

		// Token: 0x0600175A RID: 5978 RVA: 0x000166EE File Offset: 0x000148EE
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
		}

		// Token: 0x0600175B RID: 5979 RVA: 0x00016724 File Offset: 0x00014924
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			if (this.def.disablesNeed != null)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x0600175C RID: 5980 RVA: 0x00016743 File Offset: 0x00014943
		public virtual void PostRemoved()
		{
			if ((this.def.causesNeed != null || this.def.disablesNeed != null) && !this.pawn.Dead)
			{
				this.pawn.needs.AddOrRemoveNeedsAsAppropriate();
			}
		}

		// Token: 0x0600175D RID: 5981 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostTick()
		{
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public virtual void Tended(float quality, int batchPosition = 0)
		{
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Tended_NewTemp(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0001677C File Offset: 0x0001497C
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000167A6 File Offset: 0x000149A6
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

		// Token: 0x06001763 RID: 5987 RVA: 0x000DC344 File Offset: 0x000DA544
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
					}), false);
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnDied()
		{
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000DC3CC File Offset: 0x000DA5CC
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

		// Token: 0x0600176C RID: 5996 RVA: 0x000DC45C File Offset: 0x000DA65C
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

		// Token: 0x0600176D RID: 5997 RVA: 0x000167E4 File Offset: 0x000149E4
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID;
		}

		// Token: 0x04001202 RID: 4610
		public HediffDef def;

		// Token: 0x04001203 RID: 4611
		public int ageTicks;

		// Token: 0x04001204 RID: 4612
		private BodyPartRecord part;

		// Token: 0x04001205 RID: 4613
		public ThingDef source;

		// Token: 0x04001206 RID: 4614
		public BodyPartGroupDef sourceBodyPartGroup;

		// Token: 0x04001207 RID: 4615
		public HediffDef sourceHediffDef;

		// Token: 0x04001208 RID: 4616
		public int loadID = -1;

		// Token: 0x04001209 RID: 4617
		protected float severityInt;

		// Token: 0x0400120A RID: 4618
		private bool recordedTale;

		// Token: 0x0400120B RID: 4619
		protected bool causesNoPain;

		// Token: 0x0400120C RID: 4620
		private bool visible;

		// Token: 0x0400120D RID: 4621
		public WeakReference<LogEntry> combatLogEntry;

		// Token: 0x0400120E RID: 4622
		public string combatLogText;

		// Token: 0x0400120F RID: 4623
		public int temp_partIndexToSetLater = -1;

		// Token: 0x04001210 RID: 4624
		[Unsaved(false)]
		public Pawn pawn;
	}
}

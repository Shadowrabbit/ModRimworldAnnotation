using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001516 RID: 5398
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x0600747C RID: 29820 RVA: 0x0004E99F File Offset: 0x0004CB9F
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x0600747D RID: 29821 RVA: 0x0004E9A7 File Offset: 0x0004CBA7
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true);
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x0600747E RID: 29822 RVA: 0x0004E9BA File Offset: 0x0004CBBA
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true), this.MaxEntropy);
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x0600747F RID: 29823 RVA: 0x0004E9D8 File Offset: 0x0004CBD8
		public float PainMultiplier
		{
			get
			{
				return 1f + this.pawn.health.hediffSet.PainTotal * 3f;
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06007480 RID: 29824 RVA: 0x0004E9FB File Offset: 0x0004CBFB
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true) * this.PainMultiplier;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06007481 RID: 29825 RVA: 0x0004EA15 File Offset: 0x0004CC15
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06007482 RID: 29826 RVA: 0x0004EA1D File Offset: 0x0004CC1D
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06007483 RID: 29827 RVA: 0x0004EA25 File Offset: 0x0004CC25
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06007484 RID: 29828 RVA: 0x0004EA2D File Offset: 0x0004CC2D
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06007485 RID: 29829 RVA: 0x0004EA3F File Offset: 0x0004CC3F
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06007486 RID: 29830 RVA: 0x002373B8 File Offset: 0x002355B8
		public float PsychicSensitivity
		{
			get
			{
				if (this.psychicSensitivityCachedTick != Find.TickManager.TicksGame)
				{
					this.psychicSensitivityCached = this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true);
					this.psychicSensitivityCachedTick = Find.TickManager.TicksGame;
				}
				return this.psychicSensitivityCached;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06007487 RID: 29831 RVA: 0x0004EA56 File Offset: 0x0004CC56
		public float EntropyRelativeValue
		{
			get
			{
				return this.EntropyToRelativeValue(this.currentEntropy);
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06007488 RID: 29832 RVA: 0x00237404 File Offset: 0x00235604
		public PsychicEntropySeverity Severity
		{
			get
			{
				PsychicEntropySeverity result = PsychicEntropySeverity.Safe;
				foreach (PsychicEntropySeverity psychicEntropySeverity in Pawn_PsychicEntropyTracker.EntropyThresholds.Keys)
				{
					if (Pawn_PsychicEntropyTracker.EntropyThresholds[psychicEntropySeverity] >= this.EntropyRelativeValue)
					{
						break;
					}
					result = psychicEntropySeverity;
				}
				return result;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06007489 RID: 29833 RVA: 0x0004EA64 File Offset: 0x0004CC64
		public int PsyfocusBand
		{
			get
			{
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[1])
				{
					return 0;
				}
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[2])
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x0600748A RID: 29834 RVA: 0x0004EA91 File Offset: 0x0004CC91
		public Hediff_Psylink Psylink
		{
			get
			{
				if (this.psylinkCachedForTick != Find.TickManager.TicksGame)
				{
					this.psylinkCached = this.pawn.GetMainPsylinkSource();
					this.psylinkCachedForTick = Find.TickManager.TicksGame;
				}
				return this.psylinkCached;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x0600748B RID: 29835 RVA: 0x0004EACC File Offset: 0x0004CCCC
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x0600748C RID: 29836 RVA: 0x0004EB04 File Offset: 0x0004CD04
		private float PsyfocusFallPerDay
		{
			get
			{
				if (this.pawn.GetPsylinkLevel() == 0)
				{
					return 0f;
				}
				return Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x0023746C File Offset: 0x0023566C
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600748E RID: 29838 RVA: 0x0004EB29 File Offset: 0x0004CD29
		public static void ResetStaticData()
		{
			Pawn_PsychicEntropyTracker.EntropyThresholdSounds = new Dictionary<PsychicEntropySeverity, SoundDef>
			{
				{
					PsychicEntropySeverity.Overloaded,
					SoundDefOf.PsychicEntropyOverloaded
				},
				{
					PsychicEntropySeverity.Hyperloaded,
					SoundDefOf.PsychicEntropyHyperloaded
				},
				{
					PsychicEntropySeverity.BrainCharring,
					SoundDefOf.PsychicEntropyBrainCharring
				},
				{
					PsychicEntropySeverity.BrainRoasting,
					SoundDefOf.PsychicEntropyBrainRoasting
				}
			};
		}

		// Token: 0x0600748F RID: 29839 RVA: 0x002374B8 File Offset: 0x002356B8
		public void PsychicEntropyTrackerTick()
		{
			if (this.currentEntropy > 1E-45f)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy - 1.TicksToSeconds() * this.RecoveryRate, 0f);
			}
			if (this.currentEntropy > 1E-45f && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicEntropy, false))
			{
				this.pawn.health.AddHediff(HediffDefOf.PsychicEntropy, null, null, null);
			}
			if (this.currentEntropy > 1E-45f)
			{
				if (this.ticksSinceLastMote >= Pawn_PsychicEntropyTracker.TicksBetweenMotes[(int)this.Severity])
				{
					if (this.pawn.Spawned)
					{
						MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_EntropyPulse, Vector3.zero, 1f, -1f);
					}
					this.ticksSinceLastMote = 0;
				}
				else
				{
					this.ticksSinceLastMote++;
				}
			}
			else
			{
				this.ticksSinceLastMote = 0;
			}
			if (this.NeedsPsyfocus && this.pawn.IsHashIntervalTick(150))
			{
				float num = 400f;
				if (!this.IsCurrentlyMeditating)
				{
					this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus - this.PsyfocusFallPerDay / num, 0f, 1f);
				}
				MeditationUtility.CheckMeditationScheduleTeachOpportunity(this.pawn);
			}
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x0004EB65 File Offset: 0x0004CD65
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true) > this.MaxEntropy;
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00237608 File Offset: 0x00235808
		public bool TryAddEntropy(float value, Thing source = null, bool scale = true, bool overLimit = false)
		{
			PsychicEntropySeverity severity = this.Severity;
			float num = scale ? (value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true)) : value;
			if (!this.WouldOverflowEntropy(num) || overLimit)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy + num, 0f);
				foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_EntropyGained(value, num, source);
				}
				if (severity != this.Severity && num > 0f && this.Severity != PsychicEntropySeverity.Safe)
				{
					Pawn_PsychicEntropyTracker.EntropyThresholdSounds[this.Severity].PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					if (severity < PsychicEntropySeverity.Overloaded && this.Severity >= PsychicEntropySeverity.Overloaded)
					{
						Messages.Message("MessageWentOverPsychicEntropyLimit".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x0004EB93 File Offset: 0x0004CD93
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded()
		{
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x00006A05 File Offset: 0x00004C05
		[Obsolete("Only used for mod compatibility")]
		private void GiveHangoverIfNeeded_NewTemp(float entropyChange)
		{
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x00237750 File Offset: 0x00235950
		public void GainPsyfocus(Thing focus = null)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + MeditationUtility.PsyfocusGainPerTick(this.pawn, focus), 0f, 1f);
			if (focus != null && !focus.Destroyed)
			{
				CompMeditationFocus compMeditationFocus = focus.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null)
				{
					compMeditationFocus.Used(this.pawn);
				}
			}
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x0004EBA0 File Offset: 0x0004CDA0
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x0004EBB2 File Offset: 0x0004CDB2
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x0004EBD1 File Offset: 0x0004CDD1
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		// Token: 0x06007499 RID: 29849 RVA: 0x0004EC0E File Offset: 0x0004CE0E
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		// Token: 0x0600749A RID: 29850 RVA: 0x002377A8 File Offset: 0x002359A8
		public float EntropyToRelativeValue(float val)
		{
			if (val < 1E-45f)
			{
				return 0f;
			}
			if (val < this.MaxEntropy)
			{
				if (this.MaxEntropy <= 1E-45f)
				{
					return 0f;
				}
				return val / this.MaxEntropy;
			}
			else
			{
				if (this.MaxPotentialEntropy <= 1E-45f)
				{
					return 0f;
				}
				return 1f + (val - this.MaxEntropy) / this.MaxPotentialEntropy;
			}
		}

		// Token: 0x0600749B RID: 29851 RVA: 0x0004EC26 File Offset: 0x0004CE26
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x0004EC3E File Offset: 0x0004CE3E
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		// Token: 0x0600749D RID: 29853 RVA: 0x00237810 File Offset: 0x00235A10
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		// Token: 0x0600749E RID: 29854 RVA: 0x0004EC56 File Offset: 0x0004CE56
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		// Token: 0x0600749F RID: 29855 RVA: 0x0004EC72 File Offset: 0x0004CE72
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public string PsyfocusTipString()
		{
			return this.PsyfocusTipString_NewTemp(-1f);
		}

		// Token: 0x060074A0 RID: 29856 RVA: 0x0023786C File Offset: 0x00235A6C
		public string PsyfocusTipString_NewTemp(float psyfocusTargetOverride = -1f)
		{
			if (Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached == null)
			{
				for (int i = 0; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoPsycasts".Translate(Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i]) + "\n";
				}
				Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "\n";
				for (int j = 0; j < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; j++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoFallRate".Translate(Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[j].ToStringPercent()) + "\n";
				}
			}
			return "Psyfocus".Translate() + ": " + this.currentPsyfocus.ToStringPercent("0.#") + "\n" + "DesiredPsyfocus".Translate() + ": " + ((psyfocusTargetOverride >= 0f) ? psyfocusTargetOverride : this.targetPsyfocus).ToStringPercent("0.#") + "\n\n" + "DesiredPsyfocusDesc".Translate(this.pawn.Named("PAWN")) + "\n\n" + "PsyfocusDesc".Translate() + ":\n\n" + Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached;
		}

		// Token: 0x060074A1 RID: 29857 RVA: 0x00237AB0 File Offset: 0x00235CB0
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.currentEntropy, "currentEntropy", 0f, false);
			Scribe_Values.Look<float>(ref this.currentPsyfocus, "currentPsyfocus", -1f, false);
			Scribe_Values.Look<float>(ref this.targetPsyfocus, "targetPsyfocus", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastMeditationTick, "lastMeditationTick", -1, false);
			Scribe_Values.Look<bool>(ref this.limitEntropyAmount, "limitEntropyAmount", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.currentPsyfocus < 0f)
			{
				this.SetInitialPsyfocusLevel();
			}
		}

		// Token: 0x04004CD0 RID: 19664
		private Pawn pawn;

		// Token: 0x04004CD1 RID: 19665
		private float currentEntropy;

		// Token: 0x04004CD2 RID: 19666
		private int ticksSinceLastMote;

		// Token: 0x04004CD3 RID: 19667
		public bool limitEntropyAmount = true;

		// Token: 0x04004CD4 RID: 19668
		private float currentPsyfocus = -1f;

		// Token: 0x04004CD5 RID: 19669
		private float targetPsyfocus = 0.5f;

		// Token: 0x04004CD6 RID: 19670
		private int lastMeditationTick = -1;

		// Token: 0x04004CD7 RID: 19671
		private Gizmo gizmo;

		// Token: 0x04004CD8 RID: 19672
		private Hediff_Psylink psylinkCached;

		// Token: 0x04004CD9 RID: 19673
		private int psylinkCachedForTick = -1;

		// Token: 0x04004CDA RID: 19674
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		// Token: 0x04004CDB RID: 19675
		public const float PercentageAfterGainingPsylink = 0.75f;

		// Token: 0x04004CDC RID: 19676
		public const int PsyfocusUpdateInterval = 150;

		// Token: 0x04004CDD RID: 19677
		public const float PsyfocusCostTolerance = 0.0005f;

		// Token: 0x04004CDE RID: 19678
		public static readonly Dictionary<PsychicEntropySeverity, float> EntropyThresholds = new Dictionary<PsychicEntropySeverity, float>
		{
			{
				PsychicEntropySeverity.Safe,
				0f
			},
			{
				PsychicEntropySeverity.Overloaded,
				1f
			},
			{
				PsychicEntropySeverity.Hyperloaded,
				1.33f
			},
			{
				PsychicEntropySeverity.BrainCharring,
				1.66f
			},
			{
				PsychicEntropySeverity.BrainRoasting,
				2f
			}
		};

		// Token: 0x04004CDF RID: 19679
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		// Token: 0x04004CE0 RID: 19680
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		// Token: 0x04004CE1 RID: 19681
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		// Token: 0x04004CE2 RID: 19682
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		// Token: 0x04004CE3 RID: 19683
		private float psychicSensitivityCached;

		// Token: 0x04004CE4 RID: 19684
		private int psychicSensitivityCachedTick = -1;

		// Token: 0x04004CE5 RID: 19685
		public static string psyfocusLevelInfoCached = null;
	}
}

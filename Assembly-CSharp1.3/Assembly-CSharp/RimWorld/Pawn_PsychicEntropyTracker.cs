using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000E71 RID: 3697
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x060055F4 RID: 22004 RVA: 0x001D19B3 File Offset: 0x001CFBB3
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x060055F5 RID: 22005 RVA: 0x001D19BB File Offset: 0x001CFBBB
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true);
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x001D19CE File Offset: 0x001CFBCE
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true), this.MaxEntropy);
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x060055F7 RID: 22007 RVA: 0x001D19EC File Offset: 0x001CFBEC
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true);
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x060055F8 RID: 22008 RVA: 0x001D19FF File Offset: 0x001CFBFF
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x060055F9 RID: 22009 RVA: 0x001D1A07 File Offset: 0x001CFC07
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		// Token: 0x17000EE5 RID: 3813
		// (get) Token: 0x060055FA RID: 22010 RVA: 0x001D1A0F File Offset: 0x001CFC0F
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x060055FB RID: 22011 RVA: 0x001D1A17 File Offset: 0x001CFC17
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x060055FC RID: 22012 RVA: 0x001D1A29 File Offset: 0x001CFC29
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x060055FD RID: 22013 RVA: 0x001D1A40 File Offset: 0x001CFC40
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

		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x060055FE RID: 22014 RVA: 0x001D1A8C File Offset: 0x001CFC8C
		public float EntropyRelativeValue
		{
			get
			{
				return this.EntropyToRelativeValue(this.currentEntropy);
			}
		}

		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x060055FF RID: 22015 RVA: 0x001D1A9C File Offset: 0x001CFC9C
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

		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x001D1B04 File Offset: 0x001CFD04
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

		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x001D1B31 File Offset: 0x001CFD31
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

		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x06005602 RID: 22018 RVA: 0x001D1B6C File Offset: 0x001CFD6C
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005603 RID: 22019 RVA: 0x001D1BA4 File Offset: 0x001CFDA4
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

		// Token: 0x06005604 RID: 22020 RVA: 0x001D1BCC File Offset: 0x001CFDCC
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005605 RID: 22021 RVA: 0x001D1C18 File Offset: 0x001CFE18
		public static void ResetStaticData()
		{
			Pawn_PsychicEntropyTracker.EntropyThresholdSounds = new Dictionary<PsychicEntropySeverity, SoundDef>
			{
				{
					PsychicEntropySeverity.Overloaded,
					SoundDefOf.PsychicEntropyOverloaded
				},
				{
					PsychicEntropySeverity.VeryOverloaded,
					SoundDefOf.PsychicEntropyHyperloaded
				},
				{
					PsychicEntropySeverity.Extreme,
					SoundDefOf.PsychicEntropyBrainCharring
				},
				{
					PsychicEntropySeverity.Overwhelming,
					SoundDefOf.PsychicEntropyBrainRoasting
				}
			};
		}

		// Token: 0x06005606 RID: 22022 RVA: 0x001D1C54 File Offset: 0x001CFE54
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
						FleckMaker.AttachedOverlay(this.pawn, FleckDefOf.EntropyPulse, Vector3.zero, 1f, -1f);
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

		// Token: 0x06005607 RID: 22023 RVA: 0x001D1DA1 File Offset: 0x001CFFA1
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true) > this.MaxEntropy;
		}

		// Token: 0x06005608 RID: 22024 RVA: 0x001D1DD0 File Offset: 0x001CFFD0
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

		// Token: 0x06005609 RID: 22025 RVA: 0x001D1F18 File Offset: 0x001D0118
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		// Token: 0x0600560A RID: 22026 RVA: 0x001D1F28 File Offset: 0x001D0128
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

		// Token: 0x0600560B RID: 22027 RVA: 0x001D1F7E File Offset: 0x001D017E
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600560C RID: 22028 RVA: 0x001D1F90 File Offset: 0x001D0190
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		// Token: 0x0600560D RID: 22029 RVA: 0x001D1FAF File Offset: 0x001D01AF
		public void RechargePsyfocus()
		{
			this.currentPsyfocus = Mathf.Max(this.targetPsyfocus, this.currentPsyfocus);
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x001D1FC8 File Offset: 0x001D01C8
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		// Token: 0x0600560F RID: 22031 RVA: 0x001D2005 File Offset: 0x001D0205
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x001D2020 File Offset: 0x001D0220
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

		// Token: 0x06005611 RID: 22033 RVA: 0x001D2088 File Offset: 0x001D0288
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x001D20A0 File Offset: 0x001D02A0
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x001D20B8 File Offset: 0x001D02B8
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x001D2112 File Offset: 0x001D0312
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x001D2130 File Offset: 0x001D0330
		public string PsyfocusTipString(float psyfocusTargetOverride = -1f)
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

		// Token: 0x06005616 RID: 22038 RVA: 0x001D2374 File Offset: 0x001D0574
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

		// Token: 0x040032CB RID: 13003
		private Pawn pawn;

		// Token: 0x040032CC RID: 13004
		private float currentEntropy;

		// Token: 0x040032CD RID: 13005
		private int ticksSinceLastMote;

		// Token: 0x040032CE RID: 13006
		public bool limitEntropyAmount = true;

		// Token: 0x040032CF RID: 13007
		private float currentPsyfocus = -1f;

		// Token: 0x040032D0 RID: 13008
		private float targetPsyfocus = 0.5f;

		// Token: 0x040032D1 RID: 13009
		private int lastMeditationTick = -1;

		// Token: 0x040032D2 RID: 13010
		private Gizmo gizmo;

		// Token: 0x040032D3 RID: 13011
		private Hediff_Psylink psylinkCached;

		// Token: 0x040032D4 RID: 13012
		private int psylinkCachedForTick = -1;

		// Token: 0x040032D5 RID: 13013
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		// Token: 0x040032D6 RID: 13014
		public const float PercentageAfterGainingPsylink = 0.75f;

		// Token: 0x040032D7 RID: 13015
		public const int PsyfocusUpdateInterval = 150;

		// Token: 0x040032D8 RID: 13016
		public const float PsyfocusCostTolerance = 0.0005f;

		// Token: 0x040032D9 RID: 13017
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
				PsychicEntropySeverity.VeryOverloaded,
				1.33f
			},
			{
				PsychicEntropySeverity.Extreme,
				1.66f
			},
			{
				PsychicEntropySeverity.Overwhelming,
				2f
			}
		};

		// Token: 0x040032DA RID: 13018
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		// Token: 0x040032DB RID: 13019
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		// Token: 0x040032DC RID: 13020
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		// Token: 0x040032DD RID: 13021
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		// Token: 0x040032DE RID: 13022
		private float psychicSensitivityCached;

		// Token: 0x040032DF RID: 13023
		private int psychicSensitivityCachedTick = -1;

		// Token: 0x040032E0 RID: 13024
		public static string psyfocusLevelInfoCached = null;
	}
}

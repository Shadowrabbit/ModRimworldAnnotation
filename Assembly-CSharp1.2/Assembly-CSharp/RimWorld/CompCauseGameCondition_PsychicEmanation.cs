using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001780 RID: 6016
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x0600849A RID: 33946 RVA: 0x00058D00 File Offset: 0x00056F00
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x0600849B RID: 33947 RVA: 0x00058D0D File Offset: 0x00056F0D
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x0600849C RID: 33948 RVA: 0x00058D15 File Offset: 0x00056F15
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		// Token: 0x0600849D RID: 33949 RVA: 0x00058D2C File Offset: 0x00056F2C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
			this.droneLevel = this.Props.droneLevel;
		}

		// Token: 0x0600849E RID: 33950 RVA: 0x00058D4D File Offset: 0x00056F4D
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x0600849F RID: 33951 RVA: 0x002741CC File Offset: 0x002723CC
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned || !this.DroneLevelIncreases || !base.Active)
			{
				return;
			}
			this.ticksToIncreaseDroneLevel--;
			if (this.ticksToIncreaseDroneLevel <= 0)
			{
				this.IncreaseDroneLevel();
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
		}

		// Token: 0x060084A0 RID: 33952 RVA: 0x0027422C File Offset: 0x0027242C
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel == PsychicDroneLevel.BadExtreme)
			{
				return;
			}
			this.droneLevel += 1;
			TaggedString taggedString = "LetterPsychicDroneLevelIncreased".Translate(this.gender.GetLabel(false));
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), taggedString, LetterDefOf.NegativeEvent, null);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			base.ReSetupAllConditions();
		}

		// Token: 0x060084A1 RID: 33953 RVA: 0x00058D87 File Offset: 0x00056F87
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		// Token: 0x060084A2 RID: 33954 RVA: 0x00058DAE File Offset: 0x00056FAE
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x060084A3 RID: 33955 RVA: 0x00058DEC File Offset: 0x00056FEC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate()
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Action
			{
				defaultLabel = this.droneLevel.GetLabel(),
				action = delegate()
				{
					this.IncreaseDroneLevel();
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			yield break;
		}

		// Token: 0x060084A4 RID: 33956 RVA: 0x002742B0 File Offset: 0x002724B0
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		// Token: 0x060084A5 RID: 33957 RVA: 0x00274338 File Offset: 0x00272538
		public override void RandomizeSettings_NewTemp_NewTemp(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
			if (site.ActualThreatPoints < 800f)
			{
				this.droneLevel = PsychicDroneLevel.BadLow;
				return;
			}
			if (site.ActualThreatPoints < 2000f)
			{
				this.droneLevel = PsychicDroneLevel.BadMedium;
				return;
			}
			this.droneLevel = PsychicDroneLevel.BadHigh;
		}

		// Token: 0x040055E8 RID: 21992
		public Gender gender;

		// Token: 0x040055E9 RID: 21993
		private int ticksToIncreaseDroneLevel;

		// Token: 0x040055EA RID: 21994
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}

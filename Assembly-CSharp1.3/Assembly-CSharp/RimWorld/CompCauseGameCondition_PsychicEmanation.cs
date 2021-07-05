using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010F2 RID: 4338
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		// Token: 0x170011BC RID: 4540
		// (get) Token: 0x060067D1 RID: 26577 RVA: 0x002321E1 File Offset: 0x002303E1
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		// Token: 0x170011BD RID: 4541
		// (get) Token: 0x060067D2 RID: 26578 RVA: 0x002321EE File Offset: 0x002303EE
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x170011BE RID: 4542
		// (get) Token: 0x060067D3 RID: 26579 RVA: 0x002321F6 File Offset: 0x002303F6
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x0023220D File Offset: 0x0023040D
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
			this.droneLevel = this.Props.droneLevel;
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x0023222E File Offset: 0x0023042E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x00232268 File Offset: 0x00230468
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

		// Token: 0x060067D7 RID: 26583 RVA: 0x002322C8 File Offset: 0x002304C8
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel == PsychicDroneLevel.BadExtreme)
			{
				return;
			}
			this.droneLevel += 1;
			TaggedString text = "LetterPsychicDroneLevelIncreased".Translate(this.gender.GetLabel(false));
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent, null);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			base.ReSetupAllConditions();
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x00232340 File Offset: 0x00230540
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x00232367 File Offset: 0x00230567
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x002323A5 File Offset: 0x002305A5
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

		// Token: 0x060067DB RID: 26587 RVA: 0x002323B8 File Offset: 0x002305B8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x00232440 File Offset: 0x00230640
		public override void RandomizeSettings(Site site)
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

		// Token: 0x04003A78 RID: 14968
		public Gender gender;

		// Token: 0x04003A79 RID: 14969
		private int ticksToIncreaseDroneLevel;

		// Token: 0x04003A7A RID: 14970
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}

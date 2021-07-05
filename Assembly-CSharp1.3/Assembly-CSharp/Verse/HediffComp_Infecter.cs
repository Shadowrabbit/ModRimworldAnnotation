using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002A7 RID: 679
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600129D RID: 4765 RVA: 0x0006B072 File Offset: 0x00069272
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x0006B080 File Offset: 0x00069280
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				this.ticksUntilInfect = -2;
				return;
			}
			float num = this.Props.infectionChance;
			if (base.Pawn.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			if (Rand.Value <= num)
			{
				this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				return;
			}
			this.ticksUntilInfect = -2;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0006B158 File Offset: 0x00069358
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0006B183 File Offset: 0x00069383
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect--;
				if (this.ticksUntilInfect == 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0006B1AC File Offset: 0x000693AC
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.CompTended(quality, maxQuality, batchPosition);
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0006B1F4 File Offset: 0x000693F4
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			float num = 1f;
			HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
			{
				num *= this.infectionChanceFactorFromTendRoom;
				num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
			}
			num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(this.parent.Severity);
			if (base.Pawn.Faction == Faction.OfPlayer)
			{
				num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
			}
			if (Rand.Value < num)
			{
				this.ticksUntilInfect = -4;
				base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, this.parent.Part, null, null);
				return;
			}
			this.ticksUntilInfect = -3;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x0006B2F0 File Offset: 0x000694F0
		public override string CompDebugString()
		{
			if (this.ticksUntilInfect > 0)
			{
				return string.Concat(new object[]
				{
					"infection may appear in: ",
					this.ticksUntilInfect,
					" ticks\ninfectChnceFactorFromTendRoom: ",
					this.infectionChanceFactorFromTendRoom.ToStringPercent()
				});
			}
			if (this.ticksUntilInfect == -4)
			{
				return "already created infection";
			}
			if (this.ticksUntilInfect == -3)
			{
				return "failed to make infection";
			}
			if (this.ticksUntilInfect == -2)
			{
				return "will not make infection";
			}
			if (this.ticksUntilInfect == -1)
			{
				return "uninitialized data!";
			}
			return "unexpected ticksUntilInfect = " + this.ticksUntilInfect;
		}

		// Token: 0x04000E13 RID: 3603
		private int ticksUntilInfect = -1;

		// Token: 0x04000E14 RID: 3604
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x04000E15 RID: 3605
		private const int UninitializedValue = -1;

		// Token: 0x04000E16 RID: 3606
		private const int WillNotInfectValue = -2;

		// Token: 0x04000E17 RID: 3607
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x04000E18 RID: 3608
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x04000E19 RID: 3609
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x04000E1A RID: 3610
		private static readonly SimpleCurve InfectionChanceFactorFromSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.1f),
				true
			},
			{
				new CurvePoint(12f, 1f),
				true
			}
		};
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004EB RID: 1259
	public struct ShotReport
	{
		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000EB4E4 File Offset: 0x000E96E4
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 0.2f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060025F5 RID: 9717 RVA: 0x000EB534 File Offset: 0x000E9734
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null && this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
					{
						return 7.5f;
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000EB582 File Offset: 0x000E9782
		private float FactorFromCoveringGas
		{
			get
			{
				if (this.coveringGas != null)
				{
					return 1f - this.coveringGas.gas.accuracyPenalty;
				}
				return 1f;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000EB5A8 File Offset: 0x000E97A8
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.FactorFromCoveringGas * this.FactorFromExecution;
				num += this.offsetFromDarkness;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x000EB5F0 File Offset: 0x000E97F0
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060025F9 RID: 9721 RVA: 0x000EB5FF File Offset: 0x000E97FF
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x000EB60E File Offset: 0x000E980E
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x060025FB RID: 9723 RVA: 0x000EB61C File Offset: 0x000E981C
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000EB630 File Offset: 0x000E9830
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000EB638 File Offset: 0x000E9838
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			IntVec3 cell = target.Cell;
			ShotReport shotReport;
			shotReport.distance = (cell - caster.Position).LengthHorizontal;
			shotReport.target = target.ToTargetInfo(caster.Map);
			shotReport.factorFromShooterAndDist = ShotReport.HitFactorFromShooter(caster, shotReport.distance);
			shotReport.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.EquipmentSource, shotReport.distance);
			shotReport.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			shotReport.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			shotReport.coveringGas = null;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out shotReport.shootLine))
			{
				using (IEnumerator<IntVec3> enumerator = shotReport.shootLine.Points().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						Thing gas = c.GetGas(caster.Map);
						if (gas != null && (shotReport.coveringGas == null || shotReport.coveringGas.gas.accuracyPenalty < gas.def.gas.accuracyPenalty))
						{
							shotReport.coveringGas = gas.def;
						}
					}
					goto IL_14B;
				}
			}
			shotReport.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			IL_14B:
			if (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map))
			{
				shotReport.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				shotReport.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					shotReport.factorFromTargetSize = pawn.BodySize;
				}
				else
				{
					shotReport.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 2.5f;
				}
				shotReport.factorFromTargetSize = Mathf.Clamp(shotReport.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				shotReport.factorFromTargetSize = 1f;
			}
			shotReport.forcedMissRadius = verb.verbProps.ForcedMissRadius;
			shotReport.offsetFromDarkness = 0f;
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsLitOffset, true);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyOutdoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsDarkOffset, true);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					shotReport.offsetFromDarkness = caster.GetStatValue(StatDefOf.ShootingAccuracyIndoorsLitOffset, true);
				}
			}
			return shotReport;
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000EB954 File Offset: 0x000E9B54
		public static float HitFactorFromShooter(Thing caster, float distance)
		{
			return ShotReport.HitFactorFromShooter((caster is Pawn) ? caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true) : caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true), distance);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000EB97E File Offset: 0x000E9B7E
		public static float HitFactorFromShooter(float accRating, float distance)
		{
			return Mathf.Max(Mathf.Pow(accRating, distance), 0.0201f);
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000EB994 File Offset: 0x000E9B94
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + "   " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + "   " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(" " + this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + "  " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + "        " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing && this.factorFromTargetSize != 1f)
				{
					stringBuilder.AppendLine("   " + "TargetSize".Translate() + "       " + this.factorFromTargetSize.ToStringPercent());
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + "         " + this.factorFromWeather.ToStringPercent());
				}
				if (this.FactorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + this.coveringGas.LabelCap + "         " + this.FactorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + "  " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + "   " + this.FactorFromExecution.ToStringPercent());
				}
				if (ModsConfig.IdeologyActive && this.target.HasThing && this.offsetFromDarkness != 0f)
				{
					if (DarknessCombatUtility.IsOutdoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsLitOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsOutdoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyOutdoorsDarkOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndDark(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsDarkOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
					else if (DarknessCombatUtility.IsIndoorsAndLit(this.target.Thing))
					{
						stringBuilder.AppendLine("   " + StatDefOf.ShootingAccuracyIndoorsLitOffset.LabelCap + "   " + this.offsetFromDarkness.ToStringPercent());
					}
				}
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + "        " + this.PassCoverChance.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						if (coverInfo.BlockChance > 0f)
						{
							stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(coverInfo.Thing.LabelCap, coverInfo.BlockChance.ToStringPercent(), new NamedArgument(coverInfo.Thing.def, "COVER")).CapitalizeFirst());
						}
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000EBEA8 File Offset: 0x000EA0A8
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x040017D8 RID: 6104
		private TargetInfo target;

		// Token: 0x040017D9 RID: 6105
		private float distance;

		// Token: 0x040017DA RID: 6106
		private List<CoverInfo> covers;

		// Token: 0x040017DB RID: 6107
		private float coversOverallBlockChance;

		// Token: 0x040017DC RID: 6108
		private ThingDef coveringGas;

		// Token: 0x040017DD RID: 6109
		private float factorFromShooterAndDist;

		// Token: 0x040017DE RID: 6110
		private float factorFromEquipment;

		// Token: 0x040017DF RID: 6111
		private float factorFromTargetSize;

		// Token: 0x040017E0 RID: 6112
		private float factorFromWeather;

		// Token: 0x040017E1 RID: 6113
		private float forcedMissRadius;

		// Token: 0x040017E2 RID: 6114
		private float offsetFromDarkness;

		// Token: 0x040017E3 RID: 6115
		private ShootLine shootLine;
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000899 RID: 2201
	public struct ShotReport
	{
		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x0600368A RID: 13962 RVA: 0x0015C8A0 File Offset: 0x0015AAA0
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

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x0600368B RID: 13963 RVA: 0x0015C8F0 File Offset: 0x0015AAF0
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

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600368C RID: 13964 RVA: 0x0002A530 File Offset: 0x00028730
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

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600368D RID: 13965 RVA: 0x0015C940 File Offset: 0x0015AB40
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.FactorFromCoveringGas * this.FactorFromExecution;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600368E RID: 13966 RVA: 0x0002A556 File Offset: 0x00028756
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600368F RID: 13967 RVA: 0x0002A565 File Offset: 0x00028765
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06003690 RID: 13968 RVA: 0x0002A574 File Offset: 0x00028774
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003691 RID: 13969 RVA: 0x0002A582 File Offset: 0x00028782
		public float TotalEstimatedHitChance
		{
			get
			{
				return Mathf.Clamp01(this.AimOnTargetChance * this.PassCoverChance);
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003692 RID: 13970 RVA: 0x0002A596 File Offset: 0x00028796
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x0015C980 File Offset: 0x0015AB80
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
			shotReport.forcedMissRadius = verb.verbProps.forcedMissRadius;
			return shotReport;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x0002A59E File Offset: 0x0002879E
		public static float HitFactorFromShooter(Thing caster, float distance)
		{
			return ShotReport.HitFactorFromShooter((caster is Pawn) ? caster.GetStatValue(StatDefOf.ShootingAccuracyPawn, true) : caster.GetStatValue(StatDefOf.ShootingAccuracyTurret, true), distance);
		}

		// Token: 0x06003695 RID: 13973 RVA: 0x0002A5C8 File Offset: 0x000287C8
		public static float HitFactorFromShooter(float accRating, float distance)
		{
			return Mathf.Max(Mathf.Pow(accRating, distance), 0.0201f);
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x0015CBF0 File Offset: 0x0015ADF0
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

		// Token: 0x06003697 RID: 13975 RVA: 0x0015CFA0 File Offset: 0x0015B1A0
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				return coverInfo.Thing;
			}
			return null;
		}

		// Token: 0x04002618 RID: 9752
		private TargetInfo target;

		// Token: 0x04002619 RID: 9753
		private float distance;

		// Token: 0x0400261A RID: 9754
		private List<CoverInfo> covers;

		// Token: 0x0400261B RID: 9755
		private float coversOverallBlockChance;

		// Token: 0x0400261C RID: 9756
		private ThingDef coveringGas;

		// Token: 0x0400261D RID: 9757
		private float factorFromShooterAndDist;

		// Token: 0x0400261E RID: 9758
		private float factorFromEquipment;

		// Token: 0x0400261F RID: 9759
		private float factorFromTargetSize;

		// Token: 0x04002620 RID: 9760
		private float factorFromWeather;

		// Token: 0x04002621 RID: 9761
		private float forcedMissRadius;

		// Token: 0x04002622 RID: 9762
		private ShootLine shootLine;
	}
}

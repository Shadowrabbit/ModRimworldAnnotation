using System;

namespace Verse
{
	// Token: 0x020004EA RID: 1258
	public static class ShootTuning
	{
		// Token: 0x040017BB RID: 6075
		public const float DistTouch = 3f;

		// Token: 0x040017BC RID: 6076
		public const float DistShort = 12f;

		// Token: 0x040017BD RID: 6077
		public const float DistMedium = 25f;

		// Token: 0x040017BE RID: 6078
		public const float DistLong = 40f;

		// Token: 0x040017BF RID: 6079
		public const float MeleeRange = 1.42f;

		// Token: 0x040017C0 RID: 6080
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x040017C1 RID: 6081
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x040017C2 RID: 6082
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x040017C3 RID: 6083
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x040017C4 RID: 6084
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x040017C5 RID: 6085
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x040017C6 RID: 6086
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x040017C7 RID: 6087
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x040017C8 RID: 6088
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x040017C9 RID: 6089
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x040017CA RID: 6090
		public static readonly SimpleSurface MissDistanceFromAimOnChanceCurves = new SimpleSurface
		{
			new SurfaceColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new SurfaceColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new SurfaceColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new SurfaceColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new SurfaceColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new SurfaceColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x040017CB RID: 6091
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x040017CC RID: 6092
		public const float InterceptDistMin = 5f;

		// Token: 0x040017CD RID: 6093
		public const float InterceptDistMax = 12f;

		// Token: 0x040017CE RID: 6094
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x040017CF RID: 6095
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x040017D0 RID: 6096
		[Obsolete]
		public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x040017D1 RID: 6097
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x040017D2 RID: 6098
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x040017D3 RID: 6099
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x040017D4 RID: 6100
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x040017D5 RID: 6101
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x040017D6 RID: 6102
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x040017D7 RID: 6103
		public const float BodySizeClampMax = 2f;
	}
}

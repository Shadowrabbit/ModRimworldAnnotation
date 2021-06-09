using System;

namespace Verse
{
	// Token: 0x02000898 RID: 2200
	public static class ShootTuning
	{
		// Token: 0x040025FB RID: 9723
		public const float DistTouch = 3f;

		// Token: 0x040025FC RID: 9724
		public const float DistShort = 12f;

		// Token: 0x040025FD RID: 9725
		public const float DistMedium = 25f;

		// Token: 0x040025FE RID: 9726
		public const float DistLong = 40f;

		// Token: 0x040025FF RID: 9727
		public const float MeleeRange = 1.42f;

		// Token: 0x04002600 RID: 9728
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x04002601 RID: 9729
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x04002602 RID: 9730
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04002603 RID: 9731
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x04002604 RID: 9732
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04002605 RID: 9733
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x04002606 RID: 9734
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x04002607 RID: 9735
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04002608 RID: 9736
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x04002609 RID: 9737
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x0400260A RID: 9738
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

		// Token: 0x0400260B RID: 9739
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x0400260C RID: 9740
		public const float InterceptDistMin = 5f;

		// Token: 0x0400260D RID: 9741
		public const float InterceptDistMax = 12f;

		// Token: 0x0400260E RID: 9742
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x0400260F RID: 9743
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x04002610 RID: 9744
		[Obsolete]
		public const float Intercept_Pawn_HitChanceFactor_NonWildNonEnemy = 0.4f;

		// Token: 0x04002611 RID: 9745
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x04002612 RID: 9746
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x04002613 RID: 9747
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x04002614 RID: 9748
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x04002615 RID: 9749
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x04002616 RID: 9750
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x04002617 RID: 9751
		public const float BodySizeClampMax = 2f;
	}
}

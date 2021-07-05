using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001764 RID: 5988
	public class WorldLayer_Stars : WorldLayer
	{
		// Token: 0x17001689 RID: 5769
		// (get) Token: 0x06008A29 RID: 35369 RVA: 0x00319A23 File Offset: 0x00317C23
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x1700168A RID: 5770
		// (get) Token: 0x06008A2A RID: 35370 RVA: 0x00319A2A File Offset: 0x00317C2A
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		// Token: 0x1700168B RID: 5771
		// (get) Token: 0x06008A2B RID: 35371 RVA: 0x00319A60 File Offset: 0x00317C60
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x06008A2C RID: 35372 RVA: 0x00319A6A File Offset: 0x00317C6A
		protected override Quaternion Rotation
		{
			get
			{
				if (this.UseStaticRotation)
				{
					return Quaternion.identity;
				}
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06008A2D RID: 35373 RVA: 0x00319A84 File Offset: 0x00317C84
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			for (int i = 0; i < 1500; i++)
			{
				Vector3 unitVector = Rand.UnitVector3;
				Vector3 pos = unitVector * 10f;
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Stars);
				float num = WorldLayer_Stars.StarsDrawSize.RandomInRange;
				Vector3 rhs = this.UseStaticRotation ? GenCelestial.CurSunPositionInWorldSpace().normalized : Vector3.forward;
				float num2 = Vector3.Dot(unitVector, rhs);
				if (num2 > 0.8f)
				{
					num *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num2);
				}
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num, 0f, subMesh, true, true, true);
			}
			this.calculatedForStartingTile = ((Find.GameInitData != null) ? Find.GameInitData.startingTile : -1);
			this.calculatedForStaticRotation = this.UseStaticRotation;
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x040057D3 RID: 22483
		private bool calculatedForStaticRotation;

		// Token: 0x040057D4 RID: 22484
		private int calculatedForStartingTile = -1;

		// Token: 0x040057D5 RID: 22485
		public const float DistanceToStars = 10f;

		// Token: 0x040057D6 RID: 22486
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		// Token: 0x040057D7 RID: 22487
		private const int StarsCount = 1500;

		// Token: 0x040057D8 RID: 22488
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}

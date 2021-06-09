using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200205A RID: 8282
	public class WorldLayer_Stars : WorldLayer
	{
		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x0600AF8D RID: 44941 RVA: 0x00072365 File Offset: 0x00070565
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x0600AF8E RID: 44942 RVA: 0x0007236C File Offset: 0x0007056C
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x0600AF8F RID: 44943 RVA: 0x000723A2 File Offset: 0x000705A2
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600AF90 RID: 44944 RVA: 0x000723AC File Offset: 0x000705AC
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

		// Token: 0x0600AF91 RID: 44945 RVA: 0x000723C6 File Offset: 0x000705C6
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

		// Token: 0x040078AF RID: 30895
		private bool calculatedForStaticRotation;

		// Token: 0x040078B0 RID: 30896
		private int calculatedForStartingTile = -1;

		// Token: 0x040078B1 RID: 30897
		public const float DistanceToStars = 10f;

		// Token: 0x040078B2 RID: 30898
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		// Token: 0x040078B3 RID: 30899
		private const int StarsCount = 1500;

		// Token: 0x040078B4 RID: 30900
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}

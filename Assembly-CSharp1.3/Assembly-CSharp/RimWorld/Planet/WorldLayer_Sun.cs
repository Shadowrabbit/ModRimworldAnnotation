using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001765 RID: 5989
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x06008A31 RID: 35377 RVA: 0x00319A23 File Offset: 0x00317C23
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x1700168E RID: 5774
		// (get) Token: 0x06008A32 RID: 35378 RVA: 0x00319AB9 File Offset: 0x00317CB9
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x06008A33 RID: 35379 RVA: 0x00319AC5 File Offset: 0x00317CC5
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 10f, 15f, 0f, subMesh, true, false, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x040057D9 RID: 22489
		private const float SunDrawSize = 15f;
	}
}

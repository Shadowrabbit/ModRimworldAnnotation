using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200205C RID: 8284
	public class WorldLayer_Sun : WorldLayer
	{
		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x0600AF9E RID: 44958 RVA: 0x00072365 File Offset: 0x00070565
		protected override int Layer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x0600AF9F RID: 44959 RVA: 0x00072425 File Offset: 0x00070625
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x0600AFA0 RID: 44960 RVA: 0x00072431 File Offset: 0x00070631
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

		// Token: 0x040078BA RID: 30906
		private const float SunDrawSize = 15f;
	}
}

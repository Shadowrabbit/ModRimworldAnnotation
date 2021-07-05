using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001755 RID: 5973
	public class DebugTile
	{
		// Token: 0x17001674 RID: 5748
		// (get) Token: 0x060089E1 RID: 35297 RVA: 0x00318694 File Offset: 0x00316894
		private Vector2 ScreenPos
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(Find.WorldGrid.GetTileCenter(this.tile));
			}
		}

		// Token: 0x17001675 RID: 5749
		// (get) Token: 0x060089E2 RID: 35298 RVA: 0x003186AC File Offset: 0x003168AC
		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x060089E3 RID: 35299 RVA: 0x003186E4 File Offset: 0x003168E4
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x060089E4 RID: 35300 RVA: 0x00318718 File Offset: 0x00316918
		public void Draw()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			if (this.mesh == null)
			{
				Find.WorldGrid.GetTileVertices(this.tile, DebugTile.tmpVerts);
				for (int i = 0; i < DebugTile.tmpVerts.Count; i++)
				{
					Vector3 a = DebugTile.tmpVerts[i];
					DebugTile.tmpVerts[i] = a + a.normalized * 0.012f;
				}
				this.mesh = new Mesh();
				this.mesh.name = "DebugTile";
				this.mesh.SetVertices(DebugTile.tmpVerts);
				DebugTile.tmpIndices.Clear();
				for (int j = 0; j < DebugTile.tmpVerts.Count - 2; j++)
				{
					DebugTile.tmpIndices.Add(j + 2);
					DebugTile.tmpIndices.Add(j + 1);
					DebugTile.tmpIndices.Add(0);
				}
				this.mesh.SetTriangles(DebugTile.tmpIndices, 0);
			}
			Material material;
			if (this.customMat != null)
			{
				material = this.customMat;
			}
			else
			{
				material = WorldDebugMatsSpectrum.Mat(Mathf.RoundToInt(this.colorPct * 100f) % 100);
			}
			Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, material, WorldCameraManager.WorldLayer);
		}

		// Token: 0x060089E5 RID: 35301 RVA: 0x00318868 File Offset: 0x00316A68
		public void OnGUI()
		{
			if (!this.VisibleForCamera)
			{
				return;
			}
			Vector2 screenPos = this.ScreenPos;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 20f, 40f, 40f);
			if (this.displayString != null)
			{
				Widgets.Label(rect, this.displayString);
			}
		}

		// Token: 0x040057A4 RID: 22436
		public int tile;

		// Token: 0x040057A5 RID: 22437
		public string displayString;

		// Token: 0x040057A6 RID: 22438
		public float colorPct;

		// Token: 0x040057A7 RID: 22439
		public int ticksLeft;

		// Token: 0x040057A8 RID: 22440
		public Material customMat;

		// Token: 0x040057A9 RID: 22441
		private Mesh mesh;

		// Token: 0x040057AA RID: 22442
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x040057AB RID: 22443
		private static List<int> tmpIndices = new List<int>();
	}
}

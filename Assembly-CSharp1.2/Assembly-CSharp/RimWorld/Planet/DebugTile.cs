using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002042 RID: 8258
	public class DebugTile
	{
		// Token: 0x170019D4 RID: 6612
		// (get) Token: 0x0600AF0A RID: 44810 RVA: 0x00071F75 File Offset: 0x00070175
		private Vector2 ScreenPos
		{
			get
			{
				return GenWorldUI.WorldToUIPosition(Find.WorldGrid.GetTileCenter(this.tile));
			}
		}

		// Token: 0x170019D5 RID: 6613
		// (get) Token: 0x0600AF0B RID: 44811 RVA: 0x0032E4D4 File Offset: 0x0032C6D4
		private bool VisibleForCamera
		{
			get
			{
				return new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Contains(this.ScreenPos);
			}
		}

		// Token: 0x170019D6 RID: 6614
		// (get) Token: 0x0600AF0C RID: 44812 RVA: 0x0032E50C File Offset: 0x0032C70C
		public float DistanceToCamera
		{
			get
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(this.tile);
				return Vector3.Distance(Find.WorldCamera.transform.position, tileCenter);
			}
		}

		// Token: 0x0600AF0D RID: 44813 RVA: 0x0032E540 File Offset: 0x0032C740
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

		// Token: 0x0600AF0E RID: 44814 RVA: 0x0032E690 File Offset: 0x0032C890
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

		// Token: 0x04007851 RID: 30801
		public int tile;

		// Token: 0x04007852 RID: 30802
		public string displayString;

		// Token: 0x04007853 RID: 30803
		public float colorPct;

		// Token: 0x04007854 RID: 30804
		public int ticksLeft;

		// Token: 0x04007855 RID: 30805
		public Material customMat;

		// Token: 0x04007856 RID: 30806
		private Mesh mesh;

		// Token: 0x04007857 RID: 30807
		private static List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x04007858 RID: 30808
		private static List<int> tmpIndices = new List<int>();
	}
}

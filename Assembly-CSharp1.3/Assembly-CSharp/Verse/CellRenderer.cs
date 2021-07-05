using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200018E RID: 398
	public static class CellRenderer
	{
		// Token: 0x06000B34 RID: 2868 RVA: 0x0003CD84 File Offset: 0x0003AF84
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0003CDAB File Offset: 0x0003AFAB
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0003CDC6 File Offset: 0x0003AFC6
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0003CDD8 File Offset: 0x0003AFD8
		public static void RenderCell(IntVec3 c, Material mat)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(c))
			{
				return;
			}
			Vector3 position = c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, mat, 0);
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0003CE14 File Offset: 0x0003B014
		public static void RenderSpot(IntVec3 c, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), colorPct, scale);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x0003CE26 File Offset: 0x0003B026
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f, float scale = 0.15f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), scale);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x0003CE38 File Offset: 0x0003B038
		public static void RenderSpot(Vector3 loc, Material mat, float scale = 0.15f)
		{
			CellRenderer.InitFrame();
			if (!CellRenderer.viewRect.Contains(loc.ToIntVec3()))
			{
				return;
			}
			loc.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 s = new Vector3(scale, 1f, scale);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.circle, matrix, mat, 0);
		}

		// Token: 0x04000954 RID: 2388
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04000955 RID: 2389
		private static CellRect viewRect;
	}
}

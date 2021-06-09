using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024B RID: 587
	public static class CellRenderer
	{
		// Token: 0x06000EE4 RID: 3812 RVA: 0x0001133B File Offset: 0x0000F53B
		private static void InitFrame()
		{
			if (Time.frameCount != CellRenderer.lastCameraUpdateFrame)
			{
				CellRenderer.viewRect = Find.CameraDriver.CurrentViewRect;
				CellRenderer.lastCameraUpdateFrame = Time.frameCount;
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x00011362 File Offset: 0x0000F562
		private static Material MatFromColorPct(float colorPct, bool transparent)
		{
			return DebugMatsSpectrum.Mat(GenMath.PositiveMod(Mathf.RoundToInt(colorPct * 100f), 100), transparent);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0001137D File Offset: 0x0000F57D
		public static void RenderCell(IntVec3 c, float colorPct = 0.5f)
		{
			CellRenderer.RenderCell(c, CellRenderer.MatFromColorPct(colorPct, true));
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000B4820 File Offset: 0x000B2A20
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

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0001138C File Offset: 0x0000F58C
		public static void RenderSpot(Vector3 loc, float colorPct = 0.5f)
		{
			CellRenderer.RenderSpot(loc, CellRenderer.MatFromColorPct(colorPct, false), 0.15f);
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x000B485C File Offset: 0x000B2A5C
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

		// Token: 0x04000C45 RID: 3141
		private static int lastCameraUpdateFrame = -1;

		// Token: 0x04000C46 RID: 3142
		private static CellRect viewRect;
	}
}

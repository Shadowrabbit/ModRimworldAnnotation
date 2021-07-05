using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AC RID: 4780
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x0600722F RID: 29231 RVA: 0x002624DB File Offset: 0x002606DB
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06007230 RID: 29232 RVA: 0x0002974C File Offset: 0x0002794C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007231 RID: 29233 RVA: 0x0002974C File Offset: 0x0002794C
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}

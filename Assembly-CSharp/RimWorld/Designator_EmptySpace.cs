using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A0 RID: 6560
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x06009101 RID: 37121 RVA: 0x00061450 File Offset: 0x0005F650
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x06009102 RID: 37122 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06009103 RID: 37123 RVA: 0x0000FC33 File Offset: 0x0000DE33
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}

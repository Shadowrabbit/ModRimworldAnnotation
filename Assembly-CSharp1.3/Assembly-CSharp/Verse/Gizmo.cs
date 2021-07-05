using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FF RID: 1023
	public abstract class Gizmo
	{
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001EA9 RID: 7849 RVA: 0x000BF7B1 File Offset: 0x000BD9B1
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06001EAA RID: 7850
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms);

		// Token: 0x06001EAB RID: 7851 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06001EAC RID: 7852
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06001EAD RID: 7853 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000BF7BA File Offset: 0x000BD9BA
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000BF7C2 File Offset: 0x000BD9C2
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000BF7CB File Offset: 0x000BD9CB
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x040012B1 RID: 4785
		public bool disabled;

		// Token: 0x040012B2 RID: 4786
		public string disabledReason;

		// Token: 0x040012B3 RID: 4787
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x040012B4 RID: 4788
		public float order;

		// Token: 0x040012B5 RID: 4789
		public const float Height = 75f;
	}
}

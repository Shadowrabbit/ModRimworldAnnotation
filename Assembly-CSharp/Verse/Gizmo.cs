using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200071A RID: 1818
	public abstract class Gizmo
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002DEA RID: 11754 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool Visible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002DEB RID: 11755 RVA: 0x000242CE File Offset: 0x000224CE
		public virtual IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				yield break;
			}
		}

		// Token: 0x06002DEC RID: 11756
		public abstract GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth);

		// Token: 0x06002DED RID: 11757 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void GizmoUpdateOnMouseover()
		{
		}

		// Token: 0x06002DEE RID: 11758
		public abstract float GetWidth(float maxWidth);

		// Token: 0x06002DEF RID: 11759 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void ProcessInput(Event ev)
		{
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool GroupsWith(Gizmo other)
		{
			return false;
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void MergeWith(Gizmo other)
		{
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000242D7 File Offset: 0x000224D7
		public virtual bool InheritInteractionsFrom(Gizmo other)
		{
			return this.alsoClickIfOtherInGroupClicked;
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000242DF File Offset: 0x000224DF
		public virtual bool InheritFloatMenuInteractionsFrom(Gizmo other)
		{
			return this.InheritInteractionsFrom(other);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000242E8 File Offset: 0x000224E8
		public void Disable(string reason = null)
		{
			this.disabled = true;
			this.disabledReason = reason;
		}

		// Token: 0x04001F4E RID: 8014
		public bool disabled;

		// Token: 0x04001F4F RID: 8015
		public string disabledReason;

		// Token: 0x04001F50 RID: 8016
		public bool alsoClickIfOtherInGroupClicked = true;

		// Token: 0x04001F51 RID: 8017
		public float order;

		// Token: 0x04001F52 RID: 8018
		public const float Height = 75f;
	}
}

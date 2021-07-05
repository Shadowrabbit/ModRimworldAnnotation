using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F5 RID: 1013
	public class Command_Action : Command
	{
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000BEBA4 File Offset: 0x000BCDA4
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.iconDrawColorOverride;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000BEBCF File Offset: 0x000BCDCF
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.action();
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000BEBE3 File Offset: 0x000BCDE3
		public override void GizmoUpdateOnMouseover()
		{
			if (this.onHover != null)
			{
				this.onHover();
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x000BEBF8 File Offset: 0x000BCDF8
		public void SetColorOverride(Color color)
		{
			this.iconDrawColorOverride = new Color?(color);
		}

		// Token: 0x04001292 RID: 4754
		public Action action;

		// Token: 0x04001293 RID: 4755
		public Action onHover;

		// Token: 0x04001294 RID: 4756
		private Color? iconDrawColorOverride;
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001181 RID: 4481
	public class Command_Reloadable : Command_VerbTarget
	{
		// Token: 0x06006BDE RID: 27614 RVA: 0x00242EE0 File Offset: 0x002410E0
		public Command_Reloadable(CompReloadable comp)
		{
			this.comp = comp;
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06006BDF RID: 27615 RVA: 0x00242EEF File Offset: 0x002410EF
		public override string TopRightLabel
		{
			get
			{
				return this.comp.LabelRemaining;
			}
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06006BE0 RID: 27616 RVA: 0x00242EFC File Offset: 0x002410FC
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.overrideColor;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x06006BE1 RID: 27617 RVA: 0x00242F27 File Offset: 0x00241127
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x00242F3C File Offset: 0x0024113C
		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_Reloadable command_Reloadable = other as Command_Reloadable;
			return command_Reloadable != null && this.comp.parent.def == command_Reloadable.comp.parent.def;
		}

		// Token: 0x04003C09 RID: 15369
		private readonly CompReloadable comp;

		// Token: 0x04003C0A RID: 15370
		public Color? overrideColor;
	}
}

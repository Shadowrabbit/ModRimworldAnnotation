using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001826 RID: 6182
	public class Command_Reloadable : Command_VerbTarget
	{
		// Token: 0x06008910 RID: 35088 RVA: 0x0005C127 File Offset: 0x0005A327
		public Command_Reloadable(CompReloadable comp)
		{
			this.comp = comp;
		}

		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06008911 RID: 35089 RVA: 0x0005C136 File Offset: 0x0005A336
		public override string TopRightLabel
		{
			get
			{
				return this.comp.LabelRemaining;
			}
		}

		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06008912 RID: 35090 RVA: 0x00281038 File Offset: 0x0027F238
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

		// Token: 0x06008913 RID: 35091 RVA: 0x0005C143 File Offset: 0x0005A343
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x06008914 RID: 35092 RVA: 0x00281064 File Offset: 0x0027F264
		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_Reloadable command_Reloadable = other as Command_Reloadable;
			return command_Reloadable != null && this.comp.parent.def == command_Reloadable.comp.parent.def;
		}

		// Token: 0x040057F7 RID: 22519
		private readonly CompReloadable comp;

		// Token: 0x040057F8 RID: 22520
		public Color? overrideColor;
	}
}

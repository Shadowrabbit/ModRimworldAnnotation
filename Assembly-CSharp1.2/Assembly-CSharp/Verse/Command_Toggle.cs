using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000716 RID: 1814
	public class Command_Toggle : Command
	{
		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x00024214 File Offset: 0x00022414
		public override SoundDef CurActivateSound
		{
			get
			{
				if (this.isActive())
				{
					return this.turnOffSound;
				}
				return this.turnOnSound;
			}
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00024230 File Offset: 0x00022430
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0013593C File Offset: 0x00133B3C
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001359BC File Offset: 0x00133BBC
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x04001F3F RID: 7999
		public Func<bool> isActive;

		// Token: 0x04001F40 RID: 8000
		public Action toggleAction;

		// Token: 0x04001F41 RID: 8001
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x04001F42 RID: 8002
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x04001F43 RID: 8003
		public bool activateIfAmbiguous = true;
	}
}

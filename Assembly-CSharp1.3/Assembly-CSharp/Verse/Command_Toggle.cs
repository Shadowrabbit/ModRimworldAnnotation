using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FA RID: 1018
	public class Command_Toggle : Command
	{
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001E9A RID: 7834 RVA: 0x000BF4B8 File Offset: 0x000BD6B8
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

		// Token: 0x06001E9B RID: 7835 RVA: 0x000BF4D4 File Offset: 0x000BD6D4
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x000BF4E8 File Offset: 0x000BD6E8
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth, parms);
			Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
			Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
			Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000BF568 File Offset: 0x000BD768
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x0400129F RID: 4767
		public Func<bool> isActive;

		// Token: 0x040012A0 RID: 4768
		public Action toggleAction;

		// Token: 0x040012A1 RID: 4769
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x040012A2 RID: 4770
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x040012A3 RID: 4771
		public bool activateIfAmbiguous = true;
	}
}

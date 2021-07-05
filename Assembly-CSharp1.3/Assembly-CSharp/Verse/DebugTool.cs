using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B7 RID: 951
	public class DebugTool
	{
		// Token: 0x06001D72 RID: 7538 RVA: 0x000B7ADE File Offset: 0x000B5CDE
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000B7AFC File Offset: 0x000B5CFC
		public DebugTool(string label, Action clickAction, IntVec3 firstRectCorner)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = delegate()
			{
				IntVec3 intVec = UI.MouseCell();
				Vector3 vector = firstRectCorner.ToVector3Shifted();
				Vector3 vector2 = intVec.ToVector3Shifted();
				if (vector.x < vector2.x)
				{
					vector.x -= 0.5f;
					vector2.x += 0.5f;
				}
				else
				{
					vector.x += 0.5f;
					vector2.x -= 0.5f;
				}
				if (vector.z < vector2.z)
				{
					vector.z -= 0.5f;
					vector2.z += 0.5f;
				}
				else
				{
					vector.z += 0.5f;
					vector2.z -= 0.5f;
				}
				Vector2 vector3 = vector.MapToUIPosition();
				Vector2 vector4 = vector2.MapToUIPosition();
				Widgets.DrawBox(new Rect(vector3.x, vector3.y, vector4.x - vector3.x, vector4.y - vector3.y), 3, null);
			};
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000B7B3C File Offset: 0x000B5D3C
		public void DebugToolOnGUI()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0)
				{
					this.clickAction();
				}
				if (Event.current.button == 1)
				{
					DebugTools.curTool = null;
				}
				Event.current.Use();
			}
			Vector2 vector = Event.current.mousePosition + new Vector2(15f, 15f);
			Rect rect = new Rect(vector.x, vector.y, 999f, 999f);
			Text.Font = GameFont.Small;
			Widgets.Label(rect, this.label);
			if (this.onGUIAction != null)
			{
				this.onGUIAction();
			}
		}

		// Token: 0x040011A7 RID: 4519
		private string label;

		// Token: 0x040011A8 RID: 4520
		private Action clickAction;

		// Token: 0x040011A9 RID: 4521
		private Action onGUIAction;
	}
}

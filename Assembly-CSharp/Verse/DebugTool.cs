using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006A5 RID: 1701
	public class DebugTool
	{
		// Token: 0x06002C55 RID: 11349 RVA: 0x000233EC File Offset: 0x000215EC
		public DebugTool(string label, Action clickAction, Action onGUIAction = null)
		{
			this.label = label;
			this.clickAction = clickAction;
			this.onGUIAction = onGUIAction;
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0012EAB0 File Offset: 0x0012CCB0
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
				Widgets.DrawBox(new Rect(vector3.x, vector3.y, vector4.x - vector3.x, vector4.y - vector3.y), 3);
			};
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x0012EAF0 File Offset: 0x0012CCF0
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

		// Token: 0x04001DF8 RID: 7672
		private string label;

		// Token: 0x04001DF9 RID: 7673
		private Action clickAction;

		// Token: 0x04001DFA RID: 7674
		private Action onGUIAction;
	}
}

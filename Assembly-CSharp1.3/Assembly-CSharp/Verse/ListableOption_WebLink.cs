using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000434 RID: 1076
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06002067 RID: 8295 RVA: 0x000C8D7D File Offset: 0x000C6F7D
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000C8D9A File Offset: 0x000C6F9A
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x06002069 RID: 8297 RVA: 0x000C8DAB File Offset: 0x000C6FAB
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x0600206A RID: 8298 RVA: 0x000C8DBC File Offset: 0x000C6FBC
		public override float DrawOption(Vector2 pos, float width)
		{
			float num = width - ListableOption_WebLink.Imagesize.x - 3f;
			float num2 = Text.CalcHeight(this.label, num);
			float num3 = Mathf.Max(this.minHeight, num2);
			Rect rect = new Rect(pos.x, pos.y, width, num3);
			GUI.color = Color.white;
			if (this.image != null)
			{
				Rect position = new Rect(pos.x, pos.y + num3 / 2f - ListableOption_WebLink.Imagesize.y / 2f, ListableOption_WebLink.Imagesize.x, ListableOption_WebLink.Imagesize.y);
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
				GUI.DrawTexture(position, this.image);
			}
			Widgets.Label(new Rect(rect.xMax - num, pos.y, num, num2), this.label);
			GUI.color = Color.white;
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.action != null)
				{
					this.action();
				}
				else
				{
					Application.OpenURL(this.url);
				}
			}
			return num3;
		}

		// Token: 0x040013A9 RID: 5033
		public Texture2D image;

		// Token: 0x040013AA RID: 5034
		public string url;

		// Token: 0x040013AB RID: 5035
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}

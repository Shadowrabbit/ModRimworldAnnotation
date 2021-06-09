using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000776 RID: 1910
	public class ListableOption_WebLink : ListableOption
	{
		// Token: 0x06003012 RID: 12306 RVA: 0x00025E21 File Offset: 0x00024021
		public ListableOption_WebLink(string label, Texture2D image) : base(label, null, null)
		{
			this.minHeight = 24f;
			this.image = image;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x00025E3E File Offset: 0x0002403E
		public ListableOption_WebLink(string label, string url, Texture2D image) : this(label, image)
		{
			this.url = url;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x00025E4F File Offset: 0x0002404F
		public ListableOption_WebLink(string label, Action action, Texture2D image) : this(label, image)
		{
			this.action = action;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x0013D7DC File Offset: 0x0013B9DC
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

		// Token: 0x04002098 RID: 8344
		public Texture2D image;

		// Token: 0x04002099 RID: 8345
		public string url;

		// Token: 0x0400209A RID: 8346
		private static readonly Vector2 Imagesize = new Vector2(24f, 18f);
	}
}

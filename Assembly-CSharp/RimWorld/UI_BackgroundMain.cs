using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAF RID: 6831
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x060096ED RID: 38637 RVA: 0x002C05CC File Offset: 0x002BE7CC
		public override void BackgroundOnGUI()
		{
			Vector2 vector = (this.overrideBGImage != null) ? new Vector2((float)this.overrideBGImage.width, (float)this.overrideBGImage.height) : UI_BackgroundMain.BGPlanetSize;
			bool flag = true;
			if ((float)UI.screenWidth > (float)UI.screenHeight * (vector.x / vector.y))
			{
				flag = false;
			}
			Rect rect;
			if (flag)
			{
				float height = (float)UI.screenHeight;
				float num = (float)UI.screenHeight * (vector.x / vector.y);
				rect = new Rect((float)(UI.screenWidth / 2) - num / 2f, 0f, num, height);
			}
			else
			{
				float width = (float)UI.screenWidth;
				float num2 = (float)UI.screenWidth * (vector.y / vector.x);
				rect = new Rect(0f, (float)(UI.screenHeight / 2) - num2 / 2f, width, num2);
			}
			GUI.DrawTexture(rect, this.overrideBGImage ?? UI_BackgroundMain.BGPlanet, ScaleMode.ScaleToFit);
			this.DoOverlay(rect);
		}

		// Token: 0x060096EE RID: 38638 RVA: 0x002C06CC File Offset: 0x002BE8CC
		private void DoOverlay(Rect bgRect)
		{
			if (this.overlayImage != null)
			{
				if (this.fadeIn && this.curColor.a < 1f)
				{
					this.curColor.a = this.curColor.a + 0.04f;
				}
				else if (this.curColor.a > 0f)
				{
					this.curColor.a = this.curColor.a - 0.04f;
				}
				this.curColor.a = Mathf.Clamp01(this.curColor.a);
				GUI.color = this.curColor;
				GUI.DrawTexture(bgRect, this.overlayImage, ScaleMode.ScaleAndCrop);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060096EF RID: 38639 RVA: 0x00064C51 File Offset: 0x00062E51
		public void FadeOut()
		{
			this.fadeIn = false;
		}

		// Token: 0x060096F0 RID: 38640 RVA: 0x00064C5A File Offset: 0x00062E5A
		public void SetOverlayImage(Texture2D texture)
		{
			if (texture != null)
			{
				this.overlayImage = texture;
				this.fadeIn = true;
			}
		}

		// Token: 0x0400604A RID: 24650
		private Color curColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x0400604B RID: 24651
		private Texture2D overlayImage;

		// Token: 0x0400604C RID: 24652
		public Texture2D overrideBGImage;

		// Token: 0x0400604D RID: 24653
		private bool fadeIn;

		// Token: 0x0400604E RID: 24654
		private const float DeltaAlpha = 0.04f;

		// Token: 0x0400604F RID: 24655
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x04006050 RID: 24656
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}

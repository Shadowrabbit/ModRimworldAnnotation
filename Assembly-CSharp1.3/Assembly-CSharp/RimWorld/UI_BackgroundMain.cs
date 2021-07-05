using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132D RID: 4909
	[StaticConstructorOnStartup]
	public class UI_BackgroundMain : UIMenuBackground
	{
		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x060076BA RID: 30394 RVA: 0x00293BDA File Offset: 0x00291DDA
		private float DeltaAlpha
		{
			get
			{
				return Time.deltaTime * 2f;
			}
		}

		// Token: 0x060076BB RID: 30395 RVA: 0x00293BE8 File Offset: 0x00291DE8
		public void SetupExpansionFadeData()
		{
			this.expansionImageFades = new Dictionary<ExpansionDef, float>();
			foreach (ExpansionDef key in ModLister.AllExpansions)
			{
				this.expansionImageFades.Add(key, 0f);
			}
		}

		// Token: 0x060076BC RID: 30396 RVA: 0x00293C50 File Offset: 0x00291E50
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
			if (Event.current.type == EventType.Repaint)
			{
				this.DoOverlay(rect);
			}
		}

		// Token: 0x060076BD RID: 30397 RVA: 0x00293D5C File Offset: 0x00291F5C
		private void DoOverlay(Rect bgRect)
		{
			if (this.expansionImageFades == null)
			{
				return;
			}
			foreach (ExpansionDef expansionDef in ModLister.AllExpansions)
			{
				if (!expansionDef.isCore && !expansionDef.BackgroundImage.NullOrBad() && this.expansionImageFades[expansionDef] > 0f)
				{
					if (expansionDef.BackgroundImage != this.overrideBGImage)
					{
						GUI.color = new Color(1f, 1f, 1f, this.expansionImageFades[expansionDef]);
						GUI.DrawTexture(bgRect, expansionDef.BackgroundImage, ScaleMode.ScaleAndCrop);
						GUI.color = Color.white;
					}
					this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] - this.DeltaAlpha / 2f);
				}
			}
		}

		// Token: 0x060076BE RID: 30398 RVA: 0x00293E60 File Offset: 0x00292060
		public void Notify_Hovered(ExpansionDef expansionDef)
		{
			if (Event.current.type == EventType.Repaint)
			{
				this.expansionImageFades[expansionDef] = Mathf.Clamp01(this.expansionImageFades[expansionDef] + this.DeltaAlpha);
			}
		}

		// Token: 0x040041F7 RID: 16887
		public Texture2D overrideBGImage;

		// Token: 0x040041F8 RID: 16888
		private Dictionary<ExpansionDef, float> expansionImageFades;

		// Token: 0x040041F9 RID: 16889
		private static readonly Vector2 BGPlanetSize = new Vector2(2048f, 1280f);

		// Token: 0x040041FA RID: 16890
		private static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("UI/HeroArt/BGPlanet", true);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003E5 RID: 997
	public class FloatMenu : Window
	{
		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000BC110 File Offset: 0x000BA310
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x000BC123 File Offset: 0x000BA323
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x000BC131 File Offset: 0x000BA331
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06001E0D RID: 7693 RVA: 0x000BC14C File Offset: 0x000BA34C
		private float MaxViewHeight
		{
			get
			{
				if (this.UsingScrollbar)
				{
					float num = 0f;
					float num2 = 0f;
					for (int i = 0; i < this.options.Count; i++)
					{
						float requiredHeight = this.options[i].RequiredHeight;
						if (requiredHeight > num)
						{
							num = requiredHeight;
						}
						num2 += requiredHeight + -1f;
					}
					int columnCount = this.ColumnCount;
					num2 += (float)columnCount * num;
					return num2 / (float)columnCount;
				}
				return this.MaxWindowHeight;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x000BC1C4 File Offset: 0x000BA3C4
		private float TotalViewHeight
		{
			get
			{
				float num = 0f;
				float num2 = 0f;
				float maxViewHeight = this.MaxViewHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxViewHeight)
					{
						if (num2 > num)
						{
							num = num2;
						}
						num2 = requiredHeight;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return Mathf.Max(num, num2);
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x000BC238 File Offset: 0x000BA438
		private float TotalWidth
		{
			get
			{
				float num = (float)this.ColumnCount * this.ColumnWidth;
				if (this.UsingScrollbar)
				{
					num += 16f;
				}
				return num;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001E10 RID: 7696 RVA: 0x000BC268 File Offset: 0x000BA468
		private float ColumnWidth
		{
			get
			{
				float num = 70f;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredWidth = this.options[i].RequiredWidth;
					if (requiredWidth >= 300f)
					{
						return 300f;
					}
					if (requiredWidth > num)
					{
						num = requiredWidth;
					}
				}
				return Mathf.Round(num);
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x000BC2BD File Offset: 0x000BA4BD
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001E12 RID: 7698 RVA: 0x000BC2D7 File Offset: 0x000BA4D7
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x000BC2E7 File Offset: 0x000BA4E7
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x000BC2FC File Offset: 0x000BA4FC
		private int ColumnCountIfNoScrollbar
		{
			get
			{
				if (this.options == null)
				{
					return 1;
				}
				Text.Font = GameFont.Small;
				int num = 1;
				float num2 = 0f;
				float maxWindowHeight = this.MaxWindowHeight;
				for (int i = 0; i < this.options.Count; i++)
				{
					float requiredHeight = this.options[i].RequiredHeight;
					if (num2 + requiredHeight + -1f > maxWindowHeight)
					{
						num2 = requiredHeight;
						num++;
					}
					else
					{
						num2 += requiredHeight + -1f;
					}
				}
				return num;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x000BC373 File Offset: 0x000BA573
		public FloatMenuSizeMode SizeMode
		{
			get
			{
				if (this.options.Count > 60)
				{
					return FloatMenuSizeMode.Tiny;
				}
				return FloatMenuSizeMode.Normal;
			}
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x000BC388 File Offset: 0x000BA588
		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.");
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending, op.orderInPriority descending
			select op).ToList<FloatMenuOption>();
			for (int i = 0; i < options.Count; i++)
			{
				options[i].SetSizeMode(this.SizeMode);
			}
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06001E17 RID: 7703 RVA: 0x000BC46B File Offset: 0x000BA66B
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000BC484 File Offset: 0x000BA684
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + FloatMenu.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000BC534 File Offset: 0x000BA734
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			if (!this.title.NullOrEmpty())
			{
				Vector2 vector = new Vector2(this.windowRect.x, this.windowRect.y);
				Text.Font = GameFont.Small;
				float width = Mathf.Max(150f, 15f + Text.CalcSize(this.title).x);
				Rect titleRect = new Rect(vector.x + FloatMenu.TitleOffset.x, vector.y + FloatMenu.TitleOffset.y, width, 23f);
				Find.WindowStack.ImmediateWindow(6830963, titleRect, WindowLayer.Super, delegate
				{
					GUI.color = this.baseColor;
					Text.Font = GameFont.Small;
					Rect position = titleRect.AtZero();
					position.width = 150f;
					GUI.DrawTexture(position, TexUI.TextBGBlack);
					Rect rect = titleRect.AtZero();
					rect.x += 15f;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, this.title);
					Text.Anchor = TextAnchor.UpperLeft;
				}, false, false, 0f, null);
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000BC608 File Offset: 0x000BA808
		public override void DoWindowContents(Rect rect)
		{
			if (this.needSelection && Find.Selector.SingleSelectedThing == null)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			this.UpdateBaseColor();
			bool usingScrollbar = this.UsingScrollbar;
			GUI.color = this.baseColor;
			Text.Font = GameFont.Small;
			Vector2 zero = Vector2.zero;
			float maxViewHeight = this.MaxViewHeight;
			float columnWidth = this.ColumnWidth;
			if (usingScrollbar)
			{
				rect.width -= 10f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, new Rect(0f, 0f, this.TotalWidth - 16f, this.TotalViewHeight), true);
			}
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuOption floatMenuOption = this.options[i];
				float requiredHeight = floatMenuOption.RequiredHeight;
				if (zero.y + requiredHeight + -1f > maxViewHeight)
				{
					zero.y = 0f;
					zero.x += columnWidth + -1f;
				}
				Rect rect2 = new Rect(zero.x, zero.y, columnWidth, requiredHeight);
				zero.y += requiredHeight + -1f;
				if (floatMenuOption.DoGUI(rect2, this.givesColonistOrders, this))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
			}
			if (usingScrollbar)
			{
				Widgets.EndScrollView();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
				this.Close(true);
			}
			GUI.color = Color.white;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000BC789 File Offset: 0x000BA989
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000BC7A4 File Offset: 0x000BA9A4
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000BC7C0 File Offset: 0x000BA9C0
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			if (this.vanishIfMouseDistant)
			{
				Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalWindowHeight).ContractedBy(-5f);
				if (!r.Contains(Event.current.mousePosition))
				{
					float num = GenUI.DistFromRect(r, Event.current.mousePosition);
					this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
					if (num > 95f)
					{
						this.Close(false);
						this.Cancel();
						return;
					}
				}
			}
		}

		// Token: 0x0400121B RID: 4635
		public bool givesColonistOrders;

		// Token: 0x0400121C RID: 4636
		public bool vanishIfMouseDistant = true;

		// Token: 0x0400121D RID: 4637
		public Action onCloseCallback;

		// Token: 0x0400121E RID: 4638
		protected List<FloatMenuOption> options;

		// Token: 0x0400121F RID: 4639
		private string title;

		// Token: 0x04001220 RID: 4640
		private bool needSelection;

		// Token: 0x04001221 RID: 4641
		private Color baseColor = Color.white;

		// Token: 0x04001222 RID: 4642
		private Vector2 scrollPosition;

		// Token: 0x04001223 RID: 4643
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x04001224 RID: 4644
		private const float OptionSpacing = -1f;

		// Token: 0x04001225 RID: 4645
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04001226 RID: 4646
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04001227 RID: 4647
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04001228 RID: 4648
		private const float FadeStartMouseDist = 5f;

		// Token: 0x04001229 RID: 4649
		private const float FadeFinishMouseDist = 100f;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020006FA RID: 1786
	public class FloatMenu : Window
	{
		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06002D46 RID: 11590 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06002D47 RID: 11591 RVA: 0x00023BB7 File Offset: 0x00021DB7
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalWindowHeight);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06002D48 RID: 11592 RVA: 0x00023BCA File Offset: 0x00021DCA
		private float MaxWindowHeight
		{
			get
			{
				return (float)UI.screenHeight * 0.9f;
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x00023BD8 File Offset: 0x00021DD8
		private float TotalWindowHeight
		{
			get
			{
				return Mathf.Min(this.TotalViewHeight, this.MaxWindowHeight) + 1f;
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x00132F64 File Offset: 0x00131164
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

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x00132FDC File Offset: 0x001311DC
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

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06002D4C RID: 11596 RVA: 0x00133050 File Offset: 0x00131250
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

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x00133080 File Offset: 0x00131280
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

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06002D4E RID: 11598 RVA: 0x00023BF1 File Offset: 0x00021DF1
		private int MaxColumns
		{
			get
			{
				return Mathf.FloorToInt(((float)UI.screenWidth - 16f) / this.ColumnWidth);
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06002D4F RID: 11599 RVA: 0x00023C0B File Offset: 0x00021E0B
		private bool UsingScrollbar
		{
			get
			{
				return this.ColumnCountIfNoScrollbar > this.MaxColumns;
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06002D50 RID: 11600 RVA: 0x00023C1B File Offset: 0x00021E1B
		private int ColumnCount
		{
			get
			{
				return Mathf.Min(this.ColumnCountIfNoScrollbar, this.MaxColumns);
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06002D51 RID: 11601 RVA: 0x001330D8 File Offset: 0x001312D8
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

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06002D52 RID: 11602 RVA: 0x00023C2E File Offset: 0x00021E2E
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

		// Token: 0x06002D53 RID: 11603 RVA: 0x00133150 File Offset: 0x00131350
		public FloatMenu(List<FloatMenuOption> options)
		{
			if (options.NullOrEmpty<FloatMenuOption>())
			{
				Log.Error("Created FloatMenu with no options. Closing.", false);
				this.Close(true);
			}
			this.options = (from op in options
			orderby op.Priority descending
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

		// Token: 0x06002D54 RID: 11604 RVA: 0x00023C42 File Offset: 0x00021E42
		public FloatMenu(List<FloatMenuOption> options, string title, bool needSelection = false) : this(options)
		{
			this.title = title;
			this.needSelection = needSelection;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x00133210 File Offset: 0x00131410
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

		// Token: 0x06002D56 RID: 11606 RVA: 0x001332C0 File Offset: 0x001314C0
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
				}, false, false, 0f);
			}
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x00133390 File Offset: 0x00131590
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

		// Token: 0x06002D58 RID: 11608 RVA: 0x00023C59 File Offset: 0x00021E59
		public override void PostClose()
		{
			base.PostClose();
			if (this.onCloseCallback != null)
			{
				this.onCloseCallback();
			}
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x00023C74 File Offset: 0x00021E74
		public void Cancel()
		{
			SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
			Find.WindowStack.TryRemove(this, true);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreOptionChosen(FloatMenuOption opt)
		{
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x00133514 File Offset: 0x00131714
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

		// Token: 0x04001EBA RID: 7866
		public bool givesColonistOrders;

		// Token: 0x04001EBB RID: 7867
		public bool vanishIfMouseDistant = true;

		// Token: 0x04001EBC RID: 7868
		public Action onCloseCallback;

		// Token: 0x04001EBD RID: 7869
		protected List<FloatMenuOption> options;

		// Token: 0x04001EBE RID: 7870
		private string title;

		// Token: 0x04001EBF RID: 7871
		private bool needSelection;

		// Token: 0x04001EC0 RID: 7872
		private Color baseColor = Color.white;

		// Token: 0x04001EC1 RID: 7873
		private Vector2 scrollPosition;

		// Token: 0x04001EC2 RID: 7874
		private static readonly Vector2 TitleOffset = new Vector2(30f, -25f);

		// Token: 0x04001EC3 RID: 7875
		private const float OptionSpacing = -1f;

		// Token: 0x04001EC4 RID: 7876
		private const float MaxScreenHeightPercent = 0.9f;

		// Token: 0x04001EC5 RID: 7877
		private const float MinimumColumnWidth = 70f;

		// Token: 0x04001EC6 RID: 7878
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);

		// Token: 0x04001EC7 RID: 7879
		private const float FadeStartMouseDist = 5f;

		// Token: 0x04001EC8 RID: 7880
		private const float FadeFinishMouseDist = 100f;
	}
}

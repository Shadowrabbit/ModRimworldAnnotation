using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000700 RID: 1792
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06002D6A RID: 11626 RVA: 0x00023D31 File Offset: 0x00021F31
		// (set) Token: 0x06002D6B RID: 11627 RVA: 0x00023D39 File Offset: 0x00021F39
		public string Label
		{
			get
			{
				return this.labelInt;
			}
			set
			{
				if (value.NullOrEmpty())
				{
					value = "(missing label)";
				}
				this.labelInt = value.TrimEnd(Array.Empty<char>());
				this.SetSizeMode(this.sizeMode);
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06002D6C RID: 11628 RVA: 0x00023D67 File Offset: 0x00021F67
		private float VerticalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 1f;
				}
				return 4f;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06002D6D RID: 11629 RVA: 0x00023D7D File Offset: 0x00021F7D
		private float HorizontalMargin
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return 3f;
				}
				return 6f;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06002D6E RID: 11630 RVA: 0x00023D93 File Offset: 0x00021F93
		private float IconOffset
		{
			get
			{
				if (this.shownItem == null && !this.drawPlaceHolderIcon && !(this.itemIcon != null))
				{
					return 0f;
				}
				return 27f;
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06002D6F RID: 11631 RVA: 0x00023DBE File Offset: 0x00021FBE
		private GameFont CurrentFont
		{
			get
			{
				if (this.sizeMode != FloatMenuSizeMode.Normal)
				{
					return GameFont.Tiny;
				}
				return GameFont.Small;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06002D70 RID: 11632 RVA: 0x00023DCC File Offset: 0x00021FCC
		// (set) Token: 0x06002D71 RID: 11633 RVA: 0x00023DD7 File Offset: 0x00021FD7
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
			set
			{
				if (value)
				{
					this.action = null;
				}
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x00023DE3 File Offset: 0x00021FE3
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06002D73 RID: 11635 RVA: 0x00023DEB File Offset: 0x00021FEB
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x00023DF3 File Offset: 0x00021FF3
		// (set) Token: 0x06002D75 RID: 11637 RVA: 0x00023E05 File Offset: 0x00022005
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return this.priorityInt;
			}
			set
			{
				if (this.Disabled)
				{
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label, false);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x001338B0 File Offset: 0x00131AB0
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x00133914 File Offset: 0x00131B14
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.shownItem = shownItemForIcon;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x00133948 File Offset: 0x00131B48
		public FloatMenuOption(string label, Action action, Texture2D itemIcon, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget)
		{
			this.itemIcon = itemIcon;
			this.iconColor = iconColor;
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x00133978 File Offset: 0x00131B78
		public void SetSizeMode(FloatMenuSizeMode newSizeMode)
		{
			this.sizeMode = newSizeMode;
			GameFont font = Text.Font;
			Text.Font = this.CurrentFont;
			float width = 300f - (2f * this.HorizontalMargin + 4f + this.extraPartWidth + this.IconOffset);
			this.cachedRequiredHeight = 2f * this.VerticalMargin + Text.CalcHeight(this.Label, width);
			this.cachedRequiredWidth = this.HorizontalMargin + 4f + Text.CalcSize(this.Label).x + this.extraPartWidth + this.HorizontalMargin + this.IconOffset + 4f;
			Text.Font = font;
		}

		// Token: 0x06002D7A RID: 11642 RVA: 0x00133A28 File Offset: 0x00131C28
		public void Chosen(bool colonistOrdering, FloatMenu floatMenu)
		{
			if (floatMenu != null)
			{
				floatMenu.PreOptionChosen(this);
			}
			if (!this.Disabled)
			{
				if (this.action != null)
				{
					if (colonistOrdering)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002D7B RID: 11643 RVA: 0x00133A74 File Offset: 0x00131C74
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			Rect rect3 = rect;
			rect3.xMin += 4f;
			rect3.xMax = rect.x + 27f;
			rect3.yMin += 4f;
			rect3.yMax = rect.y + 27f;
			if (flag)
			{
				rect3.x += 4f;
			}
			Rect rect4 = rect;
			rect4.xMin += this.HorizontalMargin;
			rect4.xMax -= this.HorizontalMargin;
			rect4.xMax -= 4f;
			rect4.xMax -= this.extraPartWidth + this.IconOffset;
			rect4.x += this.IconOffset;
			if (flag)
			{
				rect4.x += 4f;
			}
			Rect rect5 = default(Rect);
			if (this.extraPartWidth != 0f)
			{
				float num = Mathf.Min(Text.CalcSize(this.Label).x, rect4.width - 4f);
				rect5 = new Rect(rect4.xMin + num, rect4.yMin, this.extraPartWidth, 30f);
				flag2 = Mouse.IsOver(rect5);
			}
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect2);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag && !flag2)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			if (this.sizeMode == FloatMenuSizeMode.Tiny)
			{
				rect4.y += 1f;
			}
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect4, this.Label);
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = this.iconColor;
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, this.drawPlaceHolderIcon);
			}
			else if (this.itemIcon)
			{
				GUI.DrawTexture(rect3, this.itemIcon);
			}
			GUI.color = color;
			if (this.extraPartOnGUI != null)
			{
				bool flag3 = this.extraPartOnGUI(rect5);
				GUI.color = color;
				if (flag3)
				{
					return true;
				}
			}
			if (flag && this.mouseoverGuiAction != null)
			{
				this.mouseoverGuiAction();
			}
			if (this.tutorTag != null)
			{
				UIHighlighter.HighlightOpportunity(rect, this.tutorTag);
			}
			if (!Widgets.ButtonInvisible(rect2, true))
			{
				return false;
			}
			if (this.tutorTag != null && !TutorSystem.AllowAction(this.tutorTag))
			{
				return false;
			}
			this.Chosen(colonistOrdering, floatMenu);
			if (this.tutorTag != null)
			{
				TutorSystem.Notify_Event(this.tutorTag);
			}
			return true;
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x00133DB4 File Offset: 0x00131FB4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"FloatMenuOption(",
				this.Label,
				", ",
				this.Disabled ? "disabled" : "enabled",
				")"
			});
		}

		// Token: 0x04001EDE RID: 7902
		private string labelInt;

		// Token: 0x04001EDF RID: 7903
		public Action action;

		// Token: 0x04001EE0 RID: 7904
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x04001EE1 RID: 7905
		public bool autoTakeable;

		// Token: 0x04001EE2 RID: 7906
		public float autoTakeablePriority;

		// Token: 0x04001EE3 RID: 7907
		public Action mouseoverGuiAction;

		// Token: 0x04001EE4 RID: 7908
		public Thing revalidateClickTarget;

		// Token: 0x04001EE5 RID: 7909
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x04001EE6 RID: 7910
		public float extraPartWidth;

		// Token: 0x04001EE7 RID: 7911
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x04001EE8 RID: 7912
		public string tutorTag;

		// Token: 0x04001EE9 RID: 7913
		private FloatMenuSizeMode sizeMode;

		// Token: 0x04001EEA RID: 7914
		private float cachedRequiredHeight;

		// Token: 0x04001EEB RID: 7915
		private float cachedRequiredWidth;

		// Token: 0x04001EEC RID: 7916
		private bool drawPlaceHolderIcon;

		// Token: 0x04001EED RID: 7917
		private ThingDef shownItem;

		// Token: 0x04001EEE RID: 7918
		private Texture2D itemIcon;

		// Token: 0x04001EEF RID: 7919
		private Color iconColor = Color.white;

		// Token: 0x04001EF0 RID: 7920
		public const float MaxWidth = 300f;

		// Token: 0x04001EF1 RID: 7921
		private const float NormalVerticalMargin = 4f;

		// Token: 0x04001EF2 RID: 7922
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04001EF3 RID: 7923
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04001EF4 RID: 7924
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x04001EF5 RID: 7925
		private const float MouseOverLabelShift = 4f;

		// Token: 0x04001EF6 RID: 7926
		private static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04001EF7 RID: 7927
		private static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x04001EF8 RID: 7928
		private static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x04001EF9 RID: 7929
		private static readonly Color ColorTextActive = Color.white;

		// Token: 0x04001EFA RID: 7930
		private static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x04001EFB RID: 7931
		public const float ExtraPartHeight = 30f;

		// Token: 0x04001EFC RID: 7932
		private const float ItemIconSize = 27f;

		// Token: 0x04001EFD RID: 7933
		private const float ItemIconMargin = 4f;
	}
}

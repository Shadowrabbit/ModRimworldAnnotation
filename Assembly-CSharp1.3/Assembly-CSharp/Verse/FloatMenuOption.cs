using System;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003E8 RID: 1000
	[StaticConstructorOnStartup]
	public class FloatMenuOption
	{
		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x000BCB5D File Offset: 0x000BAD5D
		// (set) Token: 0x06001E28 RID: 7720 RVA: 0x000BCB65 File Offset: 0x000BAD65
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

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x000BCB93 File Offset: 0x000BAD93
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

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001E2A RID: 7722 RVA: 0x000BCBA9 File Offset: 0x000BADA9
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

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x000BCBBF File Offset: 0x000BADBF
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

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001E2C RID: 7724 RVA: 0x000BCBEA File Offset: 0x000BADEA
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

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x000BCBF8 File Offset: 0x000BADF8
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x000BCC03 File Offset: 0x000BAE03
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

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x000BCC0F File Offset: 0x000BAE0F
		public float RequiredHeight
		{
			get
			{
				return this.cachedRequiredHeight;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001E30 RID: 7728 RVA: 0x000BCC17 File Offset: 0x000BAE17
		public float RequiredWidth
		{
			get
			{
				return this.cachedRequiredWidth;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x000BCC1F File Offset: 0x000BAE1F
		// (set) Token: 0x06001E32 RID: 7730 RVA: 0x000BCC31 File Offset: 0x000BAE31
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
					Log.Error("Setting priority on disabled FloatMenuOption: " + this.Label);
				}
				this.priorityInt = value;
			}
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000BCC58 File Offset: 0x000BAE58
		public FloatMenuOption(string label, Action action, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0)
		{
			this.Label = label;
			this.action = action;
			this.priorityInt = priority;
			this.revalidateClickTarget = revalidateClickTarget;
			this.mouseoverGuiAction = mouseoverGuiAction;
			this.extraPartWidth = extraPartWidth;
			this.extraPartOnGUI = extraPartOnGUI;
			this.revalidateWorldClickTarget = revalidateWorldClickTarget;
			this.playSelectionSound = playSelectionSound;
			this.orderInPriority = orderInPriority;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000BCCD4 File Offset: 0x000BAED4
		public FloatMenuOption(string label, Action action, ThingDef shownItemForIcon, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.shownItem = shownItemForIcon;
			if (shownItemForIcon == null)
			{
				this.drawPlaceHolderIcon = true;
			}
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000BCD0C File Offset: 0x000BAF0C
		public FloatMenuOption(string label, Action action, Texture2D itemIcon, Color iconColor, MenuOptionPriority priority = MenuOptionPriority.Default, Action<Rect> mouseoverGuiAction = null, Thing revalidateClickTarget = null, float extraPartWidth = 0f, Func<Rect, bool> extraPartOnGUI = null, WorldObject revalidateWorldClickTarget = null, bool playSelectionSound = true, int orderInPriority = 0) : this(label, action, priority, mouseoverGuiAction, revalidateClickTarget, extraPartWidth, extraPartOnGUI, revalidateWorldClickTarget, playSelectionSound, orderInPriority)
		{
			this.itemIcon = itemIcon;
			this.iconColor = iconColor;
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000BCD40 File Offset: 0x000BAF40
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

		// Token: 0x06001E37 RID: 7735 RVA: 0x000BCDF0 File Offset: 0x000BAFF0
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
					if (colonistOrdering && this.playSelectionSound)
					{
						SoundDefOf.ColonistOrdered.PlayOneShotOnCamera(null);
					}
					this.action();
					return;
				}
			}
			else if (this.playSelectionSound)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000BCE4C File Offset: 0x000BB04C
		public virtual bool DoGUI(Rect rect, bool colonistOrdering, FloatMenu floatMenu)
		{
			Rect rect2 = rect;
			float height = rect2.height;
			rect2.height = height - 1f;
			bool flag = !this.Disabled && Mouse.IsOver(rect2);
			bool flag2 = false;
			Text.Font = this.CurrentFont;
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
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
			GUI.color = new Color(this.iconColor.r, this.iconColor.g, this.iconColor.b, this.iconColor.a * GUI.color.a);
			if (this.shownItem != null || this.drawPlaceHolderIcon)
			{
				ThingStyleDef thingStyleDef;
				if ((thingStyleDef = this.thingStyle) == null)
				{
					if (this.shownItem == null || Find.World == null)
					{
						thingStyleDef = null;
					}
					else
					{
						FactionIdeosTracker ideos = Faction.OfPlayer.ideos;
						if (ideos == null)
						{
							thingStyleDef = null;
						}
						else
						{
							Ideo primaryIdeo = ideos.PrimaryIdeo;
							thingStyleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(this.shownItem) : null);
						}
					}
				}
				ThingStyleDef thingStyleDef2 = thingStyleDef;
				Widgets.DefIcon(rect3, this.shownItem, null, 1f, thingStyleDef2, this.drawPlaceHolderIcon, this.forceThingColor);
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
				this.mouseoverGuiAction(rect);
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

		// Token: 0x06001E39 RID: 7737 RVA: 0x000BD230 File Offset: 0x000BB430
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

		// Token: 0x04001239 RID: 4665
		private string labelInt;

		// Token: 0x0400123A RID: 4666
		public Action action;

		// Token: 0x0400123B RID: 4667
		private MenuOptionPriority priorityInt = MenuOptionPriority.Default;

		// Token: 0x0400123C RID: 4668
		public int orderInPriority;

		// Token: 0x0400123D RID: 4669
		public bool autoTakeable;

		// Token: 0x0400123E RID: 4670
		public float autoTakeablePriority;

		// Token: 0x0400123F RID: 4671
		public Action<Rect> mouseoverGuiAction;

		// Token: 0x04001240 RID: 4672
		public Thing revalidateClickTarget;

		// Token: 0x04001241 RID: 4673
		public WorldObject revalidateWorldClickTarget;

		// Token: 0x04001242 RID: 4674
		public float extraPartWidth;

		// Token: 0x04001243 RID: 4675
		public Func<Rect, bool> extraPartOnGUI;

		// Token: 0x04001244 RID: 4676
		public string tutorTag;

		// Token: 0x04001245 RID: 4677
		public ThingStyleDef thingStyle;

		// Token: 0x04001246 RID: 4678
		public TipSignal? tooltip;

		// Token: 0x04001247 RID: 4679
		private FloatMenuSizeMode sizeMode;

		// Token: 0x04001248 RID: 4680
		private float cachedRequiredHeight;

		// Token: 0x04001249 RID: 4681
		private float cachedRequiredWidth;

		// Token: 0x0400124A RID: 4682
		private bool drawPlaceHolderIcon;

		// Token: 0x0400124B RID: 4683
		private bool playSelectionSound = true;

		// Token: 0x0400124C RID: 4684
		private ThingDef shownItem;

		// Token: 0x0400124D RID: 4685
		private Texture2D itemIcon;

		// Token: 0x0400124E RID: 4686
		public Color iconColor = Color.white;

		// Token: 0x0400124F RID: 4687
		public Color? forceThingColor;

		// Token: 0x04001250 RID: 4688
		public const float MaxWidth = 300f;

		// Token: 0x04001251 RID: 4689
		private const float NormalVerticalMargin = 4f;

		// Token: 0x04001252 RID: 4690
		private const float TinyVerticalMargin = 1f;

		// Token: 0x04001253 RID: 4691
		private const float NormalHorizontalMargin = 6f;

		// Token: 0x04001254 RID: 4692
		private const float TinyHorizontalMargin = 3f;

		// Token: 0x04001255 RID: 4693
		private const float MouseOverLabelShift = 4f;

		// Token: 0x04001256 RID: 4694
		private static readonly Color ColorBGActive = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04001257 RID: 4695
		private static readonly Color ColorBGActiveMouseover = new ColorInt(29, 45, 50).ToColor;

		// Token: 0x04001258 RID: 4696
		private static readonly Color ColorBGDisabled = new ColorInt(40, 40, 40).ToColor;

		// Token: 0x04001259 RID: 4697
		private static readonly Color ColorTextActive = Color.white;

		// Token: 0x0400125A RID: 4698
		private static readonly Color ColorTextDisabled = new Color(0.9f, 0.9f, 0.9f);

		// Token: 0x0400125B RID: 4699
		public const float ExtraPartHeight = 30f;

		// Token: 0x0400125C RID: 4700
		private const float ItemIconSize = 27f;

		// Token: 0x0400125D RID: 4701
		private const float ItemIconMargin = 4f;
	}
}

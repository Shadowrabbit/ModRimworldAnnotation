using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020003F4 RID: 1012
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x000BE41E File Offset: 0x000BC61E
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x000BE426 File Offset: 0x000BC626
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string TopRightLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x000BE433 File Offset: 0x000BC633
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x000BE43B File Offset: 0x000BC63B
		public virtual string DescPostfix
		{
			get
			{
				return this.defaultDescPostfix;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000BE443 File Offset: 0x000BC643
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000BE44B File Offset: 0x000BC64B
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x000BE453 File Offset: 0x000BC653
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000BE453 File Offset: 0x000BC653
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x000BE45B File Offset: 0x000BC65B
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000BE462 File Offset: 0x000BC662
		public virtual Texture2D BGTextureShrunk
		{
			get
			{
				return Command.BGTexShrunk;
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000BE469 File Offset: 0x000BC669
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001E78 RID: 7800 RVA: 0x000BE470 File Offset: 0x000BC670
		public float GetShrunkSize
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000BE477 File Offset: 0x000BC677
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f), parms);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000BE49D File Offset: 0x000BC69D
		public virtual GizmoResult GizmoOnGUIShrunk(Vector2 topLeft, float size, GizmoRenderParms parms)
		{
			parms.shrunk = true;
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, size, size), parms);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000BE4C4 File Offset: 0x000BC6C4
		protected virtual GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			Text.Font = GameFont.Tiny;
			Color color = Color.white;
			bool flag = false;
			if (Mouse.IsOver(butRect))
			{
				flag = true;
				if (!this.disabled)
				{
					color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(butRect, SoundDefOf.Mouseover_Command);
			if (parms.highLight)
			{
				QuickSearchWidget.DrawStrongHighlight(butRect.ExpandedBy(12f));
			}
			Material material = (this.disabled || parms.lowLight) ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : color);
			GenUI.DrawTextureWithMaterial(butRect, parms.shrunk ? this.BGTextureShrunk : this.BGTexture, material, default(Rect));
			GUI.color = color;
			this.DrawIcon(butRect, material, parms);
			bool flag2 = false;
			GUI.color = Color.white;
			if (parms.lowLight)
			{
				GUI.color = Command.LowLightLabelColor;
			}
			KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
			{
				Vector2 vector = parms.shrunk ? new Vector2(3f, 0f) : new Vector2(5f, 3f);
				Widgets.Label(new Rect(butRect.x + vector.x, butRect.y + vector.y, butRect.width - 10f, 18f), keyCode.ToStringReadable());
				GizmoGridDrawer.drawnHotKeys.Add(keyCode);
				if (this.hotKey.KeyDownEvent)
				{
					flag2 = true;
					Event.current.Use();
				}
			}
			if (GizmoGridDrawer.customActivator != null && GizmoGridDrawer.customActivator(this))
			{
				flag2 = true;
			}
			if (Widgets.ButtonInvisible(butRect, true))
			{
				flag2 = true;
			}
			if (!parms.shrunk)
			{
				string topRightLabel = this.TopRightLabel;
				if (!topRightLabel.NullOrEmpty())
				{
					Vector2 vector2 = Text.CalcSize(topRightLabel);
					Rect position;
					Rect rect = position = new Rect(butRect.xMax - vector2.x - 2f, butRect.y + 3f, vector2.x, vector2.y);
					position.x -= 2f;
					position.width += 3f;
					Text.Anchor = TextAnchor.UpperRight;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect, topRightLabel);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				string labelCap = this.LabelCap;
				if (!labelCap.NullOrEmpty())
				{
					float num = Text.CalcHeight(labelCap, butRect.width);
					Rect rect2 = new Rect(butRect.x, butRect.yMax - num + 12f, butRect.width, num);
					GUI.DrawTexture(rect2, TexUI.GrayTextBG);
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect2, labelCap);
					Text.Anchor = TextAnchor.UpperLeft;
				}
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(butRect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
				}
				tip.text += this.DescPostfix;
				TooltipHandler.TipRegion(butRect, tip);
			}
			if (!this.HighlightTag.NullOrEmpty() && (Find.WindowStack.FloatMenu == null || !Find.WindowStack.FloatMenu.windowRect.Overlaps(butRect)))
			{
				UIHighlighter.HighlightOpportunity(butRect, this.HighlightTag);
			}
			Text.Font = GameFont.Small;
			if (flag2)
			{
				if (this.disabled)
				{
					if (!this.disabledReason.NullOrEmpty())
					{
						Messages.Message(this.disabledReason, MessageTypeDefOf.RejectInput, false);
					}
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				GizmoResult result;
				if (Event.current.button == 1)
				{
					result = new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
				}
				else
				{
					if (!TutorSystem.AllowAction(this.TutorTagSelect))
					{
						return new GizmoResult(GizmoState.Mouseover, null);
					}
					result = new GizmoResult(GizmoState.Interacted, Event.current);
					TutorSystem.Notify_Event(this.TutorTagSelect);
				}
				return result;
			}
			else
			{
				if (flag)
				{
					return new GizmoResult(GizmoState.Mouseover, null);
				}
				return new GizmoResult(GizmoState.Clear, null);
			}
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000BE908 File Offset: 0x000BCB08
		protected virtual void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			Texture2D badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			if (!this.disabled || parms.lowLight)
			{
				GUI.color = this.IconDrawColor;
			}
			else
			{
				GUI.color = this.IconDrawColor.SaturationChanged(0f);
			}
			if (parms.lowLight)
			{
				GUI.color = GUI.color.ToTransparent(0.6f);
			}
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat);
			GUI.color = Color.white;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000BE9F0 File Offset: 0x000BCBF0
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000BEA62 File Offset: 0x000BCC62
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000BEA78 File Offset: 0x000BCC78
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Command(label=",
				this.defaultLabel,
				", defaultDesc=",
				this.defaultDesc,
				")"
			});
		}

		// Token: 0x0400127C RID: 4732
		public string defaultLabel;

		// Token: 0x0400127D RID: 4733
		public string defaultDesc = "No description.";

		// Token: 0x0400127E RID: 4734
		public string defaultDescPostfix;

		// Token: 0x0400127F RID: 4735
		public Texture2D icon;

		// Token: 0x04001280 RID: 4736
		public float iconAngle;

		// Token: 0x04001281 RID: 4737
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04001282 RID: 4738
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001283 RID: 4739
		public float iconDrawScale = 1f;

		// Token: 0x04001284 RID: 4740
		public Vector2 iconOffset;

		// Token: 0x04001285 RID: 4741
		public Color defaultIconColor = Color.white;

		// Token: 0x04001286 RID: 4742
		public KeyBindingDef hotKey;

		// Token: 0x04001287 RID: 4743
		public SoundDef activateSound;

		// Token: 0x04001288 RID: 4744
		public int groupKey;

		// Token: 0x04001289 RID: 4745
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x0400128A RID: 4746
		public bool shrinkable;

		// Token: 0x0400128B RID: 4747
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x0400128C RID: 4748
		public static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x0400128D RID: 4749
		public static readonly Color LowLightBgColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x0400128E RID: 4750
		public static readonly Color LowLightIconColor = new Color(0.8f, 0.8f, 0.7f, 0.6f);

		// Token: 0x0400128F RID: 4751
		public static readonly Color LowLightLabelColor = new Color(0.8f, 0.8f, 0.7f, 0.5f);

		// Token: 0x04001290 RID: 4752
		public const float LowLightIconAlpha = 0.6f;

		// Token: 0x04001291 RID: 4753
		protected const float InnerIconDrawScale = 0.85f;
	}
}

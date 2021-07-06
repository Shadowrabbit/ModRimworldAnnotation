using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200070F RID: 1807
	[StaticConstructorOnStartup]
	public abstract class Command : Gizmo
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x00024014 File Offset: 0x00022214
		public virtual string Label
		{
			get
			{
				return this.defaultLabel;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06002DB2 RID: 11698 RVA: 0x0002401C File Offset: 0x0002221C
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual string TopRightLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06002DB4 RID: 11700 RVA: 0x00024029 File Offset: 0x00022229
		public virtual string Desc
		{
			get
			{
				return this.defaultDesc;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x00024031 File Offset: 0x00022231
		public virtual Color IconDrawColor
		{
			get
			{
				return this.defaultIconColor;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x00024039 File Offset: 0x00022239
		public virtual SoundDef CurActivateSound
		{
			get
			{
				return this.activateSound;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected virtual bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x00024041 File Offset: 0x00022241
		public virtual string HighlightTag
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x00024041 File Offset: 0x00022241
		public virtual string TutorTagSelect
		{
			get
			{
				return this.tutorTag;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06002DBA RID: 11706 RVA: 0x00024049 File Offset: 0x00022249
		public virtual Texture2D BGTexture
		{
			get
			{
				return Command.BGTex;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x00024050 File Offset: 0x00022250
		public virtual Texture2D BGTextureShrunk
		{
			get
			{
				return Command.BGTexShrunk;
			}
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x00024057 File Offset: 0x00022257
		public override float GetWidth(float maxWidth)
		{
			return 75f;
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x0002405E File Offset: 0x0002225E
		public float GetShrunkSize
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x00024065 File Offset: 0x00022265
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f), false);
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x0002408B File Offset: 0x0002228B
		public virtual GizmoResult GizmoOnGUIShrunk(Vector2 topLeft, float size)
		{
			return this.GizmoOnGUIInt(new Rect(topLeft.x, topLeft.y, size, size), true);
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x00134CF0 File Offset: 0x00132EF0
		protected virtual GizmoResult GizmoOnGUIInt(Rect butRect, bool shrunk = false)
		{
			Text.Font = GameFont.Tiny;
			bool flag = false;
			if (Mouse.IsOver(butRect))
			{
				flag = true;
				if (!this.disabled)
				{
					GUI.color = GenUI.MouseoverColor;
				}
			}
			MouseoverSounds.DoRegion(butRect, SoundDefOf.Mouseover_Command);
			Material material = this.disabled ? TexUI.GrayscaleGUI : null;
			GenUI.DrawTextureWithMaterial(butRect, shrunk ? this.BGTextureShrunk : this.BGTexture, material, default(Rect));
			this.DrawIcon(butRect, material);
			bool flag2 = false;
			KeyCode keyCode = (this.hotKey == null) ? KeyCode.None : this.hotKey.MainKey;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode))
			{
				Vector2 vector = shrunk ? new Vector2(3f, 0f) : new Vector2(5f, 3f);
				Widgets.Label(new Rect(butRect.x + vector.x, butRect.y + vector.y, butRect.width - 10f, 18f), keyCode.ToStringReadable());
				GizmoGridDrawer.drawnHotKeys.Add(keyCode);
				if (this.hotKey.KeyDownEvent)
				{
					flag2 = true;
					Event.current.Use();
				}
			}
			if (Widgets.ButtonInvisible(butRect, true))
			{
				flag2 = true;
			}
			if (!shrunk)
			{
				string topRightLabel = this.TopRightLabel;
				if (!topRightLabel.NullOrEmpty())
				{
					Vector2 vector2 = Text.CalcSize(topRightLabel);
					Rect position;
					Rect rect = position = new Rect(butRect.xMax - vector2.x - 2f, butRect.y + 3f, vector2.x, vector2.y);
					position.x -= 2f;
					position.width += 3f;
					GUI.color = Color.white;
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
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect2, labelCap);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(butRect) && this.DoTooltip)
			{
				TipSignal tip = this.Desc;
				if (this.disabled && !this.disabledReason.NullOrEmpty())
				{
					tip.text += "\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason;
				}
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

		// Token: 0x06002DC1 RID: 11713 RVA: 0x001350AC File Offset: 0x001332AC
		protected virtual void DrawIcon(Rect rect, Material buttonMat = null)
		{
			Texture2D badTex = this.icon;
			if (badTex == null)
			{
				badTex = BaseContent.BadTex;
			}
			rect.position += new Vector2(this.iconOffset.x * rect.size.x, this.iconOffset.y * rect.size.y);
			GUI.color = this.IconDrawColor;
			Widgets.DrawTextureFitted(rect, badTex, this.iconDrawScale * 0.85f, this.iconProportions, this.iconTexCoords, this.iconAngle, buttonMat);
			GUI.color = Color.white;
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x00135154 File Offset: 0x00133354
		public override bool GroupsWith(Gizmo other)
		{
			Command command = other as Command;
			return command != null && ((this.hotKey == command.hotKey && this.Label == command.Label && this.icon == command.icon) || (this.groupKey != 0 && command.groupKey != 0 && this.groupKey == command.groupKey));
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000240A7 File Offset: 0x000222A7
		public override void ProcessInput(Event ev)
		{
			if (this.CurActivateSound != null)
			{
				this.CurActivateSound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000240BD File Offset: 0x000222BD
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

		// Token: 0x04001F23 RID: 7971
		public string defaultLabel;

		// Token: 0x04001F24 RID: 7972
		public string defaultDesc = "No description.";

		// Token: 0x04001F25 RID: 7973
		public Texture2D icon;

		// Token: 0x04001F26 RID: 7974
		public float iconAngle;

		// Token: 0x04001F27 RID: 7975
		public Vector2 iconProportions = Vector2.one;

		// Token: 0x04001F28 RID: 7976
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001F29 RID: 7977
		public float iconDrawScale = 1f;

		// Token: 0x04001F2A RID: 7978
		public Vector2 iconOffset;

		// Token: 0x04001F2B RID: 7979
		public Color defaultIconColor = Color.white;

		// Token: 0x04001F2C RID: 7980
		public KeyBindingDef hotKey;

		// Token: 0x04001F2D RID: 7981
		public SoundDef activateSound;

		// Token: 0x04001F2E RID: 7982
		public int groupKey;

		// Token: 0x04001F2F RID: 7983
		public string tutorTag = "TutorTagNotSet";

		// Token: 0x04001F30 RID: 7984
		public bool shrinkable;

		// Token: 0x04001F31 RID: 7985
		public static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x04001F32 RID: 7986
		public static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/DesButBG", true);

		// Token: 0x04001F33 RID: 7987
		protected const float InnerIconDrawScale = 0.85f;
	}
}

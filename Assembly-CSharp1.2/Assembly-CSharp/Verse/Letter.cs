using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200072D RID: 1837
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002E3D RID: 11837 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002E3E RID: 11838 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x00024576 File Offset: 0x00022776
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x0002458B File Offset: 0x0002278B
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002E41 RID: 11841 RVA: 0x00024592 File Offset: 0x00022792
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x0002459F File Offset: 0x0002279F
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x000245AC File Offset: 0x000227AC
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002E44 RID: 11844 RVA: 0x000245B9 File Offset: 0x000227B9
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002E45 RID: 11845 RVA: 0x000245C1 File Offset: 0x000227C1
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x00024576 File Offset: 0x00022776
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x000245C9 File Offset: 0x000227C9
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00136F14 File Offset: 0x00135114
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x00136F98 File Offset: 0x00135198
		public virtual void DrawButtonAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			Rect rect = new Rect(num, topY, 38f, 30f);
			Rect rect2 = new Rect(rect);
			float num2 = Time.time - this.arrivalTime;
			Color color = this.def.color;
			if (num2 < 1f)
			{
				rect2.y -= (1f - num2) * 200f;
				color.a = num2 / 1f;
			}
			if (!Mouse.IsOver(rect) && this.def.bounce && num2 > 15f && num2 % 5f < 1f)
			{
				float num3 = (float)UI.screenWidth * 0.06f;
				float num4 = 2f * (num2 % 1f) - 1f;
				float num5 = num3 * (1f - num4 * num4);
				rect2.x -= num5;
			}
			if (Event.current.type == EventType.Repaint)
			{
				if (this.def.flashInterval > 0f)
				{
					float num6 = Time.time - (this.arrivalTime + 1f);
					if (num6 > 0f && num6 % this.def.flashInterval < 1f)
					{
						GenUI.DrawFlash(num, topY, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num6) * 0.55f, this.def.flashColor);
					}
				}
				GUI.color = color;
				Widgets.DrawShadowAround(rect2);
				GUI.DrawTexture(rect2, this.def.Icon);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperRight;
				string text = this.PostProcessedLabel();
				Vector2 vector = Text.CalcSize(text);
				float x = vector.x;
				float y = vector.y;
				Vector2 vector2 = new Vector2(rect2.x + rect2.width / 2f, rect2.center.y - y / 2f + 4f);
				float num7 = vector2.x + x / 2f - (float)(UI.screenWidth - 2);
				if (num7 > 0f)
				{
					vector2.x -= num7;
				}
				GUI.DrawTexture(new Rect(vector2.x - x / 2f - 6f - 1f, vector2.y, x + 12f, 16f), TexUI.GrayTextBG);
				GUI.color = new Color(1f, 1f, 1f, 0.75f);
				Widgets.Label(new Rect(vector2.x - x / 2f, vector2.y - 3f, x, 999f), text);
				GUI.color = Color.white;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			if (this.CanDismissWithRightClick && Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.LetterStack.RemoveLetter(this);
				Event.current.Use();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.OpenLetter();
				Event.current.Use();
			}
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x001372C4 File Offset: 0x001354C4
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.RawText.NullOrEmpty())
				{
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.UpperLeft;
					float num2 = Text.CalcHeight(mouseoverText, 310f);
					num2 += 20f;
					float x = num - 330f - 10f;
					Rect infoRect = new Rect(x, topY - num2 / 2f, 330f, num2);
					Find.WindowStack.ImmediateWindow(2768333, infoRect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Small;
						Rect position = infoRect.AtZero().ContractedBy(10f);
						GUI.BeginGroup(position);
						Widgets.Label(new Rect(0f, 0f, position.width, position.height), mouseoverText.Resolve());
						GUI.EndGroup();
					}, true, false, 1f);
				}
			}
		}

		// Token: 0x06002E4B RID: 11851
		protected abstract string GetMouseoverText();

		// Token: 0x06002E4C RID: 11852
		public abstract void OpenLetter();

		// Token: 0x06002E4D RID: 11853 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Received()
		{
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void Removed()
		{
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x000245AC File Offset: 0x000227AC
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x000245D1 File Offset: 0x000227D1
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000245D9 File Offset: 0x000227D9
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		// Token: 0x04001F8E RID: 8078
		public int ID;

		// Token: 0x04001F8F RID: 8079
		public LetterDef def;

		// Token: 0x04001F90 RID: 8080
		public TaggedString label;

		// Token: 0x04001F91 RID: 8081
		public LookTargets lookTargets;

		// Token: 0x04001F92 RID: 8082
		public Faction relatedFaction;

		// Token: 0x04001F93 RID: 8083
		public int arrivalTick;

		// Token: 0x04001F94 RID: 8084
		public float arrivalTime;

		// Token: 0x04001F95 RID: 8085
		public string debugInfo;

		// Token: 0x04001F96 RID: 8086
		public const float DrawWidth = 38f;

		// Token: 0x04001F97 RID: 8087
		public const float DrawHeight = 30f;

		// Token: 0x04001F98 RID: 8088
		private const float FallTime = 1f;

		// Token: 0x04001F99 RID: 8089
		private const float FallDistance = 200f;
	}
}

using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000407 RID: 1031
	public abstract class Letter : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x170005E2 RID: 1506
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanShowInLetterStack
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanDismissWithRightClick
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x06001EE1 RID: 7905 RVA: 0x000C0D9C File Offset: 0x000BEF9C
		public bool ArchivedOnly
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170005E5 RID: 1509
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x000C0DB1 File Offset: 0x000BEFB1
		public IThingHolder ParentHolder
		{
			get
			{
				return Find.World;
			}
		}

		// Token: 0x170005E6 RID: 1510
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000C0DB8 File Offset: 0x000BEFB8
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return this.def.Icon;
			}
		}

		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000C0DC5 File Offset: 0x000BEFC5
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return this.def.color;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000C0DD2 File Offset: 0x000BEFD2
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x000C0DDF File Offset: 0x000BEFDF
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.GetMouseoverText();
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x000C0DE7 File Offset: 0x000BEFE7
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.arrivalTick;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x000C0D9C File Offset: 0x000BEF9C
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Find.LetterStack.LettersListForReading.Contains(this);
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x000C0DEF File Offset: 0x000BEFEF
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06001EEA RID: 7914 RVA: 0x000C0DF8 File Offset: 0x000BEFF8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Defs.Look<LetterDef>(ref this.def, "def");
			Scribe_Values.Look<TaggedString>(ref this.label, "label", default(TaggedString), false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.arrivalTick, "arrivalTick", 0, false);
		}

		// Token: 0x06001EEB RID: 7915 RVA: 0x000C0E7C File Offset: 0x000BF07C
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

		// Token: 0x06001EEC RID: 7916 RVA: 0x000C11A8 File Offset: 0x000BF3A8
		public virtual void CheckForMouseOverTextAt(float topY)
		{
			float num = (float)UI.screenWidth - 38f - 12f;
			if (Mouse.IsOver(new Rect(num, topY, 38f, 30f)))
			{
				Find.LetterStack.Notify_LetterMouseover(this);
				TaggedString mouseoverText = this.GetMouseoverText();
				if (!mouseoverText.Resolve().NullOrEmpty())
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
					}, true, false, 1f, null);
				}
			}
		}

		// Token: 0x06001EED RID: 7917
		protected abstract string GetMouseoverText();

		// Token: 0x06001EEE RID: 7918
		public abstract void OpenLetter();

		// Token: 0x06001EEF RID: 7919 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Received()
		{
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Removed()
		{
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000C0DD2 File Offset: 0x000BEFD2
		protected virtual string PostProcessedLabel()
		{
			return this.label;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000C12AB File Offset: 0x000BF4AB
		void IArchivable.OpenArchived()
		{
			this.OpenLetter();
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000C12B3 File Offset: 0x000BF4B3
		public string GetUniqueLoadID()
		{
			return "Letter_" + this.ID;
		}

		// Token: 0x040012DC RID: 4828
		public int ID;

		// Token: 0x040012DD RID: 4829
		public LetterDef def;

		// Token: 0x040012DE RID: 4830
		public TaggedString label;

		// Token: 0x040012DF RID: 4831
		public LookTargets lookTargets;

		// Token: 0x040012E0 RID: 4832
		public Faction relatedFaction;

		// Token: 0x040012E1 RID: 4833
		public int arrivalTick;

		// Token: 0x040012E2 RID: 4834
		public float arrivalTime;

		// Token: 0x040012E3 RID: 4835
		public string debugInfo;

		// Token: 0x040012E4 RID: 4836
		public const float DrawWidth = 38f;

		// Token: 0x040012E5 RID: 4837
		public const float DrawHeight = 30f;

		// Token: 0x040012E6 RID: 4838
		private const float FallTime = 1f;

		// Token: 0x040012E7 RID: 4839
		private const float FallDistance = 200f;
	}
}

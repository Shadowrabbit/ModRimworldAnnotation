using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAC RID: 6828
	public class Screen_Credits : Window
	{
		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x060096D3 RID: 38611 RVA: 0x00023846 File Offset: 0x00021A46
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x060096D4 RID: 38612 RVA: 0x00016647 File Offset: 0x00014847
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170017BF RID: 6079
		// (get) Token: 0x060096D5 RID: 38613 RVA: 0x00064B59 File Offset: 0x00062D59
		private float ViewWidth
		{
			get
			{
				return 800f;
			}
		}

		// Token: 0x170017C0 RID: 6080
		// (get) Token: 0x060096D6 RID: 38614 RVA: 0x002BF358 File Offset: 0x002BD558
		private float ViewHeight
		{
			get
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = this.creds.Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth)) + 20f;
				Text.Font = font;
				return result;
			}
		}

		// Token: 0x170017C1 RID: 6081
		// (get) Token: 0x060096D7 RID: 38615 RVA: 0x00064B60 File Offset: 0x00062D60
		private float MaxScrollPosition
		{
			get
			{
				return Mathf.Max(this.ViewHeight - (float)UI.screenHeight / 2f, 0f);
			}
		}

		// Token: 0x170017C2 RID: 6082
		// (get) Token: 0x060096D8 RID: 38616 RVA: 0x002BF394 File Offset: 0x002BD594
		private float AutoScrollRate
		{
			get
			{
				if (!this.wonGame)
				{
					return 30f;
				}
				if (this.scrollPosition < this.victoryTextHeight - 200f)
				{
					return 20f;
				}
				float num = SongDefOf.EndCreditsSong.clip.length + 5f - 6f - this.victoryTextHeight / 20f;
				float t = (this.scrollPosition - this.victoryTextHeight) / 200f;
				return Mathf.Lerp(20f, this.MaxScrollPosition / num, t);
			}
		}

		// Token: 0x060096D9 RID: 38617 RVA: 0x00064B7F File Offset: 0x00062D7F
		public Screen_Credits() : this("")
		{
		}

		// Token: 0x060096DA RID: 38618 RVA: 0x002BF41C File Offset: 0x002BD61C
		public Screen_Credits(string preCreditsMessage)
		{
			this.doWindowBackground = false;
			this.doCloseButton = false;
			this.doCloseX = false;
			this.forcePause = true;
			this.creds = CreditsAssembler.AllCredits().ToList<CreditsEntry>();
			this.creds.Insert(0, new CreditRecord_Space(100f));
			if (!preCreditsMessage.NullOrEmpty())
			{
				this.creds.Insert(1, new CreditRecord_Space(200f));
				this.creds.Insert(2, new CreditRecord_Text(preCreditsMessage, TextAnchor.UpperLeft));
				this.creds.Insert(3, new CreditRecord_Space(50f));
				Text.Font = GameFont.Medium;
				this.victoryTextHeight = this.creds.Take(4).Sum((CreditsEntry c) => c.DrawHeight(this.ViewWidth));
			}
			this.creds.Add(new CreditRecord_Space(300f));
			this.creds.Add(new CreditRecord_Text("ThanksForPlaying".Translate(), TextAnchor.UpperCenter));
			string text = string.Empty;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				CreditRecord_Role creditRecord_Role = creditsEntry as CreditRecord_Role;
				if (creditRecord_Role == null)
				{
					text = string.Empty;
				}
				else
				{
					creditRecord_Role.displayKey = (text.NullOrEmpty() || creditRecord_Role.roleKey != text);
					text = creditRecord_Role.roleKey;
				}
			}
		}

		// Token: 0x060096DB RID: 38619 RVA: 0x00064B8C File Offset: 0x00062D8C
		public override void PreOpen()
		{
			base.PreOpen();
			this.creationRealtime = Time.realtimeSinceStartup;
			if (this.wonGame)
			{
				this.timeUntilAutoScroll = 6f;
				return;
			}
			this.timeUntilAutoScroll = 1f;
		}

		// Token: 0x060096DC RID: 38620 RVA: 0x002BF59C File Offset: 0x002BD79C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (this.timeUntilAutoScroll > 0f)
			{
				this.timeUntilAutoScroll -= Time.deltaTime;
			}
			else
			{
				this.scrollPosition += this.AutoScrollRate * Time.deltaTime;
			}
			if (this.wonGame && !this.playedMusic && Time.realtimeSinceStartup > this.creationRealtime + 5f)
			{
				Find.MusicManagerPlay.ForceStartSong(SongDefOf.EndCreditsSong, true);
				this.playedMusic = true;
			}
		}

		// Token: 0x060096DD RID: 38621 RVA: 0x002BF624 File Offset: 0x002BD824
		public override void DoWindowContents(Rect inRect)
		{
			Rect rect = new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight);
			GUI.DrawTexture(rect, BaseContent.BlackTex);
			Rect position = new Rect(rect);
			position.yMin += 30f;
			position.yMax -= 30f;
			position.xMin = rect.center.x - 400f;
			position.width = 800f;
			float viewWidth = this.ViewWidth;
			float viewHeight = this.ViewHeight;
			this.scrollPosition = Mathf.Clamp(this.scrollPosition, 0f, this.MaxScrollPosition);
			GUI.BeginGroup(position);
			Rect position2 = new Rect(0f, 0f, viewWidth, viewHeight);
			position2.y -= this.scrollPosition;
			GUI.BeginGroup(position2);
			Text.Font = GameFont.Medium;
			float num = 0f;
			foreach (CreditsEntry creditsEntry in this.creds)
			{
				float num2 = creditsEntry.DrawHeight(position2.width);
				Rect rect2 = new Rect(0f, num, position2.width, num2);
				creditsEntry.Draw(rect2);
				num += num2;
			}
			GUI.EndGroup();
			GUI.EndGroup();
			if (Event.current.type == EventType.ScrollWheel)
			{
				this.Scroll(Event.current.delta.y * 25f);
				Event.current.Use();
			}
			if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.DownArrow)
				{
					this.Scroll(250f);
					Event.current.Use();
				}
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					this.Scroll(-250f);
					Event.current.Use();
				}
			}
		}

		// Token: 0x060096DE RID: 38622 RVA: 0x00064BBE File Offset: 0x00062DBE
		private void Scroll(float offset)
		{
			this.scrollPosition += offset;
			this.timeUntilAutoScroll = 3f;
		}

		// Token: 0x04006035 RID: 24629
		private List<CreditsEntry> creds;

		// Token: 0x04006036 RID: 24630
		public bool wonGame;

		// Token: 0x04006037 RID: 24631
		private float timeUntilAutoScroll;

		// Token: 0x04006038 RID: 24632
		private float scrollPosition;

		// Token: 0x04006039 RID: 24633
		private bool playedMusic;

		// Token: 0x0400603A RID: 24634
		public float creationRealtime = -1f;

		// Token: 0x0400603B RID: 24635
		private float victoryTextHeight;

		// Token: 0x0400603C RID: 24636
		private const int ColumnWidth = 800;

		// Token: 0x0400603D RID: 24637
		private const float InitialAutoScrollDelay = 1f;

		// Token: 0x0400603E RID: 24638
		private const float InitialAutoScrollDelayWonGame = 6f;

		// Token: 0x0400603F RID: 24639
		private const float AutoScrollDelayAfterManualScroll = 3f;

		// Token: 0x04006040 RID: 24640
		private const float SongStartDelay = 5f;

		// Token: 0x04006041 RID: 24641
		private const float VictoryTextScrollSpeed = 20f;

		// Token: 0x04006042 RID: 24642
		private const float ScrollSpeedLerpHeight = 200f;

		// Token: 0x04006043 RID: 24643
		private const GameFont Font = GameFont.Medium;
	}
}

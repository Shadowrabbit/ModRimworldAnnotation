using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132B RID: 4907
	public class Screen_Credits : Window
	{
		// Token: 0x170014BE RID: 5310
		// (get) Token: 0x060076A7 RID: 30375 RVA: 0x000B9F29 File Offset: 0x000B8129
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth, (float)UI.screenHeight);
			}
		}

		// Token: 0x170014BF RID: 5311
		// (get) Token: 0x060076A8 RID: 30376 RVA: 0x000682C5 File Offset: 0x000664C5
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x060076A9 RID: 30377 RVA: 0x0029362C File Offset: 0x0029182C
		private float ViewWidth
		{
			get
			{
				return 800f;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x060076AA RID: 30378 RVA: 0x00293634 File Offset: 0x00291834
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

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x060076AB RID: 30379 RVA: 0x00293670 File Offset: 0x00291870
		private float MaxScrollPosition
		{
			get
			{
				return Mathf.Max(this.ViewHeight - (float)UI.screenHeight / 2f, 0f);
			}
		}

		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x060076AC RID: 30380 RVA: 0x00293690 File Offset: 0x00291890
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
				float num = this.EndCreditsSong.clip.length + this.songStartDelay - 6f - this.victoryTextHeight / 20f;
				float t = (this.scrollPosition - this.victoryTextHeight) / 200f;
				return Mathf.Lerp(20f, this.MaxScrollPosition / num, t);
			}
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x060076AD RID: 30381 RVA: 0x00293717 File Offset: 0x00291917
		private SongDef EndCreditsSong
		{
			get
			{
				if (this.endCreditsSong == null)
				{
					return SongDefOf.EndCreditsSong;
				}
				return this.endCreditsSong;
			}
		}

		// Token: 0x060076AE RID: 30382 RVA: 0x0029372D File Offset: 0x0029192D
		public Screen_Credits() : this("")
		{
		}

		// Token: 0x060076AF RID: 30383 RVA: 0x0029373C File Offset: 0x0029193C
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

		// Token: 0x060076B0 RID: 30384 RVA: 0x002938C8 File Offset: 0x00291AC8
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

		// Token: 0x060076B1 RID: 30385 RVA: 0x002938FA File Offset: 0x00291AFA
		public override void PostClose()
		{
			base.PostOpen();
			if (this.exitToMainMenu)
			{
				GenScene.GoToMainMenu();
			}
		}

		// Token: 0x060076B2 RID: 30386 RVA: 0x00293910 File Offset: 0x00291B10
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
			if (this.wonGame && !this.playedMusic && Time.realtimeSinceStartup > this.creationRealtime + this.songStartDelay)
			{
				Find.MusicManagerPlay.ForceStartSong(this.EndCreditsSong, true);
				this.playedMusic = true;
			}
		}

		// Token: 0x060076B3 RID: 30387 RVA: 0x0029399C File Offset: 0x00291B9C
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

		// Token: 0x060076B4 RID: 30388 RVA: 0x00293B98 File Offset: 0x00291D98
		private void Scroll(float offset)
		{
			this.scrollPosition += offset;
			this.timeUntilAutoScroll = 3f;
		}

		// Token: 0x040041E5 RID: 16869
		private List<CreditsEntry> creds;

		// Token: 0x040041E6 RID: 16870
		public bool wonGame;

		// Token: 0x040041E7 RID: 16871
		public SongDef endCreditsSong;

		// Token: 0x040041E8 RID: 16872
		public bool exitToMainMenu;

		// Token: 0x040041E9 RID: 16873
		public float songStartDelay = 5f;

		// Token: 0x040041EA RID: 16874
		private float timeUntilAutoScroll;

		// Token: 0x040041EB RID: 16875
		private float scrollPosition;

		// Token: 0x040041EC RID: 16876
		private bool playedMusic;

		// Token: 0x040041ED RID: 16877
		public float creationRealtime = -1f;

		// Token: 0x040041EE RID: 16878
		private float victoryTextHeight;

		// Token: 0x040041EF RID: 16879
		private const int ColumnWidth = 800;

		// Token: 0x040041F0 RID: 16880
		private const float InitialAutoScrollDelay = 1f;

		// Token: 0x040041F1 RID: 16881
		private const float InitialAutoScrollDelayWonGame = 6f;

		// Token: 0x040041F2 RID: 16882
		private const float AutoScrollDelayAfterManualScroll = 3f;

		// Token: 0x040041F3 RID: 16883
		private const float VictoryTextScrollSpeed = 20f;

		// Token: 0x040041F4 RID: 16884
		private const float ScrollSpeedLerpHeight = 200f;

		// Token: 0x040041F5 RID: 16885
		private const GameFont Font = GameFont.Medium;

		// Token: 0x040041F6 RID: 16886
		public const float DefaultSongStartDelay = 5f;
	}
}

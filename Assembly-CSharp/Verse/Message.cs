using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200074F RID: 1871
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x00024F20 File Offset: 0x00023120
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002F21 RID: 12065 RVA: 0x00024F2E File Offset: 0x0002312E
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x00024F3C File Offset: 0x0002313C
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002F23 RID: 12067 RVA: 0x00024F4E File Offset: 0x0002314E
		public float Alpha
		{
			get
			{
				if (this.TimeLeft < 0.6f)
				{
					return this.TimeLeft / 0.6f;
				}
				return 1f;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002F24 RID: 12068 RVA: 0x0013A268 File Offset: 0x00138468
		private static bool ShouldDrawBackground
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					return true;
				}
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					if (windowStack[i].CausesMessageBackground())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002F25 RID: 12069 RVA: 0x0000C32E File Offset: 0x0000A52E
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002F27 RID: 12071 RVA: 0x00024F6F File Offset: 0x0002316F
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x06002F28 RID: 12072 RVA: 0x00024F7C File Offset: 0x0002317C
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002F29 RID: 12073 RVA: 0x00024F84 File Offset: 0x00023184
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x06002F2A RID: 12074 RVA: 0x00024F8C File Offset: 0x0002318C
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x1700072C RID: 1836
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x00024F97 File Offset: 0x00023197
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x00024F9F File Offset: 0x0002319F
		public Message()
		{
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x0013A2A8 File Offset: 0x001384A8
		public Message(string text, MessageTypeDef def)
		{
			this.text = text;
			this.def = def;
			this.startingFrame = RealTime.frameCount;
			this.startingTime = RealTime.LastRealTime;
			this.startingTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextMessageID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x00024FBC File Offset: 0x000231BC
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x00024FCD File Offset: 0x000231CD
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x0013A324 File Offset: 0x00138524
		public void ExposeData()
		{
			Scribe_Defs.Look<MessageTypeDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<float>(ref this.startingTime, "startingTime", 0f, false);
			Scribe_Values.Look<int>(ref this.startingFrame, "startingFrame", 0, false);
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x0013A3C8 File Offset: 0x001385C8
		public Rect CalculateRect(float x, float y)
		{
			Text.Font = GameFont.Small;
			if (this.cachedSize.x < 0f)
			{
				this.cachedSize = Text.CalcSize(this.text);
			}
			this.lastDrawRect = new Rect(x, y, this.cachedSize.x, this.cachedSize.y);
			this.lastDrawRect = this.lastDrawRect.ContractedBy(-2f);
			return this.lastDrawRect;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x0013A440 File Offset: 0x00138640
		public void Draw(int xOffset, int yOffset)
		{
			Rect rect = this.CalculateRect((float)xOffset, (float)yOffset);
			Find.WindowStack.ImmediateWindow(Gen.HashCombineInt(this.ID, 45574281), rect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect = rect.AtZero();
				float alpha = this.Alpha;
				GUI.color = new Color(1f, 1f, 1f, alpha);
				if (Message.ShouldDrawBackground)
				{
					GUI.color = new Color(0.15f, 0.15f, 0.15f, 0.8f * alpha);
					GUI.DrawTexture(rect, BaseContent.WhiteTex);
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()) || this.quest != null)
				{
					UIHighlighter.HighlightOpportunity(rect, "Messages");
					Widgets.DrawHighlightIfMouseover(rect);
				}
				Widgets.Label(new Rect(2f, 0f, rect.width - 2f, rect.height), this.text);
				if (Current.ProgramState == ProgramState.Playing && Widgets.ButtonInvisible(rect, true))
				{
					if (CameraJumper.CanJump(this.lookTargets.TryGetPrimaryTarget()))
					{
						CameraJumper.TryJumpAndSelect(this.lookTargets.TryGetPrimaryTarget());
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ClickingMessages, KnowledgeAmount.Total);
					}
					else if (this.quest != null)
					{
						if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Quests)
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
						else
						{
							Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						}
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				if (Mouse.IsOver(rect))
				{
					Messages.Notify_Mouseover(this);
				}
			}, false, false, 0f);
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x0013A4A0 File Offset: 0x001386A0
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x00024FE0 File Offset: 0x000231E0
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID;
		}

		// Token: 0x04001FFE RID: 8190
		public MessageTypeDef def;

		// Token: 0x04001FFF RID: 8191
		private int ID;

		// Token: 0x04002000 RID: 8192
		public string text;

		// Token: 0x04002001 RID: 8193
		private float startingTime;

		// Token: 0x04002002 RID: 8194
		public int startingFrame;

		// Token: 0x04002003 RID: 8195
		public int startingTick;

		// Token: 0x04002004 RID: 8196
		public LookTargets lookTargets;

		// Token: 0x04002005 RID: 8197
		public Quest quest;

		// Token: 0x04002006 RID: 8198
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x04002007 RID: 8199
		public Rect lastDrawRect;

		// Token: 0x04002008 RID: 8200
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04002009 RID: 8201
		private const float FadeoutDuration = 0.6f;
	}
}

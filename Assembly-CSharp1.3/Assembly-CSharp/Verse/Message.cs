using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200041A RID: 1050
	public class Message : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06001F85 RID: 8069 RVA: 0x000C4342 File Offset: 0x000C2542
		protected float Age
		{
			get
			{
				return RealTime.LastRealTime - this.startingTime;
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x000C4350 File Offset: 0x000C2550
		protected float TimeLeft
		{
			get
			{
				return 13f - this.Age;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001F87 RID: 8071 RVA: 0x000C435E File Offset: 0x000C255E
		public bool Expired
		{
			get
			{
				return this.TimeLeft <= 0f;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x000C4370 File Offset: 0x000C2570
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

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001F89 RID: 8073 RVA: 0x000C4394 File Offset: 0x000C2594
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

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x00002688 File Offset: 0x00000888
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0001A4C7 File Offset: 0x000186C7
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x000C43D3 File Offset: 0x000C25D3
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000C43E0 File Offset: 0x000C25E0
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x000C43E8 File Offset: 0x000C25E8
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.startingTick;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06001F8F RID: 8079 RVA: 0x000C43F0 File Offset: 0x000C25F0
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return !Messages.IsLive(this);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000C43FB File Offset: 0x000C25FB
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return this.lookTargets;
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x000C4403 File Offset: 0x000C2603
		public Message()
		{
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000C4420 File Offset: 0x000C2620
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

		// Token: 0x06001F93 RID: 8083 RVA: 0x000C449A File Offset: 0x000C269A
		public Message(string text, MessageTypeDef def, LookTargets lookTargets) : this(text, def)
		{
			this.lookTargets = lookTargets;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000C44AB File Offset: 0x000C26AB
		public Message(string text, MessageTypeDef def, LookTargets lookTargets, Quest quest) : this(text, def, lookTargets)
		{
			this.quest = quest;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000C44C0 File Offset: 0x000C26C0
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

		// Token: 0x06001F96 RID: 8086 RVA: 0x000C4564 File Offset: 0x000C2764
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

		// Token: 0x06001F97 RID: 8087 RVA: 0x000C45DC File Offset: 0x000C27DC
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
			}, false, false, 0f, null);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000C463C File Offset: 0x000C283C
		void IArchivable.OpenArchived()
		{
			Find.WindowStack.Add(new Dialog_MessageBox(this.text, null, null, null, null, null, false, null, null));
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000C466B File Offset: 0x000C286B
		public string GetUniqueLoadID()
		{
			return "Message_" + this.ID;
		}

		// Token: 0x04001326 RID: 4902
		public MessageTypeDef def;

		// Token: 0x04001327 RID: 4903
		private int ID;

		// Token: 0x04001328 RID: 4904
		public string text;

		// Token: 0x04001329 RID: 4905
		private float startingTime;

		// Token: 0x0400132A RID: 4906
		public int startingFrame;

		// Token: 0x0400132B RID: 4907
		public int startingTick;

		// Token: 0x0400132C RID: 4908
		public LookTargets lookTargets;

		// Token: 0x0400132D RID: 4909
		public Quest quest;

		// Token: 0x0400132E RID: 4910
		private Vector2 cachedSize = new Vector2(-1f, -1f);

		// Token: 0x0400132F RID: 4911
		public Rect lastDrawRect;

		// Token: 0x04001330 RID: 4912
		private const float DefaultMessageLifespan = 13f;

		// Token: 0x04001331 RID: 4913
		private const float FadeoutDuration = 0.6f;
	}
}

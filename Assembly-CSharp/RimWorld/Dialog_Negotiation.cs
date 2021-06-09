using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0A RID: 6666
	public class Dialog_Negotiation : Dialog_NodeTree
	{
		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x0600935D RID: 37725 RVA: 0x00062BAE File Offset: 0x00060DAE
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(720f, 600f);
			}
		}

		// Token: 0x0600935E RID: 37726 RVA: 0x00062BBF File Offset: 0x00060DBF
		public Dialog_Negotiation(Pawn negotiator, ICommunicable commTarget, DiaNode startNode, bool radioMode) : base(startNode, radioMode, false, null)
		{
			this.negotiator = negotiator;
			this.commTarget = commTarget;
		}

		// Token: 0x0600935F RID: 37727 RVA: 0x002A7034 File Offset: 0x002A5234
		public override void DoWindowContents(Rect inRect)
		{
			GUI.BeginGroup(inRect);
			Rect rect = new Rect(0f, 0f, inRect.width / 2f, 70f);
			Rect rect2 = new Rect(0f, rect.yMax, rect.width, 60f);
			Rect rect3 = new Rect(inRect.width / 2f, 0f, inRect.width / 2f, 70f);
			Rect rect4 = new Rect(inRect.width / 2f, rect.yMax, rect.width, 60f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, this.negotiator.LabelCap);
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect3, this.commTarget.GetCallLabel());
			Text.Anchor = TextAnchor.UpperLeft;
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.7f);
			Widgets.Label(rect2, "SocialSkillIs".Translate(this.negotiator.skills.GetSkill(SkillDefOf.Social).Level));
			Text.Anchor = TextAnchor.UpperRight;
			Widgets.Label(rect4, this.commTarget.GetInfoText());
			Faction faction = this.commTarget.GetFaction();
			if (faction != null)
			{
				FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
				GUI.color = playerRelationKind.GetColor();
				Widgets.Label(new Rect(rect4.x, rect4.y + Text.CalcHeight(this.commTarget.GetInfoText(), rect4.width) + Text.SpaceBetweenLines, rect4.width, 30f), playerRelationKind.GetLabel());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
			GUI.EndGroup();
			float num = 147f;
			Rect rect5 = new Rect(0f, num, inRect.width, inRect.height - num);
			base.DrawNode(rect5);
		}

		// Token: 0x04005D5A RID: 23898
		protected Pawn negotiator;

		// Token: 0x04005D5B RID: 23899
		protected ICommunicable commTarget;

		// Token: 0x04005D5C RID: 23900
		private const float TitleHeight = 70f;

		// Token: 0x04005D5D RID: 23901
		private const float InfoHeight = 60f;
	}
}

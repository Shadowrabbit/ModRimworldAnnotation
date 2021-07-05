using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CE RID: 2510
	public class Thought_DecreeUnmet : Thought_Situational
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x00154B08 File Offset: 0x00152D08
		public override string LabelCap
		{
			get
			{
				string text = base.LabelCap;
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					int num = this.TicksSinceQuestUnmet(questPart_SituationalThought);
					if (num > 0)
					{
						text = text + " (" + num.ToStringTicksToDays("F0") + ")";
					}
				}
				return text;
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06003E3C RID: 15932 RVA: 0x00154B64 File Offset: 0x00152D64
		public override string Description
		{
			get
			{
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					return base.Description.Formatted("(" + questPart_SituationalThought.quest.name + ")");
				}
				return "";
			}
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x00154BC8 File Offset: 0x00152DC8
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
			if (questPart_SituationalThought == null)
			{
				return 0f;
			}
			float x = (float)this.TicksSinceQuestUnmet(questPart_SituationalThought) / 60000f;
			return (float)Mathf.RoundToInt(Thought_DecreeUnmet.MoodOffsetFromUnmetDaysCurve.Evaluate(x));
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x00154C33 File Offset: 0x00152E33
		private int TicksSinceQuestUnmet(QuestPart_SituationalThought questPart)
		{
			return questPart.quest.TicksSinceAccepted - questPart.delayTicks;
		}

		// Token: 0x040020E8 RID: 8424
		private static readonly SimpleCurve MoodOffsetFromUnmetDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, -5f),
				true
			},
			{
				new CurvePoint(15f, -15f),
				true
			}
		};
	}
}

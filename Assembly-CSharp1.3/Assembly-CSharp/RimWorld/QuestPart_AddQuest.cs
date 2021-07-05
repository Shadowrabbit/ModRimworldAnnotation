using System;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B61 RID: 2913
	public abstract class QuestPart_AddQuest : QuestPart
	{
		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06004421 RID: 17441
		public abstract QuestScriptDef QuestDef { get; }

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool CanAdd
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x0016A07E File Offset: 0x0016827E
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.CanAdd)
			{
				this.AddQuest();
				this.PostAdd();
			}
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0016A0AE File Offset: 0x001682AE
		private void AddQuest()
		{
			QuestUtility.GenerateQuestAndMakeAvailable(this.QuestDef, this.GetSlate()).Accept(this.acceptee);
		}

		// Token: 0x06004425 RID: 17445
		public abstract Slate GetSlate();

		// Token: 0x06004426 RID: 17446 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostAdd()
		{
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0016A0CC File Offset: 0x001682CC
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "AddQuest " + this.QuestDef.defName, true, true, true))
			{
				this.AddQuest();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0016A12B File Offset: 0x0016832B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.acceptee, "acceptee", false);
		}

		// Token: 0x04002958 RID: 10584
		public string inSignal;

		// Token: 0x04002959 RID: 10585
		public Pawn acceptee;
	}
}

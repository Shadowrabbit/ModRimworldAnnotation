using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200067B RID: 1659
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x06002EF3 RID: 12019 RVA: 0x00117B10 File Offset: 0x00115D10
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds, null)
		{
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x00117B21 File Offset: 0x00115D21
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f, Func<bool> canSendMessage = null)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
			this.canSendMessage = canSendMessage;
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x00117B59 File Offset: 0x00115D59
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x00117B91 File Offset: 0x00115D91
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x00117BCC File Offset: 0x00115DCC
		public override void DoAction(Transition trans)
		{
			if (!this.repeatAvoiderTag.NullOrEmpty() && !MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				return;
			}
			if (this.canSendMessage != null && !this.canSendMessage())
			{
				return;
			}
			TargetInfo target = (this.lookTargetGetter != null) ? this.lookTargetGetter() : this.lookTarget;
			if (!target.IsValid)
			{
				target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
			}
			Messages.Message(this.message, target, this.type, true);
		}

		// Token: 0x04001CB3 RID: 7347
		public string message;

		// Token: 0x04001CB4 RID: 7348
		public MessageTypeDef type;

		// Token: 0x04001CB5 RID: 7349
		public TargetInfo lookTarget;

		// Token: 0x04001CB6 RID: 7350
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04001CB7 RID: 7351
		public string repeatAvoiderTag;

		// Token: 0x04001CB8 RID: 7352
		public float repeatAvoiderSeconds;

		// Token: 0x04001CB9 RID: 7353
		private Func<bool> canSendMessage;
	}
}

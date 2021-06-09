using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000AE2 RID: 2786
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x060041D4 RID: 16852 RVA: 0x00030FD8 File Offset: 0x0002F1D8
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds)
		{
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x00030FE8 File Offset: 0x0002F1E8
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x00031018 File Offset: 0x0002F218
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

		// Token: 0x060041D7 RID: 16855 RVA: 0x00031050 File Offset: 0x0002F250
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

		// Token: 0x060041D8 RID: 16856 RVA: 0x00188968 File Offset: 0x00186B68
		public override void DoAction(Transition trans)
		{
			if (!this.repeatAvoiderTag.NullOrEmpty() && !MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
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

		// Token: 0x04002D44 RID: 11588
		public string message;

		// Token: 0x04002D45 RID: 11589
		public MessageTypeDef type;

		// Token: 0x04002D46 RID: 11590
		public TargetInfo lookTarget;

		// Token: 0x04002D47 RID: 11591
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04002D48 RID: 11592
		public string repeatAvoiderTag;

		// Token: 0x04002D49 RID: 11593
		public float repeatAvoiderSeconds;
	}
}

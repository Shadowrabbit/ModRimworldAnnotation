using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EE3 RID: 3811
	public class PreceptComp_UnwillingToDo : PreceptComp
	{
		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06005A90 RID: 23184 RVA: 0x001F548E File Offset: 0x001F368E
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				if (this.nullifyingTraits != null)
				{
					int num;
					for (int i = 0; i < this.nullifyingTraits.Count; i = num + 1)
					{
						yield return this.nullifyingTraits[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x001F54A0 File Offset: 0x001F36A0
		public override bool MemberWillingToDo(HistoryEvent ev)
		{
			if (this.eventDef != null && ev.def != this.eventDef)
			{
				return true;
			}
			Pawn pawn;
			if (this.nullifyingTraits != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn) && pawn.story != null)
			{
				for (int i = 0; i < this.nullifyingTraits.Count; i++)
				{
					if (this.nullifyingTraits[i].HasTrait(pawn))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005A92 RID: 23186 RVA: 0x001F5517 File Offset: 0x001F3717
		public virtual string GetProhibitionText()
		{
			return this.description ?? this.eventDef.LabelCap;
		}

		// Token: 0x06005A93 RID: 23187 RVA: 0x001F5533 File Offset: 0x001F3733
		public override IEnumerable<string> GetDescriptions()
		{
			yield return "UnwillingToDoIdeoAction".Translate() + ": " + this.eventDef.LabelCap;
			yield break;
		}

		// Token: 0x04003507 RID: 13575
		public HistoryEventDef eventDef;

		// Token: 0x04003508 RID: 13576
		public List<TraitRequirement> nullifyingTraits;
	}
}

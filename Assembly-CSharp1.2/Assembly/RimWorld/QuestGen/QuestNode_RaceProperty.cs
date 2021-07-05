using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F7B RID: 8059
	public abstract class QuestNode_RaceProperty : QuestNode
	{
		// Token: 0x0600ABA4 RID: 43940 RVA: 0x0031F944 File Offset: 0x0031DB44
		protected override bool TestRunInt(Slate slate)
		{
			if (this.Matches(this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x0600ABA5 RID: 43941 RVA: 0x0031F994 File Offset: 0x0031DB94
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.Matches(this.value.GetValue(slate)))
			{
				if (this.node != null)
				{
					this.node.Run();
					return;
				}
			}
			else if (this.elseNode != null)
			{
				this.elseNode.Run();
			}
		}

		// Token: 0x0600ABA6 RID: 43942 RVA: 0x0031F9E4 File Offset: 0x0031DBE4
		private bool Matches(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is PawnKindDef)
			{
				return this.Matches(((PawnKindDef)obj).RaceProps);
			}
			if (obj is ThingDef)
			{
				return ((ThingDef)obj).race != null && this.Matches(((ThingDef)obj).race);
			}
			if (obj is Pawn)
			{
				return this.Matches(((Pawn)obj).RaceProps);
			}
			if (obj is Faction)
			{
				return ((Faction)obj).def.basicMemberKind != null && this.Matches(((Faction)obj).def.basicMemberKind);
			}
			if (obj is IEnumerable<Pawn>)
			{
				return ((IEnumerable<Pawn>)obj).Any<Pawn>() && ((IEnumerable<Pawn>)obj).All((Pawn x) => this.Matches(x.RaceProps));
			}
			if (obj is IEnumerable<Thing>)
			{
				return ((IEnumerable<Thing>)obj).Any<Thing>() && ((IEnumerable<Thing>)obj).All((Thing x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			if (obj is IEnumerable<object>)
			{
				return ((IEnumerable<object>)obj).Any<object>() && ((IEnumerable<object>)obj).All((object x) => x is Pawn && this.Matches(((Pawn)x).RaceProps));
			}
			return obj is string && !((string)obj).NullOrEmpty() && this.Matches(DefDatabase<PawnKindDef>.GetNamed((string)obj, true).RaceProps);
		}

		// Token: 0x0600ABA7 RID: 43943
		protected abstract bool Matches(RaceProperties raceProperties);

		// Token: 0x040074F1 RID: 29937
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x040074F2 RID: 29938
		public QuestNode node;

		// Token: 0x040074F3 RID: 29939
		public QuestNode elseNode;
	}
}

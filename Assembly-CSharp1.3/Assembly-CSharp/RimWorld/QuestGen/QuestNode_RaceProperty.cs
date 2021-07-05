using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016A7 RID: 5799
	public abstract class QuestNode_RaceProperty : QuestNode
	{
		// Token: 0x060086A2 RID: 34466 RVA: 0x00304018 File Offset: 0x00302218
		protected override bool TestRunInt(Slate slate)
		{
			if (this.Matches(this.value.GetValue(slate)))
			{
				return this.node == null || this.node.TestRun(slate);
			}
			return this.elseNode == null || this.elseNode.TestRun(slate);
		}

		// Token: 0x060086A3 RID: 34467 RVA: 0x00304068 File Offset: 0x00302268
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

		// Token: 0x060086A4 RID: 34468 RVA: 0x003040B8 File Offset: 0x003022B8
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

		// Token: 0x060086A5 RID: 34469
		protected abstract bool Matches(RaceProperties raceProperties);

		// Token: 0x0400546E RID: 21614
		[NoTranslate]
		public SlateRef<object> value;

		// Token: 0x0400546F RID: 21615
		public QuestNode node;

		// Token: 0x04005470 RID: 21616
		public QuestNode elseNode;
	}
}

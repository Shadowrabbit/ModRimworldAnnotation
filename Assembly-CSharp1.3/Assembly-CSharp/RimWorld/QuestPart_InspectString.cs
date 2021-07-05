using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B7F RID: 2943
	public class QuestPart_InspectString : QuestPartActivable
	{
		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060044CF RID: 17615 RVA: 0x0016C87F File Offset: 0x0016AA7F
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.targets.Count; i = num + 1)
				{
					ISelectable selectable = this.targets[i];
					if (selectable is Thing)
					{
						yield return (Thing)selectable;
					}
					else if (selectable is WorldObject)
					{
						yield return (WorldObject)selectable;
					}
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0016C88F File Offset: 0x0016AA8F
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedInspectString = receivedArgs.GetFormattedText(this.inspectString);
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0016C8B5 File Offset: 0x0016AAB5
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.targets.Contains(target))
			{
				return this.resolvedInspectString;
			}
			return null;
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x0016C8D0 File Offset: 0x0016AAD0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ISelectable>(ref this.targets, "targets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Values.Look<string>(ref this.resolvedInspectString, "resolvedInspectString", null, false);
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x0016C91D File Offset: 0x0016AB1D
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.inspectString = "Debug inspect string.";
			}
		}

		// Token: 0x040029C0 RID: 10688
		public List<ISelectable> targets = new List<ISelectable>();

		// Token: 0x040029C1 RID: 10689
		public string inspectString;

		// Token: 0x040029C2 RID: 10690
		private string resolvedInspectString;

		// Token: 0x040029C3 RID: 10691
		private ILoadReferenceable targetRef;
	}
}

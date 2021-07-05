using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D9 RID: 4313
	public class QuestPart_InspectString : QuestPartActivable
	{
		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06005E18 RID: 24088 RVA: 0x000412EE File Offset: 0x0003F4EE
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

		// Token: 0x06005E19 RID: 24089 RVA: 0x000412FE File Offset: 0x0003F4FE
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedInspectString = receivedArgs.GetFormattedText(this.inspectString);
		}

		// Token: 0x06005E1A RID: 24090 RVA: 0x00041324 File Offset: 0x0003F524
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.targets.Contains(target))
			{
				return this.resolvedInspectString;
			}
			return null;
		}

		// Token: 0x06005E1B RID: 24091 RVA: 0x001DE4C4 File Offset: 0x001DC6C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<ISelectable>(ref this.targets, "targets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Values.Look<string>(ref this.resolvedInspectString, "resolvedInspectString", null, false);
		}

		// Token: 0x06005E1C RID: 24092 RVA: 0x0004133C File Offset: 0x0003F53C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.inspectString = "Debug inspect string.";
			}
		}

		// Token: 0x04003EE8 RID: 16104
		public List<ISelectable> targets = new List<ISelectable>();

		// Token: 0x04003EE9 RID: 16105
		public string inspectString;

		// Token: 0x04003EEA RID: 16106
		private string resolvedInspectString;

		// Token: 0x04003EEB RID: 16107
		private ILoadReferenceable targetRef;
	}
}

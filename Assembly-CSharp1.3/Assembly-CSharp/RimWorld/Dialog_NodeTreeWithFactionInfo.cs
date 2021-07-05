using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012FB RID: 4859
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x060074B8 RID: 29880 RVA: 0x0027A875 File Offset: 0x00278A75
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x0027A898 File Offset: 0x00278A98
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		// Token: 0x04004052 RID: 16466
		private Faction faction;

		// Token: 0x04004053 RID: 16467
		private const float RelatedFactionInfoSize = 79f;
	}
}

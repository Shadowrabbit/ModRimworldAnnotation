using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0B RID: 6667
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x06009360 RID: 37728 RVA: 0x00062BDA File Offset: 0x00060DDA
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06009361 RID: 37729 RVA: 0x002A7224 File Offset: 0x002A5424
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		// Token: 0x04005D5E RID: 23902
		private Faction faction;

		// Token: 0x04005D5F RID: 23903
		private const float RelatedFactionInfoSize = 79f;
	}
}

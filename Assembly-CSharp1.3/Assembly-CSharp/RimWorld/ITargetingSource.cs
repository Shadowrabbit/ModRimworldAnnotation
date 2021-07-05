using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135F RID: 4959
	public interface ITargetingSource
	{
		// Token: 0x0600783F RID: 30783
		bool CanHitTarget(LocalTargetInfo target);

		// Token: 0x06007840 RID: 30784
		bool ValidateTarget(LocalTargetInfo target, bool showMessages = true);

		// Token: 0x06007841 RID: 30785
		void DrawHighlight(LocalTargetInfo target);

		// Token: 0x06007842 RID: 30786
		void OrderForceTarget(LocalTargetInfo target);

		// Token: 0x06007843 RID: 30787
		void OnGUI(LocalTargetInfo target);

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06007844 RID: 30788
		bool CasterIsPawn { get; }

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06007845 RID: 30789
		bool IsMeleeAttack { get; }

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06007846 RID: 30790
		bool Targetable { get; }

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06007847 RID: 30791
		bool MultiSelect { get; }

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06007848 RID: 30792
		bool HidePawnTooltips { get; }

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06007849 RID: 30793
		Thing Caster { get; }

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x0600784A RID: 30794
		Pawn CasterPawn { get; }

		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x0600784B RID: 30795
		Verb GetVerb { get; }

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x0600784C RID: 30796
		Texture2D UIIcon { get; }

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x0600784D RID: 30797
		TargetingParameters targetParams { get; }

		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x0600784E RID: 30798
		ITargetingSource DestinationSelector { get; }
	}
}

using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2D RID: 6957
	public interface ITargetingSource
	{
		// Token: 0x06009929 RID: 39209
		bool CanHitTarget(LocalTargetInfo target);

		// Token: 0x0600992A RID: 39210
		bool ValidateTarget(LocalTargetInfo target);

		// Token: 0x0600992B RID: 39211
		void DrawHighlight(LocalTargetInfo target);

		// Token: 0x0600992C RID: 39212
		void OrderForceTarget(LocalTargetInfo target);

		// Token: 0x0600992D RID: 39213
		void OnGUI(LocalTargetInfo target);

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x0600992E RID: 39214
		bool CasterIsPawn { get; }

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x0600992F RID: 39215
		bool IsMeleeAttack { get; }

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06009930 RID: 39216
		bool Targetable { get; }

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x06009931 RID: 39217
		bool MultiSelect { get; }

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06009932 RID: 39218
		Thing Caster { get; }

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06009933 RID: 39219
		Pawn CasterPawn { get; }

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06009934 RID: 39220
		Verb GetVerb { get; }

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06009935 RID: 39221
		Texture2D UIIcon { get; }

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06009936 RID: 39222
		TargetingParameters targetParams { get; }

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06009937 RID: 39223
		ITargetingSource DestinationSelector { get; }
	}
}

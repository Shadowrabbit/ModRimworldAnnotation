using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA8 RID: 7080
	[StaticConstructorOnStartup]
	public static class TexCommand
	{
		// Token: 0x0400634F RID: 25423
		public static readonly Texture2D DesirePower = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true);

		// Token: 0x04006350 RID: 25424
		public static readonly Texture2D Draft = ContentFinder<Texture2D>.Get("UI/Commands/Draft", true);

		// Token: 0x04006351 RID: 25425
		public static readonly Texture2D ReleaseAnimals = ContentFinder<Texture2D>.Get("UI/Commands/ReleaseAnimals", true);

		// Token: 0x04006352 RID: 25426
		public static readonly Texture2D HoldOpen = ContentFinder<Texture2D>.Get("UI/Commands/HoldOpen", true);

		// Token: 0x04006353 RID: 25427
		public static readonly Texture2D GatherSpotActive = ContentFinder<Texture2D>.Get("UI/Commands/GatherSpotActive", true);

		// Token: 0x04006354 RID: 25428
		public static readonly Texture2D Install = ContentFinder<Texture2D>.Get("UI/Commands/Install", true);

		// Token: 0x04006355 RID: 25429
		public static readonly Texture2D SquadAttack = ContentFinder<Texture2D>.Get("UI/Commands/SquadAttack", true);

		// Token: 0x04006356 RID: 25430
		public static readonly Texture2D AttackMelee = ContentFinder<Texture2D>.Get("UI/Commands/AttackMelee", true);

		// Token: 0x04006357 RID: 25431
		public static readonly Texture2D Attack = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);

		// Token: 0x04006358 RID: 25432
		public static readonly Texture2D FireAtWill = ContentFinder<Texture2D>.Get("UI/Commands/FireAtWill", true);

		// Token: 0x04006359 RID: 25433
		public static readonly Texture2D ToggleVent = ContentFinder<Texture2D>.Get("UI/Commands/Vent", true);

		// Token: 0x0400635A RID: 25434
		public static readonly Texture2D PauseCaravan = ContentFinder<Texture2D>.Get("UI/Commands/PauseCaravan", true);

		// Token: 0x0400635B RID: 25435
		public static readonly Texture2D ForbidOff = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOff", true);

		// Token: 0x0400635C RID: 25436
		public static readonly Texture2D ForbidOn = ContentFinder<Texture2D>.Get("UI/Designators/ForbidOn", true);

		// Token: 0x0400635D RID: 25437
		public static readonly Texture2D RearmTrap = ContentFinder<Texture2D>.Get("UI/Designators/RearmTrap", true);

		// Token: 0x0400635E RID: 25438
		public static readonly Texture2D CannotShoot = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400635F RID: 25439
		public static readonly Texture2D ClearPrioritizedWork = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04006360 RID: 25440
		public static readonly Texture2D RemoveRoutePlannerWaypoint = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04006361 RID: 25441
		public static readonly Texture2D OpenLinkedQuestTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}

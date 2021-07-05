using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000E0 RID: 224
	public class MentalStateDef : Def
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001EECA File Offset: 0x0001D0CA
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0001EF0A File Offset: 0x0001D10A
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0001EF18 File Offset: 0x0001D118
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0001EF5D File Offset: 0x0001D15D
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x040004E8 RID: 1256
		public Type stateClass = typeof(MentalState);

		// Token: 0x040004E9 RID: 1257
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x040004EA RID: 1258
		public MentalStateCategory category;

		// Token: 0x040004EB RID: 1259
		public bool prisonersCanDo = true;

		// Token: 0x040004EC RID: 1260
		public bool slavesCanDo = true;

		// Token: 0x040004ED RID: 1261
		public bool unspawnedCanDo;

		// Token: 0x040004EE RID: 1262
		public bool colonistsOnly;

		// Token: 0x040004EF RID: 1263
		public bool slavesOnly;

		// Token: 0x040004F0 RID: 1264
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x040004F1 RID: 1265
		public bool blockNormalThoughts;

		// Token: 0x040004F2 RID: 1266
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x040004F3 RID: 1267
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x040004F4 RID: 1268
		public bool blockRandomInteraction;

		// Token: 0x040004F5 RID: 1269
		public EffecterDef stateEffecter;

		// Token: 0x040004F6 RID: 1270
		public TaleDef tale;

		// Token: 0x040004F7 RID: 1271
		public bool allowBeatfire;

		// Token: 0x040004F8 RID: 1272
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x040004F9 RID: 1273
		public bool ignoreDrugPolicy;

		// Token: 0x040004FA RID: 1274
		public float recoveryMtbDays = 1f;

		// Token: 0x040004FB RID: 1275
		public int minTicksBeforeRecovery = 500;

		// Token: 0x040004FC RID: 1276
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x040004FD RID: 1277
		public bool recoverFromSleep;

		// Token: 0x040004FE RID: 1278
		public bool recoverFromDowned = true;

		// Token: 0x040004FF RID: 1279
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x04000500 RID: 1280
		public ThoughtDef moodRecoveryThought;

		// Token: 0x04000501 RID: 1281
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04000502 RID: 1282
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04000503 RID: 1283
		public LetterDef beginLetterDef;

		// Token: 0x04000504 RID: 1284
		public Color nameColor = Color.green;

		// Token: 0x04000505 RID: 1285
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x04000506 RID: 1286
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04000507 RID: 1287
		public bool escapingPrisonersIgnore;

		// Token: 0x04000508 RID: 1288
		private MentalStateWorker workerInt;
	}
}

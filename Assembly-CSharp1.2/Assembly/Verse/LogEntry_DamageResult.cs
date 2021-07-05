using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001D8 RID: 472
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x0000F72F File Offset: 0x0000D92F
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0000F738 File Offset: 0x0000D938
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x0000C32E File Offset: 0x0000A52E
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000A3AC4 File Offset: 0x000A1CC4
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x000A3B24 File Offset: 0x000A1D24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.damagedParts != null)
			{
				for (int i = this.damagedParts.Count - 1; i >= 0; i--)
				{
					if (this.damagedParts[i] == null)
					{
						this.damagedParts.RemoveAt(i);
						if (i < this.damagedPartsDestroyed.Count)
						{
							this.damagedPartsDestroyed.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x04000AA9 RID: 2729
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04000AAA RID: 2730
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04000AAB RID: 2731
		protected bool deflected;
	}
}

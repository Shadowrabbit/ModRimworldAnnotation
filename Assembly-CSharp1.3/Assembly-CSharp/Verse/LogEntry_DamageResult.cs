using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000137 RID: 311
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x06000892 RID: 2194 RVA: 0x00027F55 File Offset: 0x00026155
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00027F5E File Offset: 0x0002615E
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00002688 File Offset: 0x00000888
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00027F7C File Offset: 0x0002617C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00027FDC File Offset: 0x000261DC
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

		// Token: 0x040007FF RID: 2047
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04000800 RID: 2048
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04000801 RID: 2049
		protected bool deflected;
	}
}

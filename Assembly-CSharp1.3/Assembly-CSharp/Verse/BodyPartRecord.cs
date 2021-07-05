using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000082 RID: 130
	public class BodyPartRecord
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x0001982B File Offset: 0x00017A2B
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00019836 File Offset: 0x00017A36
		public string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				return this.def.label;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00019857 File Offset: 0x00017A57
		public string LabelCap
		{
			get
			{
				if (this.customLabel.NullOrEmpty())
				{
					return this.def.LabelCap;
				}
				if (this.cachedCustomLabelCap == null)
				{
					this.cachedCustomLabelCap = this.customLabel.CapitalizeFirst();
				}
				return this.cachedCustomLabelCap;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060004CA RID: 1226 RVA: 0x00019896 File Offset: 0x00017A96
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x000198A3 File Offset: 0x00017AA3
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x000198B0 File Offset: 0x00017AB0
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000198C0 File Offset: 0x00017AC0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"BodyPartRecord(",
				(this.def != null) ? this.def.defName : "NULL_DEF",
				" parts.Count=",
				this.parts.Count,
				")"
			});
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00019920 File Offset: 0x00017B20
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00019930 File Offset: 0x00017B30
		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00019965 File Offset: 0x00017B65
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetChildParts(tag))
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001997C File Offset: 0x00017B7C
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				yield return this.parts[i];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001998C File Offset: 0x00017B8C
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001999A File Offset: 0x00017B9A
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord bodyPartRecord = this;
			while (bodyPartRecord.parent != null && bodyPartRecord.parent.def.tags.Contains(tag))
			{
				bodyPartRecord = bodyPartRecord.parent;
			}
			foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.GetChildParts(tag))
			{
				yield return bodyPartRecord2;
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040001AC RID: 428
		public BodyDef body;

		// Token: 0x040001AD RID: 429
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x040001AE RID: 430
		[MustTranslate]
		public string customLabel;

		// Token: 0x040001AF RID: 431
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x040001B0 RID: 432
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x040001B1 RID: 433
		public BodyPartHeight height;

		// Token: 0x040001B2 RID: 434
		public BodyPartDepth depth;

		// Token: 0x040001B3 RID: 435
		public float coverage = 1f;

		// Token: 0x040001B4 RID: 436
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x040001B5 RID: 437
		[NoTranslate]
		public string woundAnchorTag;

		// Token: 0x040001B6 RID: 438
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x040001B7 RID: 439
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x040001B8 RID: 440
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x040001B9 RID: 441
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}

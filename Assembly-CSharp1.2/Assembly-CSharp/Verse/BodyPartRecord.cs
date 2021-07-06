using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public class BodyPartRecord
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0000B581 File Offset: 0x00009781
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0000B58C File Offset: 0x0000978C
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0000B5AD File Offset: 0x000097AD
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

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0000B5EC File Offset: 0x000097EC
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0000B5F9 File Offset: 0x000097F9
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0000B606 File Offset: 0x00009806
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0008F530 File Offset: 0x0008D730
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

		// Token: 0x06000671 RID: 1649 RVA: 0x0000B614 File Offset: 0x00009814
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0008F590 File Offset: 0x0008D790
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

		// Token: 0x06000673 RID: 1651 RVA: 0x0000B622 File Offset: 0x00009822
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

		// Token: 0x06000674 RID: 1652 RVA: 0x0000B639 File Offset: 0x00009839
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

		// Token: 0x06000675 RID: 1653 RVA: 0x0000B649 File Offset: 0x00009849
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0000B657 File Offset: 0x00009857
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

		// Token: 0x04000347 RID: 839
		public BodyDef body;

		// Token: 0x04000348 RID: 840
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x04000349 RID: 841
		[MustTranslate]
		public string customLabel;

		// Token: 0x0400034A RID: 842
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x0400034B RID: 843
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x0400034C RID: 844
		public BodyPartHeight height;

		// Token: 0x0400034D RID: 845
		public BodyPartDepth depth;

		// Token: 0x0400034E RID: 846
		public float coverage = 1f;

		// Token: 0x0400034F RID: 847
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x04000350 RID: 848
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x04000351 RID: 849
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x04000352 RID: 850
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x04000353 RID: 851
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}

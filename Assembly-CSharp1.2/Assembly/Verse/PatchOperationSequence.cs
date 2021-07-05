using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x0200036D RID: 877
	public class PatchOperationSequence : PatchOperation
	{
		// Token: 0x06001630 RID: 5680 RVA: 0x000D5650 File Offset: 0x000D3850
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x00015BD4 File Offset: 0x00013DD4
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000D56B4 File Offset: 0x000D38B4
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}

		// Token: 0x04001104 RID: 4356
		private List<PatchOperation> operations;

		// Token: 0x04001105 RID: 4357
		private PatchOperation lastFailedOperation;
	}
}

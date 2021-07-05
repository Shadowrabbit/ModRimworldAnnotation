using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000058 RID: 88
	public static class InspectTabManager
	{
		// Token: 0x060003FB RID: 1019 RVA: 0x00015480 File Offset: 0x00013680
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x04000130 RID: 304
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}

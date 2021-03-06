using System.Collections.Generic;
using System.Globalization;

namespace IAS.Feature.Language.LanguageCopy
{
    internal class CultureComparer : Comparer<CultureInfo>
  {
    // Methods
    public override int Compare(CultureInfo x, CultureInfo y)
    {
      return x.Name.CompareTo(y.Name);
    }
  }
}

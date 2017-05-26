using System;

namespace DotNet.Ildasm
{
    public class ItemFilter
    {
        public ItemFilter(string itemFilter)
        {
            if (!string.IsNullOrEmpty(itemFilter))
            {
                var index = itemFilter.IndexOf("::", StringComparison.CurrentCultureIgnoreCase);
                if (index > -1)
                {
                    if (index > 0)
                    {
                        Class = itemFilter.Substring(0, itemFilter.Length - index);
                    }

                    Method = itemFilter.Substring(index + 2, itemFilter.Length - (index + 2));
                }
                else
                {
                    Class = itemFilter;
                }
            }
        }

        public bool HasFilter => (!string.IsNullOrEmpty(Class) || !string.IsNullOrEmpty(Method));
        public string Method { get; }
        public string Class { get; }
    }
}
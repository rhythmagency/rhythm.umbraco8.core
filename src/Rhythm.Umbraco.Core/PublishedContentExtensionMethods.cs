namespace Rhythm.Umbraco.Core
{

    // Namespaces.
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for IPublishedContent.
    /// </summary>
    public static class PublishedContentExtensionMethods
    {

        #region Extension Methods

        /// <summary>
        /// Searches for the nearest ancestors with the specified content types.
        /// </summary>
        /// <param name="source">
        /// The node to start searching from.
        /// </param>
        /// <param name="typeAliases">
        /// The aliases of the content types.
        /// </param>
        /// <param name="includeSelf">
        /// Include the supplied node in the search (by default the search
        /// will start at the parent)?
        /// </param>
        /// <returns>
        /// The nearest matching ancestor, or null.
        /// </returns>
        public static IPublishedContent NearestAncestorOfTypes(this IPublishedContent source,
            bool includeSelf = false, params string[] typeAliases)
        {
            if (!includeSelf && source != null)
            {
                source = source.Parent;
            }
            while (source != null)
            {
                var alias = source.DocumentTypeAlias;
                for (int i = 0; i < typeAliases.Length; i++)
                {
                    if (typeAliases[i].InvariantEquals(alias))
                    {
                        return source;
                    }
                }
                source = source.Parent;
            }
            return null;
        }

        /// <summary>
        /// Finds the descendant children located by the specified list of content type aliases,
        /// relative to the specified page.
        /// </summary>
        /// <param name="source">
        /// The parent page to start the search.
        /// </param>
        /// <param name="typeAliases">
        /// The content type aliases.
        /// </param>
        /// <returns>
        /// The descendant nodes.
        /// </returns>
        /// <remarks>
        /// This is faster than Umbraco's implementation of Descendants() because this version
        /// does not need to scan the entire content tree under the specified node.
        /// </remarks>
        public static List<IPublishedContent> ChildrenByTypePath(this IPublishedContent source,
            params string[] typeAliases)
        {
            var children = new List<IPublishedContent>();
            if (source != null)
            {
                children.Add(source);
                foreach (var alias in typeAliases)
                {
                    children = children.SelectMany(y =>
                        y.Children.Where(x => alias.InvariantEquals(x.DocumentTypeAlias))).ToList();
                    if (children.Count == 0)
                    {
                        break;
                    }
                }
            }
            return children;
        }

        #endregion

    }

}
namespace Rhythm.Umbraco.Core
{

    // Namespaces.
    using global::Umbraco.Core;
    using global::Umbraco.Core.Models.PublishedContent;
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
                var alias = source.ContentType.Alias;
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
                        y.Children.Where(x => alias.InvariantEquals(x.ContentType.Alias))).ToList();
                    if (children.Count == 0)
                    {
                        break;
                    }
                }
            }
            return children;
        }

        /// <summary>
        /// Finds descendants of the specified content node that have the specified content types.
        /// </summary>
        /// <param name="source">
        /// The parent page to start the search.
        /// </param>
        /// <param name="includeSelf">
        /// Include the supplied node in the search?
        /// </param>
        /// <param name="typeAliases">
        /// The aliases of the content types.
        /// </param>
        /// <returns>
        /// The collection of descendants.
        /// </returns>
        public static List<IPublishedContent> DescendantsOfTypes(this IPublishedContent source,
            bool includeSelf, params string[] typeAliases)
        {

            // Validate input.
            var descendants = new List<IPublishedContent>();
            if (source == null)
            {
                return descendants;
            }

            // Include the soure content node in the check for descendants?
            if (includeSelf)
            {
                if (typeAliases.InvariantContains(source.ContentType.Alias))
                {
                    descendants.Add(source);
                }
            }

            // Check children, and then their children, recursively.
            foreach (var child in source.Children)
            {

                // This check is performed here rather than relying on the shorter
                // recursive implementation to avoid creating a bunch of lists when
                // working with relatively flat content trees.
                if (typeAliases.InvariantContains(child.ContentType.Alias))
                {
                    descendants.Add(child);
                }

                // Recursively check further descendants.
                if (child.Children.Any())
                {
                    descendants.AddRange(child.DescendantsOfTypes(false, typeAliases));
                }

            }

            // Return the matching descendants.
            return descendants;

        }

        #endregion

    }

}


Refer to the [generated documentation](docs/generated.md) for more details.

To create a new release to NuGet, see the [NuGet documentation](docs/nuget.md).




# Introduction

A library of tools for working with Umbraco. The primary class is `PublishedContentExtensionMethods`.

Refer to the [generated documentation](docs/generated.md) for more details.

# Installation

Install with NuGet. Search for "Rhythm.Umbraco.Core".

# Overview

## PublishedContentExtensionMethods

* **NearestAncestorOfTypes** Searches for the nearest ancestors with the specified content types.
* **ChildrenByTypePath** Finds the descendant children located by the specified list of content type aliases, relative to the specified page.

The following code sample shows how to use both `NearestAncestorOfTypes` and `ChildrenByTypePath` to get the blog articles in the current site:

```c#
namespace Sample
{
    using Rhythm.Umbraco.Core;
    using System.Collections.Generic;
    using Umbraco.Core.Models;
    public class ArticleHelper
    {
        public IEnumerable<IPublishedContent> GetArticles(IPublishedContent page)
        {
            return page
                .NearestAncestorOfTypes(true, "home", "homepage")
                .ChildrenByTypePath("blogLanding", "blogArticle");
        }
    }
}
```

# Maintainers

To create a new release to NuGet, see the [NuGet documentation](docs/nuget.md).

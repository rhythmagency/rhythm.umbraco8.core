# Rhythm.Umbraco.Core

<table>
<tbody>
<tr>
<td><a href="#publishedcontentextensionmethods">PublishedContentExtensionMethods</a></td>
</tr>
</tbody>
</table>


## PublishedContentExtensionMethods

Extension methods for IPublishedContent.

### ChildrenByTypePath(source, typeAliases)

Finds the descendant children located by the specified list of content type aliases, relative to the specified page.

| Name | Description |
| ---- | ----------- |
| source | *Umbraco.Core.Models.IPublishedContent*<br>The parent page to start the search. |
| typeAliases | *System.String[]*<br>The content type aliases. |

#### Returns

The descendant nodes.

#### Remarks

This is faster than Umbraco's implementation of Descendants() because this version does not need to scan the entire content tree under the specified node.

### NearestAncestorOfTypes(source, typeAliases, includeSelf)

Searches for the nearest ancestors with the specified content types.

| Name | Description |
| ---- | ----------- |
| source | *Umbraco.Core.Models.IPublishedContent*<br>The node to start searching from. |
| typeAliases | *System.Boolean*<br>The aliases of the content types. |
| includeSelf | *System.String[]*<br>Include the supplied node in the search (by default the search will start at the parent)? |

#### Returns

The nearest matching ancestor, or null.

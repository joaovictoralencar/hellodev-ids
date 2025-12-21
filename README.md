# HelloDev IDs

GUID-based unique identifier system with localization support.

## Features

- **ID_SO** - ScriptableObject with auto-generated GUID
- Localized display names via Unity Localization
- Developer-friendly internal names (`DevName`)
- Equality comparison based on GUID (implements `IEquatable<ID_SO>`)
- `==` and `!=` operators for natural comparison syntax
- Cached GUID parsing for performance
- Auto-generates GUID on creation

## Installation

### Via Package Manager (Local)
1. Open Unity Package Manager
2. Click "+" > "Add package from disk"
3. Navigate to this folder and select `package.json`

## Usage

### Creating an ID

```
Assets > Create > HelloDev > IDs > New ID
```

### Using IDs

```csharp
using HelloDev.IDs;

public class Item : MonoBehaviour
{
    [SerializeField] private ID_SO itemId;

    // Use == operator for comparison
    public bool IsItem(ID_SO otherId) => itemId == otherId;

    // Or use Equals
    public bool IsItemAlt(ID_SO otherId) => itemId.Equals(otherId);

    public void LogInfo()
    {
        Debug.Log($"Dev Name: {itemId.DevName}");
        Debug.Log($"GUID: {itemId.Id}");
    }
}
```

### ID-Based Lookup

```csharp
using System;
using System.Collections.Generic;
using HelloDev.IDs;

private Dictionary<Guid, int> itemCounts = new();

public void AddItem(ID_SO itemId, int count)
{
    var guid = itemId.Id;  // Cached, efficient
    itemCounts[guid] = itemCounts.GetValueOrDefault(guid) + count;
}
```

### Null-Safe Comparison

```csharp
// Safe even if either is null
if (itemId == otherId) { }
if (itemId != null) { }
```

## API Reference

### ID_SO

| Member | Type | Description |
|--------|------|-------------|
| `Id` | `Guid` | The unique GUID (cached after first access) |
| `DevName` | `string` | Developer-friendly internal name |
| `DisplayName` | `LocalizedString` | Localized name for UI display |
| `Equals(ID_SO)` | `bool` | IEquatable implementation for efficient comparison |
| `==`, `!=` | operators | Natural comparison syntax |

## Dependencies

### Required
- com.hellodev.utils (1.1.0+)
- com.unity.localization

### Optional
- Odin Inspector (enhanced inspector with [ReadOnly] attribute display)

## Changelog

### v1.1.0 (2025-12-21)
**Performance:**
- GUID is now cached after first parse (avoids repeated `Guid.Parse` calls)

**Robustness:**
- Empty/null ID strings now return `Guid.Empty` instead of throwing
- Cache is invalidated on editor changes via OnValidate

**API Improvements:**
- Implements `IEquatable<ID_SO>` for efficient collection operations
- Added `==` and `!=` operators for natural comparison syntax
- Improved `Equals` with `ReferenceEquals` short-circuit

**Documentation:**
- Added class-level XML documentation
- Added XML docs to all public members
- Added comment explaining empty OnScriptableObjectReset

**Code Quality:**
- Removed extra blank lines

**Package:**
- Updated Unity version to 6000.3
- Updated utils dependency to 1.1.0

### v1.0.0
- Initial release

## License

MIT License

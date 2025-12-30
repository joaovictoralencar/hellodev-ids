# HelloDev IDs

GUID-based unique identifier system with localization support. Use IDs to create type-safe references for game entities like items, NPCs, locations, and more.

## Features

- **ID_SO** - ScriptableObject with auto-generated GUID
- Localized display names via Unity Localization
- Developer-friendly internal names (`DevName`)
- Equality comparison based on GUID (implements `IEquatable<ID_SO>`)
- `==` and `!=` operators for natural comparison syntax
- Cached GUID parsing for performance
- Auto-generates GUID on creation

## Getting Started

### 1. Install the Package

**Via Package Manager (Local):**
1. Open Unity Package Manager (Window > Package Manager)
2. Click "+" > "Add package from disk"
3. Navigate to this folder and select `package.json`

**Dependencies:** Ensure `com.hellodev.utils` and `com.unity.localization` are installed.

### 2. Create Your First ID

1. Right-click in Project window
2. Select **Create > HelloDev > IDs > New ID**
3. Name it descriptively (e.g., `ID_Sword_Iron`)
4. Set the `DevName` in the inspector (e.g., "Iron Sword")
5. Configure the `DisplayName` for localization

### 3. Use the ID in Your Code

```csharp
using HelloDev.IDs;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ID_SO itemId;

    public ID_SO ItemId => itemId;

    public bool IsItem(ID_SO otherId)
    {
        // Simple equality check using == operator
        return itemId == otherId;
    }
}
```

### 4. Build an ID-Based System

```csharp
using HelloDev.IDs;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    private Dictionary<System.Guid, int> _itemCounts = new();

    public void AddItem(ID_SO itemId, int count = 1)
    {
        var guid = itemId.Id;  // Cached GUID, efficient access
        _itemCounts[guid] = _itemCounts.GetValueOrDefault(guid) + count;
    }

    public int GetCount(ID_SO itemId)
    {
        return _itemCounts.GetValueOrDefault(itemId.Id);
    }

    public bool HasItem(ID_SO itemId)
    {
        return GetCount(itemId) > 0;
    }
}
```

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

### Creating ID Categories

Organize your IDs by creating folders:

```
Assets/
└── Data/
    └── IDs/
        ├── Items/
        │   ├── ID_Sword_Iron.asset
        │   ├── ID_Sword_Steel.asset
        │   └── ID_Potion_Health.asset
        ├── NPCs/
        │   ├── ID_NPC_Blacksmith.asset
        │   └── ID_NPC_Merchant.asset
        └── Locations/
            ├── ID_Location_Town.asset
            └── ID_Location_Dungeon.asset
```

### Using with Events

IDs work great with the HelloDev Events system:

```csharp
using HelloDev.IDs;
using HelloDev.Events;

public class Monster : MonoBehaviour
{
    [SerializeField] private ID_SO monsterId;
    [SerializeField] private GameEventID_SO onMonsterKilled;  // Custom event type

    public void OnDeath()
    {
        // Raise generic event with specific ID
        onMonsterKilled.Raise(monsterId);
    }
}
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

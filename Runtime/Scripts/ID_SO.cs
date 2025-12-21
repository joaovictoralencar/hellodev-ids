using System;
using HelloDev.Utils;
using UnityEngine;
using UnityEngine.Localization;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace HelloDev.IDs
{
    /// <summary>
    /// ScriptableObject that provides a unique, persistent GUID-based identifier.
    /// Supports localized display names and developer-friendly internal names.
    /// </summary>
    /// <remarks>
    /// GUIDs are auto-generated on creation and remain stable across sessions.
    /// Use for items, quests, achievements, or any system requiring unique identification.
    /// Implements IEquatable for efficient collection operations.
    /// </remarks>
    [CreateAssetMenu(fileName = "New ID", menuName = "HelloDev/IDs/New ID", order = 1)]
    public class ID_SO : RuntimeScriptableObject, IEquatable<ID_SO>
    {
        [Tooltip("Internal name for developers, used for identification in code.")]
        [SerializeField]
        private string devName;

        [Tooltip("A unique, permanent identifier. Auto-generated.")]
        [SerializeField]
#if ODIN_INSPECTOR
        [ReadOnly]
#endif
        private string id;

        [Tooltip("The localized display name of the identifier.")]
        [SerializeField]
        private LocalizedString displayName;

        // Cached parsed GUID to avoid parsing on every access
        private Guid _cachedGuid;
        private bool _guidCached;

        /// <summary>
        /// The unique GUID for this identifier. Cached after first access.
        /// </summary>
        public Guid Id
        {
            get
            {
                if (!_guidCached)
                {
                    _cachedGuid = string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
                    _guidCached = true;
                }
                return _cachedGuid;
            }
        }

        /// <summary>
        /// A developer-friendly name for the identifier, used for internal identification.
        /// </summary>
        public string DevName => devName;

        /// <summary>
        /// The localized name of the identifier for display in the UI.
        /// </summary>
        public LocalizedString DisplayName => displayName;

        /// <summary>
        /// Called when the script is loaded.
        /// Ensures the script has a unique ID.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            GenerateId();
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the Inspector.
        /// Ensures the ID has a unique identifier and a default dev name.
        /// </summary>
        private void OnValidate()
        {
            GenerateId();
            // Invalidate cache when ID changes in editor
            _guidCached = false;
        }

        /// <summary>
        /// Generates a unique identifier for this ScriptableObject if one does not exist.
        /// </summary>
#if ODIN_INSPECTOR
        [Button]
#endif
        private void GenerateId()
        {
            if (string.IsNullOrWhiteSpace(devName))
            {
                devName = name;
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
                _cachedGuid = Guid.Parse(id);
                _guidCached = true;
            }
        }

        /// <summary>
        /// Called when entering play mode to reset runtime state.
        /// ID_SO has no runtime state to reset, so this is intentionally empty.
        /// </summary>
        protected override void OnScriptableObjectReset()
        {
            // ID_SO has no runtime state to reset - GUIDs are permanent
        }

        /// <summary>
        /// Determines whether this ID equals another ID based on their GUIDs.
        /// </summary>
        public bool Equals(ID_SO other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        /// <summary>
        /// Determines whether this ID equals another object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is ID_SO other) return Equals(other);
            return false;
        }

        /// <summary>
        /// Returns the hash code based on the GUID.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        /// Equality operator comparing two ID_SO instances by their GUIDs.
        /// </summary>
        public static bool operator ==(ID_SO left, ID_SO right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        /// <summary>
        /// Inequality operator comparing two ID_SO instances by their GUIDs.
        /// </summary>
        public static bool operator !=(ID_SO left, ID_SO right)
        {
            return !(left == right);
        }
    }
}

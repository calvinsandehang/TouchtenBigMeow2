using Sirenix.OdinInspector;
using UnityEngine;
using static GlobalDefine;

namespace Big2Meow.Player
{
    /// <summary>
    /// ScriptableObject that defines a player's user picture with various emotional states.
    /// </summary>
    [CreateAssetMenu(fileName = "UserPicture_", menuName = "Player/UserPicture")]
    public class PlayerUserPictureSO : SerializedScriptableObject
    {
        /// <summary>
        /// The ID of the avatar.
        /// </summary>
        public AvatarType AvatarID;

        /// <summary>
        /// The sprite representing the normal emotional state.
        /// </summary>
        public Sprite Normal;

        /// <summary>
        /// The sprite representing the excited emotional state.
        /// </summary>
        public Sprite Excited;

        /// <summary>
        /// The sprite representing the happy emotional state.
        /// </summary>
        public Sprite Happy;

        /// <summary>
        /// The sprite representing the sad emotional state.
        /// </summary>
        public Sprite Sad;

        /// <summary>
        /// The sprite representing the angry emotional state.
        /// </summary>
        public Sprite Angry;
    }
}


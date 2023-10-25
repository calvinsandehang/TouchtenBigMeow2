using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserPicture_", menuName = "Player/UserPicture")]
public class PlayerUserPictureSO : SerializedScriptableObject
{
    public Sprite Normal;
    public Sprite Excited;
    public Sprite Happy;
    public Sprite Sad;
    public Sprite Angry;
}

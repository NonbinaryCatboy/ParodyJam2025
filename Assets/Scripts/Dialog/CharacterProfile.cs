using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Scriptable Objects/CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    public string Name;
    public string Voice;
    private Sprite[] reactions;

    public Sprite GetSprite(string emotion)
    {
        if (string.IsNullOrEmpty(emotion))
            return reactions[0];

        return null;
    }
}

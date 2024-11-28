using System;

[Serializable]
public class SetAvatarRequest
{
    public string newAvatar;
    public SetAvatarRequest(string avatarId)
    {
        newAvatar = avatarId;
    }
}

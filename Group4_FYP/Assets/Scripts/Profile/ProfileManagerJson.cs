using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ProfileManagerJson
{
    private static string heroProfileDirectoryPath = Application.persistentDataPath + "/HeroProfiles/";

    public static bool CreateProfile(string profileName, HeroClass heroClass, HeroInfo heroInfo)
    {
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path)) // prevents overwriting of existing heroprofile; tidy up this structure in the future maybe
        {
            // show error
            _ = Notification.Instance.ShowNotification("\"" + profileName + "\" is not available, please use another name");

            return false;
        }
        else
        {
            ProfileData profileData = new ProfileData(profileName, heroClass, heroInfo.defaultStats);
            WriteProfile(profileData, path);

            _ = Notification.Instance.ShowNotification("Successfully created Profile \"" + profileName + "\"!");

            return true;
        }
    }

    public static void SaveProfile(ProfileData profile)
    {
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profile.profileName + ".heroprofile";

        WriteProfile(profile, path);
    }

    public static ProfileData GetProfile(string profileName)
    {
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path))
        {
            return ReadProfile(path);
        }

        return null;
    }

    public static bool UpdateProfile(string profileName, string newProfileName)
    {
        if (profileName == newProfileName)
        {
            // show error
            _ = Notification.Instance.ShowNotification("Profile name has not changed");

            return false;
        }

        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";
        string newPath = heroProfileDirectoryPath + newProfileName + ".heroprofile";

        if (File.Exists(path))
        {
            // getting data
            ProfileData profileData = ReadProfile(path);

            // moving data to new profile
            if (File.Exists(newPath)) // prevents overwriting of existing heroprofile; tidy up this structure in the future maybe
            {
                // show error
                _ = Notification.Instance.ShowNotification("\"" + newProfileName + "\" is not available, please use another name");
            }
            else
            {
                ProfileData newProfileData = new ProfileData(newProfileName, profileData);
                WriteProfile(newProfileData, newPath);
                DeleteProfile(profileName);

                _ = Notification.Instance.ShowNotification("Successfully updated Profile \"" + profileName + "\" to \"" + newProfileName + "\"!");

                return true;
            }
        }

        return false;
    }

    public static bool DeleteProfile(string profileName, bool showNotification = true)
    {
        // check if profile with profileName exists or not
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path))
        {
            // delete profile
            File.Delete(path);

            if (showNotification)
            {
                _ = Notification.Instance.ShowNotification("Profile \"" + profileName + "\" deleted");
            }

            return true;
        }
        else
        {
            // show error
            if (showNotification)
            {
                _ = Notification.Instance.ShowNotification("Profile \"" + profileName + "\" does not exist");
            }
        }

        return false;
    }

    public static ProfileData[] GetProfiles() // used for showing buttons in StartMenu.cs
    {
        CreateHeroProfileDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(heroProfileDirectoryPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.heroprofile");
        List<ProfileData> profiles = new List<ProfileData>();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            profiles.Add(ReadProfile(fileInfos[i].ToString()));
        }

        return profiles.ToArray();
    }

    public static string GetHeroProfileDirectoryPath()
    {
        return heroProfileDirectoryPath;
    }

    private static void CreateHeroProfileDirectory()
    {
        if (!Directory.Exists(heroProfileDirectoryPath))
        {
            Directory.CreateDirectory(heroProfileDirectoryPath);
        }
    }

    private static void WriteProfile(ProfileData profileData, string path)
    {
        string json = JsonUtility.ToJson(profileData);

        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Write(json);
        streamWriter.Close();
    }

    private static ProfileData ReadProfile(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        string json = streamReader.ReadToEnd();
        streamReader.Close();

        return JsonUtility.FromJson<ProfileData>(json);
    }
}

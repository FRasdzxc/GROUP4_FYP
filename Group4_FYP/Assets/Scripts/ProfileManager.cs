using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ProfileManager
{
    private static string heroProfileDirectoryPath = Application.persistentDataPath + "/HeroProfiles/";

    public static bool CreateProfile(string profileName)
    {
        //if (!Directory.Exists(heroProfilePath))
        //{
        //    Directory.CreateDirectory(heroProfilePath);
        //}
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path)) // prevents overwriting of existing heroprofile; tidy up this structure in the future maybe
        {
            Debug.LogWarning("profile " + profileName + ".heroprofile exists already.");

            // show error

            return false;
        }
        else
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Create);

            ProfileData profileData = new ProfileData(profileName);

            binaryFormatter.Serialize(fileStream, profileData);
            fileStream.Close();

            return true;
        }
    }

    public static void SaveProfile(string profileName, Hero hero)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        //if (!Directory.Exists(heroProfilePath))
        //{
        //    Directory.CreateDirectory(heroProfilePath);
        //}
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        ProfileData profileData = new ProfileData(hero);

        binaryFormatter.Serialize(fileStream, profileData);
        fileStream.Close();
    }

    public static ProfileData LoadProfile(string profileName)
    {
        //if (!Directory.Exists(heroProfilePath))
        //{
        //    Directory.CreateDirectory(heroProfilePath);
        //}
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ProfileData profileData = binaryFormatter.Deserialize(fileStream) as ProfileData;
            fileStream.Close();

            return profileData;
        }

        return null;
    }

    public static bool UpdateProfile(string profileName, string newProfileName)
    {
        if (profileName == newProfileName)
        {
            Debug.LogWarning("profile name has not changed.");

            // show error

            return false;
        }

        //if (!Directory.Exists(heroProfilePath))
        //{
        //    Directory.CreateDirectory(heroProfilePath);
        //}
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";
        string newPath = heroProfileDirectoryPath + newProfileName + ".heroprofile";

        if (File.Exists(path))
        {
            // getting data
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ProfileData profileData = binaryFormatter.Deserialize(fileStream) as ProfileData;
            fileStream.Close();

            // moving data to new profile
            if (File.Exists(newPath)) // prevents overwriting of existing heroprofile; tidy up this structure in the future maybe
            {
                Debug.LogWarning("profile " + newProfileName + ".heroprofile exists already.");

                // show error
            }
            else
            {
                fileStream = new FileStream(newPath, FileMode.Create);

                ProfileData newProfileData = new ProfileData(newProfileName, profileData);

                binaryFormatter.Serialize(fileStream, newProfileData);
                fileStream.Close();

                DeleteProfile(profileName, false);

                return true;
            }
        }

        return false;
    }

    public static bool DeleteProfile(string profileName, bool requiresConfirmation)
    {
        // check if profile with profileName exists or not
        CreateHeroProfileDirectory();
        string path = heroProfileDirectoryPath + profileName + ".heroprofile";

        if (File.Exists(path))
        {
            // if requiresConfirmation, ask for confirmation using confirmation panel
            if (requiresConfirmation)
            {

            }

            // delete profile
            File.Delete(path);

            return true;
        }

        return false;
    }

    public static ProfileData[] GetProfiles() // used for showing buttons in StartMenu.cs
    {
        CreateHeroProfileDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(heroProfileDirectoryPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.heroprofile");
        List<ProfileData> profiles = new List<ProfileData>();

        BinaryFormatter binaryFormatter = new BinaryFormatter();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileStream fileStream = new FileStream(fileInfos[i].ToString(), FileMode.Open);

            profiles.Add(binaryFormatter.Deserialize(fileStream) as ProfileData);
            fileStream.Close();
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
}

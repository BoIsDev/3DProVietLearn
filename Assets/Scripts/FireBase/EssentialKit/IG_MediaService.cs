using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VoxelBusters.EssentialKit;
using System.Threading;

public class IG_MediaService : MonoBehaviour
{
    public static IG_MediaService instance;

    GalleryAccessStatus readAccessStatus;
    GalleryAccessStatus readWriteAccessStatus;
    CameraAccessStatus cameraAccessStatus;

    public Texture2D currentImage; //IG_MediaService.instance.currentImage;
    private string userID;

    void Start()
    {
        instance = this;
        currentImage = null;
    }


    public void GetImageFromGallery()
    {
        // Ki?m tra quy?n truy c?p th? vi?n
        readAccessStatus = MediaServices.GetGalleryAccessStatus(GalleryAccessMode.Read);

        if (readAccessStatus == GalleryAccessStatus.NotDetermined)
        {
            MediaServices.RequestGalleryAccess(GalleryAccessMode.Read, callback: (result, error) =>
            {
                if (result.AccessStatus == GalleryAccessStatus.Authorized)
                {
                    SelectImageFromGallery();
                }
                else
                {
                    Debug.LogWarning("Gallery access denied.");
                }
            });
        }
        else if (readAccessStatus == GalleryAccessStatus.Authorized)
        {
            SelectImageFromGallery();
        }
        else
        {
            Debug.LogWarning("Gallery access denied.");
        }
    }

    private void SelectImageFromGallery()
    {
        MediaServices.SelectImageFromGallery(canEdit: true, (textureData, error) =>
        {
            if (error == null)
            {
                Debug.Log("Image selected from gallery successfully.");
                currentImage = textureData.GetTexture();
                SaveImageToAppStorage(currentImage);
            }
            else
            {
                Debug.LogError("Failed to select image from gallery: " + error);
            }
        });
    }

    public void TakePictureWithCamera()
    {
        cameraAccessStatus = MediaServices.GetCameraAccessStatus();

        if (cameraAccessStatus == CameraAccessStatus.NotDetermined)
        {
            MediaServices.RequestCameraAccess(callback: (result, error) =>
            {
                if (result.AccessStatus == CameraAccessStatus.Authorized)
                {
                    CaptureImageFromCamera();
                }
                else
                {
                    Debug.LogWarning("Camera access denied.");
                }
            });
        }
        else if (cameraAccessStatus == CameraAccessStatus.Authorized)
        {
            CaptureImageFromCamera();
        }
        else
        {
            Debug.LogWarning("Camera access denied.");
        }
    }

    private void CaptureImageFromCamera()
    {
        MediaServices.CaptureImageFromCamera(true, (textureData, error) =>
        {
            if (error == null)
            {
                Debug.Log("Image captured from camera successfully.");
                currentImage = textureData.GetTexture();
                SaveImageToAppStorage(currentImage);
            }
            else
            {
                Debug.LogError("Failed to capture image from camera: " + error);
            }
        });
    }
    public IEnumerator WaitLoadAvatar()
    {
        Debug.Log("Waiting for user ID to load avatar...");
        Debug.Log(Thread.CurrentThread.ManagedThreadId);
        Debug.Log(ChoiceCharacter.Instance.userId);
        while (ChoiceCharacter.Instance.userId == null)
        {
            yield return null;
        }
        userID = ChoiceCharacter.Instance.userId;
        LoadAvatar();
    }

    private void SaveImageToAppStorage(Texture2D texture)
    {
        try
        {
            // Mã hóa hình ?nh sang ??nh d?ng PNG và l?u tr? nó trong t?p
            byte[] imageBytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "AvatarImage_" + userID + ".png");
            File.WriteAllBytes(filePath, imageBytes);

            // L?u ???ng d?n t?p vào PlayerPrefs
            PlayerPrefs.SetString("AvatarImagePath_" + userID, filePath);
            PlayerPrefs.Save(); // ??m b?o thông tin ???c l?u ngay l?p t?c

            Debug.Log("Image saved to app storage: " + filePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save image to app storage: " + e.Message);
        }
    }
    public void LoadAvatar()
    {
        Debug.Log("Loading avatar for user: " + userID);

        if (string.IsNullOrEmpty(userID))
        {
            Debug.LogWarning("User ID is null or empty, cannot load avatar.");
            return;
        }

        // T?i ???ng d?n t?p hình ?nh t? PlayerPrefs
        string filePath = PlayerPrefs.GetString("AvatarImagePath_" + userID, string.Empty);

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("No avatar found for user: " + userID);
            return;
        }

        Debug.Log("Loaded file path: " + filePath);
        Debug.Log("Thread ID: " + Thread.CurrentThread.ManagedThreadId);

        if (File.Exists(filePath))
        {
            try
            {
                // ??c d? li?u hình ?nh t? t?p
                byte[] imageBytes = File.ReadAllBytes(filePath);
                Texture2D avatarTexture = new Texture2D(2, 2);
                avatarTexture.LoadImage(imageBytes);

                // Gán hình ?nh vào `currentImage`
                currentImage = avatarTexture;

                Debug.Log("Avatar loaded from file for user: " + userID);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load image from file: " + e.Message);
                currentImage = LoadDefaultImage();
            }
        }
        else
        {
            // T?i ?nh m?c ??nh n?u không tìm th?y t?p hình ?nh
            currentImage = LoadDefaultImage();
            Debug.LogWarning("Avatar image not found, loaded default image for user: " + userID);
        }
    }

    private Texture2D LoadDefaultImage()
    {
        Texture2D defaultImage = new Texture2D(2, 2);
        return defaultImage;
    }
}

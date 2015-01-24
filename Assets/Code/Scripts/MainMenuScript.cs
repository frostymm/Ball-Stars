using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
	public GameObject OptionsPanel;
	public GameObject MainPanel;
	public GameObject StageSelectPanel;
	public GameObject HowToPlayPanel;

	public Button deleteSaveButton;

	public void SinglePlayer()
	{
		LoadSave();

		CloseMenus();
		
		if(!StageSelectPanel.activeSelf)
			StageSelectPanel.SetActive(true);
	}

	public void LoadRace(string stage)
	{
		GameManager.Instance().LoadRace(stage);
	}

	public void Quit()
	{
		GameManager.Instance().Quit();
	}

	public void SetTexturePath(string textPath)
	{
		GameManager.Instance().SetTexturePath(textPath);
	}

	public void LoadSave()
	{
		GameManager.Instance().Load();
	}

	public void Save()
	{
		GameManager.Instance().Save();
	}

	public void DeleteSave()
	{
		GameManager.Instance().DeleteSave();

		if(deleteSaveButton.IsInteractable())
			deleteSaveButton.interactable = false;
	}

	public void CloseMenus()
	{
		if(OptionsPanel.activeSelf)
			OptionsPanel.SetActive(false);
		if(MainPanel.activeSelf)
			MainPanel.SetActive(false);
		if(StageSelectPanel.activeSelf)
			StageSelectPanel.SetActive(false);
		if(HowToPlayPanel.activeSelf)
			HowToPlayPanel.SetActive(false);
	}

	public void LoadMain()
	{
		CloseMenus();

		if(!MainPanel.activeSelf)
			MainPanel.SetActive(true);
	}

	public void LoadOptions()
	{
		CloseMenus();

		if(!OptionsPanel.activeSelf)
			OptionsPanel.SetActive(true);

		InitializeOptions();
	}

	public void LoadHowTo()
	{
		CloseMenus();

		if(!HowToPlayPanel.activeSelf)
			HowToPlayPanel.SetActive(true);
	}

	public void LoadImage()
	{
		GameManager.Instance().LoadTexture();
		InitializeOptions();
	}

	public RawImage PreviewImage;
	public void InitializeOptions()
	{

	}
}


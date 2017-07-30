using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InputKeeper : MonoBehaviour {
	GameObject IntroPanel;
    GameObject ThresholdPanel;
    GameObject Instruction;
    GameObject FirstQuestion;
    GameObject SecondQuestion;
    GameObject ThirdQuestion;
    GameObject FourthQuestion;
    GameObject FifthQuestion;
	GameObject Numpad;

	
    GameObject InfoScroll;

	Text reflector;

    Button nextPanel;
	Button prevPanel;
    GameObject[] GamePanels;
    int index=0;
    int INDEX_LIMIT=7;
	Gaze Lookingat;

	[SerializeField]
	public static List<int> panelValues = new List<int>();



	
    // Use this for initialization
    void Start()
    {
		InfoScroll = transform.parent.gameObject;

		Lookingat = FindObjectOfType<Gaze> ();

		nextPanel = transform.Find("Next").GetComponent<Button>();
		prevPanel = transform.Find("Previous").GetComponent<Button>();

        IntroPanel = transform.Find("Intro Panel").gameObject;
        ThresholdPanel = transform.Find("Threshold Panel").gameObject;
        Instruction = transform.Find("Instruction").gameObject;
        FirstQuestion = transform.Find("First Question").gameObject;
        SecondQuestion = transform.Find("Second Question").gameObject;
        ThirdQuestion = transform.Find("Third Question").gameObject;
        FourthQuestion = transform.Find("Fourth Question").gameObject;
        FifthQuestion = transform.Find("Fifth Question").gameObject;

		Numpad = transform.Find("NumPad").gameObject;
		reflector = transform.Find("Reflector").GetComponent<Text>();
		Numpad.SetActive (false);

		//panelValues = new List<int>();

		panelValues.Add(0);
		panelValues.Add(0);
		panelValues.Add(0);
		panelValues.Add(0);
		panelValues.Add(0);

		allArrays();

		prevPanel.gameObject.SetActive(false);

    }

	public void setBoolean(){

		Debug.Log ("SetBoolean on InputKeeper");
		NumKeyPress numberPressed = FindObjectOfType<NumKeyPress> ();
		numberPressed.Reflector.text = "";
	}

	public void SetPanelValue(int input)
	{
		panelValues[index-3] = input;
	}
    void allArrays()
    {
       GamePanels = new GameObject[] { IntroPanel, ThresholdPanel, Instruction, FirstQuestion, SecondQuestion, ThirdQuestion, FourthQuestion, FifthQuestion };
    }

    public void NextAction()
    {
		GamePanels [index].SetActive (false);
		index++;
		UpdateUIAction();
	}

	public void PrevAction()
    {
		GamePanels [index].SetActive (false);
		index--;
		UpdateUIAction();
	}

	public void UpdateUIAction()
	{
		
		if(index > INDEX_LIMIT)
		{
			index = 0;
			InfoScroll.SetActive(false);
			prevPanel.gameObject.SetActive(false);
			GamePanels [index].SetActive (true);
			reflector.text = "";
			return;
		}

		if(index < 0)
		{
			index = 0;
		}

		GamePanels [index].SetActive (true);

		if(InfoScroll.activeSelf == false)
		{
			InfoScroll.SetActive(true);
		}

		if(index == 0)
		{
			prevPanel.gameObject.SetActive(false);
		}
		else if(prevPanel.gameObject.activeSelf == false) {
			prevPanel.gameObject.SetActive(true);
		}

		if (index < 3) {
			Numpad.SetActive (false);
			reflector.gameObject.SetActive(false);
		} else {
			Numpad.SetActive (true);
			reflector.gameObject.SetActive(true);
			reflector.text = panelValues[index-3].ToString();

		}
	}

}

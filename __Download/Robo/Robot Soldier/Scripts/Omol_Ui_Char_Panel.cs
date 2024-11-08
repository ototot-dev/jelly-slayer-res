using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//creates the buttons on panel of animations
public class Omol_Ui_Char_Panel : MonoBehaviour {

	public GameObject character;
	public Transform acts_table;
    public Button buttonPrefab;

    Button sel_btm;

	Omol_Actions actions;


	void Start () {
 
		actions = character.GetComponent<Omol_Actions> ();
 


		CreateActionButton("Idle");
		CreateActionButton("Cry");
		CreateActionButton("GoForwad");
		CreateActionButton("GoBack");
		CreateActionButton("TurnLeft");
		CreateActionButton("TurnRight");
		CreateActionButton("StrafeLeft");
		CreateActionButton("StrafeRight");
		CreateActionButton("Run");
		CreateActionButton("Sneak");
		CreateActionButton("Jump");

 
		CreateActionButton("Attack1");
		CreateActionButton("Attack2");
		CreateActionButton("Attack3");
		CreateActionButton("Attack4");

		CreateActionButton("DamageFront1");
		CreateActionButton("DamageFront2");
		CreateActionButton("DamageLeft");
		CreateActionButton("DamageRight");
		CreateActionButton("DamageBack");

		CreateActionButton("Dead1");
		CreateActionButton("Dead2");
		CreateActionButton("Dead3");
		CreateActionButton("Dead4");



 
    }

 

    void CreateActionButton(string name) {
		CreateActionButton(name, name);
	}

	void CreateActionButton(string name, string message) {

		Button button = CreateButton (name, acts_table);
       
        if (name == "Idle")
        {
            sel_btm = button;
            button.GetComponentInChildren<Image>().color = new Color(1F, 0.7F, 0.7F);
        }
        button.GetComponentInChildren<Text>().fontSize = 12;
        button.onClick.AddListener(()  => actions.SendMessage(message, SendMessageOptions.DontRequireReceiver));
        button.onClick.AddListener(() => select_btm(button)  );
 

    }
    void select_btm(Button btm)
    {
        sel_btm.GetComponentInChildren<Image>().color = Color.white;
        btm.GetComponentInChildren<Image>().color = new Color(1F, 0.7F, 0.7F);
        sel_btm = btm;
    }

 
    Button CreateButton(string name, Transform group) {
		GameObject obj = (GameObject) Instantiate (buttonPrefab.gameObject);
		obj.name = name;
		obj.transform.SetParent(group);
		obj.transform.localScale = Vector3.one;
		Text text = obj.transform.GetChild (0).GetComponent<Text> ();
		text.text = name;
		return obj.GetComponent<Button> ();
	}




	public void OpenPublisherPage() {
		Application.OpenURL ("https://connect.unity.com/u/58c9250f32b30600230f64fa");
	}

 
}

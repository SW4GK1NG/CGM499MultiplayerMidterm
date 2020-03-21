using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class Client : MonoBehaviour
{

    static SocketIOComponent socket;
    public InputField name;
    public InputField number;
    public Button resetButton;
    public Text outputText;

    // Start is called before the first frame update
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("youwin", winning);
        socket.On("high", tooHigh);
        socket.On("low", tooLow);
        socket.On("vivonzulul", someoneWon);
        socket.On("disableReset", stopReset);
        resetButton.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void stopReset(SocketIOEvent obj) {
        outputText.text = "New game has started!!";
        number.text = "";
        //Debug.Log("FUCK YOU");
        resetButton.enabled = false;
    }

    void someoneWon(SocketIOEvent obj) {
        string winnerName = obj.data["name"].str;
        winnerName = winnerName.ToString();
        //Debug.Log(winnerName.ToString());
        outputText.text = (winnerName + " answered correctly!!");
        resetButton.enabled = true;
    }

    void winning(SocketIOEvent obj)
    {
        string winnerName = obj.data["name"].str;
        winnerName = winnerName.ToString();
        //Debug.Log(winnerName.ToString());
        outputText.text = ("You've answered correctly!!");
        resetButton.enabled = true;
    }

    void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    void tooHigh(SocketIOEvent obj) {
        Debug.Log("High");
        outputText.text = "You answered too high";
    }

    void tooLow(SocketIOEvent obj) {
        Debug.Log("Low");
        outputText.text = "You answered too low";
    }

    public void sendAnswer () {
        
        JSONObject jSONObject = new JSONObject(JSONObject.Type.OBJECT);
        //Debug.Log(name.text);
        //Debug.Log(number.text);
        jSONObject.AddField("number", number.text);
        jSONObject.AddField("name", name.text);
        socket.Emit("message.send", jSONObject);
        Debug.Log("send");

    }

    public void restart() {
        outputText.text = "New game has started!!";
        number.text = "";
        resetButton.enabled = false;
        socket.Emit("reset");
    }
}
